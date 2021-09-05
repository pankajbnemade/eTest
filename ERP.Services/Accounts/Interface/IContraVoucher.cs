using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IContraVoucher : IRepository<Contravoucher>
    {
        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        Task<GenerateNoModel> GenerateContraVoucherNo(int companyId, int financialYearId);

        Task<int> CreateContraVoucher(ContraVoucherModel contraVoucherModel);

        Task<bool> UpdateContraVoucher(ContraVoucherModel contraVoucherModel);

        Task<bool> DeleteContraVoucher(int contraVoucherId);

        Task<bool> UpdateContraVoucherMasterAmount(int? contraVoucherId);

        Task<ContraVoucherModel> GetContraVoucherById(int contraVoucherId);
        
        /// <summary>
        /// get search  invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<ContraVoucherModel>> GetContraVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterContraVoucherModel searchFilterModel);
   
        //Task<DataTableResultModel<ContraVoucherModel>> GetContraVoucherList();
    }
}
