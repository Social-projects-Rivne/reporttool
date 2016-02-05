using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using ReportingTool.DAL.Entities;
using System.Data.Entity;
using Newtonsoft.Json;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {
        /// <summary>
        /// look for members without teams and [ isactive=>false ]
        /// </summary>
        public static HttpStatusCodeResult MemberCheck()
        {
            using (var ctx = new DB2())
            {
                try
                {
                    var query = from m in ctx.members.Include("teams")
                                orderby m.id
                                select m;

                    List<member> memberList = query.ToList<member>();

                    foreach (member memberVar in memberList)
                    {
                        if (memberVar.teams.Count == 0)
                        {
                            memberVar.isactive = false;
                            ctx.SaveChanges();
                        }
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Member table checked successfully");
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Member table is not checked ");
                }
            }
        }

        /// <summary>
        /// Get all teams for the specified project
        /// </summary>
        /// <param name="projectKey">name of a project -  string</param>
        /// <returns>A serialized list of teams with their members included</returns>
        [HttpGet]
        public string GetAll(string projectKey = "projectkey1")
        {
            List<team> teamList = new List<team>();

            using (var ctx = new DB2())
            {
                var query = from t in ctx.teams.Include("members")
                            orderby t.projectkey, t.name
                            where t.isactive == true && t.projectkey == projectKey
                            select t;

                teamList = query.ToList();
            }

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //return Json(teamList, JsonRequestBehavior.AllowGet);

            //  works
            string ouputJSON = JsonConvert.SerializeObject(teamList, Formatting.Indented);
            //string ouputJSON = JsonConvert.SerializeObject(teamList);
            return ouputJSON;
        }

        [HttpPost]
        public HttpStatusCode AddNewTeam(/*Some teamModel object*/)
        {
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Edit the specified team : add/delete some members
        /// </summary>
        /// <param name="teamFromJSON">A serialized team with a current list of members</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpPut]
        public HttpStatusCode Edit([ModelBinder(typeof(JsonNetModelBinder))] team teamFromJSON)
        //public HttpStatusCode Edit(team teamFromJSON) //  OK up to 2016-02-05
        //public JsonResult Edit(team teamFromJSON)         //  OK
        {
            team teamForUpdate = new team();

            using (var ctx = new DB2())
            {

                //  CHECK :   is a team with the specified projectkey and name present in DB ?
                teamForUpdate = ctx.teams.Include("members")
                    .SingleOrDefault<team>(t => t.name == teamFromJSON.name && t.projectkey == teamFromJSON.projectkey);

                //  CHECK RESULT  : No  ---> send a NotFound error response + exit
                if (teamForUpdate == null)
                {
                    return HttpStatusCode.NotFound;
                    //return null;    //  OK
                }

                //  CHECK RESULT  : Yes  ---> keep running

                //  the team in DB -> active
                teamForUpdate.isactive = true;

                try
                {
                    #region  1st run from DB thru JSON - members of the team from DB which are not present in JSON must be deleted
                    bool deleteMember = true;
                    member[] memberArrayDelete = teamForUpdate.members.ToArray<member>();
                    int index = 0;

                    for (int i = memberArrayDelete.GetLowerBound(0), upper = memberArrayDelete.GetUpperBound(0); i <= upper; i++)
                    {
                        deleteMember = true;
                        foreach (var itemFromJSON in teamFromJSON.members)
                        {
                            // found in JSON ?
                            if (memberArrayDelete[i].username == itemFromJSON.username)
                            {
                                deleteMember = false;
                                break;
                            }
                        }
                        // the member from DB is not found in JSON ---> delete 
                        if (deleteMember == true)
                        {
                            teamForUpdate.members.Remove(memberArrayDelete[i]);
                            ctx.SaveChanges();
                        }
                    }
                    
                    #endregion

                    #region 2nd run from JSON thru DB - members of the team from JSON which are not present in DB must be added
                    bool addMember = true;
                    member[] memberArrayAdd = new member[teamFromJSON.members.Count];
                    int idx = 0;

                    // add members present in JSON and missing in DB to add_array
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

                        // the member from JSON is not found in DB ---> add to add_array
                        if (addMember == true)
                        {
                            memberArrayAdd[idx++] = itemFromJSON;
                        }
                    }
                    //  -----
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

                                //  Insert Raw SQLcommand for team_member DB table
                                string SqlCommand = "insert into team_member(team_id, member_id) values(" +
                                    teamForUpdate.id + ", " +
                                    memberDup.id + ")";

                                int noOfRowInserted = ctx.Database.ExecuteSqlCommand(SqlCommand);

                                continue;
                            }
                        }
                        // ***********************************************************************************************

                        if (memberArrayAdd[k] != null)
                        {
                            teamForUpdate.members.Add(memberArrayAdd[k]);
                            ctx.SaveChanges();
                        }
                    }

                    #endregion

                    ctx.SaveChanges();
                }
                catch
                {
                    return HttpStatusCode.NotModified;
                }

            }

            MemberCheck();

            //return Json("TEAM UPDATED");    //  OK
            return HttpStatusCode.OK;
        }


        /// <summary>
        /// Delete a team with the specified id
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpDelete]
        public HttpStatusCodeResult Delete(int id)
        {
            using (var ctx = new DB2())
            {
                team teamDelete = ctx.teams.Include("members")
                .FirstOrDefault<team>(t => t.id == id);

                if (teamDelete == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Team is not found");
                    //return null;    //  OK
                }

                try
                {
                    ctx.teams.Remove(teamDelete);
                    ctx.teams.Attach(teamDelete);
                    ctx.Entry(teamDelete).State = EntityState.Deleted;
                    ctx.SaveChanges();

                    // now we add the deleted team to DB with isactive = false
                    teamDelete.isactive = false;
                    ctx.teams.Add(teamDelete);
                    ctx.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Team is not deleted");
                }
            }

            MemberCheck();
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
        }

    }
}