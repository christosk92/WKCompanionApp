using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace WKApp.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string _name;
        private Uri _photo;
        private string _apiKey;
        private string _DownloadLoc;
        private string _faToken;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public string faToken
        {
            get => _faToken;
            set => Set(ref _faToken, value);
        }
        public Uri Photo
        {
            get => _photo;
            set => Set(ref _photo, value);
        }

        [JsonIgnore]
        public BitmapImage PhotoBitmapImage
        {
            get => new BitmapImage(Photo);
        }
        public string ApiKey
        {
            get => _apiKey;
            set => Set(ref _apiKey, value);
        }
        public string DownloadLoc
        {
            get => _DownloadLoc;
            set => Set(ref _DownloadLoc, value);
        }
        public UserViewModel()
        {
        }
    }
}
