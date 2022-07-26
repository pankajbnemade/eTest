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
    public class DesignationService : Repository<Designation>, IDesignation
    {
        public DesignationService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateDesignation(DesignationModel designationModel)
        {
            int designationId = 0;

            // assign values.
            Designation designation = new Designation();
            designation.DesignationName = designationModel.DesignationName;
            await Create(designation);
            designationId = designation.DesignationId;

            return designationId; // returns.
        }

        public async Task<bool> UpdateDesignation(DesignationModel designationModel)
        {
            bool isUpdated = false;

            // get record.
            Designation designation = await GetByIdAsync(w => w.DesignationId == designationModel.DesignationId);
            if (null != designation)
            {
                // assign values.
                designation.DesignationName = designationModel.DesignationName;
                isUpdated = await Update(designation);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteDesignation(int designationId)
        {
            bool isDeleted = false;

            // get record.
            Designation designation = await GetByIdAsync(w => w.DesignationId == designationId);
            if (null != designation)
            {
                isDeleted = await Delete(designation);
            }

            return isDeleted; // returns.
        }

        public async Task<DesignationModel> GetDesignationById(int designationId)
        {
            DesignationModel designationModel = null;

            IList<DesignationModel> designationModelList = await GetDesignationList(designationId);
            if (null != designationModelList && designationModelList.Any())
            {
                designationModel = designationModelList.FirstOrDefault();
            }

            return designationModel; // returns.
        }

        public async Task<DataTableResultModel<DesignationModel>> GetDesignationList()
        {
            DataTableResultModel<DesignationModel> resultModel = new DataTableResultModel<DesignationModel>();

            IList<DesignationModel> designationModelList = await GetDesignationList(0);

            if (null != designationModelList && designationModelList.Any())
            {
                resultModel = new DataTableResultModel<DesignationModel>();
                resultModel.ResultList = designationModelList;
                resultModel.TotalResultCount = designationModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<DesignationModel>();
                resultModel.ResultList = new List<DesignationModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        /// <summary>
        /// get all designation list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<DesignationModel>> GetDesignationList(int designationId)
        {
            IList<DesignationModel> designationModelList = null;

            // get records by query.
            IQueryable<Designation> query = GetQueryByCondition(w => w.DesignationId != 0).Include(w => w.PreparedByUser);
            if (0 != designationId)
                query = query.Where(w => w.DesignationId == designationId);

            IList<Designation> designationList = await query.ToListAsync();
            if (null != designationList && designationList.Count > 0)
            {
                designationModelList = new List<DesignationModel>();
                foreach (Designation designation in designationList)
                {
                    designationModelList.Add(await AssignValueToModel(designation));
                }
            }

            return designationModelList; // returns.
        }

        private async Task<DesignationModel> AssignValueToModel(Designation designation)
        {
            return await Task.Run(() =>
            {
                DesignationModel designationModel = new DesignationModel();
                designationModel.DesignationId = designation.DesignationId;
                designationModel.DesignationName = designation.DesignationName;
                designationModel.PreparedByName = null != designation.PreparedByUser ? designation.PreparedByUser.UserName : null;

                return designationModel;
            });
        }

        public async Task<IList<SelectListModel>> GetDesignationSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.DesignationId != 0))
            {
                IQueryable<Designation> query = GetQueryByCondition(w => w.DesignationId != 0);
                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.DesignationName,
                    Value = s.DesignationId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
