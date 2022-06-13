using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IEmployee : IRepository<Employee>
    {
       
        Task<int> CreateEmployee(EmployeeModel employeeModel);

        Task<bool> UpdateEmployee(EmployeeModel employeeModel);

        Task<bool> DeleteEmployee(int employeeId);
       
        Task<EmployeeModel> GetEmployeeById(int employeeId);

        Task<GenerateNoModel> GenerateEmployeeCode();

        Task<IList<SelectListModel>> GetEmployeeSelectList();

        Task<DataTableResultModel<EmployeeModel>> GetEmployeeList();
    }
}
