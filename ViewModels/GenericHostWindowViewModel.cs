using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Services;
using System.ComponentModel;
using Avalonia.Controls;

namespace MasterplanXP.ViewModels
{
    /// <summary>
    /// Minimal ViewModel for the generic window wrapper.
    /// This holds the ViewModel of the actual content (e.g., MapWindowViewModel).
    /// </summary>
    public partial class GenericHostWindowViewModel : ObservableObject
    {
        private readonly IWindowService _windowService;

        /// <summary>
        /// The actual content ViewModel being hosted (e.g., MapWindowViewModel).
        /// </summary>
        [ObservableProperty]
        private ObservableObject? _contentViewModel;

        /// <summary>
        /// Title displayed on the window's title bar.
        /// </summary>
        [ObservableProperty]
        private string _windowTitle = "MasterplanXP Tool Window";

        public GenericHostWindowViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        // --- Commands and Methods ---

        /// <summary>
        /// Command to close the hosted window via a button (e.g., a custom close button).
        /// </summary>
        [RelayCommand]
        public void CloseContent()
        {
            if (ContentViewModel != null)
            {
                // CORRECT: Calls the new overload, passing the instance of the content ViewModel
                _windowService.CloseWindow(ContentViewModel);
            }
        }

        /// <summary>
        /// Called when the hosting window is about to close (e.g., user clicks the OS close button).
        /// </summary>
        /// <returns>True if the window can close, false otherwise.</returns>
        public bool CanCloseWindow()
        {
            // Allow the content ViewModel to intercept and confirm closure (e.g., checking IsDirty).
            if (ContentViewModel is MapWindowViewModel mapVM)
            {
                return mapVM.CanClose();
            }
            // Add other specialized checks here (e.g., CombatTrackerViewModel)

            return true;
        }
    }
}