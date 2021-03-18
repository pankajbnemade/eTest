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
    public class FinancialYearCompanyRelationService : Repository<Financialyearcompanyrelation>, IFinancialYearCompanyRelation
    {
        public FinancialYearCompanyRelationService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateFinancialYearCompanyRelation(FinancialYearCompanyRelationModel financialYearCompanyRelationModel)
        {
            int financialYearCompanyRelationId = 0;

            // assign values.
            Financialyearcompanyrelation financialYearCompanyRelation = new Financialyearcompanyrelation();

            //financialYearCompanyRelation.FinancialYearCompanyRelationName = financialYearCompanyRelationModel.FinancialYearCompanyRelationName;

            financialYearCompanyRelationId = await Create(financialYearCompanyRelation);

            return financialYearCompanyRelationId; // returns.
        }


        public async Task<bool> UpdateFinancialYearCompanyRelation(FinancialYearCompanyRelationModel financialYearCompanyRelationModel)
        {
            bool isUpdated = false;

            // get record.
            Financialyearcompanyrelation financialYearCompanyRelation = await GetByIdAsync(w => w.RelationId == financialYearCompanyRelationModel.RelationId);

            if (null != financialYearCompanyRelation)
            {
                // assign values.
                //financialYearCompanyRelation.FinancialYearCompanyRelationName = financialYearCompanyRelationModel.FinancialYearCompanyRelationName;

                isUpdated = await Update(financialYearCompanyRelation);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteFinancialYearCompanyRelation(int financialYearCompanyRelationId)
        {
            bool isDeleted = false;

            // get record.
            Financialyearcompanyrelation financialYearCompanyRelation = await GetByIdAsync(w => w.RelationId == financialYearCompanyRelationId);

            if (null != financialYearCompanyRelation)
            {
                isDeleted = await Delete(financialYearCompanyRelation);
            }

            return isDeleted; // returns.
        }


        public async Task<FinancialYearCompanyRelationModel> GetFinancialYearCompanyRelationById(int financialYearCompanyRelationId)
        {
            FinancialYearCompanyRelationModel financialYearCompanyRelationModel = null;

            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = await GetFinancialYearCompanyRelationList(financialYearCompanyRelationId);

            if (null != financialYearCompanyRelationModelList && financialYearCompanyRelationModelList.Any())
            {
                financialYearCompanyRelationModel = financialYearCompanyRelationModelList.FirstOrDefault();
            }

            return financialYearCompanyRelationModel; // returns.
        }


        public async Task<IList<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationByStateId(int stateId)
        {
            return await GetFinancialYearCompanyRelationList(0);
        }


        public async Task<DataTableResultModel<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationList()
        {
            DataTableResultModel<FinancialYearCompanyRelationModel> resultModel = new DataTableResultModel<FinancialYearCompanyRelationModel>();

            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = await GetFinancialYearCompanyRelationList(0);

            if (null != financialYearCompanyRelationModelList && financialYearCompanyRelationModelList.Any())
            {
                resultModel = new DataTableResultModel<FinancialYearCompanyRelationModel>();
                resultModel.ResultList = financialYearCompanyRelationModelList;
                resultModel.TotalResultCount = financialYearCompanyRelationModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationList(int financialYearCompanyRelationId)
        {
            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = null;

            // create query.
            IQueryable<Financialyearcompanyrelation> query = GetQueryByCondition(w => w.RelationId != 0);

            // apply filters.
            if (0 != financialYearCompanyRelationId)
                query = query.Where(w => w.RelationId == financialYearCompanyRelationId);

          

            // get records by query.
            List<Financialyearcompanyrelation> financialYearCompanyRelationList = await query.ToListAsync();
            if (null != financialYearCompanyRelationList && financialYearCompanyRelationList.Count > 0)
            {
                financialYearCompanyRelationModelList = new List<FinancialYearCompanyRelationModel>();

                foreach (Financialyearcompanyrelation financialYearCompanyRelation in financialYearCompanyRelationList)
                {
                    financialYearCompanyRelationModelList.Add(await AssignValueToModel(financialYearCompanyRelation));
                }
            }

            return financialYearCompanyRelationModelList; // returns.
        }

        private async Task<FinancialYearCompanyRelationModel> AssignValueToModel(Financialyearcompanyrelation financialYearCompanyRelation)
        {
            return await Task.Run(() =>
            {
                FinancialYearCompanyRelationModel financialYearCompanyRelationModel = new FinancialYearCompanyRelationModel();

                //financialYearCompanyRelationModel.RelationId = financialYearCompanyRelation.RelationId;
                //financialYearCompanyRelationModel.FinancialYearCompanyRelationName = financialYearCompanyRelation.FinancialYearCompanyRelationName;

                return financialYearCompanyRelationModel;
            });
        }
    }
}
