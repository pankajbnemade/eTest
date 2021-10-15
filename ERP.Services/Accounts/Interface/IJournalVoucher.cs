using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalVoucher : IRepository<Journalvoucher>
    {
        Task<GenerateNoModel> GenerateJournalVoucherNo(int companyId, int financialYearId);

        Task<int> CreateJournalVoucher(JournalVoucherModel paymentVoucherModel);

        Task<bool> UpdateJournalVoucher(JournalVoucherModel paymentVoucherModel);

        Task<bool> DeleteJournalVoucher(int paymentVoucherId);

        Task<bool> UpdateJournalVoucherMasterAmount(int paymentVoucherId);

         Task<bool> UpdateStatusJournalVoucher(int paymentVoucherId, int action);

        Task<JournalVoucherModel> GetJournalVoucherById(int paymentVoucherId);
        
        Task<DataTableResultModel<JournalVoucherModel>> GetJournalVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterJournalVoucherModel searchFilterModel);
   
    }
}
