using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Services.Dialog
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        bool Confirm(string message, string title);
    }
}
