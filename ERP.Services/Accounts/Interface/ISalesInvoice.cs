using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
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

        Task<int> CreateSalesInvoice(SalesInvoiceModel salesInvoiceModel);

        Task<bool> UpdateSalesInvoice(SalesInvoiceModel salesInvoiceModel);

        Task<bool> DeleteSalesInvoice(int salesInvoiceId);

        Task<bool> UpdateStatusSalesInvoice(int salesInvoiceId, int action);

        Task<bool> UpdateSalesInvoiceMasterAmount(int? salesInvoiceId);

        Task<SalesInvoiceModel> GetSalesInvoiceById(int salesInvoiceId);

        Task<IList<OutstandingInvoiceModel>> GetSalesInvoiceListByCustomerLedgerId(int custoomerLedgerId);

        /// <summary>
        /// get search sales invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<SalesInvoiceModel>> GetSalesInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterSalesInvoiceModel searchFilterModel);

        //Task<DataTableResultModel<SalesInvoiceModel>> GetSalesInvoiceList();
    }
}
