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
    public class EFTeamRepository : ITeamRepository
    {

        public IEnumerable<team> GetAll(string projectkey)
        {
            using (var ctx = new DB2())
            {
                //List<team> teams = ctx.Teams.ToList();
                //return teams.OrderBy(t => t.Id).Where(t => t.ProjectKey == projectkey);
                List<team> teams = ctx.Teams.Where(t => t.ProjectKey == projectkey).ToList();
                //IEnumerable<team> teamsOrdered = teams.OrderBy(t => t.Id).Where(t => t.ProjectKey == projectkey);

                if (teams != null)
                {
                    return teams.OrderBy(t => t.Id);
                }
                return null;
            }
            //return comments.Values.OrderBy(comment => comment.ID);
           
        }

        //public bool TryGet(int id, out team teamv)
        public bool TryGet(string name, out team teamv)
        {
            using (var ctx = new DB2())
            {
                //  teamv = ctx.Teams.FirstOrDefault(t => t.Id == id);
                teamv = ctx.Teams.FirstOrDefault(t => t.Name == name);
            }
            if (teamv != null)
            {
                return true;
            }
            return false;
        }

        //  TODO: issue with indexes
        public team Add(team teamv)
        {
            using (var ctx = new DB2())
            {
                ctx.Teams.Add(teamv);
                ctx.SaveChanges();
            }
            return teamv;
        }

        //public bool Delete(int id)
        public bool Delete(string name)
        {
            using (var ctx = new DB2())
            {
                // Query for the team with the certain Id 
                //team teamv = ctx.Teams.Where(t => t.Id == id).FirstOrDefault();
                team teamv = ctx.Teams.FirstOrDefault(t => t.Name == name);

                if (teamv != null)
                {
                    //ctx.Teams.Remove(teamv);
                    teamv.IsActive = false;

                    ctx.Teams.Attach(teamv);
                    ctx.Entry(teamv).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Update(Team teamv)
        {
            using (var ctx = new DB2())
            {
                // Query for the Team with the certain Id 
                Team teamFromDB = ctx.Teams.FirstOrDefault(t => t.Id == teamv.Id);

                if (teamFromDB != null)
                {
                    teamFromDB.Name = teamv.Name;
                    teamFromDB.IsActive = teamv.IsActive;

                    ctx.Teams.Attach(teamv);
                    ctx.Entry(teamv).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return false;
            }

        }
    }
 */
}

