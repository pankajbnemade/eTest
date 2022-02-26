using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedgerAttachment : IRepository<Ledgerattachment>
    {
        Task<int> CreateAttachment(LedgerAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(LedgerAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<LedgerAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<LedgerAttachmentModel>> GetAttachmentByLedgerId(int ledgerId);

        Task<DataTableResultModel<LedgerAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveLedgerAttachment(AttachmentModel attachmentModel);

    }
}
