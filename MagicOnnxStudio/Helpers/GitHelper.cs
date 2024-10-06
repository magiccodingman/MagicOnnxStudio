using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class GitHelper
    {
        public static (string owner, string repo) ParseUrl(string url)
        {
            // Remove any trailing slash from the URL
            url = url.TrimEnd('/');

            // Check if it's a valid URL format for GitHub or HuggingFace
            if (url.Contains("github.com") || url.Contains("huggingface.co"))
            {
                // Split the URL by '/' to break it into components
                string[] parts = url.Split('/');

                // Check if the URL has enough parts to contain an owner and repo
                if (parts.Length >= 2)
                {
                    // Get the last two parts (owner and repo)
                    string owner = parts[parts.Length - 2];
                    string repo = parts[parts.Length - 1];

                    return (owner, repo);
                }
                else
                {
                    throw new ArgumentException("URL format is invalid. It should contain both an owner and repo.");
                }
            }
            else
            {
                throw new ArgumentException("URL is not a valid GitHub or HuggingFace URL.");
            }
        }
        public async Task<bool> CheckGitAvailabilityAsync()
        {
            return await IsCommandAvailable("git", "--version");
        }

        public async Task<bool> CheckGitLfsAvailabilityAsync()
        {
            return await IsCommandAvailable("git-lfs", "version");
        }

        public async Task CloneRepositoryAsync(string repoUrl, string outputDir)
        {
            // Check if Git is available
            if (!await CheckGitAvailabilityAsync())
            {
                throw new Exception("Git is not installed or not available in the system's PATH.");
            }

            // Check if Git LFS is available
            if (!await CheckGitLfsAvailabilityAsync())
            {
                throw new Exception("Git LFS is not installed or not available in the system's PATH.");
            }

            try
            {
                // Ensure output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                Console.WriteLine("Cloning repository...");

                // Run git clone command
                await RunCommandAsync("git", $"clone {repoUrl} {outputDir}");

                // Run git-lfs pull command to fetch LFS objects
                Console.WriteLine("Fetching Git LFS objects...");
                await RunCommandAsync("git-lfs", "pull", outputDir);

                Console.WriteLine("Repository cloned successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while cloning repository: {ex.Message}");
            }
        }

        private async Task<bool> IsCommandAvailable(string command, string arguments)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string output = await process.StandardOutput.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    // Check if the command returns a version or other recognizable output
                    return process.ExitCode == 0 && !string.IsNullOrEmpty(output);
                }
            }
            catch
            {
                return false; // If an exception occurs, the command isn't available
            }
        }

        private async Task RunCommandAsync(string command, string arguments, string workingDirectory = "")
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                WorkingDirectory = !string.IsNullOrEmpty(workingDirectory) ? workingDirectory : Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = processStartInfo
            };

            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Command '{command} {arguments}' failed with exit code {process.ExitCode}");
            }
        }
    }

}
