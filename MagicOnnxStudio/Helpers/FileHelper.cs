using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class FileHelper
    {

        public static void MoveAllItemsRecursively(string sourceDir, string destDir)
        {
            // Step 1: Ensure the source directory exists
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDir}");
            }

            // Step 2: Ensure the destination directory exists, if not, create it
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Step 3: Move all the files from the source to the destination
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destDir, fileName);

                // Move the file to the new location
                File.Move(file, destFile);
            }

            // Step 4: Move all the subdirectories recursively
            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(directory);
                string destSubDir = Path.Combine(destDir, dirName);

                // Recursive call to move all contents of the subdirectory
                MoveAllItemsRecursively(directory, destSubDir);

                // Delete the empty source subdirectory after moving its contents
                Directory.Delete(directory);
            }

            // Step 5: Optionally delete the source directory if it's empty now (optional cleanup)
            if (Directory.GetFiles(sourceDir).Length == 0 && Directory.GetDirectories(sourceDir).Length == 0)
            {
                Directory.Delete(sourceDir);
            }
        }
        public static string CreateAndCleanTempMagicAIFolder()
        {
            // Step 1: Get the current logged-in user's profile path
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Step 2: Define the path for the "TempMagicAI" folder inside the user profile
            string tempMagicAIPath = Path.Combine(userProfile, "TempMagicAI");

            // Step 3: Check if the folder already exists
            if (Directory.Exists(tempMagicAIPath))
            {
                // If it exists, delete all contents within it recursively
                DirectoryInfo directoryInfo = new DirectoryInfo(tempMagicAIPath);

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete(); // Permanently delete the file
                }

                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true); // Recursively delete the subdirectories
                }
            }
            else
            {
                // If the folder does not exist, create it
                Directory.CreateDirectory(tempMagicAIPath);
            }

            // Step 4: Return the path of the TempMagicAI folder
            return tempMagicAIPath;
        }
        public static void OpenFolderInExplorer(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new ArgumentException("Directory path is invalid.", nameof(folderPath));
            }

            // Ensure the directory exists before trying to open it
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else
            {
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");
            }
        }
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
        
        public static string GetCleanTempPath()
        {
            // Get the system's temporary folder path
            string tempPath = Path.GetTempPath();
            
            // Create the full path for the "MagicTempOnnxStudio" folder
            string magicTempFolderPath = Path.Combine(tempPath, "MagicTempOnnxStudio");
            DeleteLocation(magicTempFolderPath);
            Directory.CreateDirectory(magicTempFolderPath);

            return magicTempFolderPath;
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
