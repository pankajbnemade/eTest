using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IAdvanceAdjustment : IRepository<Advanceadjustment>
    {
        Task<GenerateNoModel> GenerateAdvanceAdjustmentNo(int companyId, int financialYearId);

        Task<int> CreateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel);

        Task<bool> UpdateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel);

        Task<bool> DeleteAdvanceAdjustment(int advanceAdjustmentId);

        Task<bool> UpdateAdvanceAdjustmentMasterAmount(int advanceAdjustmentId);

        Task<bool> UpdateStatusAdvanceAdjustment(int advanceAdjustmentId, int action);

        Task<AdvanceAdjustmentModel> GetAdvanceAdjustmentById(int advanceAdjustmentId);
        
        Task<DataTableResultModel<AdvanceAdjustmentModel>> GetAdvanceAdjustmentList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterAdvanceAdjustmentModel searchFilterModel);
   
    }
}
