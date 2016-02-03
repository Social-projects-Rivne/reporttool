using ReportingTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using ReportingTool.DAL.Entities;
//using ReportingTool.DAL.Repositories;
using System.Data.Entity;
using Newtonsoft.Json;
//using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {
        //  look for members without teams and [ isactive=>false ]
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

        #region Old GET Controller
        //[HttpGet]
        //public JsonResult GetAllTeams() {
        //    var temp = new List<TempTeamModel>();
        //   TempTeamModel TempTeam = new TempTeamModel();
        //    TempTeam.teamID = 5;
        //    TempTeam.teamName= "Test Team 1";
        //    TempTeam.members.Add(new TempMemberModel("1_feodtc", "1_Feol"));
        //    TempTeam.members.Add(new TempMemberModel("1_loginName_1", "1_fullName_1"));
        //    TempTeam.members.Add(new TempMemberModel("1_loginName_2", "1_fullName_2"));
        //    temp.Add(TempTeam);

        //    TempTeam = new TempTeamModel(7, "Test Team 2");
        //    TempTeam.members.Add(new TempMemberModel("2_loginName_1", "2_fullName_1"));
        //    TempTeam.members.Add(new TempMemberModel("2_loginName_2", "2_fullName_2"));
        //    TempTeam.members.Add(new TempMemberModel("2_loginName_3", "2_fullName_3"));
        //    TempTeam.members.Add(new TempMemberModel("2_loginName_4", "2_fullName_4"));
        //    temp.Add(TempTeam);

        //    TempTeam = new TempTeamModel(11, "Test Team 3");
        //    TempTeam.members.Add(new TempMemberModel("3_loginName_1", "3_fullName_1"));
        //    TempTeam.members.Add(new TempMemberModel("3_loginName_2", "3_fullName_2"));
        //    temp.Add(TempTeam);

        //    return Json(temp, JsonRequestBehavior.AllowGet);
        //} 
        #endregion

        [HttpGet]
        // http://localhost:7898/api/teams?projectKey=projectkey1
        //public List<team> Get(string projectKey)

        //public JsonResult GetAll(string projectKey = "projectkey1")
        public string GetAll(string projectKey = "projectkey1")
        {
            List<team> teamList = new List<team>();

            using (var ctx = new DB2())
            {
                //ctx.Database.Log = Console.WriteLine;
                //Console.WriteLine("\nTEAMS : \n");
                // -------------- READ  all teams and their members -----------------------------------------------------

                var query = from t in ctx.teams.Include("members")
                            orderby t.projectkey, t.name
                            where t.isactive == true && t.projectkey == projectKey
                            select t;

                teamList = query.ToList();

                //foreach (team teamv in query)
                //{
                //    teamList.Add(teamv);
                //}

                #region JSON experiments
                //var serializerSettings = new JsonSerializerSettings { 
                //    PreserveReferencesHandling = PreserveReferencesHandling.Objects };

                //Console.WriteLine(JsonConvert.SerializeObject(teamList, serializerSettings));
                //Console.WriteLine(JsonConvert.SerializeObject(teamList, Formatting.Indented, serializerSettings));
                //  returns good JSON - OK
                //Console.WriteLine(JsonConvert.SerializeObject(teamList, Formatting.Indented));

                //return JsonConvert.SerializeObject(teamList, Formatting.Indented);
                //return teamList; 
                #endregion

            }

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //return Json(teamList, JsonRequestBehavior.AllowGet);
            string ouputJSON = JsonConvert.SerializeObject(teamList, Formatting.Indented);
            return ouputJSON;
        }

        [HttpPost]
        public HttpStatusCode AddNewTeam(/*Some teamModel object*/)
        {
            return HttpStatusCode.OK;
        }

        #region Old Put Controller
        //[HttpPut]
        //public HttpStatusCode EditTeam(/*Some teamModel object*/)
        //{
        //    return HttpStatusCode.OK;
        //} 
        #endregion

        [HttpPut]
        //public void Put(int id, [FromBody]team tvm)
        //public HttpStatusCode Edit(team t1)
        //public HttpStatusCode Edit(string tJ)
        public JsonResult Edit(team teamFromJSON)
        {
            //team t1 = JsonConvert.DeserializeObject<team>(tJ);
            //team t1 = JsonConvert.DeserializeObject(teamString, team);

            team teamForUpdate = new team();

            using (var ctx = new DB2())
            {
                #region Stub for console update test
                //ctx.Database.Log = Console.WriteLine;
                //Console.WriteLine("\nUPDATE TEAM2 : \n");

                // -- team for update ---
                //string t1Name = "teamNEW1";
                //string t1ProjectKey = "projectkey1";

                //team t1 = new team();
                //t1.name = t1Name;
                //t1.projectkey = t1ProjectKey;
                //t1.isactive = true;
                //t1.members.Add(new member { username = "username51", fullname = "fullname51", isactive = true });
                //t1.members.Add(new member { username = "username101", fullname = "fullname101", isactive = true });
                //t1.members.Add(new member { username = "username53", fullname = "fullname53", isactive = true }); 
                #endregion

                //team teamUpdate = ctx.teams.Include("members")
                //    .SingleOrDefault<team>(t => t.name == t1Name && t.projectkey == t1ProjectKey);

                //  check : 
                //  is a team with the specified projectkey and name present in DB ?
                teamForUpdate = ctx.teams.Include("members")
                    .SingleOrDefault<team>(t => t.name == teamFromJSON.name && t.projectkey == teamFromJSON.projectkey);

                //  check result : no -> send an error response + exit
                if (teamForUpdate == null)
                {
                    // worked OK
                    //return HttpStatusCode.NotFound;

                    return null;
                }

                //  check result : yes -> keep running

                //  the team in DB -> active
                teamForUpdate.isactive = true;

                #region  1st run from DB thru JSON
                //  members of the team which are not present in JSON must be deleted 
                bool deleteMember = true;
                member[] memberArrayDelete = new member[teamForUpdate.members.Count];
                int index = 0;

                foreach (var itemFromDB in teamForUpdate.members)
                //for (int i = 0; i < teamUpdate.members.Count; i++)
                {
                    deleteMember = true;
                    foreach (var itemFromJSON in teamFromJSON.members)
                    //for (int j = 0; j < t1.members.Count; i++)
                    {
                        // found in JSON
                        if (itemFromDB.username == itemFromJSON.username)
                        //if (t1.members[i].username == itemJSON.username)
                        {
                            deleteMember = false;
                            break;
                        }
                    }

                    // the member from DB is not found in JSON -> add to delete array
                    if (deleteMember == true)
                    {
                        //teamUpdate.members.Remove(itemDB);
                        memberArrayDelete[index++] = itemFromDB;
                        //ctx.SaveChanges();
                    }
                }

                // members from delete array are deleted
                for (int k = 0; k < teamForUpdate.members.Count; k++)
                {
                    if (memberArrayDelete[k] != null)
                    {
                        teamForUpdate.members.Remove(memberArrayDelete[k]);
                    }
                }
                #endregion

                #region 2nd run from JSON thru DB
                //  members of the team from JSON which are not present in DB must be added 
                bool addMember = true;
                member[] memberArrayAdd = new member[teamFromJSON.members.Count];
                int idx = 0;

                // add members present in JSON and missing in DB to temp array
                foreach (var itemFromJSON in teamFromJSON.members)
                {
                    addMember = true;
                    foreach (var itemFromDB in teamForUpdate.members)
                    {
                        // already present in DB -> not added to DB
                        if (itemFromDB.username == itemFromJSON.username)
                        {
                            addMember = false;
                            break;
                        }
                    }

                    // the member from JSON is not found in DB -> add to add_array
                    if (addMember == true)
                    {
                        //teamUpdate.members.Add(itemJSON);
                        memberArrayAdd[idx++] = itemFromJSON;
                        //ctx.SaveChanges();
                    }
                }
                //  ##########################################################################
                // members from add_array are added
                for (int k = 0; k < teamFromJSON.members.Count; k++)
                {
                    // **** check if members to add are already present in DB ***********************
                    // 
                    if (memberArrayAdd[k] != null)
                    {
                        member memberTmp = memberArrayAdd[k];

                        member memberDup = ctx.members.
                            SingleOrDefault<member>(m => m.username == memberTmp.username);

                        // if a member with the same name exists he is activated
                        if (memberDup != null)
                        {
                            memberDup.isactive = true;

                            //Insert Raw SQLcommand for team_member DB table
                            string SqlCommand = "insert into team_member(team_id, member_id) values(" +
                                teamForUpdate.id + ", " +
                                memberDup.id + ")";

                            //int noOfRowInserted = ctx.Database.ExecuteSqlCommand("insert into team_member(studentname) values('New Student')");
                            int noOfRowInserted = ctx.Database.ExecuteSqlCommand(SqlCommand);

                            continue;
                        }
                    }
                    // *************************

                    if (memberArrayAdd[k] != null)
                    {
                        teamForUpdate.members.Add(memberArrayAdd[k]);
                    }
                }

                #endregion

                ctx.SaveChanges();
            }

            MemberCheck();
            // worked OK
            //return HttpStatusCode.OK;

            //string ouputJSON = JsonConvert.SerializeObject(teamUpdate, Formatting.Indented);
            //return ouputJSON;

            //return Json(teamUpdate);
            return Json("TEAM UPDATED");
        }


        #region Old delete controller
        //[HttpDelete]
        //public HttpStatusCodeResult DeleteTeam(string teamID)
        //{
        //    return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
        //} 
        #endregion

        [HttpDelete]
        //public void Delete(int id)
        public HttpStatusCodeResult Delete(int id = 4)
        {
            using (var ctx = new DB2())
            {
                //ctx.Database.Log = Console.WriteLine;

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
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
        }

    }
}