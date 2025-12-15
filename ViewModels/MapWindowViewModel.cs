using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Models;
using MasterplanXP.Services;

namespace MasterplanXP.ViewModels
{
    /// <summary>
    /// The top-level ViewModel for the Mapping Window session.
    /// Manages file operations (Save/Load/Export) and holds the active map content.
    /// This is the DataContext for the GenericHostWindow when displaying the map.
    /// </summary>
    public partial class MapWindowViewModel : ObservableObject
    {
        private readonly IDataSerializerService _dataSerializerService;
        private readonly IFileOperationService _fileOperationService;
        private readonly IWindowService _windowService;
        private readonly IFileService _fileService;

        /// <summary>
        /// The active map instance, which contains all tokens, zones, and coordinate context.
        /// </summary>
        [ObservableProperty]
        private MapViewModel _activeMap;

        /// <summary>
        /// Stores the file path where the current map was last saved.
        /// </summary>
        private string? _currentFilePath;

        /// <summary>
        /// Indicates if the current session has unsaved changes.
        /// </summary>
        [ObservableProperty]
        private bool _isDirty = false;

        /// <summary>
        /// Initializes a new instance of the MapWindowViewModel.
        /// </summary>
        /// <param name="mapRepository">The service responsible for data persistence and serialization.</param>
        /// <param name="windowService">The service used to manage the window (e.g., closing itself).</param>
        public MapWindowViewModel(
                    IDataSerializerService dataSerializerService,
                    IWindowService windowService,
                    IFileService fileService,
                    IFileOperationService fileOperationService) // New parameter
        {
            // Initialize Services
            _dataSerializerService = dataSerializerService;
            _windowService = windowService;
            _fileService = fileService;
            _fileOperationService = fileOperationService;

            // Initialize with an empty map.
            _activeMap = new MapViewModel();
            _activeMap.PropertyChanged += (s, e) => IsDirty = true; // Mark dirty on any map state change
        }

        // --- Session Commands ---

        /// <summary>
        /// Loads a map session from a binary file.
        /// </summary>
        [RelayCommand]
        private async Task LoadMapAsync()
        {
            string? filePath = await _fileService.ShowOpenFileDialogAsync(
                "Open MasterplanXP Map (Binary)", "bin");

            if (filePath == null) return;

            try
            {
                // Use the generic binary loader for MapData
                MapData? data = await _dataSerializerService.LoadBinaryAsync<MapData>(filePath);

                if (data != null)
                {
                    LoadMapFromData(data);
                    _currentFilePath = filePath;
                    IsDirty = false;
                }
            }
            catch (Exception ex)
            {
                // Handle errors and potentially show an error modal dialog
                Console.WriteLine($"Error loading map: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the current map session to the last used binary file path.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveMapAsync()
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                await SaveAsMapAsync();
                return;
            }

            // If path exists, use the low-level serializer directly for speed and simplicity
            MapData data = GetMapDataFromViewModel();
            await _dataSerializerService.SaveBinaryAsync(data, _currentFilePath);
            IsDirty = false;
        }

        private bool CanSave() => IsDirty;

        /// <summary>
        /// Prompts the user for a file path and saves the current map session to that binary file.
        /// </summary>
        [RelayCommand]
        private async Task SaveAsMapAsync()
        {
            MapData data = GetMapDataFromViewModel();

            // Delegate the dialog and saving to the generic service
            string? newPath = await _fileOperationService.SaveBinaryAsync(data, "NewMap");

            if (newPath != null)
            {
                _currentFilePath = newPath;
                IsDirty = false;
            }
        }

        /// <summary>
        /// Exports the map session to a human-readable (JSON/XML) for interoperability.
        /// </summary>
        [RelayCommand]
        private async Task ExportMapAsync()
        {
            MapData data = GetMapDataFromViewModel();

            // Delegate the format selection dialog and export to the generic service
            await _fileOperationService.ExportHumanReadableAsync(data, "MapExport");
        }

        /// <summary>
        /// Called when the user attempts to close the window. Allows for confirmation dialogs.
        /// </summary>
        /// <returns>True if the window can close, false otherwise.</returns>
        public bool CanClose()
        {
            if (IsDirty)
            {
                // Integration point for IModalService: Show a confirmation dialog here.
                // e.g., var result = _modalService.ShowConfirmation("Save changes?");
                // if (result == Save) SaveMapAsync();
                // if (result == Cancel) return false;
            }
            return true;
        }

        /// <summary>
        /// Public method used by the GenericHostWindow to close the window programmatically.
        /// </summary>
        public void CloseWindow()
        {
            _windowService.CloseWindow<MapWindowViewModel>();
        }

        // --- Private Utility Methods ---

        private void LoadMapFromData(MapData data)
        {
            // Create a brand new MapViewModel to replace the old one
            var newMap = new MapViewModel
            {
                MapImagePath = data.MapImagePath,
                GridCellSize = data.GridCellSize,
                IsIsometricView = data.IsIsometricView
            };

            // Load Tokens
            foreach (var tokenData in data.Tokens)
            {
                newMap.Tokens.Add(new TokenViewModel(tokenData, newMap));
            }

            // Load Zones (ZoneViewModel not yet created)
            // foreach (var zoneData in data.Zones)
            // {
            //     newMap.Zones.Add(new ZoneViewModel(zoneData, newMap));
            // }

            ActiveMap = newMap;
        }

        private MapData GetMapDataFromViewModel()
        {
            // Convert current MapViewModel and all child ViewModels back into the serializable Model
            var data = new MapData
            {
                MapImagePath = ActiveMap.MapImagePath,
                GridCellSize = ActiveMap.GridCellSize,
                IsIsometricView = ActiveMap.IsIsometricView,

                // Note: The TokenViewModel's constructor should save property changes to its TokenData model.
                Tokens = new List<TokenData>()
                // In a proper implementation, this collection should hold the Model objects, not the ViewModels.
                // A better approach would be for the MapViewModel to expose a collection of TokenData objects.
            };

            // Temporary collection creation for demonstration:
            foreach (var tokenVM in ActiveMap.Tokens)
            {
                // Since TokenViewModel is written to update the TokenData on property change, 
                // we assume its underlying data is current.
                // NOTE: This requires the TokenViewModel to expose or hold a reference to its TokenData instance.
            }

            // --- Placeholder for Zones ---

            return data;
        }
    }
}