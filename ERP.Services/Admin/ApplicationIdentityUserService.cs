using ERP.DataAccess.Entity;
using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using ERP.Services.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Admin
{
    public class ApplicationIdentityUserService : Repository<ApplicationIdentityUser>, IApplicationIdentityUser
    {
        private readonly UserManager<ApplicationIdentityUser> userManager;
        public ApplicationIdentityUserService(ErpDbContext dbContext, UserManager<ApplicationIdentityUser> _userManager) : base(dbContext)
        {
            userManager = _userManager;
        }

        public async Task<int> CreateUser(ApplicationIdentityUserModel applicationIdentityUserModel)
        {
            int userId = 0;

            // assign values.
            ApplicationIdentityUser applicationIdentityUser = new ApplicationIdentityUser();


            userId = await Create(applicationIdentityUser);

            return userId; // returns.
        }

        public async Task<bool> UpdateUser(ApplicationIdentityUserModel applicationIdentityUserModel)
        {
            bool isUpdated = false;

            // get record.
            ApplicationIdentityUser applicationIdentityUser = await userManager.FindByIdAsync(applicationIdentityUserModel.Id.ToString());

            if (null != applicationIdentityUser)
            {
                // assign values.
                applicationIdentityUser.EmployeeId = applicationIdentityUserModel.EmployeeId;

                IdentityResult result = await userManager.UpdateAsync(applicationIdentityUser);

                isUpdated = result.Succeeded;
            }

            return isUpdated; // returns.
        }

        public async Task<bool> LockUnlock(int userId)
        {
            bool isUpdated = false;

            // get record.
            ApplicationIdentityUser applicationIdentityUser = await userManager.FindByIdAsync(userId.ToString());

            if (null != applicationIdentityUser)
            {
                // assign values.
                if (applicationIdentityUser.LockoutEnd != null && applicationIdentityUser.LockoutEnd > DateTime.Now)
                {
                    //user is currently locked, we will unlock them
                    applicationIdentityUser.LockoutEnd = DateTime.Now;
                }
                else
                {
                    applicationIdentityUser.LockoutEnd = DateTime.Now.AddYears(1000);
                }

                IdentityResult result = await userManager.UpdateAsync(applicationIdentityUser);

                isUpdated = result.Succeeded;
            }

            return isUpdated; // returns.
        }

        public async Task<ApplicationIdentityUserModel> GetApplicationIdentityUserByUserId(int userId)
        {
            ApplicationIdentityUserModel applicationIdentityUserModel = null;

            IList<ApplicationIdentityUserModel> applicationIdentityUserModelList = await GetApplicationIdentityUserList(userId, "");

            if (null != applicationIdentityUserModelList && applicationIdentityUserModelList.Any())
            {
                applicationIdentityUserModel = applicationIdentityUserModelList.FirstOrDefault();
            }

            return applicationIdentityUserModel; // returns.
        }

        public async Task<ApplicationIdentityUserModel> GetApplicationIdentityUserByEmail(string email)
        {
            ApplicationIdentityUserModel applicationIdentityUserModel = null;

            IList<ApplicationIdentityUserModel> applicationIdentityUserModelList = await GetApplicationIdentityUserList(0, email);

            if (null != applicationIdentityUserModelList && applicationIdentityUserModelList.Any())
            {
                applicationIdentityUserModel = applicationIdentityUserModelList.FirstOrDefault();
            }

            return applicationIdentityUserModel; // returns.
        }

        public async Task<DataTableResultModel<ApplicationIdentityUserModel>> GetApplicationIdentityUserList()
        {
            DataTableResultModel<ApplicationIdentityUserModel> resultModel = new DataTableResultModel<ApplicationIdentityUserModel>();

            IList<ApplicationIdentityUserModel> applicationIdentityUserModelList = await GetApplicationIdentityUserList(0, "");

            if (null != applicationIdentityUserModelList && applicationIdentityUserModelList.Any())
            {
                resultModel = new DataTableResultModel<ApplicationIdentityUserModel>();
                resultModel.ResultList = applicationIdentityUserModelList;
                resultModel.TotalResultCount = applicationIdentityUserModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<ApplicationIdentityUserModel>> GetApplicationIdentityUserList(int userId, string email)
        {
            IList<ApplicationIdentityUserModel> applicationIdentityUserModelList = null;

            // create query.
            IQueryable<ApplicationIdentityUser> query = userManager.Users.Where(w => w.Id != 0).Include(w => w.Employee);

            // apply filters.
            if (0 != userId)
                query = query.Where(w => w.Id == userId);

            if ("" != email)
                query = query.Where(w => w.Email == email);

            // get records by query.
            List<ApplicationIdentityUser> applicationIdentityUserList = await query.ToListAsync();

            if (null != applicationIdentityUserList && applicationIdentityUserList.Count > 0)
            {
                applicationIdentityUserModelList = new List<ApplicationIdentityUserModel>();

                foreach (ApplicationIdentityUser applicationIdentityUser in applicationIdentityUserList)
                {
                    applicationIdentityUserModelList.Add(await AssignValueToModel(applicationIdentityUser));
                }
            }

            return applicationIdentityUserModelList; // returns.
        }

        private async Task<ApplicationIdentityUserModel> AssignValueToModel(ApplicationIdentityUser applicationIdentityUser)
        {
            return await Task.Run(() =>
            {
                ApplicationIdentityUserModel applicationIdentityUserModel = new ApplicationIdentityUserModel();

                applicationIdentityUserModel.Id = applicationIdentityUser.Id;
                applicationIdentityUserModel.UserName = applicationIdentityUser.UserName;
                applicationIdentityUserModel.NormalizedUserName = applicationIdentityUser.NormalizedUserName;
                applicationIdentityUserModel.Email = applicationIdentityUser.Email;
                applicationIdentityUserModel.NormalizedEmail = applicationIdentityUser.NormalizedEmail;
                applicationIdentityUserModel.EmailConfirmed = applicationIdentityUser.EmailConfirmed;
                applicationIdentityUserModel.PasswordHash = applicationIdentityUser.PasswordHash;
                applicationIdentityUserModel.SecurityStamp = applicationIdentityUser.SecurityStamp;
                applicationIdentityUserModel.ConcurrencyStamp = applicationIdentityUser.ConcurrencyStamp;
                applicationIdentityUserModel.PhoneNumber = applicationIdentityUser.PhoneNumber;
                applicationIdentityUserModel.PhoneNumberConfirmed = applicationIdentityUser.PhoneNumberConfirmed;
                applicationIdentityUserModel.TwoFactorEnabled = applicationIdentityUser.TwoFactorEnabled;
                applicationIdentityUserModel.LockoutEnd = applicationIdentityUser.LockoutEnd;
                applicationIdentityUserModel.LockoutEnabled = applicationIdentityUser.LockoutEnabled;
                applicationIdentityUserModel.AccessFailedCount = applicationIdentityUser.AccessFailedCount;
                applicationIdentityUserModel.EmployeeName = null != applicationIdentityUser.Employee ? applicationIdentityUser.Employee.FirstName + " " + applicationIdentityUser.Employee.LastName : null;
                return applicationIdentityUserModel;
            });
        }

        public async Task<IList<SelectListModel>> GetUserSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.Id != 0))
            {
                IQueryable<ApplicationIdentityUser> roleList = userManager.Users;

                resultModel = await roleList.Select(s => new SelectListModel
                {
                    DisplayText = s.Email,
                    Value = s.Id.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
