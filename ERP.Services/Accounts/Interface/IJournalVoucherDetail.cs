using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalVoucherDetail : IRepository<Journalvoucherdetail>
    {
        Task<int> CreateJournalVoucherDetail(JournalVoucherDetailModel paymentVoucherDetailModel);

        Task<bool> UpdateJournalVoucherDetail(JournalVoucherDetailModel paymentVoucherDetailModel);

        Task<bool> UpdateJournalVoucherDetailAmount(int paymentVoucherDetailId);

        Task<bool> DeleteJournalVoucherDetail(int paymentVoucherDetailId);

        Task<JournalVoucherDetailModel> GetJournalVoucherDetailById(int paymentVoucherDetailId,int paymentVoucherId);

        Task<IList<JournalVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<JournalVoucherDetailModel>> GetJournalVoucherDetailByJournalVoucherId(int paymentVoucherId, int addRow);

        Task<IList<JournalVoucherDetailModel>> GetJournalVoucherDetailByVoucherId(int paymentVoucherId, int addRow_Blank);

    }
}
