using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Microsoft.Toolkit.Uwp.UI.Animations;
using WKApp.Core.Helpers;
using WKApp.Models;
using WKApp.Services;

namespace WKApp.ViewModels
{
    public class LevelsContentGridViewModel : ViewModelBase
    {
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<DataGridData>(OnItemClick));

        public ObservableCollection<DataGridData> Source { get; } = new ObservableCollection<DataGridData>();
        private Nihongo<Kanji> KanjiData => Singleton<Nihongo<Kanji>>.Instance;
        private Nihongo<Vocabulary> VocabData => Singleton<Nihongo<Vocabulary>>.Instance;
        private Nihongo<Radical> RadicalData => Singleton<Nihongo<Radical>>.Instance;

        public LevelsContentGridViewModel()
        {
        }

        public async Task LoadData(string level, string dataType)
        {
            Source.Clear();
            var splitLevels = level.ToLower().Replace("level", "").Split('-').ToList();
            splitLevels.ForEach(x => x.Trim());
            switch (dataType)
            {
                case "Kanji":
                    foreach (var k in KanjiData.Ids.Where(x => long.Parse(x.Level) >= int.Parse(splitLevels[0]) && long.Parse(x.Level) <= int.Parse(splitLevels[1])))
                    {
                        var x = KanjiData.Data.FirstOrDefault(y => y.id.ToString() == k.Id);
                        Source.Add(new DataGridData
                        {
                            MainReading = k.MainReading,
                            Meaning = x.data.meanings.FirstOrDefault(y => y.primary)?.meaning,
                            Level = k.Level,
                            Title = k.Title
                        });
                    }
                    break;
                case "Vocabulary":
                    break;
                case "Radicals":
                    break;
                default:
                    break;
            }
        }

        private void OnItemClick(DataGridData item)
        {
            if (item != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(item);
                NavigationService.Navigate(typeof(LevelsContentGridDetailViewModel).FullName, item);
            }
        }
    }
}
