using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using SportsStore.Models.Entities;
using SportsStore.Models.Abstract;
using SportsStore.Models;

using SportsStore.Controllers;
using System.Web.Mvc;

namespace SportStore.Test
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials() //когда пользователь указал правильный логин и пароль
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("123", "321")).Returns(true);
            LoginViewModel login = new LoginViewModel()
            {
                LoginName = "123",
                Password = "321"
            };

            AccountController target = new AccountController(mock.Object);
            ActionResult result = target.Login(login, "MuUrl");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "MuUrl");
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials() //когда мы передаем неверные даныне
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("321", "321")).Returns(false);

            LoginViewModel login = new LoginViewModel()
            {
                LoginName = "321",
                Password = "321"
            };

            AccountController target = new AccountController(mock.Object);

            ActionResult result = target.Login(login, null);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewBag.ModelState.IsValid);
        }
    }
}
