using System;
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
            string value = Session["currentUser"] as string;
            List<Template> templates = new List<Template>();
            List<OpenButtons> buttons = new List<OpenButtons>();
            using (var db = new DB2())
            {
                foreach (var template in db.Templates)
                {
                    if (template.IsActive)
                    {
                        templates.Add(new Template { Name = template.Name, Id = template.Id});
                    }
                    if (template.Owner == value)
                    {
                        buttons.Add(new OpenButtons { Id = template.Id, Button = true});
                    }
                    else
                    {
                        buttons.Add(new OpenButtons { Id = template.Id, Button = false});
                    }
                }
            }
            var query = from template in templates from b in buttons where template.Id == b.Id
                        select new {template , b.Button}; 
            var outputJSON = JsonConvert.SerializeObject(query.ToList(), Formatting.Indented);
            return outputJSON;
        }
    }

    public class OpenButtons
    {

        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("button")]
        public bool Button { get; set; }
    }
}
