using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MasterplanXP.Services
{
    /// <summary>
    /// Defines a generalized service contract for managing independent, non-modal windows.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Creates, initializes, and shows a new non-modal window associated with the given ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel that will be the DataContext of the window.</typeparam>
        /// <param name="viewModel">The instance of the ViewModel to use.</param>
        /// <returns>A task that completes when the window has been shown.</returns>
        Task ShowWindowAsync<TViewModel>(TViewModel viewModel) where TViewModel : ObservableObject;

        /// <summary>
        /// Attempts to close the window associated with the specified ViewModel type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel whose window should be closed.</typeparam>
        void CloseWindow<TViewModel>() where TViewModel : ObservableObject;

        // --- NEW: Overload to close a window based on its ViewModel instance (Fixes CS1501) ---
        void CloseWindow(ObservableObject viewModelInstance);
    }
}