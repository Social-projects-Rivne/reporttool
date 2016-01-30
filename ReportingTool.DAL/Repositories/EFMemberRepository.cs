using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.Repositories
{
/*
    public class EFMemberRepository : IMemberRepository
    {
        public IEnumerable<member> Get()
        {
            using (var ctx = new DB2())
            {
                List<member> members = ctx.Members.ToList();
                return members.OrderBy(m => m.id);
            }
            //return comments.Values.OrderBy(comment => comment.ID);
        }

        public bool TryGet(int id, out member memberv)
        //public bool TryGet(string username, out member memberv)
        {
            using (var ctx = new DB2())
            {
                memberv = ctx.Members.FirstOrDefault(t => t.id == id);
                //memberv = ctx.Members.FirstOrDefault(t => t.Username == username);
            }
            if (memberv != null)
            {
                return true;
            }
            return false;
        }

        //  TODO: issue with indexes
        public member Add(member memberv)
        {
            using (var ctx = new DB2())
            {
                ctx.Members.Add(memberv);
                ctx.SaveChanges();
            }
            return memberv;
        }

        //public bool Delete(int id)
        public bool Delete(string username)
        {
            using (var ctx = new DB2())
            {
                // Query for the member with the certain Id 
                //member memberv = ctx.Members.Where(t => t.Id == id).FirstOrDefault();
                member memberv = ctx.Members.FirstOrDefault(m => m.username == username);

                if (memberv != null)
                {
                    //ctx.Members.Remove(memberv);
                    memberv.isactive = false;

                    ctx.Members.Attach(memberv);
                    ctx.Entry(memberv).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Update(member memberv)
        {
            using (var ctx = new DB2())
            {
                // Query for the member with the certain Id 
                member memberFromDB = ctx.Members.FirstOrDefault(t => t.id == memberv.id);

                if (memberFromDB != null)
                {
                    memberFromDB.username = memberv.username;
                    memberFromDB.fullname = memberv.fullname;
                    memberFromDB.isactive = memberv.isactive;

                    ctx.Members.Attach(memberv);
                    ctx.Entry(memberv).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return false;
            }

        }

    }
 */
}
