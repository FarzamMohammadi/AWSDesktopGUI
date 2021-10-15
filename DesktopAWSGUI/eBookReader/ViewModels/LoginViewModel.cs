using eBookReader.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace eBookReader.ViewModels
{
    public class LoginViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goToSignUpView;

        public ICommand GoToSignUpView
        {
            get
            {
                return _goToSignUpView ?? (_goToSignUpView = new RelayCommand(x =>
                {
                    Mediator.Notify("GoToSignUpScreen", "");
                }));
            }
        }
    }
}
