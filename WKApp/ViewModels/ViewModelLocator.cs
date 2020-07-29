using System;

using GalaSoft.MvvmLight.Ioc;

using WKApp.Services;
using WKApp.ViewModels.DetailControls;
using WKApp.Views;
using WKApp.Views.DetailControls;

namespace WKApp.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<LevelsViewModel, LevelsPage>();
            Register<LevelsContentGridViewModel, LevelsContentGridPage>();
            Register<LevelsContentGridDetailViewModel, LevelsContentGridDetailPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<KanjiDetailViewModel, KanjiDetailPage>();
            Register<SignInViewModel, SignInPage>();
        }

        public SignInViewModel SignInViewModel => SimpleIoc.Default.GetInstance<SignInViewModel>();

        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public LevelsContentGridDetailViewModel LevelsContentGridDetailViewModel => SimpleIoc.Default.GetInstance<LevelsContentGridDetailViewModel>();

        public LevelsContentGridViewModel LevelsContentGridViewModel => SimpleIoc.Default.GetInstance<LevelsContentGridViewModel>();
        public KanjiDetailViewModel KanjiDetailViewModel => SimpleIoc.Default.GetInstance<KanjiDetailViewModel>();

        public LevelsViewModel LevelsViewModel => SimpleIoc.Default.GetInstance<LevelsViewModel>();

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => SimpleIoc.Default.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
