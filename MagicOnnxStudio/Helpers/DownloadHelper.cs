using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class DownloadHelper
    {
        public static readonly string OnnxFolderName = "ONNX_Models";
        public static readonly string AiModelDownloadFolderName = "Ai_Model_Downloads";

        public static string GetModelDownloadFolderLocation()
        {
            var settings = new Settings(true);
            string? rootDirectory = settings.DefaultFolderLocation;
            if (string.IsNullOrWhiteSpace(rootDirectory))
                rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AiModelDownloadFolderName);

            return rootDirectory;
        }

        public static string GetOnnxModelFolderLocation()
        {
            var settings = new Settings(true);
            string? rootDirectory = settings.DefaultFolderLocation;
            if (string.IsNullOrWhiteSpace(rootDirectory))
                rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OnnxFolderName);

            return rootDirectory;
        }

        public string EnsureAiModelDownloadsFolder()
        {
            var settings = new Settings(true);

            // Get the current directory where the application is running
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(settings.DefaultFolderLocation)
                && Directory.Exists(settings.DefaultFolderLocation))
            {
                currentDirectory = settings.DefaultFolderLocation;
            }


            // Combine current directory with the folder name
            string fullPath = Path.Combine(currentDirectory, AiModelDownloadFolderName);

            // Check if the folder exists, if not, create it
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            // Return the full path to the folder
            return fullPath;
        }

        public string EnsureAiModelDefaultOutputFolder()
        {
            var settings = new Settings(true);

            // Get the current directory where the application is running
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(settings.DefaultFolderLocation)
                && Directory.Exists(settings.DefaultFolderLocation))
            {
                currentDirectory = settings.DefaultFolderLocation;
            }
            // Combine current directory with the folder name
            string fullPath = Path.Combine(currentDirectory, OnnxFolderName);

            // Check if the folder exists, if not, create it
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            // Return the full path to the folder
            return fullPath;
        }
    }
}
