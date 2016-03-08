using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingTool.Core.Models;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Core.Services
{
     public static class TemplateService
    {
         public static Template CreateEntityFromModel(TemplateModel model)
         {
             return new Template { Name = model.templateName, FieldsInTemplate = FieldsService.CreateEntitiesFromModels(model.fields)};
         }
    }
}
