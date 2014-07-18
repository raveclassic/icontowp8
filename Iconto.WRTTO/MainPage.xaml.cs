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
using System.Threading.Tasks;
using System.Windows.Media;

namespace Iconto.WRTTO
{
    public partial class MainPage : BasePage
    {
        private MainViewModel VM { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            VM = (MainViewModel)DataContext;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            VM.NavigatedToCommand.Execute(null);
        }

        private void BuildApplicationBar()
        {
            var bar = new ApplicationBar()
            {
                IsVisible = false,
                //BackgroundColor = Color.FromArgb(0xFF, 0x4a, 0x39, 0x6e),
                //ForegroundColor = Colors.White,
                IsMenuEnabled = true,
                Opacity = 0.5f
            };

            var searchButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/search.png", UriKind.Relative))
            {
                Text = "Поиск"
            };           

            bar.Buttons.Add(searchButton);

            var settingsMenuItem = new ApplicationBarMenuItem("Выход");
            settingsMenuItem.Click += (sender, args) =>
            {
                VM.LogoutCommand.Execute(null);
            };
            bar.MenuItems.Add(settingsMenuItem);

            this.ApplicationBar = bar;

            Task.Delay(500).ContinueWith((task) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ApplicationBar.IsVisible = true;
                });
            });
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