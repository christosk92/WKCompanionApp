
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Threading.Tasks;
using WKApp.Core.Helpers;
using WKApp.Services;
using WKApp.Views;

namespace WKApp.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private string _statusMessage;
        private bool _isBusy;
        private RelayCommand _loginCommand;
        private string _userName;
        public string StatusMessage
        {
            get => _statusMessage;
            set => Set(ref _statusMessage, value);
        }
        public string UserName
        {
            get => _userName;
            set => Set(ref _userName, value);
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(OnLogin, () => !IsBusy));

        public SignInViewModel()
        {
        }

        private async void OnLogin()
        {
            IsBusy = true;
            //await Task.Delay(5000);
            StatusMessage = string.Empty;
            var loginResult = await IdentityService.LoginAsync(UserName);
            StatusMessage = loginResult.ToString();
            IsBusy = false;
        }
    }
}
