using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class FileHelper
    {

        public static bool AreFilesInDirectory(string directoryPath)
        {
            // Check if the directory exists first
            if (Directory.Exists(directoryPath))
            {
                // Check if there are any files in the directory
                string[] files = Directory.GetFiles(directoryPath);
                return files.Length > 0;
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
                return false;
            }
        }
        public static long CalculateFolderSize(string folderPath)
        {
            // Get all files in the directory and subdirectories, sum their sizes
            return Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
                            .Sum(file => new FileInfo(file).Length);
        }

        public static void DeleteLocation(string location)
        {

            // Create the temp directory
            Directory.CreateDirectory(location);
            // Try to delete the temporary folder contents
            try
            {
                // First, remove the read-only attributes from files and folders
                RemoveReadOnlyAttributes(new DirectoryInfo(location));
                Directory.Delete(location, true); // true to delete recursively and permanently
            }
            catch (Exception ex)
            {
                // Log the error if needed, or just ignore
                Console.WriteLine($"Failed to delete folder: {ex.Message}");
                // Optionally, use a logging framework to log the exception
            }
            // Create the temp directory
            Directory.CreateDirectory(location);
        }

        public static void RemoveReadOnlyAttributes(DirectoryInfo directoryInfo)
        {
            // Iterate over all files and remove read-only attributes
            foreach (FileInfo file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                file.Attributes = FileAttributes.Normal;
            }

            // Iterate over all directories and remove read-only attributes
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories("*", SearchOption.AllDirectories))
            {
                dir.Attributes = FileAttributes.Normal;
            }
        }
    }
}
