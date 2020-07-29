using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using WKApp.Activation;
using WKApp.Core.Helpers;
using WKApp.Models;
using WKApp.Services;
using WKApp.ViewModels;

namespace WKApp.Services
{
    // For more information on understanding and extending activation flow see
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private Lazy<UIElement> _shell;
        private IdentityService IdentityService => Singleton<IdentityService>.Instance;
        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
        private object _lastActivationArgs;

        private Nihongo<Kanji> KanjiData => Singleton<Nihongo<Kanji>>.Instance;
        private Nihongo<Vocabulary> VocabData => Singleton<Nihongo<Vocabulary>>.Instance;
        private Nihongo<Radical> RadicalData => Singleton<Nihongo<Radical>>.Instance;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
            IdentityService.LoggedIn += OnLoggedIn;
        }
        public void SetShell(Lazy<UIElement> shell)
        {
            _shell = shell;
        }
        private async void OnLoggedIn(object sender, UserLoginEventArg e)
        {
            if (IdentityService.IsLoggedIn())
            {
                await ThemeSelectorService.SetRequestedThemeAsync();
                await HandleActivationAsync(_lastActivationArgs);
                if (_shell?.Value != null)
                {
                    Window.Current.Content = _shell.Value;
                }
                else
                {
                    var frame = new Frame();
                    Window.Current.Content = frame;
                    NavigationService.Frame = frame;
                }
            }
        }
        private async Task LoadNihongo()
        {
            List<String> types = new List<string>();
            types.Add("kanji"); types.Add("radical"); types.Add("vocab");

            foreach (var type in types)
            {     
                string fname = @"Assets\" + type + "_id.json";
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile file = await InstallationFolder.GetFileAsync(fname);
                if (File.Exists(file.Path))
                {
                    // deserialize JSON directly from a file
                    using (StreamReader files = File.OpenText(file.Path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        var kanjiids = (List<IdData>)serializer.Deserialize(files, typeof(List<IdData>));
                        await FinishInit(kanjiids);           
                    }
                }
                async Task FinishInit(List<IdData> data)
                {

                    fname = @"Assets\" + type + ".json";
                    InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                    file = await InstallationFolder.GetFileAsync(fname);
                    if (File.Exists(file.Path))
                    {
                        // deserialize JSON directly from a file
                        using (StreamReader files = File.OpenText(file.Path))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            switch (type)
                            {
                                case "kanji":
                                    KanjiData.Data = (List<Kanji>)serializer.Deserialize(files, typeof(List<Kanji>));
                                    KanjiData.Ids = data;
                                    break;
                                case "radical":
                                    RadicalData.Data = (List<Radical>)serializer.Deserialize(files, typeof(List<Radical>));
                                    RadicalData.Ids = data;
                                    break;
                                case "vocab":
                                    VocabData.Data = (List<Vocabulary>)serializer.Deserialize(files, typeof(List<Vocabulary>));
                                    VocabData.Ids = data;
                                    break;
                            }
                        }
                    }
                }

            }
        }
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize services that you need before app activation
                await InitializeAsync();
                UserDataService.Initialize();
                var user = await UserDataService.GetUserFromCacheAsync();
                var silentLogin = await IdentityService.LoginAsync();
                if (!IdentityService.IsLoggedIn())
                {
                    await RedirectLoginPageAsync();
                }
                else
                {
                    await LoadNihongo();
                    if (_shell?.Value != null)
                    {
                        Window.Current.Content = _shell.Value;
                    }
                    else
                    {
                        var frame = new Frame();
                        Window.Current.Content = frame;
                        NavigationService.Frame = frame;
                    }
                }

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Shell or Frame to act as the navigation context
                    Window.Current.Content = _shell?.Value ?? new Frame();
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            if (IdentityService.IsLoggedIn())
            {
                await HandleActivationAsync(activationArgs);
            }

            _lastActivationArgs = activationArgs;

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }
        public async Task RedirectLoginPageAsync()
        {
            var frame = new Frame();
            NavigationService.Frame = frame;
            Window.Current.Content = frame;
            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate(typeof(SignInViewModel).FullName, new DrillInNavigationTransitionInfo());
        }
        public static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        private async Task InitializeAsync()
        {
            await Singleton<LiveTileService>.Instance.EnableQueueAsync().ConfigureAwait(false);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var defaultHandler = new DefaultActivationHandler(_defaultNavItem);
            if (defaultHandler.CanHandle(activationArgs))
            {
                await defaultHandler.HandleAsync(activationArgs);
            }
        }


        private async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            Singleton<LiveTileService>.Instance.SampleUpdate();
            await Task.CompletedTask;
        }
        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
