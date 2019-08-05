using System;
using TestDrucker.Controllers.PrinterCanon;
using TestDrucker.Models.PrinterCanon;
using TestDrucker.Models.TheQ;
using Xunit;

namespace XUnitTestDrucker
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DBQueue dB = new DBQueue();
            dB.AddId("canon", "C");
        }

    
    
    }
}
