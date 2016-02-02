using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReportingTool.DAL.Entities;
//using ReportingTool.DAL.Repositories;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;
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
        /*
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
        */

        /*
                static void Main1(string[] args)
                {
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
                }
        */

        public static void MemberCheck()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;
                Console.WriteLine("\nMEMBER CHECK : \n");
                var query = from m in ctx.members.Include("teams")
                            orderby m.id
                            select m;

                List<member> memberList = query.ToList<member>();

                //foreach (member memberVar in query)
                foreach (member memberVar in memberList)
                {
                    //if (memberVar.teams == null)
                    if (memberVar.teams.Count == 0)
                    {
                        memberVar.isactive = false;
                        //ctx.members.Attach(memberVar);
                        //ctx.Entry(memberVar).State = EntityState.Modified;
                        ctx.SaveChanges();
                    }
                }
            }
        }

        public static void TeamGetAll(string projectKey)
        {
            List<team> teamList = new List<team>();

            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;

                // -------------- READ  all teams and their members -----------------------------------------------------
                Console.WriteLine("\nTEAMS : \n");

                var query = from t in ctx.teams.Include("members")
                            orderby t.projectkey, t.name
                            where t.isactive == true && t.projectkey == projectKey
                            select t;

                foreach (team teamv in query)
                {
                    teamList.Add(teamv);
                    Console.WriteLine("\n{0}  {1}  {2}  {3}",
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

        public static void TeamReadOne()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;
                // -------------- READ  1 team and its members -----------------------------------------------------
                //  1
                //team teamFromDB = ctx.teams.FirstOrDefault<team>(t => t.id == 1);
                team teamFromDB = ctx.teams.Include("members").FirstOrDefault<team>(t => t.id == 1);

                //  2
                //var L2EQuery = from t in ctx.teams
                //                 where t.name  == "name1"
                //                 select t;
                //team teamFromDB = L2EQuery.FirstOrDefault<team>();

                //  print 1 team 
                Console.WriteLine("{0}  {1}  {2}  {3}",
                    teamFromDB.id, teamFromDB.projectkey, teamFromDB.name, teamFromDB.isactive);
                //  print its members
                Console.WriteLine("MEMBERS : ");
                foreach (var m in teamFromDB.members)
                {
                    Console.WriteLine("{0} {1} {2} {3}", m.id, m.username, m.fullname, m.isactive);
                }
            }
        }

        public static void TeamInsert()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;
                Console.WriteLine("\nINSERT TEAM : \n");
                // ---------------- INSERT ------------------------------------------------
                // Create a new team
                team teamFI = new team();
                teamFI.name = "teamNEW1";
                teamFI.projectkey = "projectkey1";
                teamFI.isactive = true;

                teamFI.members.Add(new member { username = "username51", fullname = "fullname51", isactive = true });
                teamFI.members.Add(new member { username = "username52", fullname = "fullname52", isactive = true });
                teamFI.members.Add(new member { username = "username53", fullname = "fullname53", isactive = true });

                ctx.teams.Add(teamFI);
                ctx.SaveChanges();
            }
        }

        // not needed
        public static void TeamInsert2()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;
                Console.WriteLine("\nINSERT TEAM : \n");
                // ---------------- INSERT ------------------------------------------------
                // Create a new team
                team teamFI = new team();
                teamFI.name = "teamNEW1";
                teamFI.projectkey = "projectkey1";
                teamFI.isactive = true;
                //
                string m1Name = "username51";

                member m1 = ctx.members.SingleOrDefault(m => m.username == m1Name);

                if (m1 == null)
                {
                    teamFI.members.Add(new member { username = "username51", fullname = "fullname51", isactive = true });
                }
                else
                {
                }


                //if (ctx.members.SingleOrDefault(m => m.username == "username51") == null)
                //{
                //    teamFI.members.Add(new member { username = "username51", fullname = "fullname51", isactive = true });
                //}
                //else
                //{
                //}

                ctx.teams.Add(teamFI);
                ctx.SaveChanges();
            }
        }

        public static void TeamUpdate()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;
                Console.WriteLine("\nUPDATE TEAM : \n");

                // ------------ UPDATE --------------------------------------------------
                team teamUpdate = ctx.teams.Include("members")
                    .FirstOrDefault<team>(t => t.name == "teamNEW1" && t.projectkey == "projectkey1");

                teamUpdate.name = "TeamUPDATE1";
                ctx.teams.Attach(teamUpdate);
                ctx.Entry(teamUpdate).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        // current work
        public static void TeamUpdate2()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;
                Console.WriteLine("\nUPDATE TEAM2 : \n");

                // -- team for update ---
                string t1Name = "teamNEW1";
                string t1ProjectKey = "projectkey1";

                team t1 = new team();
                t1.name = t1Name;
                t1.projectkey = t1ProjectKey;
                t1.isactive = true;
                t1.members.Add(new member { username = "username51", fullname = "fullname51", isactive = true });
                t1.members.Add(new member { username = "username101", fullname = "fullname101", isactive = true });
                t1.members.Add(new member { username = "username53", fullname = "fullname53", isactive = true });
                // ------

                team teamUpdate = ctx.teams.Include("members")
                    .SingleOrDefault<team>(t => t.name == t1Name && t.projectkey == t1ProjectKey);

                if (teamUpdate == null)
                {
                    Console.WriteLine("Team doesn't exist!");
                }


                #region  1st run from DB thru JSON
                bool deleteMember = true;
                member[] memberArray = new member[teamUpdate.members.Count];
                int index = 0;

                foreach (var itemDB in teamUpdate.members)
                //for (int i = 0; i < teamUpdate.members.Count; i++)
                {
                    deleteMember = true;
                    foreach (var itemJSON in t1.members)
                    //for (int j = 0; j < t1.members.Count; i++)
                    {
                        // found in JSON
                        if (itemDB.username == itemJSON.username)
                        //if (t1.members[i].username == itemJSON.username)
                        {
                            deleteMember = false;
                            break;
                        }
                    }

                    if (deleteMember == true)
                    {
                        //teamUpdate.members.Remove(itemDB);
                        memberArray[index++] = itemDB;
                        //ctx.SaveChanges();
                    }
                }

                for (int k = 0; k < teamUpdate.members.Count; k++)
                {
                    if ( memberArray[k] != null )
                    {
                        teamUpdate.members.Remove( memberArray[k] );
                    }
                }
                #endregion

                #region 2nd run from JSON thru DB
                bool addMember = true;
                member[] memberArrayAdd = new member[t1.members.Count];
                index = 0;

                foreach (var itemJSON in t1.members)
                {
                    addMember = true;
                    foreach (var itemDB in teamUpdate.members)
                    {
                        // found in DB
                        if (itemDB.username == itemJSON.username)
                        {
                            addMember = false;
                            break;
                        }
                    }

                    if (addMember == true)
                    {
                        //teamUpdate.members.Add(itemJSON);
                        memberArrayAdd[index++] = itemJSON;
                        //ctx.SaveChanges();
                    }
                }

                for (int k = 0; k < t1.members.Count; k++)
                {
                    if (memberArrayAdd[k] != null)
                    {
                        teamUpdate.members.Add(memberArrayAdd[k]);
                    }
                }

