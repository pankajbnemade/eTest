using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IDebitNoteTax : IRepository<Debitnotetax>
    {
         Task<int> GenerateSrNo(int debitNoteId);

        Task<int> CreateDebitNoteTax(DebitNoteTaxModel debitNoteTaxModel);

        Task<bool> UpdateDebitNoteTax(DebitNoteTaxModel debitNoteTaxModel);

        Task<bool> UpdateDebitNoteTaxAmountAll(int? debitNoteId);

        Task<bool> AddDebitNoteTaxByDebitNoteId(int debitNoteId,int taxRegisterId);

        Task<bool> DeleteDebitNoteTaxByDebitNoteId(int debitNoteId);

        Task<bool> DeleteDebitNoteTax(int debitNoteTaxId);

        Task<DebitNoteTaxModel> GetDebitNoteTaxById(int debitNoteTaxId);

        Task<DataTableResultModel<DebitNoteTaxModel>> GetDebitNoteTaxByDebitNoteId(int debitNoteId);

        Task<DataTableResultModel<DebitNoteTaxModel>> GetDebitNoteTaxList();
    }
}
