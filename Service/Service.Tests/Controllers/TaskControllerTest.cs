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
    public class TaskControllerTest
    {
        public TaskController TaskController { get; set; }

        public TaskControllerTest()
        {
            TaskController = new TaskController();
        }

        [TestCase]
        public void TestGet()
        {
            var result = this.TaskController.Tasks(0);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveNew()
        {
            var result = this.TaskController.SaveTasks(new Models.TaskDto { Id = 0, Description = "Test", Priority = 1 });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestParentTaskSaveNew()
        {
            var result = this.TaskController.SaveTasks(new Models.TaskDto { Id = 0, Description = "Test", IsParentTask = true });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveUpdate()
        {
            var result = this.TaskController.SaveTasks(new Models.TaskDto { Id = 5 });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveDelete()
        {
            var result = this.TaskController.delete(5);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSearch()
        {
            var result = this.TaskController.searchData("");
            Assert.IsNotNull(result);
        }
    }
}
