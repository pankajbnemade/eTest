using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedger : IRepository<Ledger>
    {
        Task<int> CreateLedger(LedgerModel ledgerModel);

        Task<bool> UpdateLedger(LedgerModel ledgerModel);

        Task<bool> DeleteLedger(int ledgerId);

        Task<LedgerModel> GetLedgerById(int ledgerId);

        Task<DataTableResultModel<LedgerModel>> GetLedgerListByParentGroupId(int parentGroupId);

        Task<DataTableResultModel<LedgerModel>> GetLedgerList();

        Task<IList<SelectListModel>> GetLedgerSelectList(int parentGroupId);

    }
}
