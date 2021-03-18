using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceDetail : IRepository<Purchaseinvoicedetail>
    {
        Task<int> CreatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel);
       
        Task<bool> UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel);
      
        Task<bool> DeletePurchaseInvoiceDetail(int purchaseInvoiceDetailId);
        
        Task<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetailById(int purchaseInvoiceDetailId);

        //Task<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetailByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailList();
    }
}
