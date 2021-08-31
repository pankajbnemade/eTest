using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IDebitNoteDetail : IRepository<Debitnotedetail>
    {
        Task<int> GenerateSrNo(int debitNoteId);

        Task<int> CreateDebitNoteDetail(DebitNoteDetailModel debitNoteDetailModel);

        Task<bool> UpdateDebitNoteDetail(DebitNoteDetailModel debitNoteDetailModel);

        Task<bool> UpdateDebitNoteDetailAmount(int? debitNoteDetailId);

        Task<bool> DeleteDebitNoteDetail(int debitNoteDetailId);

        Task<DebitNoteDetailModel> GetDebitNoteDetailById(int debitNoteDetailId);

        Task<DataTableResultModel<DebitNoteDetailModel>> GetDebitNoteDetailByDebitNoteId(int debitNoteId);

        Task<DataTableResultModel<DebitNoteDetailModel>> GetDebitNoteDetailList();

    }
}
