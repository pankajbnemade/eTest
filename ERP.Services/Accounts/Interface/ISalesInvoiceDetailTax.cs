using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceDetailTax : IRepository<Salesinvoicedetailtax>
    {
        Task<int> CreateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel);
       
        Task<bool> UpdateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel);
      
        Task<bool> DeleteSalesInvoiceDetailTax(int salesInvoiceDetailTaxId);
        
        Task<SalesInvoiceDetailTaxModel> GetSalesInvoiceDetailTaxById(int salesInvoiceDetailTaxId);

        //Task<SalesInvoiceDetailTaxModel> GetSalesInvoiceDetailTaxBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList();
    }
}
