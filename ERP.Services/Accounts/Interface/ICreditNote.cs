using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICreditNote : IRepository<Creditnote>
    {
        /// <summary>
        /// generate creditNote no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return creditNote no.
        /// </returns>
        Task<GenerateNoModel> GenerateCreditNoteNo(int companyId, int financialYearId);

        Task<int> CreateCreditNote(CreditNoteModel creditNoteModel);

        Task<bool> UpdateCreditNote(CreditNoteModel creditNoteModel);

        Task<bool> DeleteCreditNote(int creditNoteId);

        Task<bool> UpdateStatusCreditNote(int creditNoteId, int action);

        Task<bool> UpdateCreditNoteMasterAmount(int? creditNoteId);

        Task<CreditNoteModel> GetCreditNoteById(int creditNoteId);

        Task<IList<OutstandingInvoiceModel>> GetCreditNoteListByPartyLedgerId(int partyLedgerId, DateTime? voucherDate);

        /// <summary>
        /// get search purchase creditNote result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<CreditNoteModel>> GetCreditNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterCreditNoteModel searchFilterModel);

        //Task<DataTableResultModel<CreditNoteModel>> GetCreditNoteList();
    }
}
