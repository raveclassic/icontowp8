using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Iconto.PCL.Common;
using Iconto.PCL.Services.Data;
using Iconto.PCL.Services.Data.REST.Entities;
using Iconto.PCL.Services.Data.REST.Responses;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Services.Navigation;
using Iconto.PCL.Services.Settings;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iConto.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class IcontoLoginViewModel : ViewModelBase
    {
        private readonly IDataService DataService;
        private readonly ISettingsService settingsService;

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

        #region Sid
        private string sid;
        public string Sid
        {
            get
            {
                return sid;
            }
            set
            {
                if (sid != value)
                {
                    sid = value;
                    RaisePropertyChanged(() => Sid);
                }
            }
        }
        #endregion

        #region Login

        private string _login = "79043373051";
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    RaisePropertyChanged(() => Login);
                    AuthorizeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Password

        private string _password = "1234";
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged(() => Password);
                    AuthorizeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region AuthorizeCommand

        private AsyncRelayCommand _authorizeCommand;

        private bool canExecuteAuthorizeCommand()
        {
            return !String.IsNullOrEmpty(Login) && !String.IsNullOrEmpty(Password) && !AuthorizeCommand.IsExecuting;
        }

        public AsyncRelayCommand AuthorizeCommand
        {
            get
            {
                return _authorizeCommand ?? (_authorizeCommand = new AsyncRelayCommand(async () =>
                {
                    //var sessionResponse = await DataService.GetAsync<CommonResponse<Session>>("session");

                    //AuthorizeCommand.ReportProgress(() =>
                    //{
                    //    settingsService.Set("ICONTO_API_SID", sessionResponse.Data.Id);
                    //    Sid = sessionResponse.Data.Id;
                    //});

                    var payload = new Dictionary<string, object>() { 
                            {"login", Login},
                            {"password", Password}
                        };

                    var authResponse = await DataService.PostAsync<CommonResponse<AuthResponse>>("auth", payload);
                    if (authResponse.Status == 0)
                    {
                        var userResponse = await DataService.GetAsync<CommonResponse<User>>("user/current");

                        if (userResponse.Status == 0)
                        {
                            var name = getUserName(userResponse.Data);
                            AuthorizeCommand.ReportProgress(() =>
                            {
                                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                            });
                        }
                        else
                        {
                            AuthorizeCommand.ReportProgress(() =>
                            {
                                DialogService.ShowMessage(userResponse.Message, "Ошибка " + userResponse.Status);
                            });
                        }
                    }
                    else
                    {
                        AuthorizeCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(authResponse.Message, "Ошибка " + authResponse.Status);
                        });
                    }
                }, canExecuteAuthorizeCommand));
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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public IcontoLoginViewModel(IDataService dataService, ISettingsService settingsService)
        {
            this.DataService = dataService;
            this.settingsService = settingsService;
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}

        #region NavigatedToCommand

        private RelayCommand navigatedToCommand;
        public RelayCommand NavigatedToCommand
        {
            get
            {
                return navigatedToCommand ?? (navigatedToCommand = new RelayCommand(() =>
                {                    
                }));
            }
        }

        #endregion

        #region NavigatedFromCommand

        private RelayCommand navigatedFromCommand;
        public RelayCommand NavigatedFromCommand
        {
            get
            {
                return navigatedFromCommand ?? (navigatedFromCommand = new RelayCommand(() =>
                {

                }));
            }
        }

        #endregion

    }
}