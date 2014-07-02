using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iConto.Wallet.Cards
{
    public class PaymentRequestMessageBox : CustomMessageBox
    {
        public EventHandler PaymentCompleted { get; set; }

        public PaymentRequestMessageBox(double height, string url)
            : base()
        {
            var browser = new WebBrowser()
            {
                IsScriptEnabled = true,
                Height = height,
                Source = new Uri(url)
            };

            browser.Navigating += (sender, args) =>
            {
                if (args.Uri.Host == "localhost")
                {
                    Debugger.Log(0, "TEST", "opened localhost, shutting down");
                    Dismiss();
                }
            };

            Content = browser;

            this.Dismissed += (sender, args) =>
            {
                Task.Delay(1000).ContinueWith((task) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        Debugger.Log(0, "TEST", "removing popup");
                        try
                        {
                            (this.Parent as Grid).Children.Remove(this);
                        }
                        catch (Exception e)
                        {
                        }
                    });
                });
            };
        }
    }
}
