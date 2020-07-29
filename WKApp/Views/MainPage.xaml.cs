using System;

using Windows.UI.Xaml.Controls;

using WKApp.ViewModels;

namespace WKApp.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
