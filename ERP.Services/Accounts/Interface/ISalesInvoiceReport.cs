using ERP.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceReport {
        Task<SalesInvoiceModel> GetSalesInvoice(int salesInvoiceId);
        Task<IList<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList(int salesInvoiceId);
        Task<IList<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList(int salesInvoiceId);
        Task<IList<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList(int salesInvoiceId);
    }
}
