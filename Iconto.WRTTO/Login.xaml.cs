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

namespace Iconto.WRTTO
{
    public partial class Login : BasePage
    {
		private LoginViewModel VM { get; set; }
		
        // Constructor
        public Login()
        {
            InitializeComponent();
            VM = (LoginViewModel)DataContext;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //this.ClearHistory();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigationService.RemoveBackEntry();
        }

		private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.Password = ((TextBox)sender).Text;
        }

        private void LoginTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        	VM.Login = ((TextBox)sender).Text;
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