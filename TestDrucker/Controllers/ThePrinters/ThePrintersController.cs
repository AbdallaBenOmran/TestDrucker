using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestDrucker.Interfaces;
using TestDrucker.Models;
using TestDrucker.Models.PrinterCanon;
using TestDrucker.Models.TheQ;
using TestDrucker.Models.ViewModels;

namespace TestDrucker.Controllers.PrinterCanon
{
    public class ThePrintersController : Controller
    {
        public readonly IConfiguration _configuration;
        private readonly ITheQueueRepository _queueRepository;
        private readonly IPrinterRepository _printerRepository;

        //DBAccess db;
        //DBQueue dbQ;
        public List<Printer> PrinterList { get; set; }
        public List<Branch> valueBranches { get; set; }
        public List<TheQueue> queueList { get; set; }
        public string contentRoot { get; set; }

        public ThePrintersController(IHostingEnvironment env, IConfiguration configuration)
        {
            contentRoot = env.ContentRootPath;
            _configuration = configuration;
            //dbQ = new DBQueue(_configuration.GetConnectionString("ServiceDbLive"), _configuration.GetConnectionString("ServiceDbTest"));
            //db = new DBAccess(_configuration.GetConnectionString("DBAccessCon"));
            _queueRepository = new DBQueue(_configuration.GetConnectionString("ServiceDbLive"), _configuration.GetConnectionString("ServiceDbTest"));
            _printerRepository = new DBAccess(_configuration.GetConnectionString("DBAccessCon"));
        }
       
        //GET : // ThePrinters/Index
        public ViewResult Index()
        {
            var selectListBranches = new List<SelectListItem>();
            valueBranches = GetBranchAndLocation().ToList();

            foreach (var branch in valueBranches)
            {
                selectListBranches.Add(new SelectListItem(branch.BranchLocationCode, branch.BranchLocationCode));
            }

            var selectListPrinter = new List<SelectListItem>();
            PrinterList = GetAllItems().ToList();

            foreach (var print in PrinterList)
            {
                selectListPrinter.Add(new SelectListItem(print.DeviceName, print.Id));
            }

            //queueList = dbQ.GetTheQ().ToList();
            //queueList = dbQ.GetTheQueue().ToList();
            queueList = GetTheQueue();
            queueList = Display();

            var dvm = new DetailsViewModel();
            dvm.BranchAndLocations = selectListBranches;
            dvm.GetPrinters = selectListPrinter;
            //dvm.GetQueues = queueList;
            dvm.GetQueues = GetTheQueue();

            return View(dvm);
        }
        public List<TheQueue> GetTheQueue()
        {
            return _queueRepository.GetTheQueue();
        }
        public int AddId (string PrinterName, string Filename)
        {
            return _queueRepository.AddId(PrinterName, Filename);
        }
        public List<Printer> GetAllItems()
        {
            return _printerRepository.GetAllItems();
        }

        public List<Printer> GetAllBranchItems(string branchCode)
        {
            return _printerRepository.GetAllBranchItems(branchCode);
        }
        public List<Branch> GetBranchAndLocation()
        {
            return _printerRepository.GetBranchAndLocation();
        }

        [HttpPost]
        public JsonResult GetPrinters(string branchCode)
        {
            var selectListPrinter = new List<SelectListItem>();
            PrinterList = GetAllBranchItems(branchCode).ToList();
            foreach (var print in PrinterList)
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
                PrinterList = GetAllItems().ToList();

                var printer = PrinterList.Single(d => d.DeviceName == PrinterName);

                if (printer.DeviceType == "Printer" && printer.DeviceSubtype == "A4")
                {
                    FileStream PdfA4 = new FileStream("Test-A4.pdf", FileMode.Open, FileAccess.Read);
                    PdfLoadedDocument loadedDocumentA4 = new PdfLoadedDocument(PdfA4);
                    MemoryStream stream = new MemoryStream();
                    loadedDocumentA4.Save(stream);

                    var myUniqueFileName = $@"Test-A4_{Guid.NewGuid()}.pdf";
                    using (var file = new FileStream(Path.Combine(folderPath, myUniqueFileName), FileMode.Create, FileAccess.Write))
                    {
                        stream.WriteTo(file);
                        AddId(PrinterName, file.Name);
                    }
                }
                else if (printer.DeviceType == "Printer" && printer.DeviceSubtype == "Label")
                {
                    FileStream PdfLabel = new FileStream("Test-Label-3,9x7,9-inch.pdf", FileMode.Open, FileAccess.Read);
                    PdfLoadedDocument loadedDocumentLabel = new PdfLoadedDocument(PdfLabel);
                    MemoryStream stream = new MemoryStream();
                    loadedDocumentLabel.Save(stream);

                    var myUniqueFileName = $@"Test-Label-3,9x7,9-inch_{Guid.NewGuid()}.pdf";
                    using (var file = new FileStream(Path.Combine(folderPath, myUniqueFileName), FileMode.Create, FileAccess.Write))
                    {
                        stream.WriteTo(file);
                        AddId(PrinterName, file.Name);
                    }
                }
            }

            return RedirectToAction("Index");
        }
        //public List<TheQueue> Display()
        //{
        //    return _queueRepository.Display();
        //}
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