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
    public class LedgerService : Repository<Ledger>, ILedger
    {
        public LedgerService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateLedger(LedgerModel ledgerModel)
        {
            int ledgerId = 0;

            // assign values.
            Ledger ledger = new Ledger();

            //ledger.LedgerName = ledgerModel.LedgerName;

            ledgerId = await Create(ledger);

            return ledgerId; // returns.
        }


        public async Task<bool> UpdateLedger(LedgerModel ledgerModel)
        {
            bool isUpdated = false;

            // get record.
            Ledger ledger = await GetByIdAsync(w => w.LedgerId == ledgerModel.LedgerId);

            if (null != ledger)
            {
                // assign values.
                //ledger.LedgerName = ledgerModel.LedgerName;

                isUpdated = await Update(ledger);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteLedger(int ledgerId)
        {
            bool isDeleted = false;

            // get record.
            Ledger ledger = await GetByIdAsync(w => w.LedgerId == ledgerId);

            if (null != ledger)
            {
                isDeleted = await Delete(ledger);
            }

            return isDeleted; // returns.
        }


        public async Task<LedgerModel> GetLedgerById(int ledgerId)
        {
            LedgerModel ledgerModel = null;

            IList<LedgerModel> ledgerModelList = await GetLedgerList(ledgerId);

            if (null != ledgerModelList && ledgerModelList.Any())
            {
                ledgerModel = ledgerModelList.FirstOrDefault();
            }

            return ledgerModel; // returns.
        }


        public async Task<IList<LedgerModel>> GetLedgerByStateId(int stateId)
        {
            return await GetLedgerList(0);
        }


        public async Task<DataTableResultModel<LedgerModel>> GetLedgerList()
        {
            DataTableResultModel<LedgerModel> resultModel = new DataTableResultModel<LedgerModel>();

            IList<LedgerModel> ledgerModelList = await GetLedgerList(0);

            if (null != ledgerModelList && ledgerModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerModel>();
                resultModel.ResultList = ledgerModelList;
                resultModel.TotalResultCount = ledgerModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<LedgerModel>> GetLedgerList(int ledgerId)
        {
            IList<LedgerModel> ledgerModelList = null;

            // create query.
            IQueryable<Ledger> query = GetQueryByCondition(w => w.LedgerId != 0);

            // apply filters.
            if (0 != ledgerId)
                query = query.Where(w => w.LedgerId == ledgerId);

          

            // get records by query.
            List<Ledger> ledgerList = await query.ToListAsync();
            if (null != ledgerList && ledgerList.Count > 0)
            {
                ledgerModelList = new List<LedgerModel>();

                foreach (Ledger ledger in ledgerList)
                {
                    ledgerModelList.Add(await AssignValueToModel(ledger));
                }
            }

            return ledgerModelList; // returns.
        }

        private async Task<LedgerModel> AssignValueToModel(Ledger ledger)
        {
            return await Task.Run(() =>
            {
                LedgerModel ledgerModel = new LedgerModel();

                //ledgerModel.LedgerId = ledger.LedgerId;
                //ledgerModel.LedgerName = ledger.LedgerName;

                return ledgerModel;
            });
        }
    }
}
