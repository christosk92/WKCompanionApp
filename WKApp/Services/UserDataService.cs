using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using WKApp.Core.Helpers;
using WKApp.Helpers;
using WKApp.Models;
using WKApp.ViewModels;

namespace WKApp.Services
{
    public class UserDataService
    {
        private const string _userSettingsKey = "IdentityUser";
        public const string WaniKaniApiUrl = "https://api.wanikani.com/";
        private UserViewModel _user;
        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        public event EventHandler<UserLoginEventArg> UserDataUpdated;
        public IWaniKaniApi WaniKaniApi;

        private HttpClient _httpClient;
        public UserDataService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(WaniKaniApiUrl);
        }
        public void Initialize()
        {
            IdentityService.LoggedIn += OnLoggedIn;
            IdentityService.LoggedOut += OnLoggedOut;
        }
        private async void OnLoggedIn(object sender, UserLoginEventArg e)
        {
            _user = e.User;
            if (!string.IsNullOrEmpty(IdentityService.GeneratedApiKey))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", IdentityService.GeneratedApiKey);
                WaniKaniApi = RestService.For<IWaniKaniApi>(_httpClient);
            }
            await ApplicationData.Current.LocalFolder.SaveAsync(_userSettingsKey, e.User);
            UserDataUpdated?.Invoke(this, new UserLoginEventArg(_user));
        }
        private async void OnLoggedOut(object sender, UserLoginEventArg e)
        {
            _user = null;
            await ApplicationData.Current.LocalFolder.SaveAsync<UserViewModel>(_userSettingsKey, null);
        }
        public async Task<UserViewModel> GetUserFromCacheAsync()
        {
            if (_user != null)
                return _user;
            var cacheData = await ApplicationData.Current.LocalFolder.ReadAsync<UserViewModel>(_userSettingsKey);
            return cacheData;
        }
    }
}
