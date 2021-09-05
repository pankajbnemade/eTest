using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IAdvanceAdjustment : IRepository<Advanceadjustment>
    {
        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        Task<GenerateNoModel> GenerateAdvanceAdjustmentNo(int companyId, int financialYearId);

        Task<int> CreateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel);

        Task<bool> UpdateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel);

        Task<bool> DeleteAdvanceAdjustment(int advanceAdjustmentId);

        Task<bool> UpdateAdvanceAdjustmentMasterAmount(int? advanceAdjustmentId);

        Task<AdvanceAdjustmentModel> GetAdvanceAdjustmentById(int advanceAdjustmentId);
        
        /// <summary>
        /// get search  invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<AdvanceAdjustmentModel>> GetAdvanceAdjustmentList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterAdvanceAdjustmentModel searchFilterModel);
   
        //Task<DataTableResultModel<AdvanceAdjustmentModel>> GetAdvanceAdjustmentList();
    }
}
