using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IJournalVoucherDetail : IRepository<Journalvoucherdetail>
    {
        Task<int> CreateJournalVoucherDetail(JournalVoucherDetailModel journalVoucherDetailModel);

        Task<bool> UpdateJournalVoucherDetail(JournalVoucherDetailModel journalVoucherDetailModel);

        Task<bool> UpdateJournalVoucherDetailAmount(int journalVoucherDetailId);

        Task<bool> DeleteJournalVoucherDetail(int journalVoucherDetailId);

        Task<JournalVoucherDetailModel> GetJournalVoucherDetailById(int journalVoucherDetailId,int journalVoucherId);

        Task<IList<JournalVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<JournalVoucherDetailModel>> GetJournalVoucherDetailByJournalVoucherId(int journalVoucherId, int addRow);

        Task<IList<JournalVoucherDetailModel>> GetJournalVoucherDetailByVoucherId(int journalVoucherId, int addRow_Blank);

        Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId);
    }
}
