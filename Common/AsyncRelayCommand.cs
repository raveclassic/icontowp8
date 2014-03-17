using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace iConto.Common
{
    public class AsyncRelayCommand : RelayCommand, INotifyPropertyChanged
    {
        private Func<Task> asyncExecute;

        public AsyncRelayCommand(Action execute, Func<bool> canExecute) : base(execute, canExecute) { }
        public AsyncRelayCommand(Action execute) : base(execute) { }

        public AsyncRelayCommand(Func<Task> asyncExecute)
            : base(() => asyncExecute())
        {
            this.asyncExecute = asyncExecute;
        }
        public AsyncRelayCommand(Func< Task> asyncExecute, Func<bool> canExecute)
            : base(() => asyncExecute(), canExecute)
        {
            this.asyncExecute = asyncExecute;
        }

        #region IsExecuting

        private bool isExecuting = false;
        public bool IsExecuting
        {
            get
            {
                return isExecuting;
            }
            set
            {
                if (isExecuting != value)
                {
                    isExecuting = value;
                    NotifyPropertyChanged("IsExecuting");
                    RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public override void Execute(object parameter)
        {
            if (IsExecuting) return;

            if (!CanExecute(null)) return;

            IsExecuting = true;

            Task.Run(async () =>
            {
                await asyncExecute();
                ReportProgress(() =>
                {
                    IsExecuting = false;
                });
            });
        }

        public void ReportProgress(Action action)
        {
            if (IsExecuting)
            {
                Deployment.Current.Dispatcher.BeginInvoke(action);                
            }
        }
    }
}
