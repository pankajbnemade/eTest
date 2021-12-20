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
    public interface IApplicationIdentityUser : IRepository<ApplicationIdentityUser>
    {
        Task<int> CreateUser(ApplicationIdentityUserModel applicationIdentityUserModel);

        Task<bool> UpdateUser(ApplicationIdentityUserModel applicationIdentityUserModel);

        Task<bool> LockUnlock(int userId);

         //void AssignDefaultSession(string email);

        Task<ApplicationIdentityUserModel> GetApplicationIdentityUserByUserId(int userId);

        Task<ApplicationIdentityUserModel> GetApplicationIdentityUserByEmail(string email);

        Task<DataTableResultModel<ApplicationIdentityUserModel>> GetApplicationIdentityUserList();

        Task<IList<SelectListModel>> GetUserSelectList();
    }
}
