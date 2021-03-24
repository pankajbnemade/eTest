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
    public class DepartmentService : Repository<Department>, IDepartment
    {
        public DepartmentService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateDepartment(DepartmentModel departmentModel)
        {
            int departmentId = 0;

            // assign values.
            Department department = new Department();

            department.DepartmentName = departmentModel.DepartmentName;

            departmentId = await Create(department);

            return departmentId; // returns.
        }

        public async Task<bool> UpdateDepartment(DepartmentModel departmentModel)
        {
            bool isUpdated = false;

            // get record.
            Department department = await GetByIdAsync(w => w.DepartmentId == departmentModel.DepartmentId);
            if (null != department)
            {
                // assign values.
                department.DepartmentName = departmentModel.DepartmentName;

                isUpdated = await Update(department);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteDepartment(int departmentId)
        {
            bool isDeleted = false;

            // get record.
            Department department = await GetByIdAsync(w => w.DepartmentId == departmentId);
            if (null != department)
            {
                isDeleted = await Delete(department);
            }

            return isDeleted; // returns.
        }


        public async Task<DepartmentModel> GetDepartmentById(int departmentId)
        {
            DepartmentModel departmentModel = null;

            IList<DepartmentModel> departmentModelList = await GetDepartmentList(departmentId);
            if (null != departmentModelList && departmentModelList.Any())
            {
                departmentModel = departmentModelList.FirstOrDefault();
            }

            return departmentModel; // returns.
        }

        public async Task<DataTableResultModel<DepartmentModel>> GetDepartmentList()
        {
            DataTableResultModel<DepartmentModel> resultModel = new DataTableResultModel<DepartmentModel>();

            IList<DepartmentModel> departmentModelList = await GetDepartmentList(0);
            if (null != departmentModelList && departmentModelList.Any())
            {
                resultModel = new DataTableResultModel<DepartmentModel>();
                resultModel.ResultList = departmentModelList;
                resultModel.TotalResultCount = departmentModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all department list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<DepartmentModel>> GetDepartmentList(int departmentId)
        {
            IList<DepartmentModel> departmentModelList = null;

            // get records by query.

            IQueryable<Department> query = GetQueryByCondition(w => w.DepartmentId != 0).Include(w => w.PreparedByUser);

            if (0 != departmentId)
                query = query.Where(w => w.DepartmentId == departmentId);

            IList<Department> departmentList = await query.ToListAsync();

            if (null != departmentList && departmentList.Count > 0)
            {
                departmentModelList = new List<DepartmentModel>();
                foreach (Department department in departmentList)
                {
                    departmentModelList.Add(await AssignValueToModel(department));
                }
            }

            return departmentModelList; // returns.
        }

        private async Task<DepartmentModel> AssignValueToModel(Department department)
        {
            return await Task.Run(() =>
            {
                DepartmentModel departmentModel = new DepartmentModel();

                departmentModel.DepartmentId = department.DepartmentId;
                departmentModel.DepartmentName = department.DepartmentName;

                departmentModel.PreparedByName = department.PreparedByUser.UserName;

                return departmentModel;
            });
        }

        public async Task<IList<SelectListModel>> GetDepartmentSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.DepartmentId != 0))
            {
                IQueryable<Department> query = GetQueryByCondition(w => w.DepartmentId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.DepartmentName,
                                        Value = s.DepartmentId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
