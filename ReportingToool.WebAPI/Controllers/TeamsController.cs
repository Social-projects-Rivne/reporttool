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
        // GET api/teams
        public IEnumerable<TeamJSM> Get()
        //public string Get()
        {
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

            return tvmList;
            //string output = JsonConvert.SerializeObject(tvmList);
            //return output;
        }

        // GET api/teams/5
        public TeamJSM Get(int id)
        {
            return null;
        }

        // POST api/teams
        public void Post([FromBody]TeamJSM tvm)
        {
        }

        // PUT api/teams/5
        public void Put(int id, [FromBody]TeamJSM tvm)
        {
        }

        // DELETE api/teams/5
        public void Delete(int id)
        {
        }
    }
}