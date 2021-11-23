using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Print()
        {
            string mimtype = "";
            int exetension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Accounts\\rptSalesInvoice.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("param1", "ERP");
            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, exetension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");

        }
    }
}
