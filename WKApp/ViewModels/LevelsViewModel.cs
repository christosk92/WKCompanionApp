using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using Microsoft.Toolkit.Uwp.UI.Controls;


namespace WKApp.ViewModels
{
    public class LevelsViewModel : ViewModelBase
    {
        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); selectedIndex = LevelItems.ToList().FindIndex(x => x == _selectedItem); }
        }
        private string _SelectedDataType;
        public string SelectedDataType
        {
            get { return _SelectedDataType; }
            set { Set(ref _SelectedDataType, value); }
        }

        public int selectedIndex;

        public ObservableCollection<String> LevelItems { get; private set; } = new ObservableCollection<String>();

        public LevelsViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            LevelItems.Clear();

            // var data = await SampleDataService.GetMasterDetailDataAsync(); TODO: Replace with real data.

            for (int i = 1; i < 61; i += 10)
            {
                LevelItems.Add($"Level {i} - {i + 9}");
            }

            SelectedItem = LevelItems.First();

            if (viewState == MasterDetailsViewState.Both)
            {
                selectedIndex = 0;
            }
        }
    }
}
