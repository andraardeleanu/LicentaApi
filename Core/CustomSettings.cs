using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class CustomSettings : ICustomSettings
    {
        public string? DocConvertDir { get; set; }
        public string? LibreOfficeModule { get; set; }
    }
}
