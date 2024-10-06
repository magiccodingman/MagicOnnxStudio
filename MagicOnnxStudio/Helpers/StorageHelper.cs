using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class StorageHelper
    {
        public static string GetDriveDirectory(string filePath)
        {
            try
            {
                // Ensure the path is not null or empty
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("File path cannot be null or empty.");
                }

                // Get the root directory (drive letter) of the provided path
                string driveDirectory = Path.GetPathRoot(filePath);

                // Ensure a valid drive directory is found
                if (string.IsNullOrWhiteSpace(driveDirectory))
                {
                    throw new InvalidOperationException("Could not determine the drive from the provided path.");
                }

                return driveDirectory;
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as necessary
                Console.WriteLine($"Error: {ex.Message}");
                return null; // or return string.Empty if you prefer
            }
        }

        public static double GetAvailableStorageInGB(string targetDrive = null)
        {
            // Get the drive where the application is running if no target drive is specified
            if (string.IsNullOrEmpty(targetDrive))
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                targetDrive = Path.GetPathRoot(currentDirectory); // Get the root of the current directory (e.g., "C:\")
            }

            // Check if the target drive exists
            if (!DriveInfo.GetDrives().Any(d => d.Name.Equals(targetDrive, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("The specified drive does not exist.");
            }

            // Get information about the specified drive
            DriveInfo driveInfo = new DriveInfo(targetDrive);

            // Ensure the drive is ready (e.g., mounted)
            if (!driveInfo.IsReady)
            {
                throw new IOException("The drive is not ready.");
            }

            // Get the available free space on the drive in bytes
            long availableBytes = driveInfo.AvailableFreeSpace;

            // Convert bytes to gigabytes (1 GB = 1,073,741,824 bytes)
            double availableGB = availableBytes / (1024.0 * 1024.0 * 1024.0);

            return availableGB;
        }

        public static double GetTotalStorageInGB(string targetDrive = null)
        {
            // Get the drive where the application is running if no target drive is specified
            if (string.IsNullOrEmpty(targetDrive))
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                targetDrive = Path.GetPathRoot(currentDirectory); // Get the root of the current directory (e.g., "C:\")
            }

            // Check if the target drive exists
            if (!DriveInfo.GetDrives().Any(d => d.Name.Equals(targetDrive, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("The specified drive does not exist.");
            }

            // Get information about the specified drive
            DriveInfo driveInfo = new DriveInfo(targetDrive);

            // Ensure the drive is ready (e.g., mounted)
            if (!driveInfo.IsReady)
            {
                throw new IOException("The drive is not ready.");
            }

            // Get the available free space on the drive in bytes
            long availableBytes = driveInfo.TotalSize;

            // Convert bytes to gigabytes (1 GB = 1,073,741,824 bytes)
            double availableGB = availableBytes / (1024.0 * 1024.0 * 1024.0);

            return availableGB;
        }
    }
}
