using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class PythonEnvironmentInitializer
    {       

        public static void InitializePythonEnvironments()
        {
            // Get the base directory of where the application is running
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Path to the "Environments" folder
            string environmentsPath = Path.Combine(baseDirectory, "Environments");

            // Define the new folder paths
            string directMLPath = Path.Combine(environmentsPath, "Python310_DirectML");
            string cudaPath11 = Path.Combine(environmentsPath, "Python310_Cuda11");
            string cudaPath12 = Path.Combine(environmentsPath, "Python310_Cuda12");
            string cpuPath = Path.Combine(environmentsPath, "Python310_Cpu");

            Cache.DirectMlPythonPath = directMLPath;
            Cache.Cuda11MlPythonPath = cudaPath11;
            Cache.Cuda12MlPythonPath = cudaPath12;
            Cache.CpuMlPythonPath = cpuPath;

            // Ensure the "Environments" folder exists
            if (!Directory.Exists(environmentsPath))
            {
                Console.WriteLine($"The Environments folder does not exist at: {environmentsPath}");
                return;
            }

            // Path to the "Python310" folder
            string python310Path = Path.Combine(environmentsPath, "Python310");

            // Check if the "Python310" folder exists
            if (!Directory.Exists(python310Path))
            {
                Console.WriteLine($"The Python310 folder does not exist at: {python310Path}");
                return;
            }

            

            // Check if each folder exists, copy if not, and verify the file integrity
            CopyAndVerifyFolder(python310Path, directMLPath);
            CopyAndVerifyFolder(python310Path, cudaPath11);
            CopyAndVerifyFolder(python310Path, cudaPath12);
            CopyAndVerifyFolder(python310Path, cpuPath);
        }        

        private static void CopyAndVerifyFolder(string sourceFolder, string destinationFolder)
        {
            // Check if the destination folder already exists
            if (!Directory.Exists(destinationFolder))
            {
                try
                {
                    // Copy the folder recursively
                    DirectoryCopy(sourceFolder, destinationFolder, true);
                    Console.WriteLine($"Copied {sourceFolder} to {destinationFolder}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to copy folder from {sourceFolder} to {destinationFolder}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"The folder {destinationFolder} already exists.");
            }

            // Verify that all files were copied and match or exceed the size of the source files
            if (VerifyFiles(sourceFolder, destinationFolder))
            {
                Console.WriteLine($"Verification succeeded: All files in {destinationFolder} match or exceed the size of those in {sourceFolder}.");
            }
            else
            {
                Console.WriteLine($"Verification failed: Some files in {destinationFolder} are missing or have a size mismatch.");
            }
        }

        // Method to copy a directory recursively
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        // Method to verify that all files in the source folder are present and the same size or larger in the destination folder
        private static bool VerifyFiles(string sourceFolder, string destinationFolder)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourceFolder);
            DirectoryInfo destDir = new DirectoryInfo(destinationFolder);

            // Get the files in the source and destination directories
            FileInfo[] sourceFiles = sourceDir.GetFiles("*", SearchOption.AllDirectories);
            FileInfo[] destFiles = destDir.GetFiles("*", SearchOption.AllDirectories);

            // Create a dictionary of the destination files for quick lookup
            var destFileDict = new Dictionary<string, FileInfo>();
            foreach (var destFile in destFiles)
            {
                // Use relative paths for comparison
                string relativePath = Path.GetRelativePath(destDir.FullName, destFile.FullName);
                destFileDict[relativePath] = destFile;
            }

            // Iterate through each source file and verify its existence and size in the destination folder
            foreach (var sourceFile in sourceFiles)
            {
                string relativePath = Path.GetRelativePath(sourceDir.FullName, sourceFile.FullName);

                if (destFileDict.TryGetValue(relativePath, out var destFile))
                {
                    if (destFile.Length < sourceFile.Length)
                    {
                        // If the destination file is smaller than the source file, verification fails
                        return false;
                    }
                }
                else
                {
                    // If the destination file doesn't exist, verification fails
                    return false;
                }
            }

            // All files were verified successfully
            return true;
        }
    }
}
