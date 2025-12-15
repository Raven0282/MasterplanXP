using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Services;

namespace MasterplanXP.ViewModels
{
    /// <summary>
    /// View Model for the main host window.
    /// </summary>
    public partial class HostWindowViewModel : ObservableObject
    {
        private readonly IModalService _modalService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _modalResult = "No data received from dialog yet.";

        /// <summary>
        /// Initializes a new instance of the <see cref="HostWindowViewModel"/> class.
        /// </summary>
        /// <param name="modalService">The injected modal service.</param>
        /// <param name="serviceProvider">The service provider to resolve dialog ViewModels.</param>
        public HostWindowViewModel(IModalService modalService, IServiceProvider serviceProvider)
        {
            _modalService = modalService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Command to open the custom dialog.
        /// </summary>
        [RelayCommand]
        private async Task OpenCustomDialog(Window hostWindow)
        {
            // 1. Resolve the specific content view model for the dialog
            // This keeps the view model independent of the host view model lifecycle.
            var contentViewModel = _serviceProvider.GetService(typeof(ExampleModalContentViewModel)) as ExampleModalContentViewModel;

            if (contentViewModel == null)
            {
                ModalResult = "Error: Could not resolve dialog view model.";
                return;
            }

            // 2. Define the expected result type for the dialog (e.g., string from the user input)
            // Set up the close action on the content view model to communicate the result back
            // to the ModalDialog Window via a callback.
            TaskCompletionSource<string?> tcs = new();
            contentViewModel.CloseDialog = (result) => tcs.SetResult(result);

            // 3. Call the generic service with customization parameters
            string? result = await _modalService.ShowModalAsync<ExampleModalContentViewModel, string>(
                hostWindow: hostWindow,
                viewModel: contentViewModel,
                title: "Custom MasterplanXP Dialog",
                width: 400,
                height: 250);

            // 4. Process the result after the dialog closes
            if (result != null)
            {
                ModalResult = $"Dialog closed. Data received: '{result}'";
            }
            else
            {
                ModalResult = "Dialog closed without data (Canceled or Closed).";
            }
        }
    }
}




