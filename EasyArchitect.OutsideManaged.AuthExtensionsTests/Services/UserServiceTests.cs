using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyArchitect.OutsideManaged.AuthExtensions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EasyArchitect.OutsideManaged.AuthExtensions.Crypto;
using MxicFrameworkCore = Mxic.FrameworkCore.Core;
using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        /// <summary>
        /// 測試 UserService.Authenticate() 驗證功能方法
        /// </summary>
        [TestMethod()]
        public void Test_Authenticate()
        {
            // Arrange
            Mock<IUserService> target = new Mock<IUserService>();
            string secert = MxicFrameworkCore.ConfigurationManager.AppSettings["Secret"];
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

        [TestMethod]
        public void Test_GetSequence()
        {
            // Arrange
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json");

            Microsoft.Extensions.Configuration.IConfigurationRoot configuration = builder.Build();

            var options = new DbContextOptionsBuilder<ModelContext>()
                .UseOracle(configuration.GetConnectionString("OutsideDbContext"))
                .Options;

            ModelContext context = new ModelContext(options);
            int actual;
            int expected = 1;

            // Act
            DbCommand dbCmd = context.Database.GetDbConnection().CreateCommand();
            dbCmd.CommandText = "SELECT ACCOUNTVO_SEQ.nextval from DUAL";
            context.Database.OpenConnection();
            DbDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            actual = reader.GetInt32(0);

            // Assert
            Assert.IsTrue(actual >= expected);
        }
    }
}