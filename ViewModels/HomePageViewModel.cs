using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Services; // <--- ADD THIS USING DIRECTIVE
using MasterplanXP.ViewModels;
using System.Reflection;

namespace MasterplanXP.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private readonly IAppConfigurationService _configService; // <--- NEW FIELD

        // Add a constructor to receive the IAppConfigurationService via DI
        public HomePageViewModel(IAppConfigurationService configService) // <--- ADDED CONSTRUCTOR
        {
            _configService = configService;

            // The Test property is now initialized using the service's calculated string
            Test = _configService.AppVersionString;
        }

        // Update the Test property to be read-only (since the value is set in the constructor)
        public string Test { get; }

        // REMOVED: The GetAssemblyVersion() method is now moved to AppConfigurationService.cs

        // Example: Using the centralized flag for logic
        public bool ShowBetaWarning => _configService.IsBeta;

    }
}