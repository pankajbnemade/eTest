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
    public class LedgerCompanyRelationService : Repository<Ledgercompanyrelation>, ILedgerCompanyRelation
    {
        public LedgerCompanyRelationService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateLedgerCompanyRelation(LedgerCompanyRelationModel ledgerCompanyRelationModel)
        {
            int ledgerCompanyRelationId = 0;

            // assign values.
            Ledgercompanyrelation ledgerCompanyRelation = new Ledgercompanyrelation();

            ledgerCompanyRelation.CompanyId = ledgerCompanyRelationModel.CompanyId;
            ledgerCompanyRelation.LedgerId = ledgerCompanyRelationModel.LedgerId;

            await Create(ledgerCompanyRelation);

            ledgerCompanyRelationId = ledgerCompanyRelation.RelationId;

            return ledgerCompanyRelationId; // returns.
        }

        public async Task<bool> DeleteLedgerCompanyRelation(int ledgerCompanyRelationId)
        {
            bool isDeleted = false;

            // get record.
            Ledgercompanyrelation ledgerCompanyRelation = await GetByIdAsync(w => w.RelationId == ledgerCompanyRelationId);

            if (null != ledgerCompanyRelation)
            {
                isDeleted = await Delete(ledgerCompanyRelation);
            }

            return isDeleted; // returns.
        }

        public async Task<LedgerCompanyRelationModel> GetLedgerCompanyRelationById(int ledgerCompanyRelationId)
        {
            LedgerCompanyRelationModel ledgerCompanyRelationModel = null;

            IList<LedgerCompanyRelationModel> ledgerCompanyRelationModelList = await GetLedgerCompanyRelationList(ledgerCompanyRelationId, 0,0);

            if (null != ledgerCompanyRelationModelList && ledgerCompanyRelationModelList.Any())
            {
                ledgerCompanyRelationModel = ledgerCompanyRelationModelList.FirstOrDefault();
            }

            return ledgerCompanyRelationModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerCompanyRelationModel>> GetLedgerCompanyRelationByLedgerId(int ledgerId)
        {
            DataTableResultModel<LedgerCompanyRelationModel> resultModel = new DataTableResultModel<LedgerCompanyRelationModel>();

            IList<LedgerCompanyRelationModel> ledgerCompanyRelationModelList = await GetLedgerCompanyRelationList(0, 0,ledgerId);

            if (null != ledgerCompanyRelationModelList && ledgerCompanyRelationModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerCompanyRelationModel>();
                resultModel.ResultList = ledgerCompanyRelationModelList;
                resultModel.TotalResultCount = ledgerCompanyRelationModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<LedgerCompanyRelationModel>> GetLedgerCompanyRelationList()
        {
            DataTableResultModel<LedgerCompanyRelationModel> resultModel = new DataTableResultModel<LedgerCompanyRelationModel>();

            IList<LedgerCompanyRelationModel> ledgerCompanyRelationModelList = await GetLedgerCompanyRelationList(0, 0,0);

            if (null != ledgerCompanyRelationModelList && ledgerCompanyRelationModelList.Any())
            {
                resultModel = new DataTableResultModel<LedgerCompanyRelationModel>();
                resultModel.ResultList = ledgerCompanyRelationModelList;
                resultModel.TotalResultCount = ledgerCompanyRelationModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<LedgerCompanyRelationModel>> GetLedgerCompanyRelationList(int ledgerCompanyRelationId, int companyId, int ledgerId)
        {
            IList<LedgerCompanyRelationModel> ledgerCompanyRelationModelList = null;

            // create query.
            IQueryable<Ledgercompanyrelation> query = GetQueryByCondition(w => w.RelationId != 0)
                                                               .Include(w => w.Company).Include(w => w.Ledger).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != ledgerCompanyRelationId)
                query = query.Where(w => w.RelationId == ledgerCompanyRelationId);

            if (0 != companyId)
                query = query.Where(w => w.CompanyId == companyId); 
            
            if (0 != ledgerId)
                query = query.Where(w => w.LedgerId == ledgerId);

            // get records by query.
            List<Ledgercompanyrelation> ledgerCompanyRelationList = await query.ToListAsync();

            if (null != ledgerCompanyRelationList && ledgerCompanyRelationList.Count > 0)
            {
                ledgerCompanyRelationModelList = new List<LedgerCompanyRelationModel>();

                foreach (Ledgercompanyrelation ledgerCompanyRelation in ledgerCompanyRelationList)
                {
                    ledgerCompanyRelationModelList.Add(await AssignValueToModel(ledgerCompanyRelation));
                }
            }

            return ledgerCompanyRelationModelList; // returns.
        }

        private async Task<LedgerCompanyRelationModel> AssignValueToModel(Ledgercompanyrelation ledgerCompanyRelation)
        {
            return await Task.Run(() =>
            {
                LedgerCompanyRelationModel ledgerCompanyRelationModel = new LedgerCompanyRelationModel();

                ledgerCompanyRelationModel.RelationId = ledgerCompanyRelation.RelationId;
                ledgerCompanyRelationModel.CompanyId = ledgerCompanyRelation.CompanyId;
                ledgerCompanyRelationModel.LedgerId = ledgerCompanyRelation.LedgerId;
                ledgerCompanyRelationModel.CompanyName = ledgerCompanyRelation.Company.CompanyName;
                ledgerCompanyRelationModel.LedgerName = ledgerCompanyRelation.Ledger.LedgerName;
                ledgerCompanyRelationModel.PreparedByName = ledgerCompanyRelation.PreparedByUser.UserName;

                return ledgerCompanyRelationModel;
            });
        }
    }
}
