using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.Services
{
    /// <summary>
    /// Contract for showing cross-platform file dialogs (Open, Save, Export).
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Shows a Save File Dialog to prompt the user for a save location.
        /// </summary>
        /// <param name="title">The dialog window title.</param>
        /// <param name="defaultFileName">The default file name.</param>
        /// <param name="extension">The file extension filter (e.g., "bin", "json").</param>
        /// <returns>The selected file path, or null if cancelled.</returns>
        Task<string?> ShowSaveFileDialogAsync(string title, string defaultFileName, string extension);

        /// <summary>
        /// Shows an Open File Dialog to prompt the user for a file to load.
        /// </summary>
        /// <param name="title">The dialog window title.</param>
        /// <param name="extension">The file extension filter (e.g., "bin").</param>
        /// <returns>The selected file path, or null if cancelled.</returns>
        Task<string?> ShowOpenFileDialogAsync(string title, string extension);
    }
}