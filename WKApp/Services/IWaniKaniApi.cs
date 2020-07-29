using Refit;
using System.Threading.Tasks;
using WKApp.Models;

namespace WKApp.Services
{
    public interface IWaniKaniApi
    {
        [Get("/user")]
        Task<WaniKaniUser> GetUserInfo();
    }
}
