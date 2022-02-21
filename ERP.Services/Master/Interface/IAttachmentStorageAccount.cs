using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Models.Helpers;

namespace ERP.Services.Master.Interface
{
    public interface IAttachmentStorageAccount : IRepository<Attachmentstorageaccount>
    {
        Task<int> CreateStorageAccount(AttachmentStorageAccountModel storageAccountModel);

        Task<bool> UpdateStorageAccount(AttachmentStorageAccountModel storageAccountModel);

        Task<bool> DeleteStorageAccount(int storageAccountId);

        Task<AttachmentStorageAccountModel> GetStorageAccountById(int storageAccountId);

        Task<DataTableResultModel<AttachmentStorageAccountModel>> GetStorageAccountList();

        Task<IList<SelectListModel>> GetStorageAccountSelectList();
    }
}
