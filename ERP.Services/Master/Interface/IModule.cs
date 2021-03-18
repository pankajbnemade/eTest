using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IModule : IRepository<Module>
    {
        Task<int> CreateModule(ModuleModel moduleModel);

        Task<bool> UpdateModule(ModuleModel moduleModel);

        Task<bool> DeleteModule(int moduleId);
       
        Task<ModuleModel> GetModuleById(int moduleId);
        
        //Task<IList<ModuleModel>> GetModuleByActive(int isActive);

        Task<DataTableResultModel<ModuleModel>> GetModuleList();
    }
}
