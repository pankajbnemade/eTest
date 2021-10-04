using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceDetail : IRepository<Purchaseinvoicedetail>
    {
        Task<int> GenerateSrNo(int purchaseInvoiceId);

        Task<int> CreatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel);

        Task<bool> UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel);

        Task<bool> UpdatePurchaseInvoiceDetailAmount(int? purchaseInvoiceDetailId);

        Task<bool> DeletePurchaseInvoiceDetail(int purchaseInvoiceDetailId);

        Task<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetailById(int purchaseInvoiceDetailId);

        Task<DataTableResultModel<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailList();

        Task<IList<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailListByPurchaseInvoiceId(int purchaseInvoiceId);

    }
}
