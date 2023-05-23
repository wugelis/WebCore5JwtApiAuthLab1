using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EasyArchitect.OutsideManaged.AuthExtensions.Crypto;
using Mxic.FrameworkCore.Core;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        [TestMethod()]
        public void Test_Authenticate()
        {
            // Arrange
            Mock<IUserService> target = new Mock<IUserService>();
            string secert = ConfigurationManager.AppSettings["Secret"];
            string input = "test";
            string encrpto = Rijndael.EncryptString(input);
            string expectedToken = "AABB";
            string actualToken;
            AuthenticateRequest request = new AuthenticateRequest() { Username = "geliswu", Password = "eSFj5ZBtva1FpuqHR4W9Fw==" };

            target
                .Setup(c => c.Authenticate(request))
                .Returns(new AuthenticateResponse(new User()
                {
                    Id = 1,
                    Username = request.Username
                }, expectedToken) { Token = "AABB"});

            // Act
            AuthenticateResponse response = target.Object.Authenticate(request);
            actualToken = response.Token;

            // Assert
            Assert.AreEqual(expectedToken, actualToken);
        }
    }
}