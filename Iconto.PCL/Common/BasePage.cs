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

        public void Prompt(string message, string title, Action<string> onPrompt, Action onCancel = null)
        {
            var textBox = new TextBox();
            var messageBox = new CustomMessageBox()
            {
                Caption = title,
                LeftButtonContent = "Подтвердить",
                RightButtonContent = "Отмена",
                Message = message,
                Content = textBox
            };
            messageBox.Dismissed += (sender, args) =>
            {
                switch (args.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        onPrompt(textBox.Text);
                        // Do something.
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Do something.
                        if (onCancel != null) onCancel();
                        break;
                    case CustomMessageBoxResult.None:
                        // Do something.
                        if (onCancel != null) onCancel();
                        break;
                    default:
                        if (onCancel != null) onCancel();
                        break;
                }
            };
            messageBox.Show();
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
            while (NavigationService.CanGoBack) {
                NavigationService.RemoveBackEntry();
            }            
        }

        #endregion
    }
}
