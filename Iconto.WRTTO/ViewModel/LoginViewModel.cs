using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Iconto.PCL.Clients.REST;
using Iconto.PCL.Common;
using Iconto.PCL.Exceptions;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Services.Navigation;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Diagnostics;

namespace Iconto.WRTTO.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {

        private IRESTClient RESTClient { get; set; }

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
                }
            }
        }

        #endregion

        #region AuthorizeCommand

        private AsyncRelayCommand authorizeCommand;
        private bool CanExecuteAuthorizeCommand()
        {
            return true;
        }
        public AsyncRelayCommand AuthorizeCommand
        {
            get
            {
                return authorizeCommand ?? (authorizeCommand = new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        var authResponse = await RESTClient.PostJSON("auth", new { login = Login, password = Password });

                        AuthorizeCommand.ReportProgress(() =>
                        {
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        });
                    }
                    catch (ApiException ex)
                    {
                        AuthorizeCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(ex.Message, String.Format("Ошибка {0}", ex.Status));
                        });
                    }

                }, CanExecuteAuthorizeCommand));
            }
        }

        #endregion

        /// <summar>y
        /// Initializes a new instance of the LoginViewModel class.
        /// </summary>
        public LoginViewModel(IRESTClient restClient)
        {
            RESTClient = restClient;
        }
    }
}