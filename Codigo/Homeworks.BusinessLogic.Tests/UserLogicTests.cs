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
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateNullUserTest()
        {
            
        }
    }
}
