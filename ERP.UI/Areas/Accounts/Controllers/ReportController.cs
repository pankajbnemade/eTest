using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using ERP.UI.Report.Accounts;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class ReportController : Controller
    {
        private readonly ISalesInvoiceReport _salesInvoiceReport;
        IWebHostEnvironment _webHostEnvironment;

        public ReportController(IWebHostEnvironment webHostEnvironment, ISalesInvoiceReport salesInvoiceReport)
        {
            this._salesInvoiceReport = salesInvoiceReport;
            this._webHostEnvironment = webHostEnvironment;
        }

        public async Task<ActionResult> SalesInvoiceReport(int id)
        {
            SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();
            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = new List<SalesInvoiceDetailModel>();
            IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList = new List<SalesInvoiceTaxModel>();
            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = new List<SalesInvoiceDetailTaxModel>();

            salesInvoiceModel = await _salesInvoiceReport.GetSalesInvoice(id);
            salesInvoiceDetailModelList = await _salesInvoiceReport.GetSalesInvoiceDetailList(id);
            salesInvoiceTaxModelList = await _salesInvoiceReport.GetSalesInvoiceTaxList(id);
            salesInvoiceDetailTaxModelList = await _salesInvoiceReport.GetSalesInvoiceDetailTaxList(id);

            SalesInvoiceReport salesInvoiceReport = new SalesInvoiceReport();

            string webRootPath = _webHostEnvironment.WebRootPath;

            byte[] bytes = salesInvoiceReport.PrepareReport(salesInvoiceModel, salesInvoiceDetailModelList, salesInvoiceTaxModelList, salesInvoiceDetailTaxModelList, webRootPath);

            return File(bytes, "application/pdf");
        }


    }
}
