using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IUser : IRepository<User>
    {
       
        Task<int> CreateUser(UserModel userModel);

        Task<bool> UpdateUser(UserModel userModel);

        Task<bool> DeleteUser(int userId);
       
        Task<UserModel> GetUserById(int userId);

        //Task<IList<UserModel>> GetUserByEmployeeId(int employeeId);

        Task<IList<SelectListModel>> GetUserSelectList();

        Task<DataTableResultModel<UserModel>> GetUserList();
    }
}
