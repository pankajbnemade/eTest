using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedgerFinancialYearBalance : IRepository<Ledgerfinancialyearbalance>
    {
        Task<int> CreateLedgerFinancialYearBalance(LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel);
       
        Task<bool> UpdateLedgerFinancialYearBalance(LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel);
      
        Task<bool> DeleteLedgerFinancialYearBalance(int ledgerFinancialYearBalanceId);
        
        Task<LedgerFinancialYearBalanceModel> GetLedgerFinancialYearBalanceById(int ledgerFinancialYearBalanceId);

        Task<LedgerFinancialYearBalanceModel> GetLedgerFinancialYearBalance(int ledgerId, int companyId, int financialYearId);

        Task<DataTableResultModel<LedgerFinancialYearBalanceModel>> GetLedgerFinancialYearBalanceList();
    }
}
