using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ICreditNoteDetail : IRepository<Creditnotedetail>
    {
        Task<int> GenerateSrNo(int creditNoteId);

        Task<int> CreateCreditNoteDetail(CreditNoteDetailModel creditNoteDetailModel);

        Task<bool> UpdateCreditNoteDetail(CreditNoteDetailModel creditNoteDetailModel);

        Task<bool> UpdateCreditNoteDetailAmount(int? creditNoteDetailId);

        Task<bool> DeleteCreditNoteDetail(int creditNoteDetailId);

        Task<CreditNoteDetailModel> GetCreditNoteDetailById(int creditNoteDetailId);

        Task<DataTableResultModel<CreditNoteDetailModel>> GetCreditNoteDetailByCreditNoteId(int creditNoteId);

        Task<DataTableResultModel<CreditNoteDetailModel>> GetCreditNoteDetailList();

    }
}
