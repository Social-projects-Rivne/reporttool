using ReportingTool.Core.Models;
using ReportingTool.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Services
{
  public static class JiraUserService
    {
      public static JiraUserModel CreateModelFromEntity(JiraUser entity)
        {
            return new JiraUserModel
            {
                userName = entity.name,
                fullName = entity.displayName
            };
        }
    }
}