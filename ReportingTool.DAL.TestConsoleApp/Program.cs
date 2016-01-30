using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReportingTool.DAL.Entities;
using ReportingTool.DAL.Repositories;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
//using ReportingTool.DAL.Entities.JSON.Models;

namespace ReportingTool.DAL.TestConsoleApp
{
    public class Program
    {
        /*
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

        public static void DeleteTeamMember(int idv)
        {
            using (var dbctx = new DB2())
            {
                // Query for the TeamMember with the certain Id 
                var teamMember = dbctx.TeamMembers
                                .Where(t => t.Id == idv)
                                .FirstOrDefault();

                dbctx.TeamMembers.Remove(teamMember);
                dbctx.SaveChanges();
            }
        }
        */

        static void Main0(string[] args)
        {
            /*
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
             */
        }

        static void Main1(string[] args)
        {
            /*
                        //  ----- Console Test 1
                        //PrintTeamMembers();

                        //int idForDelete = 4;
                        //DeleteTeamMember(idForDelete);

                        ////Program.AddTeamMember(new TeamMember { TeamId = 5, MemberId = 2 });
                        ////Program.AddTeamMember(new TeamMember { TeamId = 5, MemberId = 3 });

                        //Console.WriteLine();
                        //PrintTeamMembers();

                        //Console.Read();

                        //  ----- Console Test 2
                        //  find Teams by projectkey
                        EFTeamRepository efTeamRepository = new EFTeamRepository();
                        string projectkeyVar = "projectkey1";
                        IEnumerable<team> teamList = efTeamRepository.GetAll(projectkeyVar);

                        //  get TeamMembers
                        EFTeamMemberRepository efTeamMemberRepository = new EFTeamMemberRepository();
                        IEnumerable<TeamMember> teamMembers = efTeamMemberRepository.Get();

                        //  get Members
                        EFMemberRepository efMemberRepository = new EFMemberRepository();
                        IEnumerable<member> members = efMemberRepository.Get();

                        //  a list for sorted out members
                        List<MemberJSM> membersVar = new List<MemberJSM>();
                        List<TeamJSM> tvmList = new List<TeamJSM>();

                        foreach (team teamvar in teamList)
                        {
                            //IEnumerable<TeamMember> teamMembersVar = teamMembers.Where(tm => tm.TeamId == t2.Id);
                            List<TeamMember> tmsVar = new List<TeamMember>();

                            // fill JSON Model
                            TeamJSM teamJSM = new TeamJSM();
                            teamJSM.teamID = teamvar.Id;
                            teamJSM.teamName = teamvar.Name;
                            teamJSM.projectKey = teamvar.ProjectKey;
                            teamJSM.isActive = teamvar.IsActive;
                            //
                            foreach (var tm in teamMembers)
                            {
                                if (tm.TeamId == teamvar.Id)
                                {
                                    tmsVar.Add(tm);
                                }
                            }

                            foreach (var tm2 in tmsVar)
                            {
                                //member memberVar = (member)members.Where(m => m.Id == tm2.MemberId);
                                member memberVar = null;
                                if (efMemberRepository.TryGet(tm2.MemberId, out memberVar))
                                {
                                    if (memberVar != null)
                                    {
                                        MemberJSM memberJSM = new MemberJSM();
                                        memberJSM.memberID = memberVar.idv;
                                        memberJSM.userName = memberVar.username;
                                        memberJSM.fullName = memberVar.Fullname;
                                        memberJSM.isActive = memberVar.isactive;

                                        teamJSM.members.Add(memberJSM);

                                        // list of Members for testing
                                        membersVar.Add(memberJSM);
                                    }
                                }
                            }
                            //Console.WriteLine("{0}  {1}  {2} {3}", t2.Id, t2.Name, t2.ProjectKey, t2.IsActive);
                            tvmList.Add(teamJSM);
                        }

                        foreach (var m in membersVar)
                        {
                            Console.WriteLine("{0}  {1}  {2}", m.memberID, m.userName, m.fullName, m.isActive);
                        }

                        ////  serialization
                        //MemoryStream stream1 = new MemoryStream();
                        //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(member));
                        //ser.WriteObject(stream1, membersVar);

                        //List<member> m2 = (List<member>) ser.ReadObject(stream1);
                        //// --------

                        Console.Read();
            */
        }

        static void Main(string[] args)
        {
            List<team> teamList = new List<team>();

            using (var ctx = new DB2())
            {
                //  1
                //team teamFromDB = ctx.teams.FirstOrDefault<team>(t => t.id == 1);
                team teamFromDB = ctx.teams.Include("members").FirstOrDefault<team>(t => t.id == 1);

                //  2
                //var L2EQuery = from t in ctx.teams
                //                 where t.name  == "name1"
                //                 select t;
                //team teamFromDB = L2EQuery.FirstOrDefault<team>();

                team teamForJSON = new team();

                Console.WriteLine("{0} | {1} | {2} | {3}",
                    teamFromDB.id, teamFromDB.projectkey, teamFromDB.name, teamFromDB.isactive);

                foreach (var m in teamFromDB.members)
                {
                    Console.WriteLine("{0} {1} {2} {3}", m.id, m.username, m.fullname, m.isactive);
                }

                //  ----
                Console.WriteLine("\nTEAMS : \n");
                var query = from t in ctx.teams.Include("members")
                            orderby t.projectkey, t.name
                            where t.isactive == true
                            select t;

                foreach (team teamv in query)
                {
                    teamList.Add(teamv);

                    Console.WriteLine("{0} | {1} | {2} | {3}",
                   teamv.id, teamv.projectkey, teamv.name, teamv.isactive);
                    Console.WriteLine("members : ");
                    foreach (var m in teamv.members)
                    {
                        Console.WriteLine("{0} {1} {2} {3} ", m.id, m.username, m.fullname, m.isactive);
                    }
                    Console.WriteLine();
                }
                
                //var serializerSettings = new JsonSerializerSettings { 
                //    PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                //Console.WriteLine(JsonConvert.SerializeObject(teamList, serializerSettings));
                //Console.WriteLine(JsonConvert.SerializeObject(teamList, Formatting.Indented, serializerSettings));
                Console.WriteLine(JsonConvert.SerializeObject(teamList, Formatting.Indented));
            }


        }
    }
}

