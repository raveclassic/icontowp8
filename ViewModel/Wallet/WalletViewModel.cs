using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iConto.Common;
using iConto.Model;
using iConto.Model.REST.Entities;
using iConto.Model.REST.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace iConto.ViewModel.Wallet
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class WalletViewModel : ViewModelBase
    {
        private IDataService DataService { get; set; }

        /// <summary>
        /// Initializes a new instance of the WalletViewModel class.
        /// </summary>
        public WalletViewModel(IDataService dataService)
        {
            DataService = dataService;
            BankCards = new ObservableCollection<Card>();
            CashCards = new ObservableCollection<Card>();
        }

        #region BankCards

        private ObservableCollection<Card> bankCards;
        public ObservableCollection<Card> BankCards
        {
            get
            {
                return bankCards;
            }
            set
            {
                if (bankCards != value)
                {
                    bankCards = value;
                    RaisePropertyChanged(() => BankCards);
                }
            }
        }

        #endregion

        #region CashCards

        private ObservableCollection<Card> cashCards;
        public ObservableCollection<Card> CashCards
        {
            get
            {
                return cashCards;
            }
            set
            {
                if (cashCards != value)
                {
                    cashCards = value;
                    RaisePropertyChanged(() => CashCards);
                }
            }
        }

        #endregion

        #region LoadIcontoCardsCommand

        private AsyncRelayCommand loadIcontoCardsCommand;
        private bool CanLoadIcontoCardsCommand()
        {
            return !LoadIcontoCardsCommand.IsExecuting;
        }
        public AsyncRelayCommand LoadIcontoCardsCommand
        {
            get {
                return loadIcontoCardsCommand ?? (loadIcontoCardsCommand = new AsyncRelayCommand(async () =>
                {
                    
                }, CanLoadIcontoCardsCommand));
            }
        }

        #endregion

        #region LoadBankCardsCommand

        private AsyncRelayCommand loadBankCardsCommand;
        private bool CanLoadBankCardsCommand()
        {
            return !LoadBankCardsCommand.IsExecuting;
        }
        public AsyncRelayCommand LoadBankCardsCommand
        {
            get
            {
                return loadBankCardsCommand ?? (loadBankCardsCommand = new AsyncRelayCommand(async () =>
                {
                    var query = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("blocked", "false"),
                        new KeyValuePair<string, string>("activated", "true"),
                    };
                    
                    var cardIdsResponse = await DataService.GetAsync<CommonArrayResponse<long>>("card", query);
                    var cards = await DataService.FindMany<Card>(cardIdsResponse.Data.Items.ToArray());

                    var bankIds = cards.Select((card) => card.BankId).Distinct();

                    var banks = await DataService.FindMany<Bank>(bankIds.ToArray());

                    foreach (var bank in banks)
                    {
                        var card = cards.Find((c) => c.BankId == bank.Id);
                        card.Bank = bank;
                    }

                    var bankCards = cards.FindAll((c) => c.Type == 0);
                    var cashCards = cards.FindAll((c) => c.Type == 1);

                    LoadBankCardsCommand.ReportProgress(() =>
                    {
                        foreach (var card in bankCards)
                        {
                            BankCards.Add(card);
                        }
                        foreach (var card in cashCards)
                        {
                            CashCards.Add(card);
                        }
                    });
                    
                    
                }, CanLoadBankCardsCommand));
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
                    LoadIcontoCardsCommand.Execute(null);
                    LoadBankCardsCommand.Execute(null);
                }));
            }
        }

        #endregion
    }
}