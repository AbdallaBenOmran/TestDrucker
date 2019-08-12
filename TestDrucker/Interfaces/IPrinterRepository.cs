using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Models;
using TestDrucker.Models.PrinterCanon;

namespace TestDrucker.Interfaces
{
   public interface IPrinterRepository
    {
        List<Printer> GetAllItems();
        List<Printer> GetAllBranchItems(string branchCode);
        List<Branch> GetBranchAndLocation();
    }
}
