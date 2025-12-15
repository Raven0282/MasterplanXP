using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Services;
using MasterplanXP.ViewModels;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection; // Used for GetRequiredService

namespace MasterplanXP.ViewModels
{
    // NOTE: Design service mocks must be accessible or defined here
    // For this demonstration, we assume the design mocks from Step 1 are accessible.

    public partial class MenuPageViewModel : ViewModelBase
    {

        private readonly IModalService _modalService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowService _windowService;

        // ----------------------------------------------------------------------------------
        // NEW: DESIGNER CONSTRUCTOR
        // ----------------------------------------------------------------------------------
        /// <summary>
        /// Constructor used exclusively by the XAML designer (called via <Design.DataContext>).
        /// Passes design-time mock services to the primary constructor.
        /// </summary>
        public MenuPageViewModel()
            : this(
                  new DesignModalService(),
                  new DesignWindowService(),
                  new DesignServiceProvider() // Use the service provider mock
            )
        {
            // Set simple, static data for the previewer
            Test = "Masterplan XP - Menu (Designer)";
        }

        // ----------------------------------------------------------------------------------
        // PRIMARY CONSTRUCTOR (Used by Dependency Injection at RUNTIME)
        // ----------------------------------------------------------------------------------
        // CONSTRUCTOR: Inject all needed services
        public MenuPageViewModel(IModalService modalService, IWindowService windowService, IServiceProvider serviceProvider)
        {
            _modalService = modalService;
            _windowService = windowService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Command executed when the user clicks a menu item to open the map (non-modal).
        /// </summary>
        [RelayCommand]
        private async Task OpenMapWindow()
        {
            // This line *requires* the IServiceProvider mock to work correctly at design time.
            var mapVM = _serviceProvider.GetRequiredService<MapViewModel>();

            await _windowService.ShowWindowAsync(mapVM);
        }

        // MODAL Window Command
        [RelayCommand]
        private async Task OpenMapModule(Window hostWindow)
        {
            if (hostWindow == null) return;

            // This line *requires* the IServiceProvider mock to work correctly at design time.
            var mapViewModel = _serviceProvider.GetRequiredService<MapViewModel>();

            await _modalService.ShowModalAsync<MapViewModel, bool>(
                hostWindow: hostWindow,
                viewModel: mapViewModel,
                title: "Mapping Module",
                width: 900,
                height: 600
            );
        }

        public string Test { get; set; } = "Masterplan XP - Menu";
    }
}