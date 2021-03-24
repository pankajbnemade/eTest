using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IStatus : IRepository<Status>
    {
        Task<int> CreateStatus(StatusModel statusModel);

        Task<bool> UpdateStatus(StatusModel statusModel);

        Task<bool> DeleteStatus(int statusId);
       
        Task<StatusModel> GetStatusById(int statusId);
        
        Task<DataTableResultModel<StatusModel>> GetStatusList();

        Task<IList<SelectListModel>> GetStatusSelectList();

    }
}
