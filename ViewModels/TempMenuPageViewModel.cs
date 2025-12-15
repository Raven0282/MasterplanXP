using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterPlanXP.Views;


    namespace MasterPlanXP.ViewModels
{
    public partial class MenuPageViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ViewModelBase? _currentPage;

        private readonly ArtifactsPageViewModel _artifactsPage = new();
        private readonly CreaturePageViewModel _creaturePage = new();
        private readonly DiseasesPageViewModel _diseasesPage = new();
        private readonly HomePageViewModel _homePage = new();
        private readonly MagicItemsPageViewModel _magicItemsPage = new();
        private readonly RitualsPageViewModel _ritualsPage = new();
        private readonly SettingsPageViewModel _settingsPage = new();
        private readonly SkillChallengePageViewModel _skillChallengePage = new();
        private readonly TemplatesPageViewModel _templatesPage = new();
        private readonly TerrainPageViewModel _terrainPage = new();
        private readonly TilesPageViewModel _tilesPage = new();
        private readonly TrapsPageViewModel _trapsPage = new();
        private readonly LibraryPageViewModel _libraryPage = new();

        public MenuPageViewModel()
        {
            CurrentPage = _homePage;
        }

        [RelayCommand]
        private void GoToHome()
        {
            CurrentPage = _homePage;
        }

    }
}
