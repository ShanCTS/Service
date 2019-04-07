using NUnit.Framework;
using Service.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests.Controllers
{
    [TestFixture]
    public class ProjectControllerTest
    {
        public ProjectController ProjectController { get; set; }

        public ProjectControllerTest()
        {
            this.ProjectController = new ProjectController();
        }

        [TestCase]
        public void TestGet()
        {
            var result = this.ProjectController.Projects(0);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveNew()
        {
            var result = this.ProjectController.SaveProjects(new Models.ProjectDto { Id = 0, Name = "Test", Priority = 1 });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveUpdate()
        {
            var result = this.ProjectController.SaveProjects(new Models.ProjectDto { Id = 5 });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveDelete()
        {
            var result = this.ProjectController.delete(5);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSearch()
        {
            var result = this.ProjectController.searchData("");
            Assert.IsNotNull(result);
        }
    }
}
