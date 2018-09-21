using Homeworks.DataAccess.Interface;
using Homeworks.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Homeworks.BusinessLogic.Tests
{
    [TestClass]
    public class UserLogicTests
    {
        [TestMethod]
        public void CreateValidUserTest()
        {
            var user = new User
            {
                UserName = "Hola",
                Password = "Hola"
            };
            var mock = new Mock<IRepository<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(It.IsAny<User>()));
            mock.Setup(m => m.Save());
            var userLogic = new UserLogic(mock.Object);

            var result = userLogic.Create(user);

            mock.VerifyAll();
            Assert.AreEqual(user.UserName, result.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateNullUserTest()
        {
            var mock = new Mock<IRepository<User>>(MockBehavior.Strict);
            var userLogic = new UserLogic(mock.Object);

            var result = userLogic.Create(null);
        }
    }
}
