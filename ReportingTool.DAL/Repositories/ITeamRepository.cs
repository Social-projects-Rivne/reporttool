using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.Repositories
{
    public interface ITeamRepository
    {
        IEnumerable<Team> Get();

        //bool TryGet(int id, out Team team);
        bool TryGet(string name, out Team team);

        Team Add(Team team);
        
        //bool Delete(int id);
        bool Delete(string name);

        bool Update(Team team);
    }
}
