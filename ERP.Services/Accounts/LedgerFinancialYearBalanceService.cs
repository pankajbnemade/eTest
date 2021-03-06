using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class LedgerFinancialYearBalanceService : Repository<Ledgerfinancialyearbalance>, ILedgerFinancialYearBalance
    {
        public LedgerFinancialYearBalanceService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateLedgerFinancialYearBalance(LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel)
        {
            int ledgerFinancialYearBalanceId = 0;

            // assign values.
            Ledgerfinancialyearbalance ledgerFinancialYearBalance = new Ledgerfinancialyearbalance();
            ledgerFinancialYearBalance.LedgerId = ledgerFinancialYearBalanceModel.LedgerId;
            ledgerFinancialYearBalance.FinancialYearId = ledgerFinancialYearBalanceModel.FinancialYearId;
            ledgerFinancialYearBalance.CompanyId = ledgerFinancialYearBalanceModel.CompanyId;
            ledgerFinancialYearBalance.CreditAmount = ledgerFinancialYearBalanceModel.CreditAmount;
            ledgerFinancialYearBalance.DebitAmount = ledgerFinancialYearBalanceModel.DebitAmount;
            await Create(ledgerFinancialYearBalance);
            ledgerFinancialYearBalanceId = ledgerFinancialYearBalance.LedgerBalanceId;

            return ledgerFinancialYearBalanceId; // returns.
        }

        public async Task<bool> UpdateLedgerFinancialYearBalance(LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel)
        {
            bool isUpdated = false;

            // get record.
            Ledgerfinancialyearbalance ledgerFinancialYearBalance = await GetByIdAsync(w => w.LedgerBalanceId == ledgerFinancialYearBalanceModel.LedgerBalanceId);

            if (null != ledgerFinancialYearBalance)
            {
                // assign values.
                ledgerFinancialYearBalance.CreditAmount = ledgerFinancialYearBalanceModel.CreditAmount;
                ledgerFinancialYearBalance.DebitAmount = ledgerFinancialYearBalanceModel.DebitAmount;

                isUpdated = await Update(ledgerFinancialYearBalance);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteLedgerFinancialYearBalance(int ledgerFinancialYearBalanceId)
        {
            bool isDeleted = false;

            // get record.
            Ledgerfinancialyearbalance ledgerFinancialYearBalance = await GetByIdAsync(w => w.LedgerBalanceId == ledgerFinancialYearBalanceId);
            if (null != ledgerFinancialYearBalance)
            {
                isDeleted = await Delete(ledgerFinancialYearBalance);
            }

            return isDeleted; // returns.
        }

        public async Task<LedgerFinancialYearBalanceModel> GetLedgerFinancialYearBalanceById(int ledgerFinancialYearBalanceId)
        {
            LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel = null;

            IList<LedgerFinancialYearBalanceModel> ledgerFinancialYearBalanceModelList = await GetLedgerFinancialYearBalanceList(ledgerFinancialYearBalanceId);

            if (null != ledgerFinancialYearBalanceModelList && ledgerFinancialYearBalanceModelList.Any())
            {
                ledgerFinancialYearBalanceModel = ledgerFinancialYearBalanceModelList.FirstOrDefault();
            }

            return ledgerFinancialYearBalanceModel; // returns.
        }

        public async Task<LedgerFinancialYearBalanceModel> GetLedgerFinancialYearBalance(int ledgerId, int companyId, int financialYearId)
        {
            LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel = null;

            IQueryable<Ledgerfinancialyearbalance> query = GetQueryByCondition(w => w.LedgerBalanceId != 0)
                                                               .Include(w => w.Ledger).Include(w => w.FinancialYear)
                                                               .Include(w => w.Company)
                                                               .Include(w => w.PreparedByUser);

            // apply filters.
            query = query.Where(w => w.LedgerId == ledgerId);
            query = query.Where(w => w.CompanyId == companyId);
            query = query.Where(w => w.FinancialYearId == financialYearId);

            // get records by query.
            Ledgerfinancialyearbalance ledgerFinancialYearBalance = await query.FirstOrDefaultAsync();

            if (null != ledgerFinancialYearBalance)
            {
                ledgerFinancialYearBalanceModel = await AssignValueToModel(ledgerFinancialYearBalance);
            }

            return ledgerFinancialYearBalanceModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerFinancialYearBalanceModel>> GetLedgerFinancialYearBalanceList()
        {
            DataTableResultModel<LedgerFinancialYearBalanceModel> resultModel = new DataTableResultModel<LedgerFinancialYearBalanceModel>();

            IList<LedgerFinancialYearBalanceModel> ledgerFinancialYearBalanceModelList = await GetLedgerFinancialYearBalanceList(0);
            if (null != ledgerFinancialYearBalanceModelList && ledgerFinancialYearBalanceModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerFinancialYearBalanceModel>();
                resultModel.ResultList = ledgerFinancialYearBalanceModelList;
                resultModel.TotalResultCount = ledgerFinancialYearBalanceModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<LedgerFinancialYearBalanceModel>();
                resultModel.ResultList = new List<LedgerFinancialYearBalanceModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<LedgerFinancialYearBalanceModel>> GetLedgerFinancialYearBalanceList(int ledgerFinancialYearBalanceId)
        {
            IList<LedgerFinancialYearBalanceModel> ledgerFinancialYearBalanceModelList = null;

            // create query.
            IQueryable<Ledgerfinancialyearbalance> query = GetQueryByCondition(w => w.LedgerBalanceId != 0)
                                                                .Include(w => w.Ledger).Include(w => w.FinancialYear)
                                                                .Include(w => w.Company)
                                                                .Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != ledgerFinancialYearBalanceId)
                query = query.Where(w => w.LedgerBalanceId == ledgerFinancialYearBalanceId);

            // get records by query.
            List<Ledgerfinancialyearbalance> ledgerFinancialYearBalanceList = await query.ToListAsync();
            if (null != ledgerFinancialYearBalanceList && ledgerFinancialYearBalanceList.Count > 0)
            {
                ledgerFinancialYearBalanceModelList = new List<LedgerFinancialYearBalanceModel>();
                foreach (Ledgerfinancialyearbalance ledgerFinancialYearBalance in ledgerFinancialYearBalanceList)
                {
                    ledgerFinancialYearBalanceModelList.Add(await AssignValueToModel(ledgerFinancialYearBalance));
                }
            }

            return ledgerFinancialYearBalanceModelList; // returns.
        }

        private async Task<LedgerFinancialYearBalanceModel> AssignValueToModel(Ledgerfinancialyearbalance ledgerFinancialYearBalance)
        {
            return await Task.Run(() =>
            {
                LedgerFinancialYearBalanceModel ledgerFinancialYearBalanceModel = new LedgerFinancialYearBalanceModel();
                ledgerFinancialYearBalanceModel.LedgerBalanceId = ledgerFinancialYearBalance.LedgerBalanceId;
                ledgerFinancialYearBalanceModel.LedgerId = ledgerFinancialYearBalance.LedgerId;
                ledgerFinancialYearBalanceModel.FinancialYearId = ledgerFinancialYearBalance.FinancialYearId;
                ledgerFinancialYearBalanceModel.CompanyId = ledgerFinancialYearBalance.CompanyId;
                ledgerFinancialYearBalanceModel.CreditAmount = ledgerFinancialYearBalance.CreditAmount;
                ledgerFinancialYearBalanceModel.DebitAmount = ledgerFinancialYearBalance.DebitAmount;

                //#####
                ledgerFinancialYearBalanceModel.LedgerName = ledgerFinancialYearBalance.Ledger.LedgerName;
                ledgerFinancialYearBalanceModel.FinancialYearName = ledgerFinancialYearBalance.FinancialYear.FinancialYearName;
                ledgerFinancialYearBalanceModel.CompanyName = ledgerFinancialYearBalance.Company.CompanyName;
                ledgerFinancialYearBalanceModel.PreparedByName = ledgerFinancialYearBalance.PreparedByUser.UserName;

                return ledgerFinancialYearBalanceModel;
            });
        }
    }
}
