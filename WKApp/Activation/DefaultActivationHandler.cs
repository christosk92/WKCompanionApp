using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

using WKApp.Core.Helpers;
using WKApp.Services;
using WKApp.ViewModels;

namespace WKApp.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly string _navElement;

        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        public DefaultActivationHandler(Type navElement)
        {
            _navElement = navElement.FullName;
        }

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            object arguments = null;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;
            }

            NavigationService.Navigate(_navElement, arguments);

            // TODO WTS: Remove or change this sample which shows a toast notification when the app is launched.
            // You can use this sample to create toast notifications where needed in your app.
            Singleton<ToastNotificationsService>.Instance.ShowToastNotificationSample();
            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null && _navElement != null;
        }
    }
}
