using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingTool.DAL;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Core.Validation
{
    public static class TemplatesValidator
    {
        public static bool TemplateNameIsCorrect(string templateName)
        {
            if (String.IsNullOrWhiteSpace(templateName) || templateName.Length > 128)
            {
                return false;                   
            }
            else
            {
                return true;
            }
        }

    }
}
