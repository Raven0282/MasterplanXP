using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Views;

public partial class TrapsPageView : UserControl
{
    public TrapsPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}