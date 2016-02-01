using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ReportingTool.DAL.Entities;
using ReportingTool.DAL.Repositories;
using System.Data.Entity;
using Newtonsoft.Json;
using ReportingTool.DAL.Entities.JSON.Models;

namespace ReportingToool.WebAPI.Controllers
{

    public class TeamsController : ApiController
    {
        //  look for members without teams --> isactive=>false
        public static void MemberCheck()
        {
            using (var ctx = new DB2())
            {
                //ctx.Database.Log = Console.WriteLine;
                //Console.WriteLine("\nMEMBER CHECK : \n");

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


        /*
                // GET api/teams
                public IEnumerable<TeamJSM> Get()
                //public string Get()
                {
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

                    return tvmList;
                    //string output = JsonConvert.SerializeObject(tvmList);
                    //return output;
                }
        */

        // GET api/teams/5
        //public TeamJSM Get(int idv)
        //{
        //    return null;
        //}

        // GET api/teams/projectkey1
        //public static void Get(string projectKey)

        // http://localhost:7898/api/teams?projectKey=projectkey1
        //public string Get(string projectKey)
        public List<team> Get(string projectKey)
        {
            List<team> teamList = new List<team>();

            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;

                // -------------- READ  all teams and their members -----------------------------------------------------
                //Console.WriteLine("\nTEAMS : \n");

                var query = from t in ctx.teams.Include("members")
                            orderby t.projectkey, t.name
                            where t.isactive == true && t.projectkey == projectKey
                            select t;

                foreach (team teamv in query)
                {
                    teamList.Add(teamv);
                    //Console.WriteLine("\n{0}  {1}  {2}  {3}",
                    //       teamv.id, teamv.projectkey, teamv.name, teamv.isactive);

                    //Console.WriteLine("members : ");
                    //foreach (var m in teamv.members)
                    //{
                    //    Console.WriteLine("{0} {1} {2} {3} ", m.id, m.username, m.fullname, m.isactive);
                    //}
                    //Console.WriteLine();
                }

                //var serializerSettings = new JsonSerializerSettings { 
                //    PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                //Console.WriteLine(JsonConvert.SerializeObject(teamList, serializerSettings));
                //Console.WriteLine(JsonConvert.SerializeObject(teamList, Formatting.Indented, serializerSettings));
                //  returns good JSON
                //Console.WriteLine(JsonConvert.SerializeObject(teamList, Formatting.Indented));

                //return JsonConvert.SerializeObject(teamList, Formatting.Indented);
                return teamList;
            }
        }

        // POST api/teams
        public void Post([FromBody]TeamJSM tvm)
        {
        }

        // PUT api/teams/5
        public void Put(int idv, [FromBody]TeamJSM tvm)
        {
        }

        // DELETE api/teams/5
        public void Delete(int  id)
        {
            using (var ctx = new DB2())
            {
                ctx.Database.Log = Console.WriteLine;

                // ------------ DELETE ----------------------------------------------
                team teamDelete = ctx.teams.Include("members")
                    //.FirstOrDefault<team>(t => t.name == "teamNEW1" && t.projectkey == "projectkey1");
                .FirstOrDefault<team>(t => t.id == id);

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
            MemberCheck();
        }
    }

}