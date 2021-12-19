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
    public interface IAssignRole : IRepository<Aspnetuserrole>
    {
        Task<int> AddUserRole(AssignRoleModel assignRoleModel);

        Task<ApplicationRoleModel> GetApplicationRoleById(int roleId);

        Task<bool> DeleteUserRole(int email, int roleId);

        Task<DataTableResultModel<AssignRoleModel>> GetUserRoleList();
    }
}
