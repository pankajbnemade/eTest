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
    public class UnitOfMeasurementService : Repository<Unitofmeasurement>, IUnitOfMeasurement
    {
        public UnitOfMeasurementService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateUnitOfMeasurement(UnitOfMeasurementModel unitOfMeasurementModel)
        {
            int unitOfMeasurementId = 0;

            // assign values.
            Unitofmeasurement unitOfMeasurement = new Unitofmeasurement();
            unitOfMeasurement.UnitOfMeasurementName = unitOfMeasurementModel.UnitOfMeasurementName;
            await Create(unitOfMeasurement);
            unitOfMeasurementId = unitOfMeasurement.UnitOfMeasurementId;

            return unitOfMeasurementId; // returns.
        }

        public async Task<bool> UpdateUnitOfMeasurement(UnitOfMeasurementModel unitOfMeasurementModel)
        {
            bool isUpdated = false;

            // get record.
            Unitofmeasurement unitOfMeasurement = await GetByIdAsync(w => w.UnitOfMeasurementId == unitOfMeasurementModel.UnitOfMeasurementId);
            if (null != unitOfMeasurement)
            {
                // assign values.
                unitOfMeasurement.UnitOfMeasurementName = unitOfMeasurementModel.UnitOfMeasurementName;
                isUpdated = await Update(unitOfMeasurement);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteUnitOfMeasurement(int unitOfMeasurementId)
        {
            bool isDeleted = false;

            // get record.
            Unitofmeasurement unitOfMeasurement = await GetByIdAsync(w => w.UnitOfMeasurementId == unitOfMeasurementId);
            if (null != unitOfMeasurement)
            {
                isDeleted = await Delete(unitOfMeasurement);
            }

            return isDeleted; // returns.
        }

        public async Task<UnitOfMeasurementModel> GetUnitOfMeasurementById(int unitOfMeasurementId)
        {
            UnitOfMeasurementModel unitOfMeasurementModel = null;

            IList<UnitOfMeasurementModel> unitOfMeasurementModelList = await GetUnitOfMeasurementList(unitOfMeasurementId);
            if (null != unitOfMeasurementModelList && unitOfMeasurementModelList.Any())
            {
                unitOfMeasurementModel = unitOfMeasurementModelList.FirstOrDefault();
            }

            return unitOfMeasurementModel; // returns.
        }

        public async Task<DataTableResultModel<UnitOfMeasurementModel>> GetUnitOfMeasurementList()
        {
            DataTableResultModel<UnitOfMeasurementModel> resultModel = new DataTableResultModel<UnitOfMeasurementModel>();

            IList<UnitOfMeasurementModel> unitOfMeasurementModelList = await GetUnitOfMeasurementList(0);
            if (null != unitOfMeasurementModelList && unitOfMeasurementModelList.Any())
            {
                resultModel = new DataTableResultModel<UnitOfMeasurementModel>();
                resultModel.ResultList = unitOfMeasurementModelList;
                resultModel.TotalResultCount = unitOfMeasurementModelList.Count();
            }

            return resultModel; // returns.
        }

        /// <summary>
        /// get all unitOfMeasurement list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<UnitOfMeasurementModel>> GetUnitOfMeasurementList(int unitOfMeasurementId)
        {
            IList<UnitOfMeasurementModel> unitOfMeasurementModelList = null;

            // get records by query.

            IQueryable<Unitofmeasurement> query = GetQueryByCondition(w => w.UnitOfMeasurementId != 0).Include(w => w.PreparedByUser);
            if (0 != unitOfMeasurementId)
                query = query.Where(w => w.UnitOfMeasurementId == unitOfMeasurementId);

            IList<Unitofmeasurement> unitOfMeasurementList = await query.ToListAsync();
            if (null != unitOfMeasurementList && unitOfMeasurementList.Count > 0)
            {
                unitOfMeasurementModelList = new List<UnitOfMeasurementModel>();
                foreach (Unitofmeasurement unitOfMeasurement in unitOfMeasurementList)
                {
                    unitOfMeasurementModelList.Add(await AssignValueToModel(unitOfMeasurement));
                }
            }

            return unitOfMeasurementModelList; // returns.
        }

        private async Task<UnitOfMeasurementModel> AssignValueToModel(Unitofmeasurement unitOfMeasurement)
        {
            return await Task.Run(() =>
            {
                UnitOfMeasurementModel unitOfMeasurementModel = new UnitOfMeasurementModel();
                unitOfMeasurementModel.UnitOfMeasurementId = unitOfMeasurement.UnitOfMeasurementId;
                unitOfMeasurementModel.UnitOfMeasurementName = unitOfMeasurement.UnitOfMeasurementName;
                unitOfMeasurementModel.PreparedByName = null != unitOfMeasurement.PreparedByUser ? unitOfMeasurement.PreparedByUser.UserName : null;

                return unitOfMeasurementModel;
            });
        }

        public async Task<IList<SelectListModel>> GetUnitOfMeasurementSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.UnitOfMeasurementId != 0))
            {
                IQueryable<Unitofmeasurement> query = GetQueryByCondition(w => w.UnitOfMeasurementId != 0);
                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.UnitOfMeasurementName,
                    Value = s.UnitOfMeasurementId.ToString()
                }).ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
