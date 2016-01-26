using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReportingTool.DAL.Entities;
using ReportingTool.DAL.Repositories;
using System.Runtime.Serialization.Json;
using System.IO;
using ReportingTool.DAL.Entities.JSON.Models;

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
            IEnumerable<Team> teamList = efTeamRepository.GetAll(projectkeyVar);

            //  get TeamMembers
            EFTeamMemberRepository efTeamMemberRepository = new EFTeamMemberRepository();
            IEnumerable<TeamMember> teamMembers = efTeamMemberRepository.Get();

            //  get Members
            EFMemberRepository efMemberRepository = new EFMemberRepository();
            IEnumerable<Member> members = efMemberRepository.Get();

            //  a list for sorted out members
            List<MemberJSM> membersVar = new List<MemberJSM>();
            List<TeamJSM> tvmList = new List<TeamJSM>();

            foreach (Team teamvar in teamList)
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
                    //Member memberVar = (Member)members.Where(m => m.Id == tm2.MemberId);
                    Member memberVar = null;
                    if (efMemberRepository.TryGet(tm2.MemberId, out memberVar))
                    {
                        if (memberVar != null)
                        {
                            MemberJSM memberJSM = new MemberJSM();
                            memberJSM.memberID = memberVar.Id;
                            memberJSM.userName = memberVar.Username;
                            memberJSM.fullName = memberVar.Fullname;
                            memberJSM.isActive = memberVar.IsActive;

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
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Member));
            //ser.WriteObject(stream1, membersVar);

            //List<Member> m2 = (List<Member>) ser.ReadObject(stream1);
            //// --------

            Console.Read();

        }
    }
}
