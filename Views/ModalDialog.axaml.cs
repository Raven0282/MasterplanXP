using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MasterplanXP.ViewModels;
using MasterplanXP.Views;
using System;

namespace MasterplanXP.Views;

/// <summary>
/// A generic Avalonia Window designed to be used as a modal dialog.
/// It receives its content via the DataContext/ViewModel set by the ModalService.
/// </summary>
public partial class ModalDialog : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModalDialog"/> class.
    /// </summary>
    public ModalDialog()
    {
        InitializeComponent();
    }

    // Standard Avalonia method for initializing XAML components
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Overrides the DataContext changed event to set up the dialog closing logic.
    /// This is where the ViewModel is linked back to the View (Window) to control the close action.
    /// </summary>
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        // We check if the DataContext is the expected dialog content ViewModel
        if (DataContext is ExampleModalContentViewModel contentViewModel)
        {
            // Set the CloseDialog Action on the ViewModel. 
            // When the ViewModel calls this, it executes the Window's Close method 
            // and passes the result (TResult, which is string? in this example).
            contentViewModel.CloseDialog = (result) =>
            {
                // Note: The Close method is overloaded in Avalonia to accept a result object.
                // This result is then returned by the service's await dialog.ShowDialog<TResult?>() call.
                Close(result);
            };
        }
    }
}