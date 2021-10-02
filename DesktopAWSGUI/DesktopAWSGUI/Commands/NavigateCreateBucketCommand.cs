using DesktopAWSGUI.Stores;
using DesktopAWSGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAWSGUI.Commands
{
    class NavigateCreateBucketCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        public NavigateCreateBucketCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new CreateBucketViewModel();
        }
    }
}
