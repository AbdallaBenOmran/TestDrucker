using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Models.TheQ;

namespace TestDrucker.Interfaces
{
    public interface IQueueRepository
    {
        List<TheQueue> GetQueue();
        int AddQueue(string PrinterName, string Filename );
    }
}
