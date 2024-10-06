using MagicOnnxStudio.Helpers;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Services
{
    public class PythonEnvironmentService //: IDisposable
    {
        private static readonly object _lock = new object(); // Ensure thread-safety
        private static PythonEnvironmentService _instance; // Singleton instance
        private bool _initialized;
        public string _PythonPath { get; set; }
        // Private constructor for singleton
        //private PythonEnvironmentService() { }

        // Singleton instance accessor
        /*public static PythonEnvironmentService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PythonEnvironmentService();
                        }
                    }
                }
                return _instance;
            }
        }*/

        // Initializes PythonNET with the environment located in the specified path
        public void InitializePythonEnvironment(string pythonPath)
        {
            //lock (_lock)
            //{
            //if (_initialized)
            //{
            //    Console.WriteLine("Python environment is already initialized.");
            //    return;
            //}

            if (string.IsNullOrWhiteSpace(pythonPath) || !Directory.Exists(pythonPath))
            {
                throw new ArgumentException("Invalid or non-existent Python path provided.");
            }
            _PythonPath = pythonPath;
            /*


                            // Set the Python DLL path
                            string pythonDllPath = Path.Combine(pythonPath, "python310.dll"); // Adjust version if necessary

                            if (!File.Exists(pythonDllPath))
                            {
                                throw new FileNotFoundException("Could not find python310.dll in the embedded environment.");
                            }

                            // Set Python.Runtime.PythonDLL to the path of python310.dll
                            Runtime.PythonDLL = pythonDllPath;
                            // Set the environment variable to disable BinaryFormatter usage
                            Environment.SetEnvironmentVariable("PYTHONNET_DISABLE_STASH", "1", EnvironmentVariableTarget.Process);
                            // Set the PYTHONHOME and PYTHONPATH to the embedded environment
                            SetPythonPaths(pythonPath);

                            // Initialize PythonNET with the selected Python environment
                            try
                            {
                                PythonEngine.Initialize();
                                _initialized = true;
                                Console.WriteLine("Python environment initialized successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Failed to initialize Python environment: {ex.Message}");
                                throw;
                            }*/
            //}
        }

        // Set the Python environment paths for PythonNET to target the embedded environment
        //private void SetPythonPaths(string pythonPath)
        //{
        //    // Set PYTHONHOME to point to the embedded Python directory
        //    Environment.SetEnvironmentVariable("PYTHONHOME", pythonPath, EnvironmentVariableTarget.Process);

        //    // Set PYTHONPATH to include the embedded Python's Lib and site-packages directories
        //    string pythonLibPath = Path.Combine(pythonPath, "Lib");
        //    string pythonSitePackages = Path.Combine(pythonLibPath, "site-packages");

        //    // Update PYTHONPATH to include the necessary directories
        //    string currentPythonPath = Environment.GetEnvironmentVariable("PYTHONPATH") ?? string.Empty;
        //    string newPythonPath = $"{pythonLibPath};{pythonSitePackages};{currentPythonPath}";

        //    Environment.SetEnvironmentVariable("PYTHONPATH", newPythonPath, EnvironmentVariableTarget.Process);

        //    // Add the directory with the DLL to the system PATH
        //    string currentPath = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        //    string scriptsPath = Path.Combine(pythonPath, "Scripts"); // Path to the Scripts folder (pip, etc.)

        //    // Add both the main Python path and Scripts folder to PATH if they aren't already in it
        //    if (!currentPath.Contains(pythonPath) || !currentPath.Contains(scriptsPath))
        //    {
        //        string newPath = $"{pythonPath};{scriptsPath};{currentPath}";
        //        Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Process);
        //    }
        //}


        // Run get-pip.py to install pip
        public IEnumerable<string> RunGetPip()
        {
            if (string.IsNullOrWhiteSpace(_PythonPath))
            {
                throw new InvalidOperationException("Python environment path is not set.");
            }

            // Locate the get-pip.py script in the Python environment directory
            string getPipScriptPath = Path.Combine(_PythonPath, "get-pip.py");

            if (!File.Exists(getPipScriptPath))
            {
                throw new FileNotFoundException($"get-pip.py not found at: {getPipScriptPath}");
            }

            // Path to the python.exe executable
            string pythonExePath = Path.Combine(_PythonPath, "python.exe");

            if (!File.Exists(pythonExePath))
            {
                throw new FileNotFoundException($"python.exe not found in the Python environment: {pythonExePath}");
            }

            // Initialize the process to run python.exe with the get-pip.py script
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = $"\"{getPipScriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _PythonPath // Set the working directory to the Python environment folder
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Read output (both stdout and stderr)
                using (StreamReader outputReader = process.StandardOutput)
                using (StreamReader errorReader = process.StandardError)
                {
                    string line;

                    // First, capture stdout
                    while ((line = outputReader.ReadLine()) != null)
                    {
                        yield return line;
                    }

                    // Now, capture stderr if there are any errors
                    while ((line = errorReader.ReadLine()) != null)
                    {
                        yield return $"ERROR: {line}";
                    }
                }

                process.WaitForExit();
            }
        }


        // Check if pip is installed by running `python -m pip --version`
        // Check if pip is installed by looking for pip3.10.exe in Scripts folder and running `--version`
        public bool IsPipInstalled()
        {
            if (string.IsNullOrWhiteSpace(_PythonPath))
            {
                throw new InvalidOperationException("Python environment path is not set.");
            }

            // Path to pip3.10.exe in the Scripts folder
            string pipExePath = Path.Combine(_PythonPath, "Scripts", "pip3.10.exe");

            if (File.Exists(pipExePath))
            {
                return true;
            }

            return false;
        }

        // Run pip3.10.exe to install a package
        public IEnumerable<string> PipCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(_PythonPath))
            {
                throw new InvalidOperationException("Python environment path is not set.");
            }

            // Path to the python.exe executable in the Python environment
            string pythonExePath = Path.Combine(_PythonPath, "python.exe");

            if (!File.Exists(pythonExePath))
            {
                yield return "python.exe not found.";
                yield break;
            }

            // The command to be executed (e.g., ".\python.exe pip_runner.py install --pre onnxruntime-genai")
            string fullCommand = $".\\python.exe pip_runner.py {command}";

            // Initialize the process to run cmd.exe with the command
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {fullCommand}",  // Use /c to run the command and then terminate
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _PythonPath // Set working directory to the Python environment
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Read output (both stdout and stderr)
                using (StreamReader outputReader = process.StandardOutput)
                using (StreamReader errorReader = process.StandardError)
                {
                    string line;

                    // First, capture stdout
                    while ((line = outputReader.ReadLine()) != null)
                    {
                        yield return line;
                    }

                    // Now, capture stderr if there are any errors
                    while ((line = errorReader.ReadLine()) != null)
                    {
                        yield return $"ERROR: {line}";
                    }
                }

                process.WaitForExit();
            }
        }

        public enum Precision
        {
            fp16 = 0,
            fp32 = 1,
            int4 = 2
        }

        public enum Execution
        {
            cpu = 0,
            dml = 1,
            cuda = 2,
        }

        public string RunOnnxConversion(string inputDirectory, string outputDirectory, Precision precision, Execution execution)
        {
            if (string.IsNullOrWhiteSpace(_PythonPath))
            {
                throw new InvalidOperationException("Python environment path is not set.");
            }

            // Path to the python.exe executable in the Python environment
            string pythonExePath = Path.Combine(_PythonPath, "python.exe");

            if (!File.Exists(pythonExePath))
            {
                throw new FileNotFoundException("python.exe not found.");
            }

            // The command to be executed
            string fullCommand = $"-m onnx_runner -i \"{inputDirectory}\" -o \"{outputDirectory}\" -p {precision.ToString()} -e {execution.ToString()}";

            // Initialize the process to run python with the command
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonExePath,  // Directly use the Python executable
                Arguments = fullCommand,
                UseShellExecute = true,    // Allow the cmd window to appear
                CreateNoWindow = false,    // Show the cmd window
                WorkingDirectory = _PythonPath // Set working directory to the Python environment
            };

            // Start the process but don't wait for it or track it
            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
            }

            // After starting the process, the C# application won't track it anymore
            return null;
        }



        // Shutdown PythonNET engine when disposing
        //public void Dispose()
        //{
        //    lock (_lock)
        //    {
        //        if (_initialized)
        //        {
        //            try
        //            {
        //                // Disable stash to avoid BinaryFormatter usage during shutdown
        //                //Environment.SetEnvironmentVariable("PYTHONNET_DISABLE_STASH", "1", EnvironmentVariableTarget.Process);

        //                // Now safely shut down Python engine
        //                //PythonEngine.Shutdown();
        //            }
        //            catch
        //            {

        //            }
        //            _PythonPath = null;
        //            _initialized = false;
        //            Console.WriteLine("Python environment has been shut down.");
        //        }
        //    }
        //}


        //public IEnumerable<string> ExecutePythonScript(string pythonCode)
        //{
        //    if (!_initialized)
        //    {
        //        throw new InvalidOperationException("Python environment is not initialized.");
        //    }
        //    pythonCode = string.Join(Environment.NewLine, pythonCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(line => line.Trim()));
        //    using (Py.GIL())
        //    {
        //        dynamic sys = Py.Import("sys");

        //        // Create and use custom Python-compatible output writer
        //        var pyOutputWriter = new PythonOutputWriter();
        //        sys.stdout = pyOutputWriter;

        //        PythonEngine.Exec(pythonCode);

        //        // Return the accumulated output
        //        yield return pyOutputWriter.ToString();
        //    }
        //}

    }


}
