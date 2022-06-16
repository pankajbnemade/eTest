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
    public class PDCReportService : IPDCReport
    {
        private readonly ErpDbContext _dbContext;

        public PDCReportService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<PDCReportModel>> GetReport(SearchFilterPDCReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            //if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            int ledgerId;

            ledgerId=(int)(searchFilterModel.LedgerId==null ? 0 : searchFilterModel.LedgerId);

            IList<PDCReportModel> pdcReportModelList = await GetList(ledgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.PDCType, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<PDCReportModel> resultModel = new DataTableResultModel<PDCReportModel>();

            if (null != pdcReportModelList && pdcReportModelList.Any())
            {
                resultModel = new DataTableResultModel<PDCReportModel>();
                resultModel.ResultList = pdcReportModelList;
                resultModel.TotalResultCount = pdcReportModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PDCReportModel>();
                resultModel.ResultList = new List<PDCReportModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<PDCReportModel>> GetList(int ledgerId, DateTime fromDate, DateTime toDate, string pdcType, int financialYearId, int companyId)
        {
            IList<PDCReportModel> pdcReportModelList = new List<PDCReportModel>();

            IList<PDCReportModel> pdcReportModelList_Trans = null;

            pdcReportModelList_Trans = await GetTransactionList(ledgerId, fromDate, toDate, pdcType, financialYearId, companyId);

            if (pdcReportModelList_Trans==null)
            {
                pdcReportModelList_Trans= new List<PDCReportModel>();
            }

            pdcReportModelList = pdcReportModelList_Trans;

            if (pdcReportModelList.Any())
            {
                pdcReportModelList.Add(new PDCReportModel()
                {
                    SequenceNo = 3,
                    SrNo = pdcReportModelList.Max(w => w.SrNo) + 1,
                    DocumentNo = "Total Amount",
                    //DocumentDate = toDate,
                    Amount = pdcReportModelList.Sum(w => w.Amount),
                });

                return pdcReportModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return pdcReportModelList;
        }

        private async Task<IList<PDCReportModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, string pdcType, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<PDCReportModel> pdcReportModelList = null;

                if (pdcType=="P")
                {
                    pdcReportModelList = _dbContext
                                .Paymentvouchers.Include(i => i.AccountLedger)
                                .Include(i => i.Currency)
                                .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.TypeCorB == TypeCorB.B.ToString()
                                    && w.PaymentTypeId == (int)PaymentType.PDC
                                    && w.IsPdcprocessed == 0
                                    && w.FinancialYearId == financialYearId
                                    && w.CompanyId == companyId
                                    && w.ChequeDate >= fromDate
                                    && w.ChequeDate <= toDate
                                    )
                                .ToList()
                                .Select(row => new PDCReportModel
                                {
                                    SequenceNo = 2,
                                    DocumentId = row.PaymentVoucherId,
                                    DocumentNo = row.VoucherNo,
                                    DocumentDate = row.VoucherDate,
                                    ChequeNo = row.ChequeNo,
                                    ChequeDate = row.ChequeDate,
                                    BankName = (null != row.AccountLedger ? row.AccountLedger.LedgerName : ""),
                                    CurrencyId = row.CurrencyId,
                                    CurrencyCode = (null != row.Currency ? row.Currency.CurrencyCode : ""),
                                    ExchangeRate = row.ExchangeRate,
                                    Narration = row.Narration,
                                    Amount_FC = row.AmountFc,
                                    Amount = row.Amount
                                })
                                .ToList();
                }
                else
                {
                    pdcReportModelList = _dbContext.
                               Receiptvouchers.Include(i => i.AccountLedger)
                               .Include(i => i.Currency)
                               .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.TypeCorB == TypeCorB.B.ToString()
                                    && w.PaymentTypeId == (int)PaymentType.PDC
                                    && w.IsPdcprocessed == 0
                                    && w.FinancialYearId == financialYearId
                                    && w.CompanyId == companyId
                                    && w.ChequeDate >= fromDate
                                    && w.ChequeDate <= toDate
                                   )
                               .ToList()
                               .Select((row, index) => new PDCReportModel
                               {
                                   SequenceNo = 2,
                                   DocumentId = row.ReceiptVoucherId,
                                   DocumentNo = row.VoucherNo,
                                   DocumentDate = row.VoucherDate,
                                   ChequeNo = row.ChequeNo,
                                   ChequeDate = row.ChequeDate,
                                   BankName = (null != row.AccountLedger ? row.AccountLedger.LedgerName : ""),
                                   CurrencyId = row.CurrencyId,
                                   CurrencyCode = (null != row.Currency ? row.Currency.CurrencyCode : ""),
                                   ExchangeRate = row.ExchangeRate,
                                   Narration = row.Narration,
                                   Amount_FC = row.AmountFc,
                                   Amount = row.Amount
                               })
                            .ToList();
                }

                if (pdcReportModelList==null)
                {
                    pdcReportModelList= new List<PDCReportModel>();
                }

                pdcReportModelList=pdcReportModelList.Union(pdcReportModelList).ToList();

                return pdcReportModelList; // returns.
            });
        }

    }
}
