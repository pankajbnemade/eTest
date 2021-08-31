using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICreditNoteDetailTax : IRepository<Creditnotedetailtax>
    {
         Task<int> GenerateSrNo(int salesCreditNoteDetId);

        Task<int> CreateCreditNoteDetailTax(CreditNoteDetailTaxModel creditNoteDetailTaxModel);

        Task<bool> UpdateCreditNoteDetailTax(CreditNoteDetailTaxModel creditNoteDetailTaxModel);

        Task<bool> DeleteCreditNoteDetailTax(int creditNoteDetailTaxId);

        Task<CreditNoteDetailTaxModel> GetCreditNoteDetailTaxById(int creditNoteDetailTaxId);

        Task<DataTableResultModel<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxByCreditNoteDetailId(int creditNoteDetailId);

        Task<DataTableResultModel<CreditNoteDetailTaxModel>> GetCreditNoteDetailTaxList();

    }
}
