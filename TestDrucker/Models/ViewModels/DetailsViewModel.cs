using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestDrucker.Models.PrinterCanon;
using TestDrucker.Models.ThePrinters;
using TestDrucker.Models.TheQ;

namespace TestDrucker.Models.ViewModels
{
    public class  DetailsViewModel
    {
        [Display(Name = " Filiale ")]
        public List<SelectListItem> BranchAndLocations { get; set; }
        public SelectListItem SelectedBranchLocation { get; set; }

        [Display(Name ="Drucker Name")]
        public List<SelectListItem> GetPrinters { get; set; }
        public SelectListItem SelectedPrinters { get; set; }
        public List<TheQueue> GetQueues { get; set; }

    }
}
