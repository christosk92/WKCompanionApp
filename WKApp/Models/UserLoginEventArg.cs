using System;

namespace WKApp.Models
{
    public class UserLoginEventArg : EventArgs
    {
        public UserLoginEventArg(ViewModels.UserViewModel _user)
        {
            User = _user;
        }

        public ViewModels.UserViewModel User { get; set; }
    }
}
