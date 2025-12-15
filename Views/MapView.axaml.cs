using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using MasterplanXP.Controls;
using MasterplanXP.ViewModels;
using System.Diagnostics;

namespace MasterplanXP.Views
{

    public partial class MapView : UserControl
    {
        private Point? _lastMousePosition;
        private TokenViewModel? _draggedToken;

        public MapView()
        {
            InitializeComponent();

            // Register for pointer events on the Canvas (or the whole Grid)
            // The GridOverlayControl is the highest layer for simple pointer tracking.
        }

        /// <summary>
        /// Handles the initial click/press on the map surface.
        /// </summary>
        private void GridOverlayControl_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is not MapViewModel mapVM) return;

            // Get the position relative to the MapCanvas
            Point canvasPosition = e.GetCurrentPoint(this).Position;

            // 1. Check if a Token was clicked to initiate a drag operation
            if (e.Source is Visual visual)
            {
                // Traverse up the visual tree to find if the clicked element belongs to a TokenView
                TokenView? tokenView = visual.FindAncestorOfType<TokenView>();

                if (tokenView?.DataContext is TokenViewModel tokenVM)
                {
                    _draggedToken = tokenVM;
                    _lastMousePosition = canvasPosition;
                    e.Pointer.Capture(tokenView); // Capture the pointer to track movement
                    return;
                }
            }

            // 2. If no token was clicked, this might be a map drag operation (panning, future feature)
            // For now, we only handle token drag.
        }

        // --- Remaining Drag Logic (PointerMoved and PointerReleased) ---

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (_draggedToken != null && _lastMousePosition.HasValue)
            {
                // Get current position relative to the Canvas
                Point currentPosition = e.GetCurrentPoint(this).Position;

                // Calculate movement delta
                double deltaX = currentPosition.X - _lastMousePosition.Value.X;
                double deltaY = currentPosition.Y - _lastMousePosition.Value.Y;

                // Calculate the Token's new screen position
                // Note: The TokenView's Canvas.Left/Top is bound to ScreenX/ScreenY.
                // We must use the current ScreenX/ScreenY as the base for the update.

                double newScreenX = _draggedToken.ScreenX + deltaX;
                double newScreenY = _draggedToken.ScreenY + deltaY;

                // Call the ViewModel's command to translate the new screen position back to logical coordinates
                _draggedToken.MoveTokenCommand.Execute(new Point(newScreenX, newScreenY));

                // Update the last position
                _lastMousePosition = currentPosition;
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            if (_draggedToken != null)
            {
                // Release the pointer capture
                e.Pointer.Capture(null);

                // Finalize movement (e.g., snap to grid is usually handled inside MoveTokenCommand)

                _draggedToken = null;
                _lastMousePosition = null;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

}