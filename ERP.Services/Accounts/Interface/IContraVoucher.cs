using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IContraVoucher : IRepository<Contravoucher>
    {
        Task<GenerateNoModel> GenerateContraVoucherNo(int companyId, int financialYearId);

        Task<int> CreateContraVoucher(ContraVoucherModel contraVoucherModel);

        Task<bool> UpdateContraVoucher(ContraVoucherModel contraVoucherModel);

        Task<bool> DeleteContraVoucher(int contraVoucherId);

        Task<bool> UpdateContraVoucherMasterAmount(int contraVoucherId);

         Task<bool> UpdateStatusContraVoucher(int contraVoucherId, int action);

        Task<ContraVoucherModel> GetContraVoucherById(int contraVoucherId);
        
        Task<DataTableResultModel<ContraVoucherModel>> GetContraVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterContraVoucherModel searchFilterModel);
   
    }
}
