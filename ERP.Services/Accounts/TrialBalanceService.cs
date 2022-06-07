using ERP.DataAccess.EntityData;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TrialBalanceReportService : ITrialBalanceReport
    {
        ErpDbContext dbContext;

        public TrialBalanceReportService(ErpDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<TrialBalanceReportModel>> GetReport(SearchFilterTrialBalanceReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<TrialBalanceReportModel> trialBalanceReportModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.ReportType, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<TrialBalanceReportModel> resultModel = new DataTableResultModel<TrialBalanceReportModel>();

            if (null != trialBalanceReportModelList && trialBalanceReportModelList.Any())
            {
                resultModel = new DataTableResultModel<TrialBalanceReportModel>();
                resultModel.ResultList = trialBalanceReportModelList;
                resultModel.TotalResultCount = trialBalanceReportModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<TrialBalanceReportModel>();
                resultModel.ResultList = new List<TrialBalanceReportModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<TrialBalanceReportModel>> GetList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {
            IList<TrialBalanceReportModel> trialBalanceReportModelList = new List<TrialBalanceReportModel>();

            IList<TrialBalanceReportModel> trialBalanceReportModelList_Trans = null;

            trialBalanceReportModelList_Trans = await GetTransactionList(fromDate, toDate, profitAndLossType, financialYearId, companyId);

            if (trialBalanceReportModelList_Trans==null)
            {
                trialBalanceReportModelList_Trans= new List<TrialBalanceReportModel>();
            }

            trialBalanceReportModelList = trialBalanceReportModelList_Trans;

            if (trialBalanceReportModelList.Any())
            {
                trialBalanceReportModelList.Add(new TrialBalanceReportModel()
                {
                    SequenceNo = 3,
                    SrNo = trialBalanceReportModelList.Max(w => w.SrNo) + 1,
                    ParticularLedgerName_Asset = "Total Amount",
                    ClosingAmount_Asset = trialBalanceReportModelList.Sum(w => w.ClosingAmount_Asset),
                    ParticularLedgerName_Liability = "Total Amount",
                    ClosingAmount_Liability = trialBalanceReportModelList.Sum(w => w.ClosingAmount_Liability),
                });

                return trialBalanceReportModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return trialBalanceReportModelList;
        }

        private async Task<IList<TrialBalanceReportModel>> GetTransactionList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<TrialBalanceReportModel> trialBalanceReportModelList = null;

                if (profitAndLossType=="P")
                {

                    trialBalanceReportModelList = dbContext
                                .Paymentvouchers.Include(i => i.AccountLedger)
                                .Include(i => i.Currency)
                                .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.FinancialYearId == financialYearId
                                    && w.CompanyId == companyId
                                    && w.ChequeDate >= fromDate
                                    && w.ChequeDate <= toDate
                                    )
                                .ToList()
                                .Select(row => new TrialBalanceReportModel
                                {
                                    SequenceNo = 2,
                                    ParticularLedgerName_Asset = "",//,row.PaymentVoucherId,
                                    ClosingAmount_Asset = row.Amount
                                })
                                .ToList();
                }
                else
                {
                    trialBalanceReportModelList = dbContext.
                               Receiptvouchers.Include(i => i.AccountLedger)
                               .Include(i => i.Currency)
                               .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                   && w.FinancialYearId == financialYearId
                                   && w.CompanyId == companyId
                                   && w.ChequeDate >= fromDate
                                   && w.ChequeDate <= toDate
                                   )
                               .ToList()
                               .Select((row, index) => new TrialBalanceReportModel
                               {
                                   SequenceNo = 2,
                                   ParticularLedgerName_Asset = "",//,row.PaymentVoucherId,
                                   ClosingAmount_Asset = row.Amount
                               })
                            .ToList();
                }


                if (trialBalanceReportModelList==null)
                {
                    trialBalanceReportModelList= new List<TrialBalanceReportModel>();
                }

                trialBalanceReportModelList=trialBalanceReportModelList.Union(trialBalanceReportModelList).ToList();

                return trialBalanceReportModelList; // returns.
            });
        }

    }
}
