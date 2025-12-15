using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using MasterplanXP.ViewModels;
using MasterplanXP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.Services
{
    public class WindowService : IWindowService
    {
        // Dictionary to track open windows, keyed by the ViewModel type.
        private readonly Dictionary<Type, Window> _openWindows = new();

        private readonly IDataTemplate _viewLocator;

        public WindowService(IDataTemplate viewLocator)
        {
            _viewLocator = viewLocator;
        }

        // ... (ShowWindowAsync method remains the same) ...
        public Task ShowWindowAsync<TViewModel>(TViewModel viewModel) where TViewModel : ObservableObject
        {
            Type vmType = typeof(TViewModel);

            // 1. Check if a window for this ViewModel type is already open
            if (_openWindows.ContainsKey(vmType))
            {
                // Optionally bring the existing window to the front
                return Task.CompletedTask;
            }

            // 2. Create the Generic Host ViewModel
            // NOTE: This requires dependency injection or direct instantiation of GenericHostWindowViewModel
            // For now, assume a ServiceLocator or direct instantiation is handled here if DI is not available.
            // In a full application, the service container would resolve this.
            var hostVM = new GenericHostWindowViewModel(this)
            {
                ContentViewModel = viewModel,
                WindowTitle = viewModel.GetType().Name.Replace("ViewModel", string.Empty)
            };

            // 3. Create the window
            // The ViewLocator is typically not used for the host window itself, 
            // but for the content *inside* the host window.
            var windowToOpen = new GenericHostWindow
            {
                DataContext = hostVM // Set the host VM as the window's DataContext
            };

            // 4. Track the window and manage its closure
            _openWindows.Add(vmType, windowToOpen);

            // Remove the window from the tracking collection when closed.
            windowToOpen.Closed += (sender, e) =>
            {
                _openWindows.Remove(vmType);
            };

            // 5. Display the window non-modally
            windowToOpen.Show();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Attempts to close the window associated with the specified ViewModel type.
        /// </summary>
        public void CloseWindow<TViewModel>() where TViewModel : ObservableObject
        {
            Type vmType = typeof(TViewModel);

            if (_openWindows.TryGetValue(vmType, out Window? windowToClose))
            {
                // Add CanClose check by casting DataContext to GenericHostWindowViewModel
                if (windowToClose.DataContext is GenericHostWindowViewModel hostVM)
                {
                    if (!hostVM.CanCloseWindow())
                    {
                        return;
                    }
                }

                windowToClose.Close();
                // The window will be removed from _openWindows by the Closed event handler.
            }
        }

        // --- NEW IMPLEMENTATION (Fixes CS0535) ---
        /// <summary>
        /// Attempts to close the window associated with the specified ViewModel *instance*.
        /// </summary>
        public void CloseWindow(ObservableObject viewModelInstance)
        {
            // We need to find the Window that has this specific viewModelInstance as its content.
            var windowEntry = _openWindows.FirstOrDefault(
                pair => pair.Value.DataContext is GenericHostWindowViewModel hostVM &&
                        hostVM.ContentViewModel == viewModelInstance);

            if (windowEntry.Value is { } windowToClose)
            {
                // Check if the window is allowed to close (e.g., asking user to save)
                if (windowToClose.DataContext is GenericHostWindowViewModel hostVM)
                {
                    if (!hostVM.CanCloseWindow())
                    {
                        return;
                    }
                }

                windowToClose.Close();
                // The window will be removed from _openWindows by the Closed event handler.
            }
            // If not found, do nothing.
        }
        // ------------------------------------------
    }
}