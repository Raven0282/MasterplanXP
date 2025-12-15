using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.Services
{
    /// <summary>
    /// Defines a generalized service for handling multiple cross-platform serialization formats
    /// for various data models (MapData, CharacterData, ItemData, etc.).
    /// </summary>
    public interface IDataSerializerService
    {
        /// <summary>
        /// Loads data of type T from a high-performance binary format (e.g., MessagePack).
        /// </summary>
        /// <typeparam name="T">The type of the data model to load.</typeparam>
        /// <param name="filePath">The path to the binary file.</param>
        Task<T?> LoadBinaryAsync<T>(string filePath) where T : class;

        /// <summary>
        /// Saves data of type T to a high-performance binary format (e.g., MessagePack).
        /// </summary>
        /// <typeparam name="T">The type of the data model to save.</typeparam>
        /// <param name="data">The data model instance to serialize.</param>
        /// <param name="filePath">The path to the binary file.</param>
        Task SaveBinaryAsync<T>(T data, string filePath) where T : class;

        /// <summary>
        /// Exports data of type T to a human-readable JSON format (for interoperability).
        /// </summary>
        /// <typeparam name="T">The type of the data model to export.</typeparam>
        /// <param name="data">The data model instance to serialize.</param>
        /// <param name="filePath">The path for the JSON export file.</param>
        Task ExportJsonAsync<T>(T data, string filePath) where T : class;

        /// <summary>
        /// Exports data of type T to a human-readable XML format (for interoperability).
        /// </summary>
        /// <typeparam name="T">The type of the data model to export.</typeparam>
        /// <param name="data">The data model instance to serialize.</param>
        /// <param name="filePath">The path for the XML export file.</param>
        Task ExportXmlAsync<T>(T data, string filePath) where T : class;
    }
}