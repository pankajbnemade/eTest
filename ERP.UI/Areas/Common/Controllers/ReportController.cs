using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Common.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Print()
        //{



        //    string mimtype = "";
        //    int exetension = 1;
        //    var path = $"{this._webHostEnvironment.WebRootPath}\\Report\\rptSalesInvoice.rdlc";
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("param1", "ERP");
        //    LocalReport localReport = new LocalReport(path);
        //    var result = localReport.Execute(RenderType.Pdf, exetension, parameters, mimtype);

        //    return File(result.MainStream, "application/pdf");

        //}

        public IActionResult Print()
        {

            var dt = new DataTable();

            dt = GetReportData();

            string mimtype = "";
            int exetension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Report\\Accounts\\rptSalesInvoice.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("param1", "ERP");
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsSalesInvoice",dt);

            var result = localReport.Execute(RenderType.Pdf, exetension, parameters, mimtype);

            return File(result.MainStream, "application/pdf");

        }

        public DataTable GetReportData()
        {
            var dt = new DataTable();
            dt.Columns.Add("InvoiceNo");
            dt.Columns.Add("InvoiceDate");

            DataRow row;

            row = dt.NewRow();

            row["InvoiceNo"] = "kjhkh1234";
            row["InvoiceDate"] ="hgjgjgjg";

            dt.Rows.Add(row);

            return dt;
        }


    }
}
