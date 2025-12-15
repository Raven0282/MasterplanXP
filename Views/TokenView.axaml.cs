using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Views
{
    /// <summary>
    /// Partial class for the TokenView code-behind.
    /// Primarily handles initialization and provides a context for the parent MapView's pointer tracking.
    /// </summary>
    public partial class TokenView : UserControl
    {
        public TokenView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}