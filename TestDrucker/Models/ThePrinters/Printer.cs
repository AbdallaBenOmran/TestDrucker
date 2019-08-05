using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestDrucker.Models.PrinterCanon
{
    public class Printer
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIP { get; set; }
        public string DeviceType { get; set; }
        public string DeviceSubtype { get; set; }
        public int BranchNo { get; set; }
    }
}
