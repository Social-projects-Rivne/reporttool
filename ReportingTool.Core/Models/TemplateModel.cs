﻿using System.Collections.Generic;

namespace ReportingTool.Core.Models
{
    public class TemplateModel
    {
        public int templateId { get; set; }
        public string templateName { get; set; }
        public bool IsOwner { get; set; }
        public List<FieldModel> fields;
    }
}
