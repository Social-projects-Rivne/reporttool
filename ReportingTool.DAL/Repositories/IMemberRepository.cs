using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.Repositories
{
    public interface IMemberRepository
    {
        IEnumerable<member> Get();

        bool TryGet(int id, out member memberv);
        //bool TryGet(string username, out member memberv);

        member Add(member memberv);

        // bool Delete(int id);
        bool Delete(string username);

        bool Update(member memberv);
    }
}
