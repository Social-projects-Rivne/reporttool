using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.TestConsoleApp
{
  public class Program
  {

    public static List<TeamMember> GetAllTeamMembers()
    {
      using (var dbctx = new DB2())
      {
        // Query for all TeamMembers 
        var teamMembers = from t1 in dbctx.TeamMembers
                          select t1;

        List<TeamMember> tmList = new List<TeamMember>();

        foreach (var tm in teamMembers)
        {
          tmList.Add(tm);
        }

        return tmList;
      }
    }

    public static void PrintTeamMembers()
    {
      using (var dbctx = new DB2())
      {
        // Query for all TeamMembers 
        var teamMembers = from t1 in dbctx.TeamMembers
                          select t1;

        foreach (var t2 in teamMembers)
        {
          Console.WriteLine("{0}  {1}  {2}", t2.Id, t2.TeamId, t2.MemberId);
        }
      }
    }

    public static void AddTeamMember(TeamMember teamMember)
    {
      using (var dbctx = new DB2())
      {
        TeamMember teamMemberInDB = dbctx.TeamMembers
            .Where(tm => (tm.MemberId == teamMember.MemberId) && (tm.TeamId == teamMember.TeamId))
            .FirstOrDefault();

        if (teamMemberInDB == null)
        {
          dbctx.TeamMembers.Add(teamMember);
          dbctx.SaveChanges();
        }
        else
        {
        }
      }
    }

    public static void DeleteTeamMember(int id)
    {
      using (var dbctx = new DB2())
      {
        // Query for the TeamMember with the certain Id 
        var teamMember = dbctx.TeamMembers
                        .Where(t => t.Id == id)
                        .FirstOrDefault();

        dbctx.TeamMembers.Remove(teamMember);
        dbctx.SaveChanges();
      }
    }

    static void Main0(string[] args)
    {
      //var db = new DB2();
      using (var dbctx = new DB2())
      {
        #region PrintBlock1
        //var teamMembers = dbctx.TeamMembers;
        // Query for all TeamMembers with the certain Id 
        var teamMembers = from t1 in dbctx.TeamMembers
                          ////where t1.Id == 1
                          select t1;

        foreach (var t2 in teamMembers)
        {
          Console.WriteLine("{0}  {1}  {2}", t2.Id, t2.TeamId, t2.MemberId);
        }
        #endregion
        //PrintTeamMembers();

        // Query for the TeamMember with the certain Id 
        var teamMember = dbctx.TeamMembers
                        .Where(t => t.Id == 1)
                        .FirstOrDefault();

        dbctx.TeamMembers.Remove(teamMember);
        dbctx.SaveChanges();

        #region PrintBlock2
        var teamMembers2 = from t2 in dbctx.TeamMembers
                           select t2;
        foreach (var t in teamMembers2)
        {
          Console.WriteLine("{0}  {1}  {2}", t.Id, t.TeamId, t.MemberId);
        }
        #endregion
        //PrintTeamMembers();
      }

      Console.Read();
    }

    static void Main(string[] args)
    {

      PrintTeamMembers();

      int idForDelete = 4;
      DeleteTeamMember(idForDelete);

      //Program.AddTeamMember(new TeamMember { TeamId = 5, MemberId = 2 });
      //Program.AddTeamMember(new TeamMember { TeamId = 5, MemberId = 3 });

      Console.WriteLine();
      PrintTeamMembers();

      Console.Read();
    }
  }
}
