using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICreditNoteAttachment : IRepository<Creditnoteattachment>
    {
        Task<int> CreateAttachment(CreditNoteAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(CreditNoteAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<CreditNoteAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<CreditNoteAttachmentModel>> GetAttachmentByCreditNoteId(int creditNoteId);

        Task<DataTableResultModel<CreditNoteAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveCreditNoteAttachment(AttachmentModel attachmentModel);

    }
}
