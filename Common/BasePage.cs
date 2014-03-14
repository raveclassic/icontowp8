using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using iConto.Services.Dialog;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iConto.Common
{
    public partial class BasePage : PhoneApplicationPage, IDialogService
    {
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Messenger.Default.Register<DialogMessage>(this, HandleDialogMessage);
            SimpleIoc.Default.Register<IDialogService>(() => this);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            Messenger.Default.Unregister<DialogMessage>(this, HandleDialogMessage);
            SimpleIoc.Default.Unregister<IDialogService>();
            base.OnNavigatedFrom(e);
        }

        protected void HandleDialogMessage(DialogMessage message)
        {
            ShowMessage(message.Content, message.Caption);
        }
    }
}
