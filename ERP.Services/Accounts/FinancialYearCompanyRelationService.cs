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
            financialYearCompanyRelation.CompanyId = financialYearCompanyRelationModel.CompanyId;
            financialYearCompanyRelation.FinancialYearId = financialYearCompanyRelationModel.FinancialYearId;
            await Create(financialYearCompanyRelation);
            financialYearCompanyRelationId = financialYearCompanyRelation.RelationId;

            return financialYearCompanyRelationId; // returns.
        }

        //public async Task<bool> UpdateFinancialYearCompanyRelation(FinancialYearCompanyRelationModel financialYearCompanyRelationModel)
        //{
        //    bool isUpdated = false;

        //    // get record.
        //    Financialyearcompanyrelation financialYearCompanyRelation = await GetByIdAsync(w => w.RelationId == financialYearCompanyRelationModel.RelationId);

        //    if (null != financialYearCompanyRelation)
        //    {
        //        // assign values.
        //        //financialYearCompanyRelation.FinancialYearCompanyRelationName = financialYearCompanyRelationModel.FinancialYearCompanyRelationName;

        //        isUpdated = await Update(financialYearCompanyRelation);
        //    }

        //    return isUpdated; // returns.
        //}

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

            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = await GetFinancialYearCompanyRelationList(financialYearCompanyRelationId, 0);
            if (null != financialYearCompanyRelationModelList && financialYearCompanyRelationModelList.Any())
            {
                financialYearCompanyRelationModel = financialYearCompanyRelationModelList.FirstOrDefault();
            }

            return financialYearCompanyRelationModel; // returns.
        }

        public async Task<DataTableResultModel<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationByFinancialYearId(int financialYearId)
        {
            DataTableResultModel<FinancialYearCompanyRelationModel> resultModel = new DataTableResultModel<FinancialYearCompanyRelationModel>();

            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = await GetFinancialYearCompanyRelationList(0, financialYearId);
            if (null != financialYearCompanyRelationModelList && financialYearCompanyRelationModelList.Any())
            {
                resultModel = new DataTableResultModel<FinancialYearCompanyRelationModel>();
                resultModel.ResultList = financialYearCompanyRelationModelList;
                resultModel.TotalResultCount = financialYearCompanyRelationModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationList()
        {
            DataTableResultModel<FinancialYearCompanyRelationModel> resultModel = new DataTableResultModel<FinancialYearCompanyRelationModel>();

            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = await GetFinancialYearCompanyRelationList(0, 0);
            if (null != financialYearCompanyRelationModelList && financialYearCompanyRelationModelList.Any())
            {
                resultModel = new DataTableResultModel<FinancialYearCompanyRelationModel>();
                resultModel.ResultList = financialYearCompanyRelationModelList;
                resultModel.TotalResultCount = financialYearCompanyRelationModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<FinancialYearCompanyRelationModel>> GetFinancialYearCompanyRelationList(int financialYearCompanyRelationId, int financialYearId)
        {
            IList<FinancialYearCompanyRelationModel> financialYearCompanyRelationModelList = null;

            // create query.
            IQueryable<Financialyearcompanyrelation> query = GetQueryByCondition(w => w.RelationId != 0)
                                                               .Include(w => w.Company).Include(w => w.FinancialYear).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != financialYearCompanyRelationId)
                query = query.Where(w => w.RelationId == financialYearCompanyRelationId);

            // apply filters.
            if (0 != financialYearId)
                query = query.Where(w => w.FinancialYearId == financialYearId);

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
                financialYearCompanyRelationModel.RelationId = financialYearCompanyRelation.RelationId;
                financialYearCompanyRelationModel.CompanyId = financialYearCompanyRelation.CompanyId;
                financialYearCompanyRelationModel.FinancialYearId = financialYearCompanyRelation.FinancialYearId;
                financialYearCompanyRelationModel.CompanyName = financialYearCompanyRelation.Company.CompanyName;
                financialYearCompanyRelationModel.FinancialYearName = financialYearCompanyRelation.FinancialYear.FinancialYearName;
                financialYearCompanyRelationModel.PreparedByName = financialYearCompanyRelation.PreparedByUser.UserName;

                return financialYearCompanyRelationModel;
            });
        }
    }
}
