using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iConto.Model;
using iConto.Model.REST.Entities;
using iConto.Model.REST.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Messaging;
using iConto.Messages;
using System.Diagnostics;
using Iconto.PCL.Services.Settings;
using Iconto.PCL.Services.Dialog;
using Iconto.PCL.Common;

namespace iConto.ViewModel.Wallet
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class WalletCardsLayoutViewModel : ViewModelBase
    {
        private IDataService DataService { get; set; }
        private ISettingsService SettingsService { get; set; }

        private IDialogService DialogService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogService>();
            }
        }        

        /// <summary>
        /// Initializes a new instance of the WalletViewModel class.
        /// </summary>
        public WalletCardsLayoutViewModel(IDataService dataService, ISettingsService settingsService)
        {
            DataService = dataService;
            SettingsService = settingsService;
            BankCards = new ObservableRangeCollection<Card>();
            CashCards = new ObservableRangeCollection<Card>();
            NewCashCard = new Card();

#if DEBUG
            if (IsInDesignMode)
            {
                var testBankCard = new Card()
                {
                    Type = CardType.Bank,
                    CardNumber = "XXXX XXXX XXXX 1234",
                    BankId = 7,
                    Bank = new Bank()
                    {
                        Id = 7,
                        Name = "Альфа банк"
                    },
                    Balance = 341.2
                };
                BankCards.Add(testBankCard);
                BankCards.Add(testBankCard);

                var testCashCard = new Card()
                {
                    Type = CardType.Cash,
                    Title = "На куртизанок",
                    Balance = 342.1,
                    UpdatedAt = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
                };
                CashCards.Add(testCashCard);
                CashCards.Add(testCashCard);

                IcontoWallet = new iConto.Model.REST.Entities.Wallet()
                {
                    Balance = 4324234.34
                };
                IcontoCashback = new Card()
                {
                    Balance = 3423.52
                };
            }
#endif
        }

        #region iConto.Wallet
        private iConto.Model.REST.Entities.Wallet icontoWallet;
        public iConto.Model.REST.Entities.Wallet IcontoWallet
        {
            get
            {
                return icontoWallet;
            }
            set
            {
                if (icontoWallet != value)
                {
                    icontoWallet = value;
                    RaisePropertyChanged(() => IcontoWallet);
                }
            }
        }
        #endregion

        #region iConto.Cashback
        private Card icontoCashback;
        public Card IcontoCashback
        {
            get
            {
                return icontoCashback;
            }
            set
            {
                if (icontoCashback != value)
                {
                    icontoCashback = value;
                    RaisePropertyChanged(() => IcontoCashback);
                }
            }
        }
        #endregion

        #region BankCards

        private ObservableRangeCollection<Card> _bankCards;
        public ObservableRangeCollection<Card> BankCards
        {
            get
            {
                return _bankCards;
            }
            set
            {
                if (_bankCards != value)
                {
                    _bankCards = value;
                    RaisePropertyChanged(() => BankCards);
                }
            }
        }

        #endregion

        #region CashCards

        private ObservableRangeCollection<Card> _cashCards;
        public ObservableRangeCollection<Card> CashCards
        {
            get
            {
                return _cashCards;
            }
            set
            {
                if (_cashCards != value)
                {
                    _cashCards = value;
                    RaisePropertyChanged(() => CashCards);
                }
            }
        }

        #endregion

        #region LoadIcontoWalletCommand

        private AsyncRelayCommand loadIcontoWalletCommand;
        private bool CanLoadIcontoWalletCommand()
        {
            return !LoadIcontoWalletCommand.IsExecuting;
        }
        public AsyncRelayCommand LoadIcontoWalletCommand
        {
            get
            {
                return loadIcontoWalletCommand ?? (loadIcontoWalletCommand = new AsyncRelayCommand(async () =>
                {
                    var walletIdsResponse = await DataService.GetAsync<CommonArrayResponse<long>>("wallet");
                    var wallets = await DataService.FindMany<iConto.Model.REST.Entities.Wallet>(walletIdsResponse.Data.Items.ToArray());
                    var wallet = wallets.First();

                    LoadIcontoWalletCommand.ReportProgress(() =>
                    {
                        IcontoWallet = wallet;
                    });

                }, CanLoadIcontoWalletCommand));
            }
        }

        #endregion

        #region LoadIcontoCashBackCommand

        private AsyncRelayCommand loadIcontoCashBackCommand;
        private bool CanLoadIcontoCashBackCommand()
        {
            return !LoadIcontoCashBackCommand.IsExecuting;
        }
        public AsyncRelayCommand LoadIcontoCashBackCommand
        {
            get
            {
                return loadIcontoCashBackCommand ?? (loadIcontoCashBackCommand = new AsyncRelayCommand(async () =>
                {
                    var userResponse = await DataService.GetAsync<CommonResponse<User>>("user/current");

                    LoadIcontoCashBackCommand.ReportProgress(() =>
                    {
                        IcontoCashback = new Card()
                        {
                            Balance = userResponse.Data.Balance
                        };
                    });

                }, CanLoadIcontoCashBackCommand));
            }
        }

        #endregion

        #region LoadCardsCommand

        private AsyncRelayCommand loadCardsCommand;
        private bool CanLoadCardsCommand()
        {
            return !LoadCardsCommand.IsExecuting;
        }
        public AsyncRelayCommand LoadCardsCommand
        {
            get
            {
                return loadCardsCommand ?? (loadCardsCommand = new AsyncRelayCommand(async () =>
                {
                    var query = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("blocked", "false"),
                        new KeyValuePair<string, string>("activated", "true"),
                    };

                    var cardIdsResponse = await DataService.GetAsync<CommonArrayResponse<long>>("card", query);
                    var cards = await DataService.FindMany<Card>(cardIdsResponse.Data.Items.ToArray());

                    var bankIds = cards.Select((card) => card.BankId).Distinct().ToList().FindAll((id) => id != 0);

                    var banks = await DataService.FindMany<Bank>(bankIds.ToArray());

                    foreach (var bank in banks)
                    {
                        var card = cards.Find((c) => c.BankId == bank.Id);
                        card.Bank = bank;
                    }

                    var bankCards = cards.FindAll((c) => c.Type == CardType.Bank);
                    var cashCards = cards.FindAll((c) => c.Type == CardType.Cash);

                    LoadCardsCommand.ReportProgress(() =>
                    {
                        CashCards.ReplaceRange(cashCards);
                        BankCards.ReplaceRange(bankCards);
                    });

                }, CanLoadCardsCommand));
            }
        }

        #endregion

        #region NavigatedToCommand

        private RelayCommand navigatedToCommand;
        public RelayCommand NavigatedToCommand
        {
            get
            {
                return navigatedToCommand ?? (navigatedToCommand = new RelayCommand(() =>
                {
                    LoadCardsCommand.Execute(null);
                    LoadIcontoWalletCommand.Execute(null);
                    LoadIcontoCashBackCommand.Execute(null);
                }));
            }
        }

        #endregion

        #region NewCashCard
        public Card NewCashCard { get; set; }
        #endregion
        #region CreateCashCardCommand
        private AsyncRelayCommand createCashCardCommand;
        private bool CanCreateCashCardCommand()
        {
            return !CreateCashCardCommand.IsExecuting;
        }
        public AsyncRelayCommand CreateCashCardCommand
        {
            get {
                return createCashCardCommand ?? (createCashCardCommand = new AsyncRelayCommand(async () =>
                {
                    var balance = NewCashCard.Balance;
                    var title = NewCashCard.Title;
                    CreateCashCardCommand.ReportProgress(() =>
                    {
                        NewCashCard = new Card();
                    });
                    
                    var data = new Dictionary<string, object>()
                    {
                        { "title", title },
                        { "type", 1 }
                    };

                    var response = await DataService.PostAsync<CommonResponse<CreateCashCardResponse>>("card", data);
                    if (response.Status == 0)
                    {
                        var cash = new Card()
                        {
                            Type = CardType.Cash,
                            Id = response.Data.Id,
                            Title = title,
                            Balance = 0
                        };

                        CreateCashCardCommand.ReportProgress(() =>
                        {
                            CashCards.Add(cash);
                        });
                    }
                    else
                    {
                        CreateCashCardCommand.ReportProgress(() =>
                        {
                            DialogService.ShowMessage(response.Message, "Ошибка " + response.Status);
                        });
                    }
                    
                }, CanCreateCashCardCommand));
            }
        }
        #endregion

        #region DeleteCashCardCommand
        private AsyncRelayCommand deleteCashCardCommand;
        public AsyncRelayCommand DeleteCashCardCommand
        {
            get
            {
                return deleteCashCardCommand ?? (deleteCashCardCommand = new AsyncRelayCommand(async (card) =>
                {
                    Debugger.Log(0, "TEST", "got delete cash card comand");
                }));
            }
        }
        #endregion

        #region CreateOrderCommand
        private AsyncRelayCommand createOrderCommand;
        public AsyncRelayCommand CreateOrderCommand
        {
            get
            {
                return createOrderCommand ?? (createOrderCommand = new AsyncRelayCommand(async (redirectUrl) =>
                {
                    var data = new Dictionary<string, object>()
                    {
                        { "type", 1 }
                    };
                    if (redirectUrl != null)
                    {
                        data.Add("redirect_url", redirectUrl);
                    }

                    var response = await DataService.PostAsync<CommonResponse<CreateOrderResponse>>("order", data);

                    CreateOrderCommand.ReportProgress(() =>
                    {
                        if (response.Status == 0)
                        {
                            var sid = (string)SettingsService.Get("ICONTO_API_SID");
                            Messenger.Default.Send<PaymentRequestMessage>(new PaymentRequestMessage(response.Data, sid));
                        }
                        else
                        {
                            DialogService.ShowMessage(response.Message, "Ошибка " + response.Status);
                        }
                    });
                }));
            }
        }
        #endregion
    }
}