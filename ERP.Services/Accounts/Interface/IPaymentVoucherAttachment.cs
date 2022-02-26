using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPaymentVoucherAttachment : IRepository<Paymentvoucherattachment>
    {
        Task<int> CreateAttachment(PaymentVoucherAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(PaymentVoucherAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<PaymentVoucherAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<PaymentVoucherAttachmentModel>> GetAttachmentByPaymentVoucherId(int paymentVoucherId);

        Task<DataTableResultModel<PaymentVoucherAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SavePaymentVoucherAttachment(AttachmentModel attachmentModel);

    }
}
