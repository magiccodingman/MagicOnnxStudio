using MagicOnnxStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class ModelInfoFinder
    {
        public static List<DownloadedModelInfo> GetDownloadedModelInfos()
        {
            string rootDirectory = DownloadHelper.GetModelDownloadFolderLocation();

            // Create a list to hold the deserialized model info objects
            List<DownloadedModelInfo> modelInfoList = new List<DownloadedModelInfo>();

            // Recursively search for ModelInfo.json files in all subdirectories
            SearchDirectory(rootDirectory, modelInfoList);

            return modelInfoList;
        }

        public static List<DownloadedModelInfo> GetOnnxConvertedModelInfos()
        {
            string rootDirectory = DownloadHelper.GetOnnxModelFolderLocation();

            // Create a list to hold the deserialized model info objects
            List<DownloadedModelInfo> modelInfoList = new List<DownloadedModelInfo>();

            // Recursively search for ModelInfo.json files in all subdirectories
            SearchDirectory(rootDirectory, modelInfoList);

            return modelInfoList;
        }

        private static void SearchDirectory(string directory, List<DownloadedModelInfo> modelInfoList)
        {
            // Get all subdirectories
            string[] subDirectories = Directory.GetDirectories(directory);

            foreach (var subDirectory in subDirectories)
            {
                // Look for the ModelInfo.json in the current directory
                string modelInfoPath = Path.Combine(subDirectory, "ModelInfo.json");

                if (File.Exists(modelInfoPath))
                {
                    try
                    {
                        // Read the content of the ModelInfo.json file
                        string jsonContent = File.ReadAllText(modelInfoPath);

                        // Deserialize the content into the DownloadedModelInfo object
                        DownloadedModelInfo modelInfo = JsonSerializer.Deserialize<DownloadedModelInfo>(jsonContent);

                        if (modelInfo != null)
                        {
                            // Set the ModelPath to the current directory where the file was found
                            modelInfo.ModelPath = subDirectory;

                            // Add the deserialized object to the list
                            modelInfoList.Add(modelInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading or deserializing file: {modelInfoPath}. Exception: {ex.Message}");
                    }
                }

                // Recursively search in the subdirectories
                SearchDirectory(subDirectory, modelInfoList);
            }
        }
    }
}
