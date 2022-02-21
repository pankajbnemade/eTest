using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Models.Helpers;

namespace ERP.Services.Master.Interface
{
    public interface IAttachmentCategory : IRepository<Attachmentcategory>
    {
        Task<int> CreateCategory(AttachmentCategoryModel categoryModel);

        Task<bool> UpdateCategory(AttachmentCategoryModel categoryModel);

        Task<bool> DeleteCategory(int categoryId);

        Task<AttachmentCategoryModel> GetCategoryById(int categoryId);

        Task<DataTableResultModel<AttachmentCategoryModel>> GetCategoryList();

        Task<IList<SelectListModel>> GetCategorySelectList();
    }
}
