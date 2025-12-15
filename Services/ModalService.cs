// MasterplanXP/Services/ModalService.cs
using Avalonia.Controls;
using MasterplanXP.ViewModels;
using MasterplanXP.Views;
using MasterplanXP.Services;
using MasterplanXP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MasterplanXP.Services;

/// <summary>
/// Provides an implementation of <see cref="IModalService"/> for showing Avalonia dialogs.
/// </summary>
public class ModalService : IModalService
{
    /// <inheritdoc/>
    public async Task<TResult?> ShowModalAsync<TViewModel, TResult>(
        Window hostWindow,
        TViewModel viewModel,
        string title,
        double width,
        double height)
        where TViewModel : class
    {
        // 1. Create the dialog window
        var dialog = new ModalDialog
        {
            // 2. Apply customization properties
            Title = title,
            Width = width,
            Height = height,

            // 3. Set the view model as the DataContext.
            // The ModalDialog's ContentControl will find the corresponding View via the ViewLocator.
            DataContext = viewModel,

            // 4. Ensure the dialog is tied to the host for modality
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        // 5. Show the dialog modally and wait for the result
        // Use 'ShowDialog<TResult>' which handles cross-platform modality.
        return await dialog.ShowDialog<TResult?>(hostWindow);
    }
}