using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceTax : IRepository<Salesinvoicetax>
    {
        Task<int> CreateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel);
       
        Task<bool> UpdateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel);
      
        Task<bool> DeleteSalesInvoiceTax(int salesInvoiceTaxId);
        
        Task<SalesInvoiceTaxModel> GetSalesInvoiceTaxById(int salesInvoiceTaxId);

        //Task<SalesInvoiceTaxModel> GetSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList();
    }
}
