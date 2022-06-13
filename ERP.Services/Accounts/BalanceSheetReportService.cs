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
    public class BalanceSheetReportService : IBalanceSheetReport
    {
        private readonly ErpDbContext _dbContext;

        public BalanceSheetReportService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<BalanceSheetReportModel>> GetReport(SearchFilterBalanceSheetReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<BalanceSheetReportModel> balanceSheetReportModelList = await GetList(searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.ReportType, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<BalanceSheetReportModel> resultModel = new DataTableResultModel<BalanceSheetReportModel>();

            if (null != balanceSheetReportModelList && balanceSheetReportModelList.Any())
            {
                resultModel = new DataTableResultModel<BalanceSheetReportModel>();
                resultModel.ResultList = balanceSheetReportModelList;
                resultModel.TotalResultCount = balanceSheetReportModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<BalanceSheetReportModel>();
                resultModel.ResultList = new List<BalanceSheetReportModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<BalanceSheetReportModel>> GetList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {
            IList<BalanceSheetReportModel> balanceSheetReportModelList = new List<BalanceSheetReportModel>();

            IList<BalanceSheetReportModel> balanceSheetReportModelList_Trans = null;

            balanceSheetReportModelList_Trans = await GetTransactionList(fromDate, toDate, profitAndLossType, financialYearId, companyId);

            if (balanceSheetReportModelList_Trans==null)
            {
                balanceSheetReportModelList_Trans= new List<BalanceSheetReportModel>();
            }

            balanceSheetReportModelList = balanceSheetReportModelList_Trans;

            if (balanceSheetReportModelList.Any())
            {
                balanceSheetReportModelList.Add(new BalanceSheetReportModel()
                {
                    SequenceNo = 3,
                    SrNo = balanceSheetReportModelList.Max(w => w.SrNo) + 1,
                    ParticularLedgerName_Asset = "Total Amount",
                    ClosingAmount_Asset = balanceSheetReportModelList.Sum(w => w.ClosingAmount_Asset),
                    ParticularLedgerName_Liability = "Total Amount",
                    ClosingAmount_Liability = balanceSheetReportModelList.Sum(w => w.ClosingAmount_Liability),
                });

                return balanceSheetReportModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return balanceSheetReportModelList;
        }

        private async Task<IList<BalanceSheetReportModel>> GetTransactionList(DateTime fromDate, DateTime toDate, string profitAndLossType, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<BalanceSheetReportModel> balanceSheetReportModelList = null;

                if (profitAndLossType=="P")
                {

                    balanceSheetReportModelList = _dbContext
                                .Paymentvouchers.Include(i => i.AccountLedger)
                                .Include(i => i.Currency)
                                .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.FinancialYearId == financialYearId
                                    && w.CompanyId == companyId
                                    && w.ChequeDate >= fromDate
                                    && w.ChequeDate <= toDate
                                    )
                                .ToList()
                                .Select(row => new BalanceSheetReportModel
                                {
                                    SequenceNo = 2,
                                    ParticularLedgerName_Asset = "",//,row.PaymentVoucherId,
                                    ClosingAmount_Asset = row.Amount
                                })
                                .ToList();
                }
                else
                {
                    balanceSheetReportModelList = _dbContext.
                               Receiptvouchers.Include(i => i.AccountLedger)
                               .Include(i => i.Currency)
                               .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                   && w.FinancialYearId == financialYearId
                                   && w.CompanyId == companyId
                                   && w.ChequeDate >= fromDate
                                   && w.ChequeDate <= toDate
                                   )
                               .ToList()
                               .Select((row, index) => new BalanceSheetReportModel
                               {
                                   SequenceNo = 2,
                                   ParticularLedgerName_Asset = "",//,row.PaymentVoucherId,
                                   ClosingAmount_Asset = row.Amount
                               })
                            .ToList();
                }


                if (balanceSheetReportModelList==null)
                {
                    balanceSheetReportModelList= new List<BalanceSheetReportModel>();
                }

                balanceSheetReportModelList=balanceSheetReportModelList.Union(balanceSheetReportModelList).ToList();

                return balanceSheetReportModelList; // returns.
            });
        }

    }
}
