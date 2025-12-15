using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.ViewModels
{
    /// <summary>
    /// Represents the core state, context, and collection of interactive elements for a single map.
    /// </summary>
    public partial class MapViewModel : ObservableObject
    {
        // --- Map Display State ---

        /// <summary>
        /// The path or URI to the background image for the map.
        /// </summary>
        [ObservableProperty]
        private string _mapImagePath = string.Empty;

        /// <summary>
        /// Toggles the rendering between 2D Orthogonal and Isometric projection modes.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsometricProjectionActive))] // Notify tokens when this changes
        private bool _isIsometricView = false;

        /// <summary>
        /// True if the current projection is Isometric. Convenience property.
        /// </summary>
        public bool IsometricProjectionActive => IsIsometricView;

        /// <summary>
        /// The size (in pixels, at 1.0 zoom) of a single grid cell. Used for all coordinate transformations.
        /// </summary>
        [ObservableProperty]
        private double _gridCellSize = 50.0; // Defaulting to 50x50 pixels per logical unit

        [ObservableProperty]
        private GridType _currentGrid = GridType.Square; // Assumed GridType enum

        // --- PROPERTY REQUIRED BY GridOverlayControl.cs (Fixes CS1061) ---
        [ObservableProperty]
        private bool _isGridVisible = true;
        // ---------------------------------------------------------------


        // --- Interactive Elements ---

        /// <summary>
        /// Collection of all tokens currently on the map.
        /// </summary>
        public ObservableCollection<TokenViewModel> Tokens { get; } = new();

        /// <summary>
        /// Collection of all zone overlays currently on the map.
        /// </summary>
        public ObservableCollection<ZoneViewModel> Zones { get; } = new();


        // --- Initialization and Commands ---

        public MapViewModel()
        {
            // Subscribe to the change of the key properties that affect all tokens
            PropertyChanged += MapViewModel_PropertyChanged;

            // Placeholder: Load some initial data
            // LoadMap(new MapData(/* ... */));
        }

        private void MapViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // When IsIsometricView or GridCellSize changes, tell all tokens to recalculate their screen position.
            if (e.PropertyName == nameof(IsIsometricView) || e.PropertyName == nameof(GridCellSize))
            {
                foreach (var token in Tokens)
                {
                    token.UpdateScreenPosition();
                }
                // Need to implement UpdateScreenPosition() for Zones as well.
            }
        }

        /// <summary>
        /// Command to add a new token (e.g., from a button click).
        /// </summary>
        [RelayCommand]
        public void AddNewToken()
        {
            // Placeholder data for testing
            var newTokenData = new Models.TokenData
            {
                Name = $"New Token {Tokens.Count + 1}",
                ImagePath = "/Assets/default_token.png",
                X = 2,
                Y = 2,
                Scale = 1.0
            };

            // Create the ViewModel and add it to the collection
            var tokenVM = new TokenViewModel(newTokenData, this);
            Tokens.Add(tokenVM);
        }

        // --- Placeholder for other commands (AddZone, TileMap, etc.) ---
    }
}