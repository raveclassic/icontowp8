using GalaSoft.MvvmLight;
using Iconto.PCL.Clients.REST;
using Iconto.PCL.Clients.REST.Entities;
using Iconto.PCL.Common;
using Iconto.PCL.Exceptions;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Services.Navigation;
using Iconto.WRTTO.Model;
using Microsoft.Practices.ServiceLocation;
using System;

namespace Iconto.WRTTO.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IRESTClient RESTClient;

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

        private string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }
       
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IRESTClient restClient)
        {
            RESTClient = restClient;
        }

        #region NavigatedToCommand
        private AsyncRelayCommand navigatedToCommand;

        public AsyncRelayCommand NavigatedToCommand
        {
            get
            {
                return navigatedToCommand ?? (navigatedToCommand = new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        var user = await RESTClient.GetJSON<User>("user/current");

                        NavigatedToCommand.ReportProgress(() =>
                        {
                            Name = String.Format("{0} {1}", user.FirstName, user.LastName);
                        });
                    }
                    catch (ApiException ae)
                    {
                        NavigatedToCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(ae.Message, string.Format("Ошибка {0}", ae.Status));
                        });
                    }
                }));
            }
        }

        #endregion

        #region LogoutCommand

        private AsyncRelayCommand logoutCommand;

        private bool CanExecuteLogoutCommand()
        {
            return !LogoutCommand.IsExecuting;
        }

        public AsyncRelayCommand LogoutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        var logoutResponse = await RESTClient.DeleteJSON("auth");

                        LogoutCommand.ReportProgress(() =>
                        {
                            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                        });
                    }
                    catch (ApiException ae)
                    {
                        LogoutCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(ae.Message, string.Format("Ошибка {0}", ae.Status));
                        });
                    }
                }, CanExecuteLogoutCommand));
            }
        }

        #endregion
    }
}