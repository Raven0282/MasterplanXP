using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.Services
{
    /// <summary>
    /// Contract for generic file operations, handling the interaction flow (dialogs)
    /// and coordinating with the serializer.
    /// </summary>
    public interface IFileOperationService
    {
        /// <summary>
        /// Prompts the user to select a location and saves the provided data model to a binary file.
        /// </summary>
        /// <typeparam name="TData">The type of data model being saved.</typeparam>
        /// <param name="data">The data model instance.</param>
        /// <param name="defaultFileName">The default file name to suggest.</param>
        /// <returns>The path saved to, or null if cancelled.</returns>
        Task<string?> SaveBinaryAsync<TData>(TData data, string defaultFileName) where TData : class;

        /// <summary>
        /// Prompts the user to select a format (JSON/XML) and location, then exports the data.
        /// </summary>
        /// <typeparam name="TData">The type of data model being exported.</typeparam>
        /// <param name="data">The data model instance.</param>
        /// <param name="defaultFileName">The default file name to suggest (without extension).</param>
        Task ExportHumanReadableAsync<TData>(TData data, string defaultFileName) where TData : class;
    }
}