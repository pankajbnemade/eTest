using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceDetailTax : IRepository<Purchaseinvoicedetailtax>
    {
        Task<int> GenerateSrNo(int purchaseInvoiceDetId);

        Task<int> CreatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel);

        Task<bool> UpdatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel);

        Task<bool> AddPurchaseInvoiceDetailTaxByPurchaseInvoiceId(int purchaseInvoiceId, int taxRegisterId);

        Task<bool> AddPurchaseInvoiceDetailTaxByPurchaseInvoiceDetId(int purchaseInvoiceDetId, int taxRegisterId);

        Task<bool> UpdatePurchaseInvoiceDetailTaxAmountOnDetailUpdate(int? purchaseInvoiceDetailId);

        Task<bool> DeletePurchaseInvoiceDetailTax(int purchaseInvoiceDetailTaxId);

        Task<bool> DeletePurchaseInvoiceDetailTaxByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<PurchaseInvoiceDetailTaxModel> GetPurchaseInvoiceDetailTaxById(int purchaseInvoiceDetailTaxId);

        Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxByPurchaseInvoiceDetailId(int purchaseInvoiceDetailId);

        Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList();

    }
}
