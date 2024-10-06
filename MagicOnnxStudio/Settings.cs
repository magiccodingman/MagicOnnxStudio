using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagicOnnxStudio
{
    public class Settings
    {
        private static readonly string FileName = "settings.json";
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
        private bool _saveOnChange { get; set; } = false;
        public Settings(bool LoadFile = false, bool readOnly = false)
        {
            if (LoadFile == true)
            {
                LoadFromFile();
                if (readOnly == false)
                    _saveOnChange = true;
            }
        }
        public Settings()
        {
            _saveOnChange = false;
        }
        // Properties
        private string? _defaultFolderLocation;
        public string? DefaultFolderLocation
        {
            get => _defaultFolderLocation;
            set
            {
                _defaultFolderLocation = value;
                if (_saveOnChange)
                    SaveToFile();
            }
        }

        // Method to create or load the settings.json file
        private void LoadFromFile()
        {
            if (!File.Exists(FilePath))
            {
                // If the file doesn't exist, create a new file with default settings
                SaveToFile();
            }
            else
            {
                bool previousSaveOnChange = _saveOnChange;
                // Read the file and deserialize its content into the current class
                try
                {
                    _saveOnChange = false;
                    string json = File.ReadAllText(FilePath);
                    var loadedSettings = JsonSerializer.Deserialize<Settings>(json);

                    if (loadedSettings != null)
                    {
                        _defaultFolderLocation = loadedSettings.DefaultFolderLocation;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading settings: {ex.Message}");
                }
                finally
                {
                    _saveOnChange = previousSaveOnChange;
                }
            }
        }

        // Method to save the current class state to the settings.json file
        private void SaveToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        // Static method to get the full path of the settings file
        public static string GetSettingsFilePath()
        {
            return FilePath;
        }
    }
}
