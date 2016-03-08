using ReportingTool.Core.Models;
using ReportingTool.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Services
{
    public static class FieldsService
    {
        public static ICollection<FieldsInTemplate> CreateEntitiesFromModels(List<FieldModel> models)
        {
            ICollection<FieldsInTemplate> enteties = new List<FieldsInTemplate>();
            foreach (FieldModel model in models)
            {
                enteties.Add(new FieldsInTemplate { DefaultValue = model.fieldDefaultValue, FieldId = model.fieldID });
            }
            return enteties;
        }
    }
}
