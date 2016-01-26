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
        IEnumerable<Member> Get();

        bool TryGet(int id, out Member member);
        //bool TryGet(string username, out Member member);

        Member Add(Member member);

        // bool Delete(int id);
        bool Delete(string username);

        bool Update(Member member);
    }
}
