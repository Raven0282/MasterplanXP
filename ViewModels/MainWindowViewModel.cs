using Avalonia.Controls;
using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Services;
using MasterplanXP.ViewModels;
using MasterplanXP.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MasterplanXP.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public SvgImage SideMenuImage => new SvgImage { Source = SvgSource.Load(path: $"avares://{nameof(MasterplanXP)}/Assets/Images/{(SideMenuExpanded ? "logo-menu" : "logo-small")}.svg") };

        private readonly IModalService _modalService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowService _windowService;

        // Keep a reference to the MenuPageViewModel if needed, but it won't be in the main content.
        // public MenuPageViewModel MenuPage { get; } 
        private readonly MenuPageViewModel _menuPage; // Use a private field if you need it

        [RelayCommand]
        private void SideMenuResize()
        {
            SideMenuExpanded = !SideMenuExpanded;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SideMenuImage))]
        private bool _sideMenuExpanded = true;

        [ObservableProperty]
        private ViewModelBase? _currentPage;

        // Injected Pages (Singletons)
        public MenuPageViewModel MenuPage { get; }
        public HomePageViewModel HomePage { get; } // NEW: Public property for HomePage

        // Private Page Instances (for pages not managed by DI as Singletons)
        private readonly ArtifactsPageViewModel _artifactsPage = new();
        private readonly CreaturePageViewModel _creaturePage = new();
        private readonly DiseasesPageViewModel _diseasesPage = new();
        // private readonly HomePageViewModel _homePage = new(); // REMOVE this line if you are injecting it
        private readonly MagicItemsPageViewModel _magicItemsPage = new();
        private readonly RitualsPageViewModel _ritualsPage = new();
        private readonly SkillChallengePageViewModel _skillChallengePage = new();
        private readonly TemplatesPageViewModel _templatesPage = new();
        private readonly TerrainPageViewModel _terrainPage = new();
        private readonly TilesPageViewModel _tilesPage = new();
        private readonly TrapsPageViewModel _trapsPage = new();
        private readonly LibraryPageViewModel _libraryPage = new();
        private readonly SettingsPageViewModel _settingsPage = new();

        // Use Constructor Injection for both services and ViewModels
        public MainWindowViewModel(MenuPageViewModel menuPage, IModalService modalService, IServiceProvider serviceProvider, IWindowService windowService)
        {
            _menuPage = menuPage; // Store for potential use, but not directly bound to content

            // Assign the injected services
            _modalService = modalService;
            _serviceProvider = serviceProvider;
            _windowService = windowService;

            // Set the initial page to your Home/Dashboard page, not MenuPage
            CurrentPage = _serviceProvider.GetRequiredService<HomePageViewModel>();
        }


        // FIX 2: Command to navigate to the Menu Page
        [RelayCommand]
        private void GoToMenu()
        {
            CurrentPage = _menuPage;
        }

        // FIX 3: Command to navigate back to the Home Page
        [RelayCommand]
        private void GoToHome()
        {
            CurrentPage = HomePage;
        }
        
        [RelayCommand]
        private void GoToCreature()
        {
            CurrentPage = _creaturePage;
        }

        [RelayCommand]
        private void GoToTemplates()
        {
            CurrentPage = _templatesPage;
        }

        [RelayCommand]
        private void GoToSkillChallenge()
        {
            CurrentPage = _skillChallengePage;
        }

        [RelayCommand]
        private void GoToDiseases()
        {
            CurrentPage = _diseasesPage;
        }

        [RelayCommand]
        private void GoToMagicItems()
        {
            CurrentPage = _magicItemsPage;
        }

        [RelayCommand]
        private void GoToArtifacts()
        {
            CurrentPage = _artifactsPage;
        }

        [RelayCommand]
        private void GoToRituals()
        {
            CurrentPage = _ritualsPage;
        }

        [RelayCommand]
        private void GoToTraps()
        {
            CurrentPage = _trapsPage;
        }

        [RelayCommand]
        private void GoToTerrain()
        {
            CurrentPage = _terrainPage;
        }

        [RelayCommand]
        private void GoToTiles()
        {
            CurrentPage = _tilesPage;
        }

        [RelayCommand]
        private void GoToLibrary()
        {
            CurrentPage = _libraryPage;
        }

        [RelayCommand]
        private void GoToSettings()
        {
            CurrentPage = _settingsPage;
        }

        [RelayCommand]
        private async Task OpenMapModule(Window hostWindow)
        {
            if (hostWindow == null) return;

            // 1. Resolve the MapViewModel
            // Note: Ensure MapViewModel is registered in App.axaml.cs (it looked like it was!)
            var mapViewModel = _serviceProvider.GetRequiredService<MapViewModel>();

            // 2. Configure the Map (Optional: pass project ID, load default layers, etc.)
            // mapViewModel.LoadProject(currentProjectId);

            // 3. Open the Modal
            // Since Map doesn't necessarily return a "Result" like "Yes/No", 
            // we can use 'bool' or 'object' as the generic result type and ignore it.
            await _modalService.ShowModalAsync<MapViewModel, bool>(
                hostWindow: hostWindow,
                viewModel: mapViewModel,
                title: "Mapping Module",
                width: 900,
                height: 600
            );
        }
        [RelayCommand]
        private async Task OpenCreatures(Window hostWindow)
        {
            if (hostWindow == null) return;

            // 1. Resolve the MapViewModel
            // Note: Ensure MapViewModel is registered in App.axaml.cs (it looked like it was!)
            var mapViewModel = _serviceProvider.GetRequiredService<CreaturePageViewModel>();

            // 2. Configure the Map (Optional: pass project ID, load default layers, etc.)
            // mapViewModel.LoadProject(currentProjectId);

            // 3. Open the Modal
            // Since Map doesn't necessarily return a "Result" like "Yes/No", 
            // we can use 'bool' or 'object' as the generic result type and ignore it.
            await _modalService.ShowModalAsync<CreaturePageViewModel, bool>(
                hostWindow: hostWindow,
                viewModel: mapViewModel,
                title: "Creatures",
                width: 900,
                height: 600
            );
        }

    }
}
