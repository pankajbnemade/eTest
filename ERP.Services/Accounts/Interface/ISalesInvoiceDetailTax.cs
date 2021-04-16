using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceDetailTax : IRepository<Salesinvoicedetailtax>
    {
        /// <summary>
        /// generate sr no based on salesInvoiceDetId
        /// </summary>
        /// <param name="salesInvoiceDetId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        Task<int> GenerateSrNo(int salesInvoiceDetId);

        Task<int> CreateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel);
       
        Task<bool> UpdateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel);
      
        Task<bool> DeleteSalesInvoiceDetailTax(int salesInvoiceDetailTaxId);
        
        Task<SalesInvoiceDetailTaxModel> GetSalesInvoiceDetailTaxById(int salesInvoiceDetailTaxId);

        Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxBySalesInvoiceDetailId(int salesInvoiceDetailId);

        Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList();
    }
}
