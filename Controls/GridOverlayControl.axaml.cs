using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Controls
{
    /// <summary>
    /// Partial class for the GridOverlayControl code-behind.
    /// All custom drawing logic is handled within the base class definition (GridOverlayControl.cs).
    /// </summary>
    // NOTE: The main class definition must be 'public partial class GridOverlayControl : Control' 
    // to match the definition in GridOverlayControl.cs.
    public partial class GridOverlayControl : Control
    {
        // REMOVED: public GridOverlayControl() constructor to resolve CS0111.

        /// <summary>
        /// Required by Avalonia for component initialization.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}