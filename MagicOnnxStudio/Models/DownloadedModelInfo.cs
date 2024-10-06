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
        public string ModelPath { get; set; }
        public string precision { get; set; }
        public string execution { get; set; }
    }
}
