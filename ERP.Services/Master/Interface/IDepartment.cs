using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IDepartment : IRepository<Department>
    {
       
        Task<int> CreateDepartment(DepartmentModel departmentModel);

        Task<bool> UpdateDepartment(DepartmentModel departmentModel);

        Task<bool> DeleteDepartment(int departmentId);
       
        Task<DepartmentModel> GetDepartmentById(int departmentId);
        
        Task<DataTableResultModel<DepartmentModel>> GetDepartmentList();

        Task<IList<SelectListModel>> GetDepartmentSelectList();

    }
}
