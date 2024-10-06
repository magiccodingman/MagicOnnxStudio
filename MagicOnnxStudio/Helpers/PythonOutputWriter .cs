using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Helpers
{
    public class PythonOutputWriter
    {
        private readonly StringBuilder _output;

        public PythonOutputWriter()
        {
            _output = new StringBuilder();
        }

        // This method will be called by Python's sys.stdout.write
        public void write(string value)
        {
            _output.Append(value);
        }

        // Optional: This can be called by sys.stdout.flush()
        public void flush()
        {
            // Flush is optional in this case, as we're just appending to a StringBuilder
        }

        // Return the accumulated output
        public override string ToString()
        {
            return _output.ToString();
        }
    }
}
