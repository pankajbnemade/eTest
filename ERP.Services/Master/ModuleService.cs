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
    public class ModuleService : Repository<Module>, IModule
    {
        public ModuleService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateModule(ModuleModel moduleModel)
        {
            int moduleId = 0;

            // assign values.
            Module module = new Module();

            module.ModuleName = moduleModel.ModuleName;
            module.IsActive = moduleModel.IsActive;
           
            moduleId = await Create(module);

            return moduleId; // returns.
        }

        public async Task<bool> UpdateModule(ModuleModel moduleModel)
        {
            bool isUpdated = false;

            // get record.
            Module module = await GetByIdAsync(w => w.ModuleId == moduleModel.ModuleId);
            if (null != module)
            {
                // assign values.
                module.ModuleName = moduleModel.ModuleName;
                module.IsActive = moduleModel.IsActive;

                isUpdated = await Update(module);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteModule(int moduleId)
        {
            bool isDeleted = false;

            // get record.
            Module module = await GetByIdAsync(w => w.ModuleId == moduleId);
            if (null != module)
            {
                isDeleted = await Delete(module);
            }

            return isDeleted; // returns.
        }


        public async Task<ModuleModel> GetModuleById(int moduleId)
        {
            ModuleModel moduleModel = null;

            IList<ModuleModel> moduleModelList = await GetModuleList(moduleId);
            if (null != moduleModelList && moduleModelList.Any())
            {
                moduleModel = moduleModelList.FirstOrDefault();
            }

            return moduleModel; // returns.
        }

        public async Task<DataTableResultModel<ModuleModel>> GetModuleList()
        {
            DataTableResultModel<ModuleModel> resultModel = new DataTableResultModel<ModuleModel>();

            IList<ModuleModel> moduleModelList = await GetModuleList(0);
            if (null != moduleModelList && moduleModelList.Any())
            {
                resultModel = new DataTableResultModel<ModuleModel>();
                resultModel.ResultList = moduleModelList;
                resultModel.TotalResultCount = moduleModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all module list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<ModuleModel>> GetModuleList(int moduleId)
        {
            IList<ModuleModel> moduleModelList = null;

            // get records by query.

            IQueryable<Module> query = GetQueryByCondition(w => w.ModuleId != 0);

            if (0 != moduleId)
                query = query.Where(w => w.ModuleId == moduleId);

            IList<Module> moduleList = await query.ToListAsync();

            if (null != moduleList && moduleList.Count > 0)
            {
                moduleModelList = new List<ModuleModel>();
                foreach (Module module in moduleList)
                {
                    moduleModelList.Add(await AssignValueToModel(module));
                }
            }

            return moduleModelList; // returns.
        }

        private async Task<ModuleModel> AssignValueToModel(Module module)
        {
            return await Task.Run(() =>
            {
                ModuleModel moduleModel = new ModuleModel();

                moduleModel.ModuleId = module.ModuleId;
                moduleModel.ModuleName = module.ModuleName;
                moduleModel.IsActive = module.IsActive;

                return moduleModel;
            });
        }

    }
}
