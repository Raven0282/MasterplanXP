using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MasterplanXP.Views;

public partial class LibraryContentPageView : UserControl
{
    public LibraryContentPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}