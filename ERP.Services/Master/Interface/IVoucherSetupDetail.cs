using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IVoucherSetupDetail : IRepository<Vouchersetupdetail>
    {
       
        Task<int> CreateVoucherSetupDetail(VoucherSetupDetailModel voucherSetupDetailModel);

        Task<bool> UpdateVoucherSetupDetail(VoucherSetupDetailModel voucherSetupDetailModel);

        Task<bool> DeleteVoucherSetupDetail(int voucherSetupDetailId);
       
        Task<VoucherSetupDetailModel> GetVoucherSetupDetailById(int voucherSetupDetailId);
        
        //Task<IList<VoucherSetupDetailModel>> GetVoucherSetupDetailByVoucherSetupId(int voucherSetupId);

        Task<DataTableResultModel<VoucherSetupDetailModel>> GetVoucherSetupDetailList();
    }
}
