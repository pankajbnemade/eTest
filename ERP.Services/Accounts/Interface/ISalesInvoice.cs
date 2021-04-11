using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoice : IRepository<Salesinvoice>
    {
        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        Task<GenerateNoModel> GenerateInvoiceNo(int companyId, int financialYearId);

        /// <summary>
        /// create new sales invoice.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        Task<int> CreateSalesInvoice(SalesInvoiceModel salesInvoiceModel);

        /// <summary>
        /// update sales invoice.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> UpdateSalesInvoice(SalesInvoiceModel salesInvoiceModel);

        /// <summary>
        /// delete sales invoice.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> DeleteSalesInvoice(int salesInvoiceId);

        Task<bool> UpdateSalesInvoiceMasterAmount(int? salesInvoiceId);

        /// <summary>
        /// get sales invoice based on invoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        Task<SalesInvoiceModel> GetSalesInvoiceById(int salesInvoiceId);

        /// <summary>
        /// get search sales invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<SalesInvoiceModel>> GetSalesInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterSalesInvoiceModel searchFilterModel);
    }
}
