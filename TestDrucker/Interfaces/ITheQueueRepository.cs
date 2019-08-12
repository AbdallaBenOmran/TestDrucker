using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Models.TheQ;

namespace TestDrucker.Interfaces
{
    public interface ITheQueueRepository
    {
        List<TheQueue> GetTheQueue();
        int AddId(string PrinterName, string Filename );
        //List<TheQueue> Display();
    }
}
