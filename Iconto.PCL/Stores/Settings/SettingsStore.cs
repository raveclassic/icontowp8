using Iconto.PCL.Adapters.Encryption;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Stores.Settings
{
    public class SettingsStore : ISettingsStore
    {
        private IEncryptedDataAdapter encryptedDataAdapter;
        private readonly IsolatedStorageSettings isolatedStorage;
        private UTF8Encoding encoding;

        private const string PASSWORD_KEY_NAME = "password";
        private const string LOGIN_KEY_NAME = "login";
        private const string SID_KEY_NAME = "sid";

        public SettingsStore(IEncryptedDataAdapter encryptedDataAdapter)
        {
            this.encryptedDataAdapter = encryptedDataAdapter;
            this.isolatedStorage = IsolatedStorageSettings.ApplicationSettings;
            this.encoding = new UTF8Encoding();
        }

        public string Login
        {
            get
            {
                return (string)Get(LOGIN_KEY_NAME, "");
            }
            set
            {
                Set(LOGIN_KEY_NAME, value);
            }
        }

        public string Password
        {
            get
            {
                var decrypted = PasswordByteArray;
                return encoding.GetString(decrypted, 0, decrypted.Length);
            }
            set {
                PasswordByteArray = encoding.GetBytes(value);
            }
        }

        private byte[] PasswordByteArray
        {
            get
            {
                var encryptedValue = (byte[])Get(PASSWORD_KEY_NAME, new byte[0]);
                if (encryptedValue.Length == 0)
                {
                    return new byte[0];
                }
                return encryptedDataAdapter.Decrypt(encryptedValue);
            }
            set
            {
                var encryotedValue = encryptedDataAdapter.Encrypt(value);
                Set(PASSWORD_KEY_NAME, encryotedValue);
            }
        }

        public string Sid
        {
            get
            {
                var decrypted = SIDByteArray;
                return encoding.GetString(decrypted, 0, decrypted.Length);
            }
            set
            {
                SIDByteArray = encoding.GetBytes(value);
            }
        }

        private byte[] SIDByteArray
        {
            get
            {
                var encryptedValue = (byte[])Get(SID_KEY_NAME, new byte[0]);
                if (encryptedValue.Length == 0)
                {
                    return new byte[0]; 
                }
                return encryptedDataAdapter.Decrypt(encryptedValue);
            }
            set
            {
                var encryotedValue = encryptedDataAdapter.Encrypt(value);
                Set(SID_KEY_NAME, encryotedValue);
            }
        }

        public bool SubscribeToPushNotifications
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool LocationServiceAllowed
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool BackgroundTasksAllowed
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler UserChanged;

        public void Set(string key, object value)
        {
            if (isolatedStorage.Contains(key))
            {
                isolatedStorage[key] = value;
            }
            else
            {
                isolatedStorage.Add(key, value);
            }
        }

        public object Get(string key, object defaultValue = null)
        {
            if (isolatedStorage.Contains(key))
            {
                return isolatedStorage[key];
            }
            else
            {
                return defaultValue;
            }
        }

        public void Remove(string key)
        {
            if (isolatedStorage.Contains(key))
            {
                isolatedStorage.Remove(key);
            }
        }

        public void Save()
        {
            isolatedStorage.Save();
        }
    }
}
