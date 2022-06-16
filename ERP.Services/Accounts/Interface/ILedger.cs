using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedger : IRepository<Ledger>
    {
        Task<GenerateNoModel> GenerateLedgerCode();

        Task<int> CreateLedger(LedgerModel ledgerModel);

        Task<bool> UpdateLedger(LedgerModel ledgerModel);

        Task<bool> DeleteLedger(int ledgerId);

        Task<LedgerModel> GetLedgerById(int ledgerId);

        Task<DataTableResultModel<LedgerModel>> GetLedgerListByParentGroupId(int parentGroupId);

        Task<LedgerModel> GetClosingBalanceByAccountLedgerId(int ledgerId, DateTime voucherDate);

        Task<DataTableResultModel<LedgerModel>> GetLedgerList();

        Task<IList<SelectListModel>> GetGroupSelectList(int parentGroupId);

        Task<IList<SelectListModel>> GetLedgerSelectList(int parentGroupId, int companyId, Boolean IsLegderOnly);
        Task<IList<SelectListModel>> GetLedgerSelectList(IList<int> parentGroupIdList, int companyId, Boolean IsLegderOnly);

        Task<DataTableResultModel<LedgerModel>> GetLedgerList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterLedgerModel searchFilterModel);

    }
}
