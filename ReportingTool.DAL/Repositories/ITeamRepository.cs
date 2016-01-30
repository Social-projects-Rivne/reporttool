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
        //IEnumerable<team> Get();
        IEnumerable<team> GetAll(string projectkey);

        //bool TryGet(int id, out team teamv);
        bool TryGet(string name, out team teamv);

        team Add(team teamv);
        
        //bool Delete(int id);
        bool Delete(string name);

        bool Update(team teamv);
    }
}
