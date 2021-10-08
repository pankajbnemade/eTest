using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceDetailTax : IRepository<Salesinvoicedetailtax>
    {
        Task<int> GenerateSrNo(int salesInvoiceDetId);

        Task<int> CreateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel);

        Task<bool> UpdateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel);

        Task<bool> AddSalesInvoiceDetailTaxBySalesInvoiceId(int salesInvoiceId, int taxRegisterId);

        Task<bool> AddSalesInvoiceDetailTaxBySalesInvoiceDetId(int salesInvoiceDetId, int taxRegisterId);

        Task<bool> UpdateSalesInvoiceDetailTaxAmountOnDetailUpdate(int? salesInvoiceDetailId);

        Task<bool> DeleteSalesInvoiceDetailTax(int salesInvoiceDetailTaxId);

        Task<bool> DeleteSalesInvoiceDetailTaxBySalesInvoiceId(int salesInvoiceId);

        Task<SalesInvoiceDetailTaxModel> GetSalesInvoiceDetailTaxById(int salesInvoiceDetailTaxId);

        Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxBySalesInvoiceDetailId(int salesInvoiceDetailId);

        Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList();

    }
}
