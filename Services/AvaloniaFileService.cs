using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MasterplanXP.Services;
using System.IO;
using System.Reflection;

namespace MasterplanXP.Services
{
    public class AvaloniaFileService : IFileService
    {
        // Session-level tracking for the last directory used
        private string _lastUsedPath;

        // This must be set by the application's startup logic (e.g., in App.axaml.cs)
        // to provide the necessary context for Avalonia dialogs.
        private TopLevel? _topLevel;

        /// <summary>
        /// Initializes the file service, setting the initial path to the application's assembly location.
        /// </summary>
        public AvaloniaFileService()
        {
            // 1. Discover the assembly location
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            _lastUsedPath = Path.GetDirectoryName(assemblyPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        /// <summary>
        /// Sets the active TopLevel window required to display Avalonia dialogs.
        /// </summary>
        public void SetTopLevel(TopLevel topLevel)
        {
            _topLevel = topLevel;
        }

        private IStorageProvider GetStorageProvider()
        {
            if (_topLevel == null || !TopLevel.GetTopLevel(_topLevel)!.StorageProvider.CanSave)
            {
                // Fallback or throw if no valid TopLevel is available
                throw new InvalidOperationException("Avalonia TopLevel context is required to show file dialogs.");
            }
            return _topLevel.StorageProvider;
        }

        /// <inheritdoc />
        public async Task<string?> ShowSaveFileDialogAsync(string title, string extension, string suggestedFileName)
        {
            var provider = GetStorageProvider();

            var fileType = new FilePickerFileType($"{extension.ToUpperInvariant()} Files")
            {
                Patterns = new[] { $"*.{extension.ToLowerInvariant()}" }
            };

            var result = await provider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = title,
                SuggestedFileName = suggestedFileName,
                DefaultExtension = extension.ToLowerInvariant(),

                // --- FIX: Replace 'Directory' with 'SuggestedStartLocation' ---
                // The _lastUsedPath must be converted from a string path into an IStorageFolder.
                SuggestedStartLocation = await provider.TryGetFolderFromPathAsync(_lastUsedPath),
                // ---------------------------------------------------------------

                FileTypeChoices = new[] { fileType }
            });

            if (result != null)
            {
                // 2. Update the last used path for the current session
                _lastUsedPath = Path.GetDirectoryName(result.Path.LocalPath) ?? _lastUsedPath;
                return result.Path.LocalPath;
            }

            return null;
        }
        /// <inheritdoc />
        public async Task<string?> ShowOpenFileDialogAsync(string title, string extension)
        {
            var provider = GetStorageProvider();

            var fileType = new FilePickerFileType($"{extension.ToUpperInvariant()} Files")
            {
                Patterns = new[] { $"*.{extension.ToLowerInvariant()}" }
            };

            var results = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                SuggestedStartLocation = await provider.TryGetFolderFromPathAsync(_lastUsedPath), // Start from last used path
                FileTypeFilter = new[] { fileType }
            });

            if (results.Count == 1)
            {
                // 2. Update the last used path for the current session
                _lastUsedPath = Path.GetDirectoryName(results[0].Path.LocalPath) ?? _lastUsedPath;
                return results[0].Path.LocalPath;
            }

            return null;
        }
    }
}