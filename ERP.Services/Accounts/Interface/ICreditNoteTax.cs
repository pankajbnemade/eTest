using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICreditNoteTax : IRepository<Creditnotetax>
    {
         Task<int> GenerateSrNo(int creditNoteId);

        Task<int> CreateCreditNoteTax(CreditNoteTaxModel creditNoteTaxModel);

        Task<bool> UpdateCreditNoteTax(CreditNoteTaxModel creditNoteTaxModel);

        Task<bool> UpdateCreditNoteTaxAmountAll(int? creditNoteId);

        Task<bool> AddCreditNoteTaxByCreditNoteId(int creditNoteId,int taxRegisterId);

        Task<bool> DeleteCreditNoteTaxByCreditNoteId(int creditNoteId);

        Task<bool> DeleteCreditNoteTax(int creditNoteTaxId);

        Task<CreditNoteTaxModel> GetCreditNoteTaxById(int creditNoteTaxId);

        Task<DataTableResultModel<CreditNoteTaxModel>> GetCreditNoteTaxByCreditNoteId(int creditNoteId);

        Task<DataTableResultModel<CreditNoteTaxModel>> GetCreditNoteTaxList();
    }
}
