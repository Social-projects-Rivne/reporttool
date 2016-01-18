using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresTest1
{
  class Program
  {
    #region PetsExample
    //static void Main(string[] args)
    //{
    //  var db = new DB();

    //  //var pet = new Pet { ID = 7, Name = "Stevie" };

    //  var pet1 = new Pet { Name = "Parrot" };
    //  var pet2 = new Pet { Name = "Eagle" };
    //  var pet3 = new Pet { Name = "Falcon" };
    //  db.Pets.Add(pet1);
    //  db.Pets.Add(pet2);
    //  db.Pets.Add(pet3);
    //  db.SaveChanges();

    //  //var pet = new Pet();
    //  ////pet.ID = 7;
    //  //pet.Name = "FOX";
    //  //db.Pets.Add(pet);
    //  //db.SaveChanges();

    //  var pets = db.Pets;
    //  foreach (var p in pets)
    //  {
    //    Console.WriteLine(p.Name);
    //  }
    //  Console.Read();
    //} 
    #endregion


    static void PrintTeamMembers()
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

    static void DeleteTeamMember(int id)
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

      int idForDelete = 2;
      DeleteTeamMember(idForDelete);

      Console.WriteLine();
      PrintTeamMembers();

      Console.Read();
    }
  }
}
