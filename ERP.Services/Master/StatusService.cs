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
    public class StatusService : Repository<Status>, IStatus
    {
        public StatusService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateStatus(StatusModel statusModel)
        {
            int statusId = 0;

            // assign values.
            Status status = new Status();

            status.StatusName = statusModel.StatusName;

            statusId = await Create(status);

            return statusId; // returns.
        }

        public async Task<bool> UpdateStatus(StatusModel statusModel)
        {
            bool isUpdated = false;

            // get record.
            Status status = await GetByIdAsync(w => w.StatusId == statusModel.StatusId);
            if (null != status)
            {
                // assign values.
                status.StatusName = statusModel.StatusName;

                isUpdated = await Update(status);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteStatus(int statusId)
        {
            bool isDeleted = false;

            // get record.
            Status status = await GetByIdAsync(w => w.StatusId == statusId);
            if (null != status)
            {
                isDeleted = await Delete(status);
            }

            return isDeleted; // returns.
        }


        public async Task<StatusModel> GetStatusById(int statusId)
        {
            StatusModel statusModel = null;

            IList<StatusModel> statusModelList = await GetStatusList(statusId);
            if (null != statusModelList && statusModelList.Any())
            {
                statusModel = statusModelList.FirstOrDefault();
            }

            return statusModel; // returns.
        }

        public async Task<DataTableResultModel<StatusModel>> GetStatusList()
        {
            DataTableResultModel<StatusModel> resultModel = new DataTableResultModel<StatusModel>();

            IList<StatusModel> statusModelList = await GetStatusList(0);
            if (null != statusModelList && statusModelList.Any())
            {
                resultModel = new DataTableResultModel<StatusModel>();
                resultModel.ResultList = statusModelList;
                resultModel.TotalResultCount = statusModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all status list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<StatusModel>> GetStatusList(int statusId)
        {
            IList<StatusModel> statusModelList = null;

            // get records by query.

            IQueryable<Status> query = GetQueryByCondition(w => w.StatusId != 0).Include(w => w.PreparedByUser);

            if (0 != statusId)
                query = query.Where(w => w.StatusId == statusId);

            IList<Status> statusList = await query.ToListAsync();

            if (null != statusList && statusList.Count > 0)
            {
                statusModelList = new List<StatusModel>();
                foreach (Status status in statusList)
                {
                    statusModelList.Add(await AssignValueToModel(status));
                }
            }

            return statusModelList; // returns.
        }

        private async Task<StatusModel> AssignValueToModel(Status status)
        {
            return await Task.Run(() =>
            {
                StatusModel statusModel = new StatusModel();

                statusModel.StatusId = status.StatusId;
                statusModel.StatusName = status.StatusName;

                if (null != status.PreparedByUser)
                {
                    statusModel.PreparedByName = status.PreparedByUser.UserName;
                }

                return statusModel;
            });
        }


        public async Task<IList<SelectListModel>> GetStatusSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.StatusId != 0))
            {
                IQueryable<Status> query = GetQueryByCondition(w => w.StatusId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.StatusName,
                                        Value = s.StatusId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
