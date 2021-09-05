using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalVoucher : IRepository<Journalvoucher>
    {
        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        Task<GenerateNoModel> GenerateJournalVoucherNo(int companyId, int financialYearId);

        Task<int> CreateJournalVoucher(JournalVoucherModel journalVoucherModel);

        Task<bool> UpdateJournalVoucher(JournalVoucherModel journalVoucherModel);

        Task<bool> DeleteJournalVoucher(int journalVoucherId);

        Task<bool> UpdateJournalVoucherMasterAmount(int? journalVoucherId);

        Task<JournalVoucherModel> GetJournalVoucherById(int journalVoucherId);
        
        /// <summary>
        /// get search  invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<JournalVoucherModel>> GetJournalVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterJournalVoucherModel searchFilterModel);
   
        //Task<DataTableResultModel<JournalVoucherModel>> GetJournalVoucherList();
    }
}
