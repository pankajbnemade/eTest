using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IContraVoucherAttachment : IRepository<Contravoucherattachment>
    {
        Task<int> CreateAttachment(ContraVoucherAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(ContraVoucherAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<ContraVoucherAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<ContraVoucherAttachmentModel>> GetAttachmentByContraVoucherId(int contraVoucherId);

        Task<DataTableResultModel<ContraVoucherAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveContraVoucherAttachment(AttachmentModel attachmentModel);

    }
}
