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
    public class ApplicationRoleService : Repository<ApplicationRole>, IApplicationRole
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationRoleService(ErpDbContext dbContext,
             RoleManager<ApplicationRole> _roleManager) : base(dbContext)
        {
            roleManager = _roleManager;
        }

        public async Task<int> CreateRole(ApplicationRoleModel roleModel)
        {
            int applicationRoleId = 0;

            // assign values.
            ApplicationRole applicationRole = new ApplicationRole();

            applicationRole.Name = roleModel.Name;

            IdentityResult result = await roleManager.CreateAsync(applicationRole);

            applicationRoleId = applicationRole.Id;

            return applicationRoleId; // returns.
        }

        public async Task<bool> UpdateRole(ApplicationRoleModel roleModel)
        {
            bool isUpdated = false;

            // get record.
            ApplicationRole applicationRole = await roleManager.FindByIdAsync(roleModel.Id.ToString());

            if (null != applicationRole)
            {
                // assign values.
                applicationRole.Name = roleModel.Name;
                
                IdentityResult result = await roleManager.UpdateAsync(applicationRole);

                isUpdated = result.Succeeded;
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteRole(int applicationRoleId)
        {
            bool isDeleted = false;

            // get record.
            ApplicationRole applicationRole = await roleManager.FindByIdAsync(applicationRoleId.ToString());

            if (null != applicationRole)
            {
                IdentityResult result = await roleManager.DeleteAsync(applicationRole);

                isDeleted = result.Succeeded;
            }

            return isDeleted; // returns.
        }

        public async Task<ApplicationRoleModel> GetApplicationRoleById(int applicationRoleId)
        {
            ApplicationRoleModel roleModel = null;

            ApplicationRole applicationRole = await roleManager.FindByIdAsync(applicationRoleId.ToString());

            if (null != applicationRole)
            {
                roleModel = await AssignValueToModel(applicationRole);
            }

            return roleModel; // returns.
        }

        public async Task<DataTableResultModel<ApplicationRoleModel>> GetApplicationRoleList()
        {
            DataTableResultModel<ApplicationRoleModel> resultModel = new DataTableResultModel<ApplicationRoleModel>();

            IList<ApplicationRoleModel> roleModelList = await GetRoleList();

            if (null != roleModelList && roleModelList.Any())
            {
                resultModel = new DataTableResultModel<ApplicationRoleModel>();
                resultModel.ResultList = roleModelList;
                resultModel.TotalResultCount = roleModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all applicationRole list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<ApplicationRoleModel>> GetRoleList()
        {
            IList<ApplicationRoleModel> roleModelList = null;

            IList<ApplicationRole> roleList = await roleManager.Roles.ToListAsync();

            if (null != roleList && roleList.Count > 0)
            {
                roleModelList = new List<ApplicationRoleModel>();

                foreach (ApplicationRole applicationRole in roleList)
                {
                    roleModelList.Add(await AssignValueToModel(applicationRole));
                }
            }

            return roleModelList; // returns.
        }

        private async Task<ApplicationRoleModel> AssignValueToModel(ApplicationRole applicationRole)
        {
            return await Task.Run(() =>
            {
                ApplicationRoleModel roleModel = new ApplicationRoleModel();

                roleModel.Id = applicationRole.Id;
                roleModel.Name = applicationRole.Name;
                roleModel.NormalizedName = applicationRole.NormalizedName;
                roleModel.ConcurrencyStamp = applicationRole.ConcurrencyStamp;

                return roleModel;
            });
        }

        public async Task<IList<SelectListModel>> GetRoleSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.Id != 0))
            {
                IQueryable<ApplicationRole> roleList = roleManager.Roles;

                resultModel = await roleList.Select(s => new SelectListModel
                {
                    DisplayText = s.Name,
                    Value = s.Id.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
