using ERP.DataAccess.Entity;
using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Admin
{
    public class AssignRoleService : Repository<Aspnetuserrole>, IAssignRole
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public AssignRoleService(ErpDbContext dbContext,
             RoleManager<ApplicationRole> roleManager,
              UserManager<ApplicationIdentityUser> userManager) : base(dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserRole(AssignRoleModel assignRoleModel)
        {
            bool isAdded = false;

            // assign values.
            ApplicationIdentityUser applicationIdentityUser = await _userManager.FindByIdAsync(assignRoleModel.UserId.ToString());
            ApplicationRole applicationRole = await _roleManager.FindByIdAsync(assignRoleModel.RoleId.ToString());

            if (applicationIdentityUser != null && applicationRole != null && !(await _userManager.IsInRoleAsync(applicationIdentityUser, applicationRole.Name)))
            {
                IdentityResult result = await _userManager.AddToRoleAsync(applicationIdentityUser, applicationRole.Name);

                isAdded = result.Succeeded;
            }

            return isAdded; // returns.
        }

        public async Task<bool> DeleteUserRole(int userId, int roleId)
        {
            bool isDeleted = false;

            // get record.
            ApplicationIdentityUser applicationIdentityUser = await _userManager.FindByIdAsync(userId.ToString());

            ApplicationRole applicationRole = await _roleManager.FindByIdAsync(roleId.ToString());

            if (applicationIdentityUser != null && applicationRole != null && (await _userManager.IsInRoleAsync(applicationIdentityUser, applicationRole.Name)))
            {
                IdentityResult result = await _userManager.RemoveFromRoleAsync(applicationIdentityUser, applicationRole.Name);

                isDeleted = result.Succeeded;
            }

            return isDeleted; // returns.
        }

        public async Task<DataTableResultModel<AssignRoleModel>> GetUserRoleList()
        {
            DataTableResultModel<AssignRoleModel> resultModel = new DataTableResultModel<AssignRoleModel>();

            IList<AssignRoleModel> roleModelList = await GetAllUserRoleList();

            if (null != roleModelList && roleModelList.Any())
            {
                resultModel = new DataTableResultModel<AssignRoleModel>();
                resultModel.ResultList = roleModelList;
                resultModel.TotalResultCount = roleModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<IList<AssignRoleModel>> GetAllUserRoleList()
        {
            IList<AssignRoleModel> roleModelList = null;

            IQueryable<Aspnetuserrole> query = GetQueryByCondition(w => w.UserId != 0)
                                                .Include(w => w.User)
                                                .Include(w => w.Role);
            
            IList<Aspnetuserrole> userRoleList = await query.ToListAsync();

            if (null != userRoleList && userRoleList.Count > 0)
            {
                roleModelList = new List<AssignRoleModel>();

                foreach (Aspnetuserrole aspNetUserRole in userRoleList)
                {
                    roleModelList.Add(await AssignValueToModel(aspNetUserRole));
                }
            }

            return roleModelList; // returns.
        }

        private async Task<AssignRoleModel> AssignValueToModel(Aspnetuserrole aspNetUserRole)
        {
            return await Task.Run(() =>
            {
                AssignRoleModel roleModel = new AssignRoleModel();

                roleModel.UserId = aspNetUserRole.UserId;
                roleModel.Email = aspNetUserRole.User.Email;
                roleModel.RoleId = aspNetUserRole.RoleId;
                roleModel.RoleName = aspNetUserRole.Role.Name;

                return roleModel;
            });
        }

    }
}
