namespace ReportingTool.Core.Models
{
    public class FieldModel
    {
        public int fieldID { get; set; }
        public string fieldName { get; set; }
        public string fieldType { get; set; }
        public string fieldDefaultValue { get; set; }
        public bool isSelected { get; set; }
    }
}