using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.Repositories
{
    public class EFMemberRepository : IMemberRepository
    {
        public IEnumerable<Member> Get()
        {
            using (var ctx = new DB2())
            {
                List<Member> members = ctx.Members.ToList();
                return members.OrderBy(m => m.Id);
            }
            //return comments.Values.OrderBy(comment => comment.ID);
        }

        //public bool TryGet(int id, out Member member)
        public bool TryGet(string username, out Member member)
        {
            using (var ctx = new DB2())
            {
                //  member = ctx.Members.FirstOrDefault(t => t.Id == id);
                member = ctx.Members.FirstOrDefault(t => t.Username == username);
            }
            if (member != null)
            {
                return true;
            }
            return false;
        }

        //  TODO: issue with indexes
        public Member Add(Member member)
        {
            using (var ctx = new DB2())
            {
                ctx.Members.Add(member);
                ctx.SaveChanges();
            }
            return member;
        }

        //public bool Delete(int id)
        public bool Delete(string username)
        {
            using (var ctx = new DB2())
            {
                // Query for the Member with the certain Id 
                //Member member = ctx.Members.Where(t => t.Id == id).FirstOrDefault();
                Member member = ctx.Members.FirstOrDefault(m => m.Username == username);

                if (member != null)
                {
                    //ctx.Members.Remove(member);
                    member.IsActive = false;

                    ctx.Members.Attach(member);
                    ctx.Entry(member).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Update(Member member)
        {
            using (var ctx = new DB2())
            {
                // Query for the Member with the certain Id 
                Member memberFromDB = ctx.Members.FirstOrDefault(t => t.Id == member.Id);

                if (memberFromDB != null)
                {
                    memberFromDB.Username = member.Username;
                    memberFromDB.Fullname = member.Fullname;
                    memberFromDB.IsActive = member.IsActive;

                    ctx.Members.Attach(member);
                    ctx.Entry(member).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return false;
            }

        }

    }
}
