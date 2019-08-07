using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using TestDrucker.Controllers.PrinterCanon;
using TestDrucker.Models.PrinterCanon;
using TestDrucker.Models.TheQ;
using Xunit;

namespace XUnitTestDrucker
{
    public class UnitTest1
    {
        public IConfiguration _configuration;
        DBQueue dbQ;

        public UnitTest1()
        {
            CreateConfiguration();
            dbQ = new DBQueue(_configuration.GetConnectionString("ServiceDbLive"), _configuration.GetConnectionString("ServiceDbTest"));

        }

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
            //DBQueue dbQ = new DBQueue();
            var res = dbQ.AddId("canon", "C");
            

        }
    }
}