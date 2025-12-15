using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Views;

public partial class GenericHostWindow : Window
{
    public GenericHostWindow()
    {
        InitializeComponent();
    }

        private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}