using eBookReader.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace eBookReader.ViewModels
{
    public class LoginViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goTo2;

        public ICommand GoTo2
        {
            get
            {
                return _goTo2 ?? (_goTo2 = new RelayCommand(x =>
                {
                    Mediator.Notify("GoTo2Screen", "");
                }));
            }
        }
    }
}