////
//                foreach (var itemDB in teamUpdate.members)
//                //for (int i = 0; i < teamUpdate.members.Count; i++)
//                {
//                    deleteMember = true;
//                    foreach (var itemJSON in t1.members)
//                    //for (int j = 0; j < t1.members.Count; i++)
//                    {
//                        // found in JSON
//                        if (itemDB.username == itemJSON.username)
//                        //if (t1.members[i].username == itemJSON.username)
//                        {
//                            deleteMember = false;
//                            break;
//                        }
//                    }

//                    if (deleteMember == true)
//                    {
//                        //teamUpdate.members.Remove(itemDB);
//                        memberArray[index++] = itemDB;
//                        //ctx.SaveChanges();
//                    }
//                }

//                for (int k = 0; k < teamUpdate.members.Count; k++)
//                {
//                    if (memberArray[k] != null)
//                    {
//                        teamUpdate.members.Remove(memberArray[k]);
//                    }
//                }
//

                #endregion

                ctx.SaveChanges();
            }

            MemberCheck();
        }

        public static void TeamDelete()
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;

                // ------------ DELETE ----------------------------------------------
                team teamDelete = ctx.teams.Include("members")
                    .FirstOrDefault<team>(t => t.name == "teamNEW1" && t.projectkey == "projectkey1");

                ctx.teams.Remove(teamDelete);
                ctx.teams.Attach(teamDelete);
                ctx.Entry(teamDelete).State = EntityState.Deleted;

                // delete related members - NO NEED
                //ctx.members.RemoveRange(teamDelete.members);

                ctx.SaveChanges();
                // now we add the deleted team to DB with isactive = false
                teamDelete.isactive = false;
                ctx.teams.Add(teamDelete);
                ctx.SaveChanges();
            }
        }

        static void Main(string[] args)
        {
            //TeamInsert();

            //TeamUpdate();

            //TeamUpdate2();

            //TeamDelete();

            //TeamReadOne();

            string projectKey = "projectkey1";
            TeamGetAll(projectKey);

            MemberCheck();
        }
    }
}

