using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceDetailTax : IRepository<Purchaseinvoicedetailtax>
    {
         Task<int> GenerateSrNo(int salesInvoiceDetId);

        Task<int> CreatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel);

        Task<bool> UpdatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel);

        Task<bool> DeletePurchaseInvoiceDetailTax(int purchaseInvoiceDetailTaxId);

        Task<PurchaseInvoiceDetailTaxModel> GetPurchaseInvoiceDetailTaxById(int purchaseInvoiceDetailTaxId);

        Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxByPurchaseInvoiceDetailId(int purchaseInvoiceDetailId);

        Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList();

    }
}
