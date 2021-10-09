using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IDebitNoteDetailTax : IRepository<Debitnotedetailtax>
    {
        Task<int> GenerateSrNo(int debitNoteDetId);

        Task<int> CreateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel);

        Task<bool> UpdateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel);

        Task<bool> AddDebitNoteDetailTaxByDebitNoteId(int debitNoteId, int taxRegisterId);

        Task<bool> AddDebitNoteDetailTaxByDebitNoteDetId(int debitNoteDetId, int taxRegisterId);

        Task<bool> UpdateDebitNoteDetailTaxAmountOnDetailUpdate(int? debitNoteDetailId);

        Task<bool> DeleteDebitNoteDetailTax(int debitNoteDetailTaxId);

        Task<bool> DeleteDebitNoteDetailTaxByDebitNoteId(int debitNoteId);

        Task<DebitNoteDetailTaxModel> GetDebitNoteDetailTaxById(int debitNoteDetailTaxId);

        Task<DataTableResultModel<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxByDebitNoteDetailId(int debitNoteDetailId);

        Task<DataTableResultModel<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxList();

    }
}
