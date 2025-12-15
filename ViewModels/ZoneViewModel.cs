using Avalonia.LogicalTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.ViewModels
{
    public partial class ZoneViewModel : ObservableObject
    {
        private readonly ZoneData _zoneData;
        private readonly MapViewModel _mapViewModel;

        // Logical Grid Properties (Bound to Model)
        [ObservableProperty] private double _logicalX;
        [ObservableProperty] private double _logicalY;
        [ObservableProperty] private double _logicalWidth;
        [ObservableProperty] private double _logicalHeight;
        [ObservableProperty] private int _elevation;

        // Visual Properties (Screen Calculated)
        [ObservableProperty] private double _screenX;
        [ObservableProperty] private double _screenY;
        [ObservableProperty] private double _screenWidth;
        [ObservableProperty] private double _screenHeight;

        // Zone Details (Bound to Model)
        public string Name => _zoneData.Name;
        public string Details => _zoneData.Details;
        public string ColorHex => _zoneData.ColorHex;
        public double Opacity => _zoneData.Opacity;

        // --- NEW PROPERTY REQUIRED BY ZoneView.axaml.cs (Fixes CS0117/CS1061) ---
        // This property drives the logic in ZoneView's ConfigureShape method.
        [ObservableProperty] private string _shapeType = "rectangle";
        // -----------------------------------------------------------------------


        // We will focus on Rectangle/Ellipse (based on X/Y/W/H) for simplicity now.
        // Complex ShapeType handling will...

        // ... (rest of the file content remains the same) ...

        /* ... (The remainder of your original file content continues here) ... */

        public ZoneViewModel(ZoneData zoneData, MapViewModel mapViewModel)
        {
            _zoneData = zoneData;
            _mapViewModel = mapViewModel;

            // Initialize from Model
            _logicalX = zoneData.X;
            _logicalY = zoneData.Y;
            _logicalWidth = zoneData.Width;
            _logicalHeight = zoneData.Height;
            _elevation = zoneData.Elevation;

            // Subscribe to map changes for position/size updates
            _mapViewModel.PropertyChanged += OnMapViewModelPropertyChanged;
            PropertyChanged += OnSelfPropertyChanged;

            // Initial calculation
            UpdateScreenPosition();
        }

        private void OnMapViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MapViewModel.IsIsometricView) ||
                e.PropertyName == nameof(MapViewModel.GridCellSize))
            {
                // If GridCellSize changes, we must recalculate all screen dimensions
                UpdateScreenPosition();
            }
        }

        private void OnSelfPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LogicalX) ||
                e.PropertyName == nameof(LogicalY) ||
                e.PropertyName == nameof(Elevation))
            {
                // Update the underlying model immediately for saving consistency
                _zoneData.X = LogicalX;
                _zoneData.Y = LogicalY;
                _zoneData.Elevation = Elevation;

                UpdateScreenPosition();
            }
            else if (e.PropertyName == nameof(LogicalWidth) ||
                     e.PropertyName == nameof(LogicalHeight))
            {
                _zoneData.Width = LogicalWidth;
                _zoneData.Height = LogicalHeight;

                UpdateScreenPosition();
            }
        }

        /// <summary>
        /// Converts logical (grid) coordinates and dimensions into screen coordinates.
        /// </summary>
        public void UpdateScreenPosition()
        {
            (double screenX, double screenY) position;
            (double screenW, double screenH) dimensions;

            if (_mapViewModel.IsIsometricView)
            {
                position = IsometricToScreen(LogicalX, LogicalY, Elevation, _mapViewModel.GridCellSize);
                dimensions = DimensionsToScreenIsometric(LogicalWidth, LogicalHeight, _mapViewModel.GridCellSize);
            }
            else
            {
                position = OrthogonalToScreen(LogicalX, LogicalY, _mapViewModel.GridCellSize);
                dimensions = DimensionsToScreenOrthogonal(LogicalWidth, LogicalHeight, _mapViewModel.GridCellSize);
            }

            ScreenX = position.screenX;
            ScreenY = position.screenY;
            ScreenWidth = dimensions.screenW;
            ScreenHeight = dimensions.screenH;
        }

        // ... (OrthogonalToScreen, IsometricToScreen, etc. helper methods follow) ...
        // Ensure you have these two dimension conversion methods:
        private static (double W, double H) DimensionsToScreenOrthogonal(double width, double height, double cellSize)
        {
            return (width * cellSize, height * cellSize);
        }

        private static (double W, double H) DimensionsToScreenIsometric(double width, double height, double cellSize)
        {
            // Simple approach: Width/Height in isometric is generally derived from logical dimensions * cell size.
            // Complex approach would require calculating the isometric projection of the zone's corners.
            // Use the simple approach for now, assuming the zone is a 'floor' marker.
            return (width * cellSize, height * cellSize);
        }

        // --- Commands for Interaction ---

        /// <summary>
        /// Command executed when the zone is moved by the user.
        /// </summary>
        [RelayCommand]
        public void MoveZone(Avalonia.Point newScreenPosition)
        {
            // ... (conversion logic similar to TokenViewModel) ...
        }

        /// <summary>
        /// Command executed when the zone is resized.
        /// </summary>
        [RelayCommand]
        public void ResizeZone(Avalonia.Point newScreenDimensions)
        {
            double cellSize = _mapViewModel.GridCellSize;

            // Conversion is simpler for size, as W/H is typically just scaled, regardless of projection.
            LogicalWidth = newScreenDimensions.X / cellSize;
            LogicalHeight = newScreenDimensions.Y / cellSize;
        }

        // --- Cleanup ---

        public void Dispose()
        {
            _mapViewModel.PropertyChanged -= OnMapViewModelPropertyChanged;
        }

        // ... (Static helper methods for coordinate conversion follow) ...
        private static (double X, double Y) IsometricToScreen(double x, double y, int elevation, double cellSize)
        {
            const double ISO_X_SCALE = 0.5;
            const double ISO_Y_SCALE = 1.0;
            const double ISO_ELEVATION_FACTOR = 0.5;

            double isoX = (x - y) * ISO_X_SCALE * cellSize;
            double isoY = (x + y) * ISO_Y_SCALE * cellSize;
            double screenY = isoY - (elevation * ISO_ELEVATION_FACTOR * cellSize);

            return (isoX, screenY);
        }

        private static (double X, double Y) OrthogonalToScreen(double x, double y, double cellSize)
        {
            double screenX = x * cellSize;
            double screenY = y * cellSize;
            return (screenX, screenY);
        }
    }
}