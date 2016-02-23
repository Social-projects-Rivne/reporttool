using ReportingTool.Core.Models;
using ReportingTool.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Services
{
    class JiraUserService
    {
        public JiraUserModel CreateModelFromEntity(JiraUser entity)
        {
            return new JiraUserModel
            {
                userName = entity.name,
                fullName = entity.displayName
            };
        }

        public JiraUser CreateEntityFromModel(JiraUserModel model)
        {
            return new JiraUser
            {
                name = model.userName,
                displayName = model.fullName
            };
        }
    }
}