using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IAdvanceAdjustmentAttachment : IRepository<Advanceadjustmentattachment>
    {
        Task<int> CreateAttachment(AdvanceAdjustmentAttachmentModel attachmentModel);
       
        Task<bool> UpdateAttachment(AdvanceAdjustmentAttachmentModel attachmentModel);
      
        Task<bool> DeleteAttachment(int associationId);
        
        Task<AdvanceAdjustmentAttachmentModel> GetAttachmentById(int associationId);

        Task<DataTableResultModel<AdvanceAdjustmentAttachmentModel>> GetAttachmentByAdvanceAdjustmentId(int advanceAdjustmentId);

        Task<DataTableResultModel<AdvanceAdjustmentAttachmentModel>> GetAttachmentList();

        Task<AttachmentModel> SaveAdvanceAdjustmentAttachment(AttachmentModel attachmentModel);

    }
}
