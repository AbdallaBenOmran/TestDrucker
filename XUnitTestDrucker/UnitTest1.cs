using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Configuration;
using System.IO;
using TestDrucker.Controllers.PrinterCanon;
using TestDrucker.Interfaces;
using TestDrucker.Models.PrinterCanon;
using TestDrucker.Models.TheQ;
using Xunit;

namespace XUnitTestDrucker
{
    public class UnitTest1
    {
        public IConfiguration _configuration;
        private readonly IQueueRepository _queueRepository;


        public UnitTest1()
        {
            CreateConfiguration();
            _queueRepository = new DBQueueRepository(_configuration.GetConnectionString("ServiceDbLive"), _configuration.GetConnectionString("ServiceDbTest"));

        }

        [Fact]
        public void CreateConfiguration()
        {
            var configPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\", "appsettings.json"));
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();
        }

        [Fact]
        public void Test1()
        {
            var mock = new Mock<IQueueRepository>();
            mock.Setup(m => m.AddQueue("printer", "C File")).Returns(1);
                       

        }
    }
}