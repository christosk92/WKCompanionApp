using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using WKApp.Core.Helpers;
using WKApp.Models;
using WKApp.Services;

namespace WKApp.ViewModels.DetailControls
{
    public partial class ReadingCustom
    {
        public string Onyomi { get; set; }
        public bool OnyomiEnabled { get; set; } = true;
        public string Kunyomi { get; set; }
        public bool KunyomiEnabled { get; set; } = true;
        public string Nanori { get; set; }
        public bool NanoriEnabled { get; set; } = false;
    }

    public class KanjiDetailViewModel : ViewModelBase
    {
        public ReadingCustom Readings = new ReadingCustom();
        private Kanji _item;
        private IdData _passedItem;
        private bool _isVocab;
        private bool _isKanji;
        private string _altMeaningsL;
        private string _OtherReadingsType;
        private string _MainReadingType;
        private string _ComponentsTypeName;
        public bool IsVocab
        {
            get { return _isVocab; }
            set { Set(ref _isVocab, value); }
        }
        public string ComponentsTypeName
        {
            get { return _ComponentsTypeName; }
            set { Set(ref _ComponentsTypeName, value); }
        }
        public bool IsKanji = true;
        public Kanji Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }
        public IdData PassedItem
        {
            get { return _passedItem; }
            set { Set(ref _passedItem, value); }
        }
        public string altMeaningsL
        {
            get { return _altMeaningsL; }
            set { Set(ref _altMeaningsL, value); }
        }
        public string OtherReadingsType
        {
            get { return _OtherReadingsType; }
            set { Set(ref _OtherReadingsType, value); }
        }
        public string MainReadingType
        {
            get { return _MainReadingType; }
            set { Set(ref _MainReadingType, value); }
        }
        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<DataGridData>(OnItemClick));
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;
        public ObservableCollection<IdData> Source { get; } = new ObservableCollection<IdData>();
        public ObservableCollection<IdData> FoundInVocab { get; } = new ObservableCollection<IdData>();

        public List<String> OtherReadings { get; set; } = new List<string>();
        private Nihongo<Kanji> KanjiData => Singleton<Nihongo<Kanji>>.Instance;
        private Nihongo<Radical> RadicalData => Singleton<Nihongo<Radical>>.Instance;
        private Nihongo<Vocabulary> VocabData => Singleton<Nihongo<Vocabulary>>.Instance;

        public KanjiDetailViewModel()
        {
        }
        private void OnItemClick(DataGridData clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(ViewModels.MainViewModel).FullName, clickedItem.Title);
            }
        }
        public void InitializeAsync(IdData kData)
        {
            Source.Clear();
            FoundInVocab.Clear();
            PassedItem = kData;
            IsKanji = true;
            IsVocab = false;
            ComponentsTypeName = "Radicals";
            Item = KanjiData.Data.Find(x => x.id == int.Parse(kData.Id));
            MainReadingType = Item.data.readings.First(x => x.primary).type;
            OtherReadings = Item.data.readings.Where(x => x.reading != kData.MainReading).Select(x => x.reading).ToList();
            OtherReadingsType = string.Join(",", Item.data.readings.Where(x => x.reading != kData.MainReading).Select(x => x.type).ToList());
            string j = string.Join(",", Item.data.meanings.Where(x => x.primary == false).Select(x => x.meaning).ToList());
            altMeaningsL = String.IsNullOrEmpty(j) ? "" : "Alternative: " + j;
            var inst = Item.data.readings;
            Readings = new ReadingCustom
            {
                Kunyomi = inst.Where(x => x.type == "kunyomi").Count() > 0
                ? string.Join(",", inst.Where(x => x.type == "kunyomi").Select(x => x.reading).ToList())
                : "None",
                KunyomiEnabled = inst.Where(x => x.type == "kunyomi").Count() > 0 ? false : true,

                Onyomi = inst.Where(x => x.type == "onyomi").Count() > 0
                ? string.Join(",", inst.Where(x => x.type == "onyomi").Select(x => x.reading).ToList())
                : "None",
                OnyomiEnabled = inst.Where(x => x.type == "onyomi").Count() > 0 ? false : true,

                Nanori = inst.Where(x => x.type == "nanori").Count() > 0
                ? string.Join(",", inst.Where(x => x.type == "nanori").Select(x => x.reading).ToList())
                : "None",
                NanoriEnabled = inst.Where(x => x.type == "nanori").Count() > 0 ? false : true,
            };
            foreach (var c in Item.data.component_subject_ids)
            {
                var k = RadicalData.Ids.First(x => long.Parse(x.Id) == c);
                Source.Add(k);
            }
            foreach(var c in Item.data.amalgamation_subject_ids)
            {
                var k = VocabData.Ids.First(x => long.Parse(x.Id) == c);
                FoundInVocab.Add(k);
            }
        }
    }
}
