using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private IsolatedStorageSettings storage { get; set; }

        public SettingsService()
        {
            storage = IsolatedStorageSettings.ApplicationSettings;
        }

        public void Set(string key, object value)
        {
            if (storage.Contains(key))
            {
                storage[key] = value;
            }
            else
            {
                storage.Add(key, value);
            }
        }

        public object Get(string key)
        {
            if (storage.Contains(key))
            {
                return storage[key];
            }
            else
            {
                return null;
            }
        }

        public void Unset(string key)
        {
            if (storage.Contains(key))
            {
                storage.Remove(key);
            }
        }

        public void Save()
        {
            storage.Save();
        }
    }
}
