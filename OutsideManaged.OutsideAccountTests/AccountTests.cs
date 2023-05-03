using EasyArchitect.OutsideManaged.AuthExtensions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OutsideManaged.OutsideAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutsideManaged.OutsideAccount.Tests
{
    [TestClass()]
    public class AccountTests
    {
        /// <summary>
        /// 這是如何在單元測試裡使用 Moq 處理需要注入 IServiceProvider 的範例 
        /// </summary>
        [TestMethod()]
        public async Task Test_GetAccountListView()
        {
            // Arrange
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json");

            IConfigurationRoot configuration = builder.Build();
            DbContextOptions<ModelContext> dbContextOptions = new DbContextOptionsBuilder<ModelContext>()
                .UseOracle(configuration.GetConnectionString("OutsideDbContext"))
                .Options;

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(ModelContext)))
                .Returns(new ModelContext(dbContextOptions));

            Account target = new Account(serviceProvider.Object);

            bool actual;
            bool expected = true;

            // Act
            actual = await target.CheckAccountIsValid("gelis", new DateTime(2023, 1, 31));

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}