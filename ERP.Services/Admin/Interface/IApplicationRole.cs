using ERP.DataAccess.Entity;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Admin.Interface
{
    public interface IApplicationRole : IRepository<ApplicationRole>
    {
        Task<int> CreateRole(ApplicationRoleModel applicationRoleModel);

        Task<bool> UpdateRole(ApplicationRoleModel applicationRoleModel);

        Task<ApplicationRoleModel> GetApplicationRoleById(int roleId);

        Task<bool> DeleteRole(int applicationRoleId);
        Task<DataTableResultModel<ApplicationRoleModel>> GetApplicationRoleList();

        Task<IList<SelectListModel>> GetRoleSelectList();
    }
}
