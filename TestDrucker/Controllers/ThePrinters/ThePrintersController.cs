using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestDrucker.Models;
using TestDrucker.Models.PrinterCanon;
using TestDrucker.Models.TheQ;
using TestDrucker.Models.ViewModels;

namespace TestDrucker.Controllers.PrinterCanon
{
    public class ThePrintersController : Controller
    {
        DBAccess db = new DBAccess();
        DBQueue dbQ = new DBQueue();
        public List<Printer> Printer { get; set; }
        public List<Branch> valueBranches { get; set; }
        public List<TheQueue> queueList { get; set; }
        public string contentRoot { get; set; }
        public ThePrintersController(IHostingEnvironment env)
        {
            contentRoot = env.ContentRootPath;
        }

        //GET : // ThePrinters/Index
        public ViewResult Index()
        {
            var selectListBranches = new List<SelectListItem>();
            valueBranches = db.GetBranchAndLocation().ToList();

            foreach (var branch in valueBranches)
            {
                selectListBranches.Add(new SelectListItem(branch.BranchLocationCode, branch.BranchLocationCode));
            }

            var selectListPrinter = new List<SelectListItem>();
            Printer = db.GetAllItems().ToList();

            foreach (var print in Printer)
            {
                selectListPrinter.Add(new SelectListItem(print.DeviceName, print.Id));
            }

            queueList = dbQ.GetTheQ().ToList();
            queueList = Display();

            var dvm = new DetailsViewModel();
            dvm.BranchAndLocations = selectListBranches;
            dvm.GetPrinters = selectListPrinter;
            dvm.GetQueues = queueList;
  
            return View(dvm);
        }

        [HttpPost]
        public JsonResult GetPrinters(string branchCode)
        {
            var selectListPrinter = new List<SelectListItem>();
            Printer = db.GetAllBranchItems(branchCode).ToList();
            foreach (var print in Printer)
            {
                selectListPrinter.Add(new SelectListItem(print.DeviceName, print.DeviceName));
            }
            return Json(selectListPrinter);
        }

        //Button Action testseite drucken
        public IActionResult TestPrinter(string PrinterName)
        {
            if (ModelState.IsValid)
            {
                string folderPath = @"C:\Users\BenOmran\Desktop\savepdf";
                Printer = db.GetAllItems().ToList();

                FileStream PdfA4 = new FileStream("Test-A4.pdf", FileMode.Open, FileAccess.Read);
                FileStream PdfLabel = new FileStream("Test-Label-3,9x7,9-inch.pdf", FileMode.Open, FileAccess.Read);

                PdfLoadedDocument loadedDocumentA4 = new PdfLoadedDocument(PdfA4);
                PdfLoadedDocument loadedDocumentLabel = new PdfLoadedDocument(PdfLabel);

                PdfPageBase pageA4 = loadedDocumentA4.Pages[0];
                PdfPageBase pageLabel = loadedDocumentLabel.Pages[0];

                MemoryStream stream = new MemoryStream();
                loadedDocumentA4.Save(stream);

                string extractedTextA4 = pageA4.ExtractText(true);
                string extractedTextLabel = pageLabel.ExtractText(true);

                if (extractedTextA4.StartsWith("(Kundennummer:)"))
                {
                    var myUniqueFileName = $@"Test-A4_{Guid.NewGuid()}.pdf";
                    using (var file = new FileStream(Path.Combine(folderPath, myUniqueFileName), FileMode.Create, FileAccess.Write))
                    {
                        stream.WriteTo(file);
                        dbQ.AddId(PrinterName, file.Name);
                    }
                }
                else if (extractedTextLabel.StartsWith("Von:Fahrrad-XXL.de"))
                {
                    var myUniqueFileName = $@"Test-Label-3,9x7,9-inch_{Guid.NewGuid()}.pdf";
                    using (var file = new FileStream(Path.Combine(folderPath, myUniqueFileName), FileMode.Create, FileAccess.Write))
                    {
                        stream.WriteTo(file);
                        dbQ.AddId(PrinterName, file.Name);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public List<TheQueue> Display()
        {
            foreach (var item in queueList)
            {
                if (item.LastStatus != "Printed" && item.LastStatus != "Error")
                {
                    RefreshPage();
                }
                else if (item.TimeDiffInMinutes < 2)
                {
                    RefreshPage();
                }
            }
            return queueList;
        }
        public void RefreshPage()
        {
            Response.Headers.Add("Refresh", "60");
        }
    }
}