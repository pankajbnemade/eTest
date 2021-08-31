using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IDebitNoteDetailTax : IRepository<Debitnotedetailtax>
    {
         Task<int> GenerateSrNo(int salesDebitNoteDetId);

        Task<int> CreateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel);

        Task<bool> UpdateDebitNoteDetailTax(DebitNoteDetailTaxModel debitNoteDetailTaxModel);

        Task<bool> DeleteDebitNoteDetailTax(int debitNoteDetailTaxId);

        Task<DebitNoteDetailTaxModel> GetDebitNoteDetailTaxById(int debitNoteDetailTaxId);

        Task<DataTableResultModel<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxByDebitNoteDetailId(int debitNoteDetailId);

        Task<DataTableResultModel<DebitNoteDetailTaxModel>> GetDebitNoteDetailTaxList();

    }
}
