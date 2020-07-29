using System;

using Windows.UI.Xaml.Controls;

using WKApp.ViewModels;

namespace WKApp.Views
{
    public sealed partial class SignInPage : Page
    {

        private SignInViewModel ViewModel
        {
            get { return ViewModelLocator.Current.SignInViewModel; }
        }

        public SignInPage()
        {
            InitializeComponent();
        }

        private void RegisterButtonTextBlock_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //TODO: Implement webView navigation and acquire apikey.
            throw new NotImplementedException();
        }
    }
}
