using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceTax : IRepository<Salesinvoicetax>
    {
        /// <summary>
        /// generate sr no based on salesInvoiceId
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        Task<int> GenerateSrNo(int salesInvoiceId);

        Task<int> CreateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel);

        Task<bool> UpdateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel);

        Task<bool> DeleteSalesInvoiceTax(int salesInvoiceTaxId);

        Task<SalesInvoiceTaxModel> GetSalesInvoiceTaxById(int salesInvoiceTaxId);

        Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList();
    }
}
