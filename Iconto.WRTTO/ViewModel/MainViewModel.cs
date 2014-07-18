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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Devices.Geolocation;

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
        private const double DEFAULT_LATITUDE = 59.56f;
        private const double DEFAULT_LONGITUDE = 30.18f;

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

        private void Test<T>(T data)
        {
            if (data is IEnumerable)
            {
                Debugger.Log(0, "YES", "data is IEnumerable");
            }

            if (data is IEnumerable<object>)
            {
                Debugger.Log(0, "YES", "data is IEnumerable<object>");
            }
        }

        #region NavigatedToCommand
        private AsyncRelayCommand navigatedToCommand;

        public AsyncRelayCommand NavigatedToCommand
        {
            get
            {
                return navigatedToCommand ?? (navigatedToCommand = new AsyncRelayCommand(async () =>
                {
                    var locator = new Geolocator()
                    {
                        MovementThreshold = 100,
                        DesiredAccuracy = PositionAccuracy.High
                    };
                    double lat = DEFAULT_LATITUDE;
                    double lon = DEFAULT_LONGITUDE;
                    try
                    {
                        var position = await locator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(1), timeout: TimeSpan.FromSeconds(10));
                        lat = position.Coordinate.Latitude;
                        lon = position.Coordinate.Longitude;
                    }
                    catch (UnauthorizedAccessException e) { }

                    var query = new List<KeyValuePair<string, string>>();
                    query.Add(new KeyValuePair<string, string>("query", "macarena"));
                    var limit = 1;
                    query.Add(new KeyValuePair<string, string>("limit", limit.ToString()));
                    query.Add(new KeyValuePair<string, string>("lat", lat.ToString()));
                    query.Add(new KeyValuePair<string,string>("lon", lon.ToString()));

                    Test<List<object>>(new List<object>());

                    try
                    {
                        var addresses = await RESTClient.GetList<Address>("address", query);
                    }
                    catch (Exception e)
                    {
                        Debugger.Log(0, "error", e.Message);
                    }

                    try
                    {
                        var user = await RESTClient.Get<User>("user/current");

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
                        var logoutResponse = await RESTClient.Delete<object>("auth");

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