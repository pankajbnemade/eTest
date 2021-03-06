using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IDebitNote : IRepository<Debitnote>
    {
        /// <summary>
        /// generate debitNote no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return debitNote no.
        /// </returns>
        Task<GenerateNoModel> GenerateDebitNoteNo(int companyId, int financialYearId);

        Task<int> CreateDebitNote(DebitNoteModel debitNoteModel);

        Task<bool> UpdateDebitNote(DebitNoteModel debitNoteModel);

        Task<bool> DeleteDebitNote(int debitNoteId);

        Task<bool> UpdateStatusDebitNote(int debitNoteId, int action);

        Task<bool> UpdateDebitNoteMasterAmount(int? debitNoteId);

        Task<DebitNoteModel> GetDebitNoteById(int debitNoteId);

        Task<IList<OutstandingInvoiceModel>> GetDebitNoteListByPartyLedgerId(int partyLedgerId, DateTime? voucherDate);

        /// <summary>
        /// get search purchase debitNote result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<DebitNoteModel>> GetDebitNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterDebitNoteModel searchFilterModel);

        Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId);

        //Task<DataTableResultModel<DebitNoteModel>> GetDebitNoteList();
    }
}
