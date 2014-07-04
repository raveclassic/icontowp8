using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Iconto.PCL.Common;
using Iconto.PCL.Services.Data;
using Iconto.PCL.Services.Data.REST.Entities;
using Iconto.PCL.Services.Data.REST.Responses;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Services.Navigation;
using Iconto.PCL.Services.Settings;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;

namespace iConto.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SplashScreenViewModel : ViewModelBase
    {
        private ISettingsService SettingsService { get; set; }
        private IDataService DataService { get; set; }

        private IDialogService DialogService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogService>();
            }
        }
        private INavigationService NavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the SplashScreenViewModel class.
        /// </summary>
        public SplashScreenViewModel(ISettingsService settingsService, IDataService dataService)
        {
            SettingsService = settingsService;
            DataService = dataService;
        }

        #region NavigatedToCommand

        private AsyncRelayCommand navigatedToCommand;
        public AsyncRelayCommand NavigatedToCommand
        {
            get
            {
                return navigatedToCommand ?? (navigatedToCommand = new AsyncRelayCommand(async () =>
                {
                    //the initial request over entire application
                    var sessionResponse = await DataService.GetAsync<CommonResponse<Session>>("session");

                    var userResponse = await DataService.GetAsync<CommonResponse<User>>("user/current");

                    if (userResponse.Status == 0)
                    {
                        //authorized
                        var name = getUserName(userResponse.Data);
                        
                        NavigatedToCommand.ReportProgress(() =>
                        {
                            PhoneApplicationService.Current.State.Add("user", userResponse.Data);
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        });
                    }
                    else
                    {
                        //unauthorized
                        NavigatedToCommand.ReportProgress(() =>
                        {
                            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                        });
                    }
                    //var sid = SettingsService.Load("ICONTO_API_SID");
                }));
            }
        }

        #endregion

        private static string getUserName(User user)
        {
            var name = user.Id.ToString();

            if (!String.IsNullOrEmpty(user.FirstName) || !String.IsNullOrEmpty(user.LastName))
            {
                name = String.Join(" ", user.FirstName, user.LastName);
            }
            else
            {
                if (!String.IsNullOrEmpty(user.Nickname))
                {
                    name = user.Nickname;
                }
            }
            return name;
        }
    }
}