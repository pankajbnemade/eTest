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
    public class ProfitAndLossReportService : IProfitAndLossReport
    {
        private readonly ErpDbContext _dbContext;

        public ProfitAndLossReportService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<ProfitAndLossReportModel>> GetReport(SearchFilterProfitAndLossReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<ProfitAndLossReportModel> profitAndLossReportModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.ReportType, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<ProfitAndLossReportModel> resultModel = new DataTableResultModel<ProfitAndLossReportModel>();

            if (null != profitAndLossReportModelList && profitAndLossReportModelList.Any())
            {
                resultModel = new DataTableResultModel<ProfitAndLossReportModel>();
                resultModel.ResultList = profitAndLossReportModelList;
                resultModel.TotalResultCount = profitAndLossReportModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<ProfitAndLossReportModel>();
                resultModel.ResultList = new List<ProfitAndLossReportModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<ProfitAndLossReportModel>> GetList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {
            IList<ProfitAndLossReportModel> profitAndLossReportModelList = new List<ProfitAndLossReportModel>();

            IList<ProfitAndLossReportModel> profitAndLossReportModelList_Trans = null;

            profitAndLossReportModelList_Trans = await GetTransactionList(fromDate, toDate, profitAndLossType, financialYearId, companyId);

            if (profitAndLossReportModelList_Trans==null)
            {
                profitAndLossReportModelList_Trans= new List<ProfitAndLossReportModel>();
            }

            profitAndLossReportModelList = profitAndLossReportModelList_Trans;

            if (profitAndLossReportModelList.Any())
            {
                profitAndLossReportModelList.Add(new ProfitAndLossReportModel()
                {
                    SequenceNo = 3,
                    SrNo = profitAndLossReportModelList.Max(w => w.SrNo) + 1,
                    ParticularLedgerName_Asset = "Total Amount",
                    ClosingAmount_Asset = profitAndLossReportModelList.Sum(w => w.ClosingAmount_Asset),
                    ParticularLedgerName_Liability = "Total Amount",
                    ClosingAmount_Liability = profitAndLossReportModelList.Sum(w => w.ClosingAmount_Liability),
                });

                return profitAndLossReportModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return profitAndLossReportModelList;
        }

        private async Task<IList<ProfitAndLossReportModel>> GetTransactionList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<ProfitAndLossReportModel> profitAndLossReportModelList = null;

                if (profitAndLossType=="P")
                {

                    profitAndLossReportModelList = _dbContext
                                .Paymentvouchers.Include(i => i.AccountLedger)
                                .Include(i => i.Currency)
                                .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.FinancialYearId == financialYearId
                                    && w.CompanyId == companyId
                                    && w.ChequeDate >= fromDate
                                    && w.ChequeDate <= toDate
                                    )
                                .ToList()
                                .Select(row => new ProfitAndLossReportModel
                                {
                                    SequenceNo = 2,
                                    ParticularLedgerName_Asset = "",//,row.PaymentVoucherId,
                                    ClosingAmount_Asset = row.Amount
                                })
                                .ToList();
                }
                else
                {
                    profitAndLossReportModelList = _dbContext.
                               Receiptvouchers.Include(i => i.AccountLedger)
                               .Include(i => i.Currency)
                               .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                   && w.FinancialYearId == financialYearId
                                   && w.CompanyId == companyId
                                   && w.ChequeDate >= fromDate
                                   && w.ChequeDate <= toDate
                                   )
                               .ToList()
                               .Select((row, index) => new ProfitAndLossReportModel
                               {
                                   SequenceNo = 2,
                                   ParticularLedgerName_Asset = "",//,row.PaymentVoucherId,
                                   ClosingAmount_Asset = row.Amount
                               })
                            .ToList();
                }


                if (profitAndLossReportModelList==null)
                {
                    profitAndLossReportModelList= new List<ProfitAndLossReportModel>();
                }

                profitAndLossReportModelList=profitAndLossReportModelList.Union(profitAndLossReportModelList).ToList();

                return profitAndLossReportModelList; // returns.
            });
        }

    }
}
