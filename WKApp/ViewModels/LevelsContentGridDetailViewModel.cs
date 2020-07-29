using System;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;


namespace WKApp.ViewModels
{
    public class LevelsContentGridDetailViewModel : ViewModelBase
    {
        private String _item;

        public String Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public LevelsContentGridDetailViewModel()
        {
        }

        public async Task InitializeAsync(long orderID)
        {
           // var data = await SampleDataService.GetContentGridDataAsync();
           // Item = data.First(i => i.OrderID == orderID);
        }
    }
}
