using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminLoginTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            // arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);

            LoginViewModel model = new LoginViewModel
                {
                    UserName = "admin",
                    Password = "secret"
                };

            AccountController target = new AccountController(mock.Object);

            // act
            ActionResult result = target.Login(model, "/MyURL");

            // assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            // arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            LoginViewModel model = new LoginViewModel
                {
                    UserName = "badUser",
                    Password = "badPass"
                };

            AccountController target = new AccountController(mock.Object);

            // act
            ActionResult result = target.Login(model, "/MyURL");

            // assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
