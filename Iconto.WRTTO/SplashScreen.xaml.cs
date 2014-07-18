using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Iconto.WRTTO.Resources;
using Iconto.PCL.Common;
using Iconto.WRTTO.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Iconto.PCL.Clients.REST;
using System.Threading.Tasks;
using Iconto.PCL.Stores.Settings;
using Iconto.PCL.Exceptions;
using Microsoft.Practices.ServiceLocation;

namespace Iconto.WRTTO
{
    public partial class SplashScreen : BasePage
    {
        // Constructor
        public SplashScreen()
        {
            InitializeComponent();
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //check authorization
            Task.Run(async () =>
            {
                var serviceLocator = ServiceLocator.Current;
                var settingsStore = serviceLocator.GetInstance<ISettingsStore>();
                var restClient = serviceLocator.GetInstance<IRESTClient>();

                var route = @"/MainPage.xaml";

                if (String.IsNullOrEmpty(settingsStore.Sid))
                {
                    //first launch - no sid found in application settings
                    //go to Login
                    route = @"/Login.xaml";
                }
                else
                {
                    try
                    {
                        await restClient.Get("user/current");
                        //success - go to main
                    }
                    catch (ApiException ex)
                    {
                        //unauthorized
                        //go to login
                        route = @"/Login.xaml";
                    }
                }

                Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    NavigationService.Navigate(new Uri(route, UriKind.Relative));
                }));


            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigationService.RemoveBackEntry();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}