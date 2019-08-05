
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestDrucker.Models.TheQ
{
    public class TheQueue
    {
        public int Id { get; set; }
        public string LastStatus { get; set; }
        public string LastStatusDetails { get; set; }
        public DateTime AddedToQueue { get; set; }
        public string PrinterName { get; set; }
        public string Filename { get; set; }
        public double TimeDiffInMinutes
        {
            get
            {
                return DateTime.Now.Subtract(AddedToQueue).TotalMinutes;
            }
        }

    }
}
