using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOnnxStudio.Models
{
    public class ConsoleText
    {
        public string Text { get; set; }
        public MudBlazor.Typo MudType { get; set; } = MudBlazor.Typo.body1;
        public MudBlazor.Color MudColor { get; set; } = MudBlazor.Color.Primary;

    }
}
