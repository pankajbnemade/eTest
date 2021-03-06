using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class FinancialYearService : Repository<Financialyear>, IFinancialYear
    {
        private readonly ErpDbContext _dbContext;

        public FinancialYearService(ErpDbContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }

        public async Task<int> CreateFinancialYear(FinancialYearModel financialYearModel)
        {
            int financialYearId = 0;

            // assign values.
            Financialyear financialYear = new Financialyear();

            financialYear.FinancialYearName = financialYearModel.FinancialYearName;
            financialYear.FromDate = financialYearModel.FromDate;
            financialYear.ToDate = financialYearModel.ToDate;

            await Create(financialYear);

            financialYearId = financialYear.FinancialYearId;

            //---------------------------------

            IList<Company> companyList = _dbContext.Companies.ToList();

            Financialyearcompanyrelation financialYearCompanyRelation;

            foreach (Company company in companyList)
            {
                financialYearCompanyRelation = new Financialyearcompanyrelation()
                {
                    CompanyId = company.CompanyId,
                    FinancialYearId = financialYearId,
                };

                _dbContext.Financialyearcompanyrelations.Add(financialYearCompanyRelation);
            }

            await _dbContext.SaveChangesAsync();

            return financialYearId; // returns.
        }

        public async Task<bool> UpdateFinancialYear(FinancialYearModel financialYearModel)
        {
            bool isUpdated = false;

            // get record.
            Financialyear financialYear = await GetByIdAsync(w => w.FinancialYearId == financialYearModel.FinancialYearId);
            if (null != financialYear)
            {
                // assign values.
                financialYear.FinancialYearName = financialYearModel.FinancialYearName;
                financialYear.FromDate = financialYearModel.FromDate;
                financialYear.ToDate = financialYearModel.ToDate;
                isUpdated = await Update(financialYear);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteFinancialYear(int financialYearId)
        {
            bool isDeleted = false;

            // get record.
            Financialyear financialYear = await GetByIdAsync(w => w.FinancialYearId == financialYearId);

            if (null != financialYear)
            {
                isDeleted = await Delete(financialYear);
            }

            return isDeleted; // returns.
        }

        public async Task<FinancialYearModel> GetFinancialYearById(int financialYearId)
        {
            FinancialYearModel financialYearModel = null;

            IList<FinancialYearModel> financialYearModelList = await GetFinancialYearList(financialYearId);
            if (null != financialYearModelList && financialYearModelList.Any())
            {
                financialYearModel = financialYearModelList.FirstOrDefault();
            }

            return financialYearModel; // returns.
        }

        public async Task<DataTableResultModel<FinancialYearModel>> GetFinancialYearList()
        {
            DataTableResultModel<FinancialYearModel> resultModel = new DataTableResultModel<FinancialYearModel>();

            IList<FinancialYearModel> financialYearModelList = await GetFinancialYearList(0);
            if (null != financialYearModelList && financialYearModelList.Any())
            {
                resultModel = new DataTableResultModel<FinancialYearModel>();
                resultModel.ResultList = financialYearModelList;
                resultModel.TotalResultCount = financialYearModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<FinancialYearModel>();
                resultModel.ResultList = new List<FinancialYearModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<FinancialYearModel>> GetFinancialYearList(int financialYearId)
        {
            IList<FinancialYearModel> financialYearModelList = null;

            // create query.
            IQueryable<Financialyear> query = GetQueryByCondition(w => w.FinancialYearId != 0)
                                                   .Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != financialYearId)
                query = query.Where(w => w.FinancialYearId == financialYearId);

            // get records by query.
            List<Financialyear> financialYearList = await query.ToListAsync();
            if (null != financialYearList && financialYearList.Count > 0)
            {
                financialYearModelList = new List<FinancialYearModel>();

                foreach (Financialyear financialYear in financialYearList)
                {
                    financialYearModelList.Add(await AssignValueToModel(financialYear));
                }
            }

            return financialYearModelList; // returns.
        }

        private async Task<FinancialYearModel> AssignValueToModel(Financialyear financialYear)
        {
            return await Task.Run(() =>
            {
                FinancialYearModel financialYearModel = new FinancialYearModel();

                financialYearModel.FinancialYearId = financialYear.FinancialYearId;
                financialYearModel.FinancialYearName = financialYear.FinancialYearName;
                financialYearModel.FromDate = (DateTime)financialYear.FromDate;
                financialYearModel.ToDate = (DateTime)financialYear.ToDate;
                financialYearModel.PreparedByName = null != financialYear.PreparedByUser ? financialYear.PreparedByUser.UserName : null;

                return financialYearModel;
            });
        }

        public async Task<FinancialYearModel> GetFinancialYearByDateNCompanyId(int companyId, DateTime date)
        {

            FinancialYearModel financialYearModel = new FinancialYearModel();

            // create query.
            IQueryable<Financialyear> query = GetQueryByCondition(w => w.FinancialYearId != 0)
                                                   .Include(w => w.Financialyearcompanyrelations);

            query = query.Where(w => EF.Functions.DateDiffDay(w.FromDate, date) >= 0 && EF.Functions.DateDiffDay(date, w.ToDate) >= 0);

            query = query.Where(w => w.Financialyearcompanyrelations.Any(c => c.CompanyId == companyId));

            // get records by query.
            Financialyear financialYear = await query.FirstOrDefaultAsync();

            if (null != financialYear)
            {
                {
                    financialYearModel = await AssignValueToModel(financialYear);
                }
            }

            return financialYearModel; // returns.
        }


        public async Task<IList<SelectListModel>> GetFinancialYearSelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.FinancialYearId != 0))
            {
                IQueryable<Financialyear> query = GetQueryByCondition(w => w.FinancialYearId != 0);
                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.FinancialYearName,
                    Value = s.FinancialYearId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();
            }

            return resultModel; // returns.
        }
    }
}
