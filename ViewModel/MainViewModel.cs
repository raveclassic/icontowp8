using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using iConto.Common;
using iConto.Model;
using iConto.Model.REST.Entities;
using iConto.Model.REST.Responses;
using iConto.Model.Serializer;
using iConto.Services.Dialog;
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
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService dataService;

        private IDialogService DialogService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogService>();
            }
        }

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

        private AsyncRelayCommand _authorizeCommand;
        public AsyncRelayCommand AuthorizeCommand
        {
            get
            {
                return _authorizeCommand ?? (_authorizeCommand = new AsyncRelayCommand(async () =>
                {
                    var sessionResponse = await dataService.GetAsync<CommonResponse<Session>>("session");

                    var payload = new Dictionary<string, string>() { 
                            { "login", Login },
                            { "password",  Password }
                        };

                    var authResponse = await dataService.PostAsync<CommonResponse<AuthResponse>>("auth", payload);
                    if (authResponse.Status == 0)
                    {
                        var userResponse = await dataService.GetAsync<CommonResponse<User>>("user/current");

                        if (userResponse.Status == 0)
                        {
                            var name = getUserName(userResponse.Data);
                            AuthorizeCommand.ReportProgress(() =>
                            {
                                DialogService.ShowMessage("Привет, " + name, "success");
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
                }));
            }
        }

        //private RelayCommand __authorizeCommand;
        //public RelayCommand __AuthorizeCommand
        //{
        //    get
        //    {
        //        return __authorizeCommand ?? (__authorizeCommand = new RelayCommand(() =>
        //        {
        //            Task.Run(async () =>
        //            {
                        

        //            });
        //        }));
        //    }
        //}

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
        public MainViewModel(IDataService dataService)
        {
            this.dataService = dataService;  
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}