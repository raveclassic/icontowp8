using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iConto.ViewModel.Wallet;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Messaging;
using iConto.Messages;
using System.Diagnostics;
using System.Threading.Tasks;
using Iconto.PCL.Common;

namespace iConto.Wallet.Cards
{
    public partial class CardsLayout : BasePage
    {
        private WalletCardsLayoutViewModel VM { get; set; }

        // Constructor
        public CardsLayout()
        {
            InitializeComponent();
            VM = (WalletCardsLayoutViewModel)DataContext;
            
            ChangeAppBar(Pivot.Items.First() as PivotItem);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Register<PaymentRequestMessage>(this, HandlePaymentRequestMessage);
            base.OnNavigatedTo(e);
            VM.NavigatedToCommand.Execute(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Messenger.Default.Unregister<PaymentRequestMessage>(this, HandlePaymentRequestMessage);
            base.OnNavigatedFrom(e);
        }

        private void HandlePaymentRequestMessage(PaymentRequestMessage message)
        {            
            var formUrl = message.OrderResponse.FormUrl;
            formUrl = formUrl.Replace("iconto.net", "dev.iconto.net");
            formUrl += (formUrl.Contains("?") ? "&" : "?") + "sid=" + message.Sid;

            Debugger.Log(0, "PAYMENT", "Opening " + formUrl);

            var popup = new PaymentRequestMessageBox(this.ActualHeight + 1, formUrl);
            popup.PaymentCompleted += (sender, args) =>
            {
                Debugger.Log(0, "PAYMENT", "Payment completed");
            };

            popup.Show();

            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;            
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeAppBar(Pivot.SelectedItem as PivotItem);
        }

        private void ChangeAppBar(PivotItem item)
        {
            switch (item.Name)
            {
                case "CashCards":
                    this.ApplicationBar = this.Resources["CashCardsAppBar"] as ApplicationBar;
                    break;
                case "BankCards":
                    this.ApplicationBar = this.Resources["BankCardsAppBar"] as ApplicationBar;
                    break;
                case "IcontoCards":
                    this.ApplicationBar = null;
                    //this.ApplicationBar = this.Resources["IcontoCardsAppBar"] as ApplicationBar;
                    break;
            }
        }

        private void BankCardsAppBarAddButtonClick(object sender, EventArgs e)
        {
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
            
            VM.CreateOrderCommand.Execute("http://localhost");
        }
        private void CashCardsAppBarAddButtonClick(object sender, EventArgs e)
        {
            var popup = new CustomMessageBox()
            {
                Caption = "Создание наличного кошелька",
                ContentTemplate = (DataTemplate)Resources["NewBackCardPopup"],
                LeftButtonContent = "ОК",
                RightButtonContent = "Cancel"
            };
            popup.Dismissed += (p, args) =>
            {
                var res = args.Result;
                if (args.Result == CustomMessageBoxResult.LeftButton)
                {
                    VM.CreateCashCardCommand.Execute(popup);
                }
            };
            popup.Show();
        }

        private void NewCashCardTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            VM.NewCashCard.Title = (sender as TextBox).Text;
        }

        private void NewCashCardBalance_TextChanged(object sender, TextChangedEventArgs e)
        {
                       
        }

        private void StackPanel_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var card = (sender as FrameworkElement).DataContext;
            VM.DeleteCashCardCommand.Execute(card);
        }
    }
}