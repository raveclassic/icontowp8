using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Stores.Settings
{
    public interface ISettingsStore
    {
        string Sid { get; set; }
        string Login { get; set; }
        string Password { get; set; }

        bool SubscribeToPushNotifications { get; set; }
        bool LocationServiceAllowed { get; set; }
        bool BackgroundTasksAllowed { get; set; }
        event EventHandler UserChanged;

        void Set(string key, object value);
        object Get(string key, object defaultValue = null);
        void Remove(string key);
        void Save();
    }
}
