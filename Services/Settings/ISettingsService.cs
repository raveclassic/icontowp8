using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Services.Settings
{
    public interface ISettingsService
    {
        void Set(string key, object value);
        object Get(string key);
        void Unset(string key);
        void Save();
    }
}
