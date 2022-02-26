using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceAttachment : IRepository<Purchaseinvoiceattachment>
    {
        Task<int> CreateAttachment(PurchaseInvoiceAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(PurchaseInvoiceAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<PurchaseInvoiceAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<PurchaseInvoiceAttachmentModel>> GetAttachmentByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveInvoiceAttachment(AttachmentModel attachmentModel);

    }
}
