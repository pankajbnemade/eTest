using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class UserService : Repository<User>, IUser
    {
        public UserService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateUser(UserModel userModel)
        {
            int userId = 0;

            // assign values.
            User user = new User();

            user.UserName = userModel.UserName;
          
            userId = await Create(user);

            return userId; // returns.
        }

        public async Task<bool> UpdateUser(UserModel userModel)
        {
            bool isUpdated = false;

            // get record.
            User user = await GetByIdAsync(w => w.UserId == userModel.UserId);
            if (null != user)
            {
                // assign values.
                user.UserName = userModel.UserName;
               
                isUpdated = await Update(user);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteUser(int userId)
        {
            bool isDeleted = false;

            // get record.
            User user = await GetByIdAsync(w => w.UserId == userId);
            if (null != user)
            {
                isDeleted = await Delete(user);
            }

            return isDeleted; // returns.
        }


        public async Task<UserModel> GetUserById(int userId)
        {
            UserModel userModel = null;

            IList<UserModel> userModelList = await GetUserList(userId);
            if (null != userModelList && userModelList.Any())
            {
                userModel = userModelList.FirstOrDefault();
            }

            return userModel; // returns.
        }

        public async Task<DataTableResultModel<UserModel>> GetUserList()
        {
            DataTableResultModel<UserModel> resultModel = new DataTableResultModel<UserModel>();

            IList<UserModel> userModelList = await GetUserList(0);
            if (null != userModelList && userModelList.Any())
            {
                resultModel = new DataTableResultModel<UserModel>();
                resultModel.ResultList = userModelList;
                resultModel.TotalResultCount = userModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all user list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<UserModel>> GetUserList(int userId)
        {
            IList<UserModel> userModelList = null;

            // get records by query.

            IQueryable<User> query = GetQueryByCondition(w => w.UserId != 0);

            if (0 != userId)
                query = query.Where(w => w.UserId == userId);

            IList<User> userList = await query.ToListAsync();

            if (null != userList && userList.Count > 0)
            {
                userModelList = new List<UserModel>();
                foreach (User user in userList)
                {
                    userModelList.Add(await AssignValueToModel(user));
                }
            }

            return userModelList; // returns.
        }

        private async Task<UserModel> AssignValueToModel(User user)
        {
            return await Task.Run(() =>
            {
                UserModel userModel = new UserModel();

                userModel.UserId = user.UserId;
                userModel.UserName = user.UserName;

                return userModel;
            });
        }

    }
}
