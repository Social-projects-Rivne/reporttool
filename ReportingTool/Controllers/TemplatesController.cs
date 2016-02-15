
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
﻿using Newtonsoft.Json;
﻿using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        [HttpGet]
        public string GetAllTemplates()
        {
            List<Template> templates = new List<Template>();
            using (var db = new DB2())
            {
                foreach (var template in db.Templates)
                {
                    if (template.IsActive)
                    {
                        templates.Add(new Template { Name = template.Name, Id = template.Id, Owner = template.Owner });
                    }
                    
                }
            }
            var alltemplates = templates.ToList();
            var outputJSON = JsonConvert.SerializeObject(alltemplates, Formatting.Indented);
            return outputJSON;
        }
    }
}
