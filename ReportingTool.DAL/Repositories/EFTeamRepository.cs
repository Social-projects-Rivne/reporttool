using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.Repositories
{
    public class EFTeamRepository : ITeamRepository
    {

        public IEnumerable<Team> Get()
        {
            using (var ctx = new DB2())
            {
                List<Team> teams = ctx.Teams.ToList();
                return teams.OrderBy(t => t.Id);
            }
            //return comments.Values.OrderBy(comment => comment.ID);
        }

        //public bool TryGet(int id, out Team team)
        public bool TryGet(string name, out Team team)
        {
            using (var ctx = new DB2())
            {
                //  team = ctx.Teams.FirstOrDefault(t => t.Id == id);
                team = ctx.Teams.FirstOrDefault(t => t.Name == name);
            }
            if (team != null)
            {
                return true;
            }
            return false;
        }

        //  TODO: issue with indexes
        public Team Add(Team team)
        {
            using (var ctx = new DB2())
            {
                ctx.Teams.Add(team);
                ctx.SaveChanges();
            }
            return team;
        }

        //public bool Delete(int id)
        public bool Delete(string name)
        {
            using (var ctx = new DB2())
            {
                // Query for the Team with the certain Id 
                //Team team = ctx.Teams.Where(t => t.Id == id).FirstOrDefault();
                Team team = ctx.Teams.FirstOrDefault(t => t.Name == name);

                if (team != null)
                {
                    //ctx.Teams.Remove(team);
                    team.IsActive = false;

                    ctx.Teams.Attach(team);
                    ctx.Entry(team).State = EntityState.Modified;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool Update(Team team)
        {
            using (var ctx = new DB2())
            {
                // Query for the Team with the certain Id 
                Team teamFromDB = ctx.Teams.FirstOrDefault(t => t.Id == team.Id);

                if (teamFromDB != null)
                {
                    teamFromDB.Name = team.Name;
                    teamFromDB.IsActive = team.IsActive;

                    ctx.Teams.Attach(team);
                    ctx.Entry(team).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return false;
            }

        }

    }
}

