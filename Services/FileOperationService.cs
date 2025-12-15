using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterplanXP.Services;
using System.Threading.Tasks;

namespace MasterplanXP.Services
{
    public class FileOperationService : IFileOperationService
    {
        private readonly IFileService _fileService;
        private readonly IDataSerializerService _serializerService;
        // private readonly IModalService _modalService; // Required for the format dialog

        public FileOperationService(IFileService fileService, IDataSerializerService serializerService /*, IModalService modalService */)
        {
            _fileService = fileService;
            _serializerService = serializerService;
            // _modalService = modalService;
        }

        /// <inheritdoc />
        public async Task<string?> SaveBinaryAsync<TData>(TData data, string defaultFileName) where TData : class
        {
            const string extension = "bin";
            string title = $"Save {typeof(TData).Name.Replace("Data", "")}";

            string? filePath = await _fileService.ShowSaveFileDialogAsync(
                title,
                $"{defaultFileName}.{extension}",
                extension);

            if (filePath == null)
                return null;

            await _serializerService.SaveBinaryAsync(data, filePath);
            return filePath;
        }

        /// <inheritdoc />
        public async Task ExportHumanReadableAsync<TData>(TData data, string defaultFileName) where TData : class
        {
            // 1. Generic Dialog: Ask the user which format to export (JSON or XML)
            // Since we haven't defined the modal dialog infrastructure here, 
            // we will use a temporary placeholder that assumes JSON for now.

            // TODO: Replace this with a call to IModalService to ask for format (JSON or XML)
            string selectedFormat = "json";

            if (selectedFormat == "json")
            {
                const string extension = "json";
                string? filePath = await _fileService.ShowSaveFileDialogAsync(
                    $"Export {typeof(TData).Name.Replace("Data", "")} (JSON)",
                    $"{defaultFileName}.{extension}",
                    extension);

                if (filePath != null)
                {
                    await _serializerService.ExportJsonAsync(data, filePath);
                }
            }
            else if (selectedFormat == "xml")
            {
                const string extension = "xml";
                string? filePath = await _fileService.ShowSaveFileDialogAsync(
                   $"Export {typeof(TData).Name.Replace("Data", "")} (XML)",
                   $"{defaultFileName}.{extension}",
                   extension);

                if (filePath != null)
                {
                    await _serializerService.ExportXmlAsync(data, filePath);
                }
            }
        }
    }
}