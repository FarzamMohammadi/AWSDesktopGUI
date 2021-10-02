using DesktopAWSGUI.Services;
using DesktopAWSGUI.Stores;
using DesktopAWSGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopAWSGUI.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}