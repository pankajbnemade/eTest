using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class AttachmentCategoryService : Repository<Attachmentcategory>, IAttachmentCategory
    {
        public AttachmentCategoryService(ErpDbContext dbContext) : base(dbContext) { }
        
        public async Task<int> CreateCategory(AttachmentCategoryModel categoryModel)
        {
            int categoryId = 0;

            // assign values.
            Attachmentcategory category = new Attachmentcategory();
            category.CategoryName = categoryModel.CategoryName;

            await Create(category);

            categoryId = category.CategoryId;

            return categoryId; // returns.
        }

        /// <summary>
        /// update category.
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateCategory(AttachmentCategoryModel categoryModel)
        {
            bool isUpdated = false;

            // get record.
            Attachmentcategory category = await GetByIdAsync(w => w.CategoryId == categoryModel.CategoryId);

            if (null != category)
            {
                // assign values.
                category.CategoryName = categoryModel.CategoryName;

                isUpdated = await Update(category);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// update category.
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteCategory(int categoryId)
        {
            bool isDeleted = false;

            // get record.
            Attachmentcategory category = await GetByIdAsync(w => w.CategoryId == categoryId);

            if (null != category)
            {
                isDeleted = await Delete(category);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get category based on categoryId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<AttachmentCategoryModel> GetCategoryById(int categoryId)
        {
            AttachmentCategoryModel categoryModel = null;

            IList<AttachmentCategoryModel> categoryModelList = await GetCategoryList(categoryId);

            if (null != categoryModelList && categoryModelList.Any())
            {
                categoryModel = categoryModelList.FirstOrDefault();
            }

            return categoryModel; // returns.
        }

        /// <summary>
        /// get all category list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<AttachmentCategoryModel>> GetCategoryList()
        {
            DataTableResultModel<AttachmentCategoryModel> resultModel = new DataTableResultModel<AttachmentCategoryModel>();

            IList<AttachmentCategoryModel> categoryModelList = await GetCategoryList(0);

            if (null != categoryModelList && categoryModelList.Any())
            {
                resultModel = new DataTableResultModel<AttachmentCategoryModel>();
                resultModel.ResultList = categoryModelList;
                resultModel.TotalResultCount = categoryModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<AttachmentCategoryModel>> GetCategoryList(int categoryId)
        {
            IList<AttachmentCategoryModel> categoryModelList = null;

            // create query.
            IQueryable<Attachmentcategory> query = GetQueryByCondition(w => w.CategoryId != 0).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != categoryId)
                query = query.Where(w => w.CategoryId == categoryId);

            // get records by query.
            List<Attachmentcategory> categoryList = await query.ToListAsync();

            if (null != categoryList && categoryList.Count > 0)
            {
                categoryModelList = new List<AttachmentCategoryModel>();
                foreach (Attachmentcategory category in categoryList)
                {
                    categoryModelList.Add(await AssignValueToModel(category));
                }
            }

            return categoryModelList; // returns.
        }

        private async Task<AttachmentCategoryModel> AssignValueToModel(Attachmentcategory category)
        {
            return await Task.Run(() =>
            {
                // assign values.
                AttachmentCategoryModel categoryModel = new AttachmentCategoryModel();

                categoryModel.CategoryId = category.CategoryId;
                categoryModel.CategoryName = category.CategoryName;
                categoryModel.PreparedByName = null != category.PreparedByUser ? category.PreparedByUser.UserName : null;

                return categoryModel;
            });
        }

        public async Task<IList<SelectListModel>> GetCategorySelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.CategoryId != 0))
            {
                IQueryable<Attachmentcategory> query = GetQueryByCondition(w => w.CategoryId != 0);

                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.CategoryName,
                    Value = s.CategoryId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
