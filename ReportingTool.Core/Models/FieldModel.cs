using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Models
{
    class FieldModel
    {
        public int fieldID { get; set; }
        public int fieldName { get; set; }
        public int fieldType { get; set; }
        public int fieldDefaultValue { get; set; }
    }
}