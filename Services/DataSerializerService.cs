using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterplanXP.Services;
using MessagePack;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace MasterplanXP.Services
{
    /// <summary>
    /// Concrete implementation of the IDataSerializerService using MessagePack, System.Text.Json, and XML.
    /// </summary>
    public class DataSerializerService : IDataSerializerService
    {
        // --- Binary Serialization (High Performance Save) ---

        /// <inheritdoc />
        public async Task<T?> LoadBinaryAsync<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
                return null;

            using FileStream stream = File.OpenRead(filePath);
            // Use MessagePack's recommended async deserialization
            return await MessagePackSerializer.DeserializeAsync<T>(stream).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task SaveBinaryAsync<T>(T data, string filePath) where T : class
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Ensure the directory exists before writing
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using FileStream stream = File.Create(filePath);
            // Use MessagePack's recommended async serialization
            await MessagePackSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
        }

        // --- JSON Serialization (Human-Readable Export) ---

        /// <inheritdoc />
        public async Task ExportJsonAsync<T>(T data, string filePath) where T : class
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Configure JSON options for human readability (Indented)
            var options = new JsonSerializerOptions { WriteIndented = true };

            // Ensure the directory exists before writing
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using FileStream stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, data, options).ConfigureAwait(false);
        }

        // --- XML Serialization (Human-Readable Export) ---

        /// <inheritdoc />
        public async Task ExportXmlAsync<T>(T data, string filePath) where T : class
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Ensure the directory exists before writing
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            // System.Xml.Serialization requires the use of synchronous I/O for simple implementation
            // Using Task.Run to push synchronous file writing onto a background thread to prevent UI blocking
            await Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StreamWriter(filePath);
                serializer.Serialize(writer, data);
            });
        }
    }
}