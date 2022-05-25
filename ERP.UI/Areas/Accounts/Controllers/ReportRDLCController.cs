using AspNetCore.Reporting;
using ERP.Models.Accounts;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class ReportRDLCController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ISalesInvoiceDetail _salesInvoiceDetail;

        public ReportController(
            IWebHostEnvironment webHostEnvironment,
             ISalesInvoiceDetail salesInvoiceDetail)
        {
            this._salesInvoiceDetail = salesInvoiceDetail;
            this._webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task<IActionResult> SalesInvoice(int id)
        {
            IList<SalesInvoiceReportModel> salesInvoiceReportModelList = await _salesInvoiceDetail.GetSalesInvoiceReportDataById(id);

            int exetension = 1;

            string rdlcFilePath = $"{this._webHostEnvironment.ContentRootPath}\\Report\\Accounts\\rptSalesInvoice.rdlc";

            string fileName = salesInvoiceReportModelList.FirstOrDefault().InvoiceNo + ".pdf";

            LocalReport localReport = new LocalReport(rdlcFilePath);

            localReport.AddDataSource("dsSalesInvoiceDet", salesInvoiceReportModelList);

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("salesInvoiceId", id.ToString());

            ReportResult result = localReport.Execute(RenderType.Pdf, exetension, parameters);

            //To Download File in new tab
            return File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

    }
}
