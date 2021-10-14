using eBookReader.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace eBookReader.ViewModels
{
    class SignUpViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goToLogin;

        public ICommand GoToLogin
        {
            get
            {
                return _goToLogin ?? (_goToLogin = new RelayCommand(x =>
                {
                    Mediator.Notify("GoToLoginScreen", "");
                }));
            }
        }
    }
}
