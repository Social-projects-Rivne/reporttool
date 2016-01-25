using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using ReportingTool.DAL.Entities;


namespace ReportingTool.DAL.Repositories
{
    public class EFTeamMemberRepository : ITeamMemberRepository
    {
        public IEnumerable<TeamMember> Get()
        {
            using (var ctx = new DB2())
            {
                List<TeamMember> teams = ctx.TeamMembers.ToList();
                return teams.OrderBy(t => t.Id);
            }
            //return comments.Values.OrderBy(comment => comment.ID);
        }

        //public bool TryGet(int id, out TeamMember teamMember)
        public bool TryGet(int id, out TeamMember teamMember)
        {
            using (var ctx = new DB2())
            {
                //  teamMember = ctx.Teams.FirstOrDefault(t => t.Id == id);
                teamMember = ctx.TeamMembers.FirstOrDefault(t => t.Id == id);
            }
            if (teamMember != null)
            {
                return true;
            }
            return false;
        }

        //  TODO: issue with indexes
        public TeamMember Add(TeamMember teamMember)
        {
            using (var ctx = new DB2())
            {
                ctx.TeamMembers.Add(teamMember);
                ctx.SaveChanges();
            }
            return teamMember;
        }

        //public bool Delete(int id)
        public bool Delete(int id)
        {
            using (var ctx = new DB2())
            {
                // Query for the TeamMember with the certain Id 
                //TeamMember teamMember = ctx.Teams.Where(t => t.Id == id).FirstOrDefault();
                TeamMember teamMember = ctx.TeamMembers.FirstOrDefault(t => t.Id == id);

                if (teamMember != null)
                {
                    ctx.TeamMembers.Remove(teamMember);

                    //teamMember.IsActive = false;
                    // ctx.Teams.Attach(teamMember);
                    //ctx.Entry(teamMember).State = EntityState.Modified;

                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Update(TeamMember teamMember)
        {
            using (var ctx = new DB2())
            {
                // Query for the TeamMember with the certain Id 
                TeamMember teamMemberFromDB = ctx.TeamMembers.FirstOrDefault(t => t.Id == teamMember.Id);

                if (teamMemberFromDB != null)
                {
                    teamMemberFromDB.MemberId = teamMember.MemberId;
                    teamMemberFromDB.TeamId = teamMember.TeamId;

                    ctx.TeamMembers.Attach(teamMember);
                    ctx.Entry(teamMember).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return false;
            }

        }
    }
}
