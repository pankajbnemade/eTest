using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IAdvanceAdjustmentDetail : IRepository<Advanceadjustmentdetail>
    {
        Task<int> CreateAdvanceAdjustmentDetail(AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel);

        Task<bool> UpdateAdvanceAdjustmentDetail(AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel);

        Task<bool> UpdateAdvanceAdjustmentDetailAmount(int? advanceAdjustmentDetailId);

        Task<bool> DeleteAdvanceAdjustmentDetail(int advanceAdjustmentDetailId);

        Task<AdvanceAdjustmentDetailModel> GetAdvanceAdjustmentDetailById(int advanceAdjustmentDetailId);

        Task<IList<AdvanceAdjustmentDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailByAdvanceAdjustmentId(int advanceAdjustmentId);

        Task<DataTableResultModel<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailList();

    }
}
