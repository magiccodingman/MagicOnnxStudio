using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Models
{
    public class DownloadedModelInfo
    {
        public string Owner { get; set; }
        public string Repo { get; set; }
        public string ProjectUrl { get; set; }
        public double TotalSizeGB { get; set; }

        public double TotalSizeGB_Rounded2
        {
            get
            {
                return Math.Round(TotalSizeGB, 2);
            }
        }

        public string ModelPath { get; set; }
        public string precision { get; set; }
        public string execution { get; set; }

        // Read-only property to check for .safetensors files in the directory
        public bool IsSafeTensor
        {
            get
            {
                // Check if the directory exists before searching for files
                if (Directory.Exists(ModelPath))
                {
                    // Get all files in the directory and check if any have a ".safetensors" extension (case-insensitive)
                    return Directory.GetFiles(ModelPath)
                                    .Any(file => file.EndsWith(".safetensors", System.StringComparison.OrdinalIgnoreCase));
                }
                return false; // Return false if the directory doesn't exist or no matching files were found
            }
        }


        // Read-only property to check for .gguf files in the directory
        public bool IsGGUF
        {
            get
            {
                // Check if the directory exists before searching for files
                if (Directory.Exists(ModelPath))
                {
                    // Get all files in the directory and check if any have a ".gguf" extension (case-insensitive)
                    return Directory.GetFiles(ModelPath)
                                    .Any(file => file.EndsWith(".gguf", System.StringComparison.OrdinalIgnoreCase));
                }
                return false; // Return false if the directory doesn't exist or no matching files were found
            }
        }
    }
}
