﻿using eBookReader.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace eBookReader.ViewModels
{
    public class LoginViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goToSignUp;

        public ICommand GoToSignUp
        {
            get
            {
                return _goToSignUp ?? (_goToSignUp = new RelayCommand(x =>
                {
                    Mediator.Notify("GoToSignUpScreen", "");
                }));
            }
        }
    }
}
