using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICreditNoteDetailTax : IRepository<Creditnotedetailtax>
    {
        Task<int> GenerateSrNo(int creditNoteDetId);

        Task<int> CreateCreditNoteDetailTax(CreditNoteDetailTaxModel creditNoteDetailTaxModel);

        Task<bool> UpdateCreditNoteDetailTax(CreditNoteDetailTaxModel creditNoteDetailTaxModel);

        Task<bool> AddCreditNoteDetailTaxByCreditNoteId(int creditNoteId, int taxRegisterId);

        Task<bool> AddCreditNoteDetailTaxByCreditNoteDetId(int creditNoteDetId, int taxRegisterId);

        Task<bool> UpdateCreditNoteDetailTaxAmountOnDetailUpdate(int? creditNoteDetailId);

        Task<bool> DeleteCreditNoteDetailTax(int creditNoteDetailTaxId);

        Task<bool> DeleteCreditNoteDetailTaxByCreditNoteId(int creditNoteId);

        Task<CreditNoteDetailTaxModel> GetCreditNoteDetailTaxById(int creditNoteDetailTaxId);

        Task<DataTableResultModel<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxByCreditNoteDetailId(int creditNoteDetailId);

        Task<DataTableResultModel<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxList();

    }
}
