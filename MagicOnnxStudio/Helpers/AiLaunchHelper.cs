using MagicOnnxStudio.Models;
using MagicOnnxStudio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MagicOnnxStudio.Services.PythonEnvironmentService;

namespace MagicOnnxStudio.Helpers
{
    public class AiLaunchHelper
    {
        public static async Task RunAiModel(DownloadedModelInfo _ModelInfo)
        {
            var pyService = new PythonEnvironmentService();

            string pythonPath;

            if (_ModelInfo.execution == Execution.cpu.ToString())
            {
                pythonPath = Cache.CpuMlPythonPath;
            }
            else if (_ModelInfo.execution == Execution.dml.ToString())
            {
                pythonPath = Cache.DirectMlPythonPath;
            }
            else if (_ModelInfo.execution == Execution.cuda.ToString())
            {
                pythonPath = Cache.Cuda11MlPythonPath;
            }
            else if (_ModelInfo.execution == Execution.cuda12.ToString())
            {
                pythonPath = Cache.Cuda12MlPythonPath;
            }
            else
            {
                pythonPath = Cache.CpuMlPythonPath; // Default to CPU if something goes wrong
            }

            // Initialize the Python environment
            pyService.InitializePythonEnvironment(pythonPath);

            pyService.RunAiModelTest(
                        _ModelInfo.ModelPath
                    );
        }
    }
}
