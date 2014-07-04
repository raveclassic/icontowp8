using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Navigation
{
    public interface INavigationService
    {
        void Navigate(Uri uri, Dictionary<string, string> query = null);
        void GoBack();
        void GoForward();
        void ClearHistory();
    }
}
