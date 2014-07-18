using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Iconto.PCL.Clients.REST;
using Iconto.PCL.Common;
using Iconto.PCL.Exceptions;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Services.Navigation;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

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
        //private string _login = "79312066986";
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
        public AsyncRelayCommand AuthorizeCommand
        {
            get
            {
                return authorizeCommand ?? (authorizeCommand = new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        await RESTClient.Post("auth", new { login = Login, password = Password });

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

                }));
            }
        }

        #endregion

        #region SignupCommand

        private AsyncRelayCommand signupCommand;
        public AsyncRelayCommand SignupCommand
        {
            get
            {
                return signupCommand ?? (signupCommand = new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        await RESTClient.Post("user", new { login = Login });

                        SignupCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage("На ваш номер выслан пароль", "Регистрация успешно завершена");
                            MessengerInstance.Send<NotificationMessage>(null, "signup:complete");
                        });
                    }
                    catch (ApiException ex)
                    {
                        SignupCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(ex.Message, String.Format("Ошибка {0}", ex.Status));
                        });
                    }

                }));
            }
        }

        #endregion

        #region RestorePasswordCommand

        private AsyncRelayCommand restorePasswordCommand;
        public AsyncRelayCommand RestorePasswordCommand
        {
            get
            {
                return restorePasswordCommand ?? (restorePasswordCommand = new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        var request = new Dictionary<string, string>();
                        request.Add("event", "on_change_password");
                        request.Add("login", Login);
                        await RESTClient.Put("confirmation-code", request);

                        RestorePasswordCommand.ReportProgress(() =>
                        {
                            DialogService.Prompt("Введите код подтверждения", "", (code) =>
                            {
                                Task.Run(async () =>
                                {
                                    try
                                    {
                                        await RESTClient.Put("auth", new { login = Login, smscode = code });

                                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                                        {
                                            DialogService.ShowMessage("Вам выслан новый пароль", "Успешно");
                                        });
                                    }
                                    catch (ApiException ex)
                                    {
                                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                                        {
                                            DialogService.ShowMessage(ex.Message, String.Format("Ошибка {0}", ex.Status));
                                        });
                                    }
                                });
                                
                            });
                        });
                    }
                    catch (ApiException ex)
                    {
                        RestorePasswordCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(ex.Message, String.Format("Ошибка {0}", ex.Status));
                        });
                    }

                }));
            }
        }

        #endregion

        #region ShowTermsCommand

        private RelayCommand showTermsCommand;
        public RelayCommand ShowTermsCommand
        {
            get
            {
                return showTermsCommand ?? (showTermsCommand = new RelayCommand(() =>
                {
                    var webBrowserTask = new WebBrowserTask();

                    webBrowserTask.Uri = new Uri("https://iconto.net/terms", UriKind.Absolute);

                    webBrowserTask.Show();   
                }));
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