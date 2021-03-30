using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceTax : IRepository<Purchaseinvoicetax>
    {
        Task<int> CreatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel);

        Task<bool> UpdatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel);

        Task<bool> DeletePurchaseInvoiceTax(int purchaseInvoiceTaxId);

        Task<PurchaseInvoiceTaxModel> GetPurchaseInvoiceTaxById(int purchaseInvoiceTaxId);

        Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxList();
    }
}
