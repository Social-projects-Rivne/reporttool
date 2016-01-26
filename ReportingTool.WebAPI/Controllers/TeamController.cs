using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ReportingTool.DAL.Entities;
using ReportingTool.DAL.Repositories;

namespace ReportingTool.WebAPI.Controllers
{
    public class TeamController : ApiController
    {
       
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/team
        public IEnumerable<TeamViewModel> Get()
        {
            //  find Teams by projectkey
            EFTeamRepository efTeamRepository = new EFTeamRepository();
            string projectkeyVar = "projectkey1";
            IEnumerable<Team> teams = efTeamRepository.Get(projectkeyVar);

            //  get TeamMembers
            EFTeamMemberRepository efTeamMemberRepository = new EFTeamMemberRepository();
            IEnumerable<TeamMember> teamMembers = efTeamMemberRepository.Get();

            //  get Members
            EFMemberRepository efMemberRepository = new EFMemberRepository();
            IEnumerable<Member> members = efMemberRepository.Get();

            //  a list for sorted out members
            List<Member> membersVar = new List<Member>();
            List<TeamViewModel> tvmList = new List<TeamViewModel>();

            foreach (Team t2 in teams)
            {
                //IEnumerable<TeamMember> teamMembersVar = teamMembers.Where(tm => tm.TeamId == t2.Id);
                List<TeamMember> tmsVar = new List<TeamMember>();
                //
                TeamViewModel tvm = new TeamViewModel();
                tvm.Id = t2.Id;
                tvm.Name = t2.Name;
                tvm.ProjectKey = t2.ProjectKey;
                tvm.IsActive = t2.IsActive;
                //
                foreach (var tm in teamMembers)
                {
                    if (tm.TeamId == t2.Id)
                    {
                        tmsVar.Add(tm);
                    }
                }

                foreach (var tm2 in tmsVar)
                {
                    //Member memberVar = (Member)members.Where(m => m.Id == tm2.MemberId);
                    Member member = null;
                    if (efMemberRepository.TryGet(tm2.MemberId, out member))
                    {
                        if (member != null)
                        {
                            tvm.Members.Add(member);

                            // list of Members for testing
                            membersVar.Add(member);
                        }
                    }
                }
                //Console.WriteLine("{0}  {1}  {2} {3}", t2.Id, t2.Name, t2.ProjectKey, t2.IsActive);
                tvmList.Add(tvm);
            }

            //foreach (var m in membersVar)
            //{
            //    Console.WriteLine("{0}  {1}  {2}", m.Id, m.Username, m.Fullname, m.IsActive);
            //}

            return tvmList;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}