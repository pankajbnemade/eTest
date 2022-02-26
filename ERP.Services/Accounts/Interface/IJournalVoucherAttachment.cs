using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalVoucherAttachment : IRepository<Journalvoucherattachment>
    {
        Task<int> CreateAttachment(JournalVoucherAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(JournalVoucherAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<JournalVoucherAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<JournalVoucherAttachmentModel>> GetAttachmentByJournalVoucherId(int journalVoucherId);

        Task<DataTableResultModel<JournalVoucherAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveJournalVoucherAttachment(AttachmentModel attachmentModel);

    }
}
