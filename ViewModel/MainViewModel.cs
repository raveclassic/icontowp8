using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
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
        private readonly IDataService _dataService;

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
                }
            }
        }

        private RelayCommand _authorizeCommand;
        public RelayCommand AuthorizeCommand
        {
            get
            {
                return _authorizeCommand ?? (_authorizeCommand = new RelayCommand(() =>
                {
                    _dataService.GetAsync<Session>("session").ContinueWith(async (t) =>
                    {
                        var payload = new Dictionary<string, string>() { 
                            { "login", Login },
                            { "password",  Password }
                        };

                        var authResponse = await _dataService.PostAsync<CommonResponse<AuthResponse>>("auth", payload);
                        if (authResponse.Status == 0)
                        {
                            //DialogService.ShowMessage("Успешно!", "success");
                            try
                            {
                                var user = await _dataService.FindOne<User>("current");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            //var userResponse = await _dataService.GetAsync<CommonResponse<User>>("user/current"
                        }
                        else
                        {
                            //DialogService.ShowMessage(authResponse.Message, "Ошибка " + authResponse.Status);
                        }
                    });
                }));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;  
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}