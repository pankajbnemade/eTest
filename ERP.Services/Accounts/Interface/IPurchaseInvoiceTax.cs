using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceTax : IRepository<Purchaseinvoicetax>
    {
         Task<int> GenerateSrNo(int salesInvoiceId);

        Task<int> CreatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel);

        Task<bool> UpdatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel);

        Task<bool> UpdatePurchaseInvoiceTaxAmountAll(int? purchaseInvoiceId);

        Task<bool> AddPurchaseInvoiceTaxByPurchaseInvoiceId(int purchaseInvoiceId,int taxRegisterId);

        Task<bool> DeletePurchaseInvoiceTaxByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<bool> DeletePurchaseInvoiceTax(int purchaseInvoiceTaxId);

        Task<PurchaseInvoiceTaxModel> GetPurchaseInvoiceTaxById(int purchaseInvoiceTaxId);

        Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxList();
    }
}
