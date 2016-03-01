using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Models
{
    public class FieldModel
    {
        public int fieldID { get; set; }
        public string fieldName { get; set; }
        public string fieldType { get; set; }
        public string fieldDefaultValue { get; set; }
    }
}