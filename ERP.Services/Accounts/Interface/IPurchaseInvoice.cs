using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoice : IRepository<Purchaseinvoice>
    {
        Task<GenerateNoModel> GenerateInvoiceNo(int? companyId, int? financialYearId);

        Task<int> CreatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel);

        Task<bool> UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel);

        Task<bool> DeletePurchaseInvoice(int purchaseInvoiceId);

        Task<bool> UpdatePurchaseInvoiceMasterAmount(int? purchaseInvoiceId);

        Task<PurchaseInvoiceModel> GetPurchaseInvoiceById(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceModel>> GetPurchaseInvoiceList();
    }
}
