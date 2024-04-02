using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Templates
{
    public static class TemplateHelper
    {
        public static string GetBillTemplateFilePath()
        {
            string documentFilePath = Path.Combine(GetTemplatesFolderPath(), "BillTemplate.docx");

            return documentFilePath;
        }

        private static string GetTemplatesFolderPath()
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyPath) ?? string.Empty;
            string templateFolderPath = Path.Combine(assemblyDirectory, "Templates");

            return templateFolderPath;
        }
    }
}
