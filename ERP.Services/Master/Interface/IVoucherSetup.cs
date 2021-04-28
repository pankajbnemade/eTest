using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IVoucherSetup : IRepository<Vouchersetup>
    {

        Task<int> CreateVoucherSetup(VoucherSetupModel voucherSetupModel);

        Task<bool> UpdateVoucherSetup(VoucherSetupModel voucherSetupModel);

        Task<bool> DeleteVoucherSetup(int voucherSetupId);

        Task<VoucherSetupModel> GetVoucherSetupById(int voucherSetupId);

        Task<DataTableResultModel<VoucherSetupModel>> GetVoucherSetupList();

        Task<IList<SelectListModel>> GetVoucherSetupSelectList();
    }
}
