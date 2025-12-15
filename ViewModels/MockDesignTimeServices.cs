using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using MasterplanXP.Services;
using MasterplanXP.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MasterplanXP.ViewModels
{
    // --- Design-Time Implementations ---

    // 1. IModalService Mock
    public class DesignModalService : IModalService
    {
        public Task<TResult?> ShowModalAsync<TViewModel, TResult>(Window hostWindow, TViewModel viewModel, string title, double width, double height) where TViewModel : class
            => Task.FromResult(default(TResult));
    }

    // 2. IWindowService Mock
    public class DesignWindowService : IWindowService
    {
        public Task ShowWindowAsync<TViewModel>(TViewModel viewModel) where TViewModel : ObservableObject
            => Task.CompletedTask;
        public void CloseWindow<TViewModel>() where TViewModel : ObservableObject { /* No op */ }
        public void CloseWindow(ObservableObject viewModelInstance) { /* No op */ }
    }

    // 3. IFileService Mock (Included for completeness, in case a command uses it)
    public class DesignFileService : IFileService
    {
        public Task<string?> ShowSaveFileDialogAsync(string title, string defaultFileName, string extension) => Task.FromResult<string?>(null);
        public Task<string?> ShowOpenFileDialogAsync(string title, string extension) => Task.FromResult<string?>(null);
    }

    // 4. IDataSerializerService Mock (Included for completeness)
    public class DesignDataSerializerService : IDataSerializerService
    {
        public Task<T?> LoadBinaryAsync<T>(string filePath) where T : class => Task.FromResult<T?>(null);
        public Task SaveBinaryAsync<T>(T data, string filePath) where T : class => Task.CompletedTask;
        public Task ExportJsonAsync<T>(T data, string filePath) where T : class => Task.CompletedTask;
        public Task ExportXmlAsync<T>(T data, string filePath) where T : class => Task.CompletedTask;
    }

    // 5. IServiceProvider Mock
    // The most reliable mock is one that always returns a *design-time* version of any requested service.
    public class DesignServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            // Simple mock: we return a design-time service if we know it.
            if (serviceType == typeof(IModalService)) return new DesignModalService();
            if (serviceType == typeof(IWindowService)) return new DesignWindowService();
            // IMPORTANT: To resolve MapViewModel, we need to mock it as well.
            if (serviceType.Name == "MapViewModel")
                // We assume MapViewModel has a parameterless design constructor
                return Activator.CreateInstance(serviceType);

            return null;
        }
    }
}