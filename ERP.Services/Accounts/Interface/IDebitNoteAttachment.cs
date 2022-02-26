using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IDebitNoteAttachment : IRepository<Debitnoteattachment>
    {
        Task<int> CreateAttachment(DebitNoteAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(DebitNoteAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<DebitNoteAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<DebitNoteAttachmentModel>> GetAttachmentByDebitNoteId(int debitNoteId);

        Task<DataTableResultModel<DebitNoteAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveDebitNoteAttachment(AttachmentModel attachmentModel);

    }
}
