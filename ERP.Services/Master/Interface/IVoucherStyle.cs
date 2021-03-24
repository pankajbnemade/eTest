using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IVoucherStyle : IRepository<Voucherstyle>
    {
        Task<int> CreateVoucherStyle(VoucherStyleModel voucherStyleModel);

        Task<bool> UpdateVoucherStyle(VoucherStyleModel voucherStyleModel);

        Task<bool> DeleteVoucherStyle(int voucherStyleId);
       
        Task<VoucherStyleModel> GetVoucherStyleById(int voucherStyleId);

        Task<IList<SelectListModel>> GetVoucherStyleSelectList();

        Task<DataTableResultModel<VoucherStyleModel>> GetVoucherStyleList();
    }
}
