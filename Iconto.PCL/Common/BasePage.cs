using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Services.Navigation;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Iconto.PCL.Common
{
    public partial class BasePage : PhoneApplicationPage, IDialogService, INavigationService
    {
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            SimpleIoc.Default.Register<IDialogService>(() => this);
            SimpleIoc.Default.Register<INavigationService>(() => this);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            SimpleIoc.Default.Unregister<IDialogService>();
            SimpleIoc.Default.Unregister<INavigationService>();
            base.OnNavigatedFrom(e);
        }

        #region IDialogService members

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        public bool Confirm(string message, string title)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                default:
                    return false;
            }
        }

        #endregion        

        #region INavigationService members

        public void Navigate(Uri uri, Dictionary<string, string> query = null)
        {
            NavigationService.Navigate(uri);
        }

        public void GoBack()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        public void GoForward()
        {
            if (NavigationService.CanGoForward)
            {
                NavigationService.GoForward();
            }
        }

        public void ClearHistory()
        {
            try
            {
                while (true)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
            catch { }
        }

        #endregion
    }
}
