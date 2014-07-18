using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Dialog
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        void Prompt(string message, string title, Action<string> onPrompt, Action onCancel = null);
        bool Confirm(string message, string title);
    }
}
