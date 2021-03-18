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

            //ledgerFinancialYearBalance.LedgerFinancialYearBalanceName = ledgerFinancialYearBalanceModel.LedgerFinancialYearBalanceName;

            ledgerFinancialYearBalanceId = await Create(ledgerFinancialYearBalance);

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
                //ledgerFinancialYearBalance.LedgerFinancialYearBalanceName = ledgerFinancialYearBalanceModel.LedgerFinancialYearBalanceName;

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


        public async Task<IList<LedgerFinancialYearBalanceModel>> GetLedgerFinancialYearBalanceByStateId(int stateId)
        {
            return await GetLedgerFinancialYearBalanceList(0);
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

            return resultModel; // returns.
        }

        private async Task<IList<LedgerFinancialYearBalanceModel>> GetLedgerFinancialYearBalanceList(int ledgerFinancialYearBalanceId)
        {
            IList<LedgerFinancialYearBalanceModel> ledgerFinancialYearBalanceModelList = null;

            // create query.
            IQueryable<Ledgerfinancialyearbalance> query = GetQueryByCondition(w => w.LedgerBalanceId != 0);

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

                //ledgerFinancialYearBalanceModel.Id = ledgerFinancialYearBalance.Id;
                //ledgerFinancialYearBalanceModel.LedgerFinancialYearBalanceName = ledgerFinancialYearBalance.LedgerFinancialYearBalanceName;

                return ledgerFinancialYearBalanceModel;
            });
        }
    }
}
