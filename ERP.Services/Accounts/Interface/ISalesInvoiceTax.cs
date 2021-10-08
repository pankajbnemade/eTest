using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceTax : IRepository<Salesinvoicetax>
    {
         Task<int> GenerateSrNo(int salesInvoiceId);

        Task<int> CreateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel);

        Task<bool> UpdateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel);

        Task<bool> UpdateSalesInvoiceTaxAmountAll(int? salesInvoiceId);

        Task<bool> AddSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId,int taxRegisterId);

        Task<bool> DeleteSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId);

        Task<bool> DeleteSalesInvoiceTax(int salesInvoiceTaxId);

        Task<SalesInvoiceTaxModel> GetSalesInvoiceTaxById(int salesInvoiceTaxId);

        Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList();
    }
}
