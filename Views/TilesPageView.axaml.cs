using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Views;

public partial class TilesPageView : UserControl
{
    public TilesPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}