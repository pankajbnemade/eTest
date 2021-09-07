using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class FinancialYearService : Repository<Financialyear>, IFinancialYear
    {
        public FinancialYearService(ErpDbContext dbContext) : base(dbContext) { }

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
                financialYearModel.FromDate = financialYear.FromDate;
                financialYearModel.ToDate = financialYear.ToDate;
                financialYearModel.PreparedByName = null != financialYear.PreparedByUser ? financialYear.PreparedByUser.UserName : null;

                return financialYearModel;
            });
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
