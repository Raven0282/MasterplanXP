using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MasterplanXP.Services;
using MasterplanXP.ViewModels;
using MasterplanXP.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

[assembly: XmlnsDefinition("https://github.com/avaloniaui", "MasterplanXP.Controls")]

namespace MasterplanXP
{
    public partial class App : Application
    {
        // Expose the ServiceProvider statically for simpler ViewModel resolution if needed later
        public static IServiceProvider ServiceProvider { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

#if DEBUG
                this.AttachDeveloperTools();
#endif
        }


        public override void OnFrameworkInitializationCompleted()
        {

            // 1. Setup DI container
            var services = new ServiceCollection();

            // 2. Register core services
            // FIX: The IDataTemplate (ViewLocator) must be registered for WindowService
            // registration to succeed (resolves System.InvalidOperationException).
            services.AddSingleton<IDataTemplate, ViewLocator>();
            // For Modal windows
            services.AddSingleton<IModalService, ModalService>();
            // For Non-Modal windows (The WindowService depends on IDataTemplate)
            services.AddSingleton<IWindowService, WindowService>();
            // Register other required services (inferred from MapWindowViewModel constructor)
            services.AddSingleton<IDataSerializerService, DataSerializerService>(); // <--- ASSUMED SERVICE
            services.AddSingleton<IFileService, AvaloniaFileService>();            // <--- ASSUMED SERVICE
            services.AddSingleton<IFileOperationService, FileOperationService>();  // <--- ASSUMED SERVICE
            // Handles Beta App logic
            services.AddSingleton<IAppConfigurationService, AppConfigurationService>(); // <--- ADDED


            // 3. Register ViewModels
            // ... other registrations ...
            // Register ALL ViewModels (MapViewModel, MenuPageViewModel etc.)
            services.AddSingleton<MenuPageViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<MapViewModel>();
            services.AddSingleton<SettingsPageViewModel>();
            services.AddSingleton<CreaturePageViewModel>();
            services.AddTransient<MapWindowViewModel>();
            services.AddTransient<GenericHostWindowViewModel>();
            // ADDED: Register the CreatureEditorViewModel
            services.AddSingleton<CreatureEditorViewModel>();


            // Sample page for testing
            services.AddSingleton<HostWindowViewModel>();

            // Register as Transient any view model intended for display *inside* the dialog
            services.AddTransient<ExampleModalContentViewModel>();
            //services.AddTransient<MapViewModel>();

            // Build the provider
            ServiceProvider = services.BuildServiceProvider();

            // *****************************************************************
            // NEW: Register the Modal View/ViewModel Mappings
           
            var modalService = ServiceProvider.GetRequiredService<IModalService>();


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                // 2. Resolve and set up Main Window
                var mainWindow = new MainWindow();
                var hostWindow = new HostWindow();
               
                // Resolve the ViewModel using the provider
                var menuViewModel = ServiceProvider.GetRequiredService<MenuPageViewModel>();
                var mainWindowViewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
                var hostWindowViewModel = ServiceProvider.GetRequiredService<HostWindowViewModel>();

                desktop.MainWindow = mainWindow;
                //desktop.MainWindow.DataContext = menuViewModel;
                desktop.MainWindow.DataContext = mainWindowViewModel;

                // SPECIAL STEP: Set the TopLevel context for the file service (if using AvaloniaFileService)
                // This ensures dialogs can be opened from the service.
                // You must update AvaloniaFileService to have a SetTopLevel method or use a factory.
                // Assuming SetTopLevel exists:
                if (ServiceProvider.GetRequiredService<IFileService>() is AvaloniaFileService fileService)
                {
                    fileService.SetTopLevel(desktop.MainWindow);
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}