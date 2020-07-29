using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WKApp.Core.Helpers;
using WKApp.Helpers;
using WKApp.Models;
using WKApp.ViewModels;

namespace WKApp.Services
{
    internal class IdentityService
    {
        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
        public string GeneratedApiKey;
        public event EventHandler<UserLoginEventArg> LoggedIn;
        public event EventHandler<UserLoginEventArg> LoggedOut;
        public bool IsLoggedIn() => GeneratedApiKey != null;
        const string avatarNodeV1 = "//span[@class='avatar user-avatar-default']";
        public async Task<LoginResultType> LoginAsync(string apiKey = null)
        {
            if (apiKey != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UserDataService.WaniKaniApiUrl);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                    var authResponse = await client.GetAsync("v2/user");
                    if (authResponse.StatusCode == HttpStatusCode.Unauthorized ||
                          authResponse.StatusCode == HttpStatusCode.Forbidden)
                        return LoginResultType.Unauthorized;
                    if (authResponse.IsSuccessStatusCode)
                    {
                        string dataString = await authResponse.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(dataString);
                        string username = data.data.username;
                        string url = (data.data.profile_url);
                        var uri = url.Replace("www.", "");
                        string res = await TryGetPhoto(uri);
                        var _user = new UserViewModel
                        {
                            Name = username,
                            Photo = new Uri(res),
                            ApiKey = apiKey
                        };
                        GeneratedApiKey = apiKey;
                        LoggedIn?.Invoke(this, new UserLoginEventArg(_user));
                        return LoginResultType.Success;
                    }
                }
            }
            else
            {
                var x = await UserDataService.GetUserFromCacheAsync();
                GeneratedApiKey = x?.ApiKey;
            }

            return LoginResultType.Unauthorized;
        }
        public async Task<string> TryGetPhoto(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;
                HttpContent content = response.Content;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(await content.ReadAsStringAsync());
                var node = document.DocumentNode.ChildNodes[2].ChildNodes[3].ChildNodes[1].ChildNodes[7].ChildNodes[1];

                string newImg = "new-value.png";
                if (node.Attributes.Contains("style"))
                {
                    string style = node.Attributes["style"].Value;
                    string oldImg = Regex.Match(style, @"(?<=\().+?(?=\))").Value;
                    newImg = node.Attributes["style"].Value.Split("&amp")[0].Split("//")[1].Replace("www.", "https://");
                }
                return newImg;
            }
        }
        public async Task Logout()
        {
            GeneratedApiKey = null;
            LoggedOut?.Invoke(this, null);
        }
    }
}
