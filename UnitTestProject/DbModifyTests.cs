using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Web.Mvc;
using ReportingTool.Controllers;
using ReportingTool.DAL.Entities;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for DbModifyTests
    /// </summary>
    [TestClass]
    public class DbModifyTests
    {
        //public DbModifyTests()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}

        //private TestContext testContextInstance;

        ///// <summary>
        /////Gets or sets the test context which provides
        /////information about and functionality for the current test run.
        /////</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}

        //#region Additional test attributes
        ////
        //// You can use the following additional attributes as you write your tests:
        ////
        //// Use ClassInitialize to run code before running the first test in the class
        //// [ClassInitialize()]
        //// public static void MyClassInitialize(TestContext testContext) { }
        ////
        //// Use ClassCleanup to run code after all tests in a class have run
        //// [ClassCleanup()]
        //// public static void MyClassCleanup() { }
        ////
        //// Use TestInitialize to run code before running each test 
        //// [TestInitialize()]
        //// public void MyTestInitialize() { }
        ////
        //// Use TestCleanup to run code after each test has run
        //// [TestCleanup()]
        //// public void MyTestCleanup() { }
        ////
        //#endregion

        [TestMethod]
        //public void TestMethod1()
        public async Task TestMethod1()
        {
            // Arrange - create the controller
            TemplatesController target = new TemplatesController();

            // Arrange - create a template
            //  string inputJSON = "{ \"templateName\": \"testtemplate01\", \"isActive\": true, \"owner\": \"testowner01\" }";
            string name = "testtemplate01";
            Template t1 = new Template { Name = name, Owner = "testowner01", IsActive = true };

            //// Act - try add a template
            ActionResult result = target.AddNewTemplate(t1);
            #region  manual delete
            //using (var db = new DB2())
            //{
            //    db.Templates.Add(t1);
            //    db.SaveChanges();
            //} 
            #endregion

            //// Assert - check that a template exists
            #region ver 1 of assert
            //using (var db = new DB2())
            //{
            //    Assert.IsNotNull(
            //        db.Templates.FirstOrDefaultAsync<Template>(t => t.Name == name && t.IsActive == true) );
            //} 
            #endregion

            Template temp = new Template();
            using (var db = new DB2())
            {
                temp = await db.Templates.FirstOrDefaultAsync<Template>(t => t.Name == name && t.IsActive == true);
                Assert.IsNotNull(temp);
            }

            //// Act - try delete a template
            result = target.DeleteTemplate(temp.Id);

            //// Assert - check that a template is not active
            using (var db = new DB2())
            {
                temp = await db.Templates.FirstOrDefaultAsync<Template>(t => t.Name == name && t.IsActive == true);
                Assert.IsNull(temp);
            }

        }

        #region Async LINQ Query Example - just info !
        //public async Task<IHttpActionResult> GetBook(int id)
        //{
        //    BookDto book = await db.Books.Include(b => b.Author)
        //        .Where(b => b.BookId == id)
        //        .Select(AsBookDto)
        //        .FirstOrDefaultAsync();
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(book);
        //} 
        #endregion
        // ---
    }
}
