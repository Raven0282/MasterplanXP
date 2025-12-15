using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MasterplanXP.ViewModels
{

    /// <summary>
    /// Represents the content displayed within the generic modal dialog.
    /// </summary>
    public partial class ExampleModalContentViewModel : ViewModelBase
    {
        // A property to hold the result that will be returned when the dialog closes.
        [ObservableProperty]
        private string _userInput = "Default Value";

        // The Avalonia convention to close a dialog and return a result is to use 
        // an Action<TResult> set on the Window. For MVVM, we use a command 
        // that the View (ModalDialog's content) can bind to.
        public Action<string>? CloseDialog { get; set; }

        /// <summary>
        /// Command executed when the user clicks 'OK' in the dialog.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            // When OK is pressed, we close the dialog and pass the UserInput as the result.
            CloseDialog?.Invoke(UserInput);
        }

        /// <summary>
        /// Command executed when the user clicks 'Cancel' in the dialog.
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            // When Cancel is pressed, we close the dialog and pass null/default as the result.
            CloseDialog?.Invoke(default!);
        }
    }


}
