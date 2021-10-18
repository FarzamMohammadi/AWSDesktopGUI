using eBookReader.Commands;
using System.Collections.Generic;
using System.Linq;


namespace eBookReader.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        private void OnGo2MainScreen(object obj)
        {
            ChangeViewModel(PageViewModels[0]);
        }

        private void OnGo2SignUpScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }
        private void OnGo2ProfileScreen(object obj)
        {
            ChangeViewModel(PageViewModels[2]);
        }

        public MainWindowViewModel()
        {
            // Add available pages and set page
            PageViewModels.Add(new LoginViewModel());
            PageViewModels.Add(new SignUpViewModel());
            PageViewModels.Add(new ProfileViewModel());

            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("GoTo1Screen", OnGo2MainScreen);
            Mediator.Subscribe("GoToSignUpScreen", OnGo2SignUpScreen);
            Mediator.Subscribe("GoToProfileView", OnGo2ProfileScreen);
        }
    }
}
