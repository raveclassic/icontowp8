using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Iconto.PCL.Services.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace iConto.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the Wallet class.
        /// </summary>
        public MainPageViewModel()
        {
        }

        private RelayCommand<string> navigateCommand;
        public RelayCommand<string> NavigateCommand
        {
            get
            {
                return navigateCommand ?? (new RelayCommand<string>((url) =>
                {
                    NavigationService.Navigate(new System.Uri(url, System.UriKind.Relative));
                }));
            }
        }
    }
}