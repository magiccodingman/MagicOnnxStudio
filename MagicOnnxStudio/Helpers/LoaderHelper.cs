using MagicOnnxStudio.Components.Shared;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class LoaderHelper
    {
        private readonly MudBlazor.IDialogService _dialogService;
        private MudBlazor.IDialogReference _dialogPleaseWaitReference;
        private readonly string PopupTitle;
        // Constructor with dialog service injected
        public LoaderHelper(MudBlazor.IDialogService dialogService, string _PopupTitle)
        {
            _dialogService = dialogService;
            PopupTitle = _PopupTitle;
        }

        // Method to open the spinner dialog
        public void ShowWaitSpinner()
        {
            var options = new DialogOptions() { CloseOnEscapeKey = false, BackdropClick = false, CloseButton = false};
            // Open the spinner dialog and store the dialog reference
            _dialogPleaseWaitReference = _dialogService.Show<WaitPopup>(PopupTitle, options); // No title for spinner
        }

        // Method to close the spinner dialog
        public void CloseWaitSpinner()
        {
            if (_dialogPleaseWaitReference != null)
            {
                _dialogPleaseWaitReference.Close();
            }
        }
    }

}
