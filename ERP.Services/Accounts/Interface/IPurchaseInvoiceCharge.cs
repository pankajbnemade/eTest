using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceCharge : IRepository<Purchaseinvoicecharge>
    {
        Task<int> CreatePurchaseInvoiceCharge(PurchaseInvoiceChargeModel purchaseInvoiceChargeModel);
       
        Task<bool> UpdatePurchaseInvoiceCharge(PurchaseInvoiceChargeModel purchaseInvoiceChargeModel);
      
        Task<bool> DeletePurchaseInvoiceCharge(int purchaseInvoiceChargeId);
        
        Task<PurchaseInvoiceChargeModel> GetPurchaseInvoiceChargeById(int purchaseInvoiceChargeId);

        //Task<PurchaseInvoiceChargeModel> GetPurchaseInvoiceChargeBysId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceChargeModel>> GetPurchaseInvoiceChargeList();
    }
}
