using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MasterplanXP.ViewModels;

namespace MasterplanXP;

public partial class ModalHostWindow : Window
{
    public ModalHostWindow()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // MODIFIED: New constructor that accepts content, viewmodel, title, width, and height.
    public ModalHostWindow(Control content, object viewModel, string title, double width, double height) : this()
    {
        // 1. Set the Title
        this.Title = title;

        // 2. Set the content (the UserControl)
        this.Content = content;

        // 3. Set the DataContext (the ViewModel)
        this.DataContext = viewModel;

        // Set desired size from parameters
        this.Width = width;
        this.Height = height;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

}