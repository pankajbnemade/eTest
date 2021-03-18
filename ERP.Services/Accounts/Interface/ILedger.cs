using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedger : IRepository<Ledger>
    {
        Task<int> CreateLedger(LedgerModel ledgerModel);

        Task<bool> UpdateLedger(LedgerModel ledgerModel);

        Task<bool> DeleteLedger(int ledgerId);

        Task<LedgerModel> GetLedgerById(int ledgerId);

        //Task<LedgerModel> GetLedgerByParentGroupId(int parentGroupId);

        Task<DataTableResultModel<LedgerModel>> GetLedgerList();
    }
}
