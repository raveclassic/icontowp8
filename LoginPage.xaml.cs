﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iConto.Resources;
using iConto.ViewModel;
using Iconto.PCL.Common;

namespace iConto
{
    public partial class LoginPage : BasePage
    {
        private IcontoLoginViewModel VM { get; set; }
        // Constructor
        public LoginPage()
        {
            InitializeComponent();
            VM = (IcontoLoginViewModel)DataContext;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.Login = ((TextBox)sender).Text;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.Password = ((TextBox)sender).Text;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            VM.NavigatedToCommand.Execute(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigationService.RemoveBackEntry();
            VM.NavigatedFromCommand.Execute(e);
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