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
    public class EmployeeService : Repository<Employee>, IEmployee
    {
        public EmployeeService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateEmployee(EmployeeModel employeeModel)
        {
            int employeeId = 0;

            // assign values.
            Employee employee = new Employee();

            employee.EmployeeCode = employeeModel.EmployeeCode;
            employee.FirstName = employeeModel.FirstName;
            employee.LastName = employeeModel.LastName;
            employee.DesignationId = employeeModel.DesignationId;
            employee.DepartmentId = employeeModel.DepartmentId;
            employee.EmailAddress = employeeModel.EmailAddress;

            employeeId = await Create(employee);

            return employeeId; // returns.
        }

        public async Task<bool> UpdateEmployee(EmployeeModel employeeModel)
        {
            bool isUpdated = false;

            // get record.
            Employee employee = await GetByIdAsync(w => w.EmployeeId == employeeModel.EmployeeId);
            if (null != employee)
            {
                // assign values.
                employee.EmployeeCode = employeeModel.EmployeeCode;
                employee.FirstName = employeeModel.FirstName;
                employee.LastName = employeeModel.LastName;
                employee.DesignationId = employeeModel.DesignationId;
                employee.DepartmentId = employeeModel.DepartmentId;
                employee.EmailAddress = employeeModel.EmailAddress;

                isUpdated = await Update(employee);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteEmployee(int employeeId)
        {
            bool isDeleted = false;

            // get record.
            Employee employee = await GetByIdAsync(w => w.EmployeeId == employeeId);
            if (null != employee)
            {
                isDeleted = await Delete(employee);
            }

            return isDeleted; // returns.
        }


        public async Task<EmployeeModel> GetEmployeeById(int employeeId)
        {
            EmployeeModel employeeModel = null;

            IList<EmployeeModel> employeeModelList = await GetEmployeeList(employeeId);
            if (null != employeeModelList && employeeModelList.Any())
            {
                employeeModel = employeeModelList.FirstOrDefault();
            }

            return employeeModel; // returns.
        }

        public async Task<DataTableResultModel<EmployeeModel>> GetEmployeeList()
        {
            DataTableResultModel<EmployeeModel> resultModel = new DataTableResultModel<EmployeeModel>();

            IList<EmployeeModel> employeeModelList = await GetEmployeeList(0);
            if (null != employeeModelList && employeeModelList.Any())
            {
                resultModel = new DataTableResultModel<EmployeeModel>();
                resultModel.ResultList = employeeModelList;
                resultModel.TotalResultCount = employeeModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all employee list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<EmployeeModel>> GetEmployeeList(int employeeId)
        {
            IList<EmployeeModel> employeeModelList = null;

            // get records by query.

            IQueryable<Employee> query = GetQueryByCondition(w => w.EmployeeId != 0).Include(w => w.PreparedByUser);

            if (0 != employeeId)
                query = query.Where(w => w.EmployeeId == employeeId);

            IList<Employee> employeeList = await query.ToListAsync();

            if (null != employeeList && employeeList.Count > 0)
            {
                employeeModelList = new List<EmployeeModel>();
                foreach (Employee employee in employeeList)
                {
                    employeeModelList.Add(await AssignValueToModel(employee));
                }
            }

            return employeeModelList; // returns.
        }

        private async Task<EmployeeModel> AssignValueToModel(Employee employee)
        {
            return await Task.Run(() =>
            {
                EmployeeModel employeeModel = new EmployeeModel();

                employeeModel.EmployeeId = employee.EmployeeId;
                employeeModel.EmployeeCode = employee.EmployeeCode;
                employeeModel.FirstName = employee.FirstName;
                employeeModel.LastName = employee.LastName;
                employeeModel.DesignationId = employee.DesignationId;
                employeeModel.DepartmentId = employee.DepartmentId;
                employeeModel.EmailAddress = employee.EmailAddress;

                employeeModel.DesignationName = employee.Designation.DesignationName;
                employeeModel.DepartmentName = employee.Department.DepartmentName;
                employeeModel.PreparedByName = employee.PreparedByUser.UserName;

                return employeeModel;
            });
        }

        public async Task<IList<SelectListModel>> GetEmployeeSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.EmployeeId != 0))
            {
                IQueryable<Employee> query = GetQueryByCondition(w => w.EmployeeId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.FirstName + " " + s.LastName,
                                        Value = s.EmployeeId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
