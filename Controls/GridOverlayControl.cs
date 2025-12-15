using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using MasterplanXP.ViewModels; // Required for MapViewModel property
using System;
using System.Diagnostics;

namespace MasterplanXP.Controls
{
    /// <summary>
    /// The main partial class for the custom control that draws the grid overlay.
    /// </summary>
    public partial class GridOverlayControl : Control
    {
        // --- Dependency Properties ---

        // 1. MapViewModel Dependency Property (used for binding GridCellSize and IsIsometricView)
        public static readonly StyledProperty<MapViewModel?> MapViewModelProperty =
            AvaloniaProperty.Register<GridOverlayControl, MapViewModel?>(nameof(MapViewModel));

        public MapViewModel? MapViewModel
        {
            get => GetValue(MapViewModelProperty);
            set => SetValue(MapViewModelProperty, value);
        }

        // 2. GridLineBrush Dependency Property
        public static readonly StyledProperty<IBrush> GridLineBrushProperty =
            AvaloniaProperty.Register<GridOverlayControl, IBrush>(nameof(GridLineBrush), new ImmutableSolidColorBrush(Colors.LightGray, 0.5));

        public IBrush GridLineBrush
        {
            get => GetValue(GridLineBrushProperty);
            set => SetValue(GridLineBrushProperty, value);
        }

        // --- Constructor and Initialization ---

        // The parameterless constructor is defined here (resolves CS0111)
        public GridOverlayControl()
        {
            // Subscribe to property changes to trigger visual invalidation (redraw)
            // MapViewModelProperty.Changed.Subscribe(OnMapViewModelChanged);

            // Trigger a redraw when the control's size changes
            this.SizeChanged += (s, e) => InvalidateVisual();
        }

        private void OnMapViewModelChanged(AvaloniaPropertyChangedEventArgs<MapViewModel?> e)
        {
            // Unsubscribe from old VM
            if (e.OldValue.Value is MapViewModel oldVM)
            {
                oldVM.PropertyChanged -= OnMapPropertiesChanged;
            }
            // Subscribe to new VM
            if (e.NewValue.Value is MapViewModel newVM)
            {
                newVM.PropertyChanged += OnMapPropertiesChanged;
            }

            // Trigger redraw immediately
            InvalidateVisual();
        }

        private void OnMapPropertiesChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Redraw if the grid size or view type changes
            if (e.PropertyName == nameof(MapViewModel.GridCellSize) ||
                e.PropertyName == nameof(MapViewModel.IsIsometricView))
            {
                InvalidateVisual();
            }
        }

        // --- Rendering Logic ---

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (MapViewModel == null || !MapViewModel.IsGridVisible || MapViewModel.GridCellSize <= 0)
            {
                return;
            }

            // Create a pen using the dependency property
            var pen = new Pen(GridLineBrush, 1.0);
            double cellSize = MapViewModel.GridCellSize;
            double width = Bounds.Width;
            double height = Bounds.Height;

            // Simple Orthogonal Grid Drawing (Isometric is complex and deferred)
            if (!MapViewModel.IsIsometricView)
            {
                // Draw vertical lines
                for (double x = cellSize; x < width; x += cellSize)
                {
                    // Snap to half-pixel offset for sharp rendering on whole-pixel lines
                    context.DrawLine(pen, new Point(x - 0.5, 0), new Point(x - 0.5, height));
                }

                // Draw horizontal lines
                for (double y = cellSize; y < height; y += cellSize)
                {
                    context.DrawLine(pen, new Point(0, y - 0.5), new Point(width, y - 0.5));
                }
            }
            // Isometric drawing logic would go here if implemented
        }
    }
}