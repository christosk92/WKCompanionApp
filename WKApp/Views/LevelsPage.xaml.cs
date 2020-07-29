using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WKApp.ViewModels;

namespace WKApp.Views
{
    public sealed partial class LevelsPage : Page
    {
        private LevelsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LevelsViewModel; }
        }

        public LevelsPage()
        {
            InitializeComponent();
            Loaded += LevelsPage_Loaded;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is string)
            {
                switch (e.Parameter)
                {
                    case "Content":
                        ViewModel.SelectedDataType = "日本語";
                        break;
                    case "Kanji":
                        ViewModel.SelectedDataType = "漢字";
                        break;
                    case "Vocabulary":
                        ViewModel.SelectedDataType = "単語";
                        break;
                    default:
                        ViewModel.SelectedDataType = e.Parameter.ToString();
                        break;
                }
                LevelsDetailControl.DataType = e.Parameter.ToString();
            }
            base.OnNavigatedTo(e);
        }
        private async void LevelsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
