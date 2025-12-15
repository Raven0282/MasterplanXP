using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Views;

public partial class ExampleModalContentView : UserControl
{
    public ExampleModalContentView()
    {
        InitializeComponent();    
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}

