using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoice : IRepository<Purchaseinvoice>
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

        Task<int> CreatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel);

        Task<bool> UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel);

        Task<bool> DeletePurchaseInvoice(int purchaseInvoiceId);

        Task<bool> UpdateStatusPurchaseInvoice(int purchaseInvoiceId, int action);

        Task<bool> UpdatePurchaseInvoiceMasterAmount(int? purchaseInvoiceId);

        Task<PurchaseInvoiceModel> GetPurchaseInvoiceById(int purchaseInvoiceId);

        Task<IList<OutstandingInvoiceModel>> GetPurchaseInvoiceListBySupplierLedgerId(int supplierLedgerId, DateTime? voucherDate);

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<PurchaseInvoiceModel>> GetPurchaseInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterPurchaseInvoiceModel searchFilterModel);

        Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId);

        //Task<DataTableResultModel<PurchaseInvoiceModel>> GetPurchaseInvoiceList();
    }
}
