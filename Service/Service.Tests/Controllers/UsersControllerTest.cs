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
    public class UsersControllerTest
    {
        public UserController UserController { get; set; }

        public UsersControllerTest()
        {
            UserController = new UserController();
        }

        [TestCase]
        public void TestGet()
        {
            var result = this.UserController.Users(0);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveNew()
        {
            var result = this.UserController.SaveUser(new Models.UserDto { Id = 0, FirstName = "Test", LastName = "Test" });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveUpdate()
        {
            var result = this.UserController.SaveUser(new Models.UserDto { Id = 5 });
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSaveDelete()
        {
            var result = this.UserController.deleteUser(5);
            Assert.IsNotNull(result);
        }

        [TestCase]
        public void TestSearch()
        {
            var result = this.UserController.searchData("");
            Assert.IsNotNull(result);
        }
    }
}
