using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IContraVoucherDetail : IRepository<Contravoucherdetail>
    {
        Task<int> CreateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel);

        Task<bool> UpdateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel);

        Task<bool> UpdateContraVoucherDetailAmount(int? contraVoucherDetailId);

        Task<bool> DeleteContraVoucherDetail(int contraVoucherDetailId);

        Task<ContraVoucherDetailModel> GetContraVoucherDetailById(int contraVoucherDetailId);

        Task<DataTableResultModel<ContraVoucherDetailModel>> GetContraVoucherDetailByContraVoucherId(int contraVoucherId);

        Task<DataTableResultModel<ContraVoucherDetailModel>> GetContraVoucherDetailList();

    }
}
