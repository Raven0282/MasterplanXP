using Avalonia.LogicalTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.ViewModels
{
    public partial class TokenViewModel : ObservableObject
    {
        private readonly TokenData _tokenData;
        private readonly MapViewModel _mapViewModel;

        // --- NEW: Pass-Through Property for Grid Cell Size ---
        /// <summary>
        /// Provides the current GridCellSize from the MapViewModel context for use in binding/converters.
        /// </summary>
        public double GridCellSize => _mapViewModel.GridCellSize;

        // Logical Grid Coordinates (Bound to Model)
        [ObservableProperty]
        private double _logicalX;

        [ObservableProperty]
        private double _logicalY;

        [ObservableProperty]
        private int _elevation;

        // Visual Properties (Screen Calculated)
        [ObservableProperty]
        private double _screenX;

        [ObservableProperty]
        private double _screenY;

        // Token Details
        [ObservableProperty]
        private string _tokenName;

        [ObservableProperty]
        private string _tokenDetails;

        [ObservableProperty]
        private double _scaleFactor;

        public string ImagePath => _tokenData.ImagePath;

        /// <summary>
        /// Initializes a new TokenViewModel.
        /// </summary>
        /// <param name="tokenData">The underlying model data for the token.</param>
        /// <param name="mapViewModel">The parent map view model for coordinate context.</param>
        public TokenViewModel(TokenData tokenData, MapViewModel mapViewModel)
        {
            _tokenData = tokenData;
            _mapViewModel = mapViewModel;

            // Initialize from Model
            _logicalX = tokenData.X;
            _logicalY = tokenData.Y;
            _elevation = tokenData.Elevation;
            _tokenName = tokenData.Name;
            _tokenDetails = tokenData.Details;
            _scaleFactor = tokenData.Scale;

            // Subscribe to changes in the MapViewModel's properties that affect position or size.
            _mapViewModel.PropertyChanged += OnMapViewModelPropertyChanged;
            PropertyChanged += OnSelfPropertyChanged;

            // Initial calculation
            UpdateScreenPosition();
        }

        // --- Event Handlers for Re-calculation ---

        /// <summary>
        /// Handles changes in the parent MapViewModel that affect positioning (e.g., IsIsometricView or GridCellSize).
        /// </summary>
        private void OnMapViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Re-calculate position if the view mode or grid size changes
            if (e.PropertyName == nameof(MapViewModel.IsIsometricView) ||
                e.PropertyName == nameof(MapViewModel.GridCellSize))
            {
                // If GridCellSize changes, we must notify the View that the GridCellSize property 
                // exposed by THIS ViewModel has changed, which triggers the TokenView's size update.
                if (e.PropertyName == nameof(MapViewModel.GridCellSize))
                {
                    OnPropertyChanged(nameof(GridCellSize));
                }

                UpdateScreenPosition();
            }
        }

        /// <summary>
        /// Handles changes to the Token's own logical properties.
        /// </summary>
        private void OnSelfPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LogicalX) ||
                e.PropertyName == nameof(LogicalY) ||
                e.PropertyName == nameof(Elevation))
            {
                // Update the underlying model immediately for saving consistency
                _tokenData.X = LogicalX;
                _tokenData.Y = LogicalY;
                _tokenData.Elevation = Elevation;

                UpdateScreenPosition();
            }
            // Update the underlying model for non-position changes
            else if (e.PropertyName == nameof(TokenName))
            {
                _tokenData.Name = TokenName;
            }
            // ... handle other property changes (ScaleFactor, TokenDetails, etc.)
        }

        // --- Core Coordinate Transformation Logic ---

        /// <summary>
        /// Converts logical (grid) coordinates and elevation into screen coordinates.
        /// </summary>
        public void UpdateScreenPosition()
        {
            (double screenX, double screenY) result;

            if (_mapViewModel.IsIsometricView)
            {
                result = IsometricToScreen(LogicalX, LogicalY, Elevation, _mapViewModel.GridCellSize);
            }
            else
            {
                result = OrthogonalToScreen(LogicalX, LogicalY, _mapViewModel.GridCellSize);
            }

            ScreenX = result.screenX;
            ScreenY = result.screenY;
        }

        /// <summary>
        /// Conversion for standard 2D view.
        /// </summary>
        private static (double X, double Y) OrthogonalToScreen(double x, double y, double cellSize)
        {
            // In 2D, the screen position is a direct scaling of the logical grid position.
            double screenX = x * cellSize;
            double screenY = y * cellSize;
            return (screenX, screenY);
        }

        /// <summary>
        /// Conversion for the Isometric view, incorporating elevation.
        /// </summary>
        private static (double X, double Y) IsometricToScreen(double x, double y, int elevation, double cellSize)
        {
            // Standard Isometric Projection Angles:
            // The X-axis runs up-right, the Y-axis runs up-left. The Z-axis (Elevation) runs straight up.
            // These coefficients define the visual skew and scale for a typical 2:1 isometric grid.
            const double ISO_X_SCALE = 0.5; // cos(30 deg) * some scale factor
            const double ISO_Y_SCALE = 1.0; // sin(30 deg) * some scale factor
            const double ISO_ELEVATION_FACTOR = 0.5; // How many pixels represent 1 unit of elevation

            // 1. Convert the 2D grid position (X, Y) to the isometric screen plane (ISO_X, ISO_Y)
            // (X * grid_size) projects along the diagonal up-right
            // (Y * grid_size) projects along the diagonal up-left

            // X-Screen = (X_Grid - Y_Grid) * ISO_X_SCALE * CellSize 
            double isoX = (x - y) * ISO_X_SCALE * cellSize;

            // Y-Screen = (X_Grid + Y_Grid) * ISO_Y_SCALE * CellSize
            double isoY = (x + y) * ISO_Y_SCALE * cellSize;

            // 2. Adjust for Elevation (Z-axis is straight vertical offset)
            // Elevation is subtracted from Y because higher elevation means lower Y coordinate (up on screen).
            double screenY = isoY - (elevation * ISO_ELEVATION_FACTOR * cellSize);

            return (isoX, screenY);
        }

        // --- Commands for Interaction ---

        /// <summary>
        /// Command executed when the token is moved by the user (e.g., Drag-and-Drop).
        /// </summary>
        /// <param name="newScreenPosition">The new screen position (in raw screen pixels).</param>
        [RelayCommand]
        public void MoveToken(Avalonia.Point newScreenPosition) // Use Avalonia.Point for screen coordinates
        {
            // 1. Convert the new screen position back to logical grid coordinates.
            (double newX, double newY) logicalCoords;

            if (_mapViewModel.IsIsometricView)
            {
                // Must account for the current Elevation when reversing the transform!
                logicalCoords = ScreenToIsometric(newScreenPosition.X, newScreenPosition.Y, Elevation, _mapViewModel.GridCellSize);
            }
            else
            {
                logicalCoords = ScreenToOrthogonal(newScreenPosition.X, newScreenPosition.Y, _mapViewModel.GridCellSize);
            }

            // 2. Snap to the nearest grid point if required (optional, but good for VTT)
            // logicalCoords.newX = Math.Round(logicalCoords.newX);
            // logicalCoords.newY = Math.Round(logicalCoords.newY);

            // 3. Update the logical properties, triggering a screen position update.
            LogicalX = logicalCoords.newX;
            LogicalY = logicalCoords.newY;

            Debug.WriteLine($"Token moved to Logical: ({LogicalX:F2}, {LogicalY:F2}), Screen: ({ScreenX:F2}, {ScreenY:F2})");
        }

        /// <summary>
        /// Inverse conversion for standard 2D view.
        /// </summary>
        private static (double X, double Y) ScreenToOrthogonal(double screenX, double screenY, double cellSize)
        {
            double logicalX = screenX / cellSize;
            double logicalY = screenY / cellSize;
            return (logicalX, logicalY);
        }

        /// <summary>
        /// Inverse conversion for the Isometric view.
        /// </summary>
        private static (double X, double Y) ScreenToIsometric(double screenX, double screenY, int currentElevation, double cellSize)
        {
            const double ISO_X_SCALE = 0.5;
            const double ISO_Y_SCALE = 1.0;
            const double ISO_ELEVATION_FACTOR = 0.5;

            // 1. Reverse the Elevation adjustment to find the ground plane screen Y
            double groundScreenY = screenY + (currentElevation * ISO_ELEVATION_FACTOR * cellSize);

            // 2. Use the reversed transformation matrix (requires linear algebra, but we can simplify):
            // The ground screen coordinates are:
            // isoX = (X - Y) * C1
            // groundScreenY = (X + Y) * C2

            double C1 = ISO_X_SCALE * cellSize;
            double C2 = ISO_Y_SCALE * cellSize;

            // X + Y = groundScreenY / C2
            // X - Y = isoX / C1

            // Add the two equations: (2 * X) = (groundScreenY / C2) + (isoX / C1)
            double sum = (groundScreenY / C2) + (screenX / C1);
            double logicalX = sum / 2.0;

            // Subtract the second from the first: (2 * Y) = (groundScreenY / C2) - (isoX / C1)
            double diff = (groundScreenY / C2) - (screenX / C1);
            double logicalY = diff / 2.0;

            return (logicalX, logicalY);
        }

        // --- Cleanup ---

        /// <summary>
        /// Explicitly unsubscribes from the parent to prevent memory leaks.
        /// </summary>
        public void Dispose()
        {
            _mapViewModel.PropertyChanged -= OnMapViewModelPropertyChanged;
            // Optionally implement IDisposable
        }
    }
}