
// MasterplanXP/Services/IModalService.cs
using Avalonia.Controls;
using System.Threading.Tasks;
using MasterplanXP.ViewModels;

namespace MasterplanXP.Services;

/// <summary>
/// Defines a service for showing modal dialogs with customizable properties.
/// </summary>
public interface IModalService
{
    /// <summary>
    /// Shows a modal dialog for the given view model content.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model for the dialog's content.</typeparam>
    /// <typeparam name="TResult">The expected result type from the dialog (e.g., bool, string, custom object).</typeparam>
    /// <param name="hostWindow">The host window that owns the dialog (for modality).</param>
    /// <param name="viewModel">The view model instance to set as the dialog's DataContext.</param>
    /// <param name="title">The title for the dialog window.</param>
    /// <param name="width">The desired width of the dialog window.</param>
    /// <param name="height">The desired height of the dialog window.</param>
    /// <returns>A Task that returns the result when the dialog is closed.</returns>
    Task<TResult?> ShowModalAsync<TViewModel, TResult>(
        Window hostWindow,
        TViewModel viewModel,
        string title,
        double width,
        double height)
        where TViewModel : class;
}