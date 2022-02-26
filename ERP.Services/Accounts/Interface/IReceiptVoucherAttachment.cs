using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceiptVoucherAttachment : IRepository<Receiptvoucherattachment>
    {
        Task<int> CreateAttachment(ReceiptVoucherAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(ReceiptVoucherAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<ReceiptVoucherAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<ReceiptVoucherAttachmentModel>> GetAttachmentByReceiptVoucherId(int receiptVoucherId);

        Task<DataTableResultModel<ReceiptVoucherAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveReceiptVoucherAttachment(AttachmentModel attachmentModel);

    }
}
