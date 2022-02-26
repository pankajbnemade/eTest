using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Models.Helpers;

namespace ERP.Services.Master.Interface
{
    public interface IAttachment : IRepository<Attachment>
    {
        Task<AttachmentModel> SaveAttachment(AttachmentModel attachmentModel);

        Task<string> GetUrl(int attachmentId);

        Task<int> CreateAttachment(AttachmentModel attachmentModel);

        Task<bool> UpdateAttachment(AttachmentModel attachmentModel);

        Task<bool> DeleteAttachment(int attachmentId);

        Task<AttachmentModel> GetAttachmentById(int attachmentId);

        Task<DataTableResultModel<AttachmentModel>> GetAttachmentList();

        //Task<IList<SelectListModel>> GetAttachmentSelectList();
    }
}
