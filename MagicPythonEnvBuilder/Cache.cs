using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPythonEnvBuilder
{
    public static class Cache
    {
        public static string DirectMlPythonPath { get; set; }
        public static string Cuda11MlPythonPath { get; set; }
        public static string Cuda12MlPythonPath { get; set; }
        public static string CpuMlPythonPath { get; set; }
        
    }
}
