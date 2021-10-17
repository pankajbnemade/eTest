using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalVoucher : IRepository<Journalvoucher>
    {
        Task<GenerateNoModel> GenerateJournalVoucherNo(int companyId, int financialYearId);

        Task<int> CreateJournalVoucher(JournalVoucherModel journalVoucherModel);

        Task<bool> UpdateJournalVoucher(JournalVoucherModel journalVoucherModel);

        Task<bool> DeleteJournalVoucher(int journalVoucherId);

        Task<bool> UpdateJournalVoucherMasterAmount(int journalVoucherId);

         Task<bool> UpdateStatusJournalVoucher(int journalVoucherId, int action);

        Task<JournalVoucherModel> GetJournalVoucherById(int paymentVoucherId);
        
        Task<DataTableResultModel<JournalVoucherModel>> GetJournalVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterJournalVoucherModel searchFilterModel);
   
    }
}
