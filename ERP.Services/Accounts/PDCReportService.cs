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
        ErpDbContext dbContext;

        public PDCReportService(ErpDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<PDCReportModel>> GetReport(SearchFilterPDCReportModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {
            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

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

                //if (pdcType=="P")
                //{

                //    pdcReportModelList = dbContext
                //                .Paymentvoucherdetails
                //                .Include(i => i.ParticularLedger)
                //                .Include(i => i.PaymentVoucher).ThenInclude(i => i.AccountLedger)
                //                .Include(i => i.PaymentVoucher).ThenInclude(i => i.Currency)
                //                .Where(w => w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved
                //                    && w.PaymentVoucher.PaymentTypeId == (int)PaymentType.PDC
                //                    &&(w.ParticularLedgerId == ledgerId || ledgerId ==0)
                //                    && w.PaymentVoucher.FinancialYearId == financialYearId
                //                    && w.PaymentVoucher.CompanyId == companyId
                //                    && w.PaymentVoucher.ChequeDate >= fromDate
                //                    && w.PaymentVoucher.ChequeDate <= toDate
                //                    )
                //                .ToList()
                //                .Select(row => new PDCReportModel
                //                {
                //                    SequenceNo = 2,
                //                    //SrNo = index + 1,
                //                    //DocumentDetId = row.PaymentVoucherDetId,
                //                    DocumentId = row.PaymentVoucherId,
                //                    DocumentNo = row.PaymentVoucher.VoucherNo,
                //                    DocumentDate = row.PaymentVoucher.VoucherDate,
                //                    ChequeNo = row.PaymentVoucher.ChequeNo,
                //                    ChequeDate = row.PaymentVoucher.ChequeDate,
                //                    BankName = (null != row.PaymentVoucher ? (null != row.PaymentVoucher.AccountLedger ? row.PaymentVoucher.AccountLedger.LedgerName : "") : ""),
                //                    CurrencyId = row.PaymentVoucher.CurrencyId,
                //                    CurrencyCode = (null != row.PaymentVoucher ? (null != row.PaymentVoucher.Currency ? row.PaymentVoucher.Currency.CurrencyCode : "") : ""),
                //                    ExchangeRate = row.PaymentVoucher.ExchangeRate,
                //                    ParticularLedgerName = (null != row.ParticularLedger ? row.ParticularLedger.LedgerName : ""),
                //                    Narration = row.PaymentVoucher.Narration,
                //                    Amount_FC = row.PaymentVoucher.AmountFc,
                //                    Amount = row.PaymentVoucher.Amount
                //                })
                //                //.ToList()
                //                //.GroupBy(row => row)
                //                //.Select(row => row.FirstOrDefault())
                //                .ToList()
                //                ;
                //}
                //else
                //{
                //    pdcReportModelList = dbContext
                //               .Receiptvoucherdetails
                //               .Include(i => i.ReceiptVoucher)
                //               .Include(i => i.ReceiptVoucher).ThenInclude(i => i.AccountLedger)
                //               .Include(i => i.ReceiptVoucher).ThenInclude(i => i.Currency)
                //               .Where(w => w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved
                //                    && w.ReceiptVoucher.PaymentTypeId == (int)PaymentType.PDC
                //                     &&(w.ParticularLedgerId == ledgerId || ledgerId ==0)
                //                   && w.ReceiptVoucher.FinancialYearId == financialYearId
                //                   && w.ReceiptVoucher.CompanyId == companyId
                //                   && w.ReceiptVoucher.ChequeDate >= fromDate
                //                   && w.ReceiptVoucher.ChequeDate <= toDate
                //                   )
                //               .ToList()
                //               .Select((row, index) => new PDCReportModel
                //               {
                //                   SequenceNo = 2,
                //                   //SrNo = index + 1,
                //                   //DocumentDetId = row.PaymentVoucherDetId,
                //                   DocumentId = row.ReceiptVoucherId,
                //                   DocumentNo = row.ReceiptVoucher.VoucherNo,
                //                   DocumentDate = row.ReceiptVoucher.VoucherDate,
                //                   ChequeNo = row.ReceiptVoucher.ChequeNo,
                //                   ChequeDate = row.ReceiptVoucher.ChequeDate,
                //                   BankName = (null != row.ReceiptVoucher ? (null != row.ReceiptVoucher.AccountLedger ? row.ReceiptVoucher.AccountLedger.LedgerName : "") : ""),
                //                   CurrencyId = row.ReceiptVoucher.CurrencyId,
                //                   CurrencyCode = (null != row.ReceiptVoucher ? (null != row.ReceiptVoucher.Currency ? row.ReceiptVoucher.Currency.CurrencyCode : "") : ""),
                //                   ExchangeRate = row.ReceiptVoucher.ExchangeRate,
                //                   ParticularLedgerName = (null != row.ParticularLedger ? row.ParticularLedger.LedgerName : ""),
                //                   Narration = row.ReceiptVoucher.Narration,
                //                   Amount_FC = row.ReceiptVoucher.AmountFc,
                //                   Amount = row.ReceiptVoucher.Amount
                //               })
                //            //   .ToList()
                //            //.GroupBy(row => row)
                //            //.Select(row => row.FirstOrDefault())
                //            .ToList();
                //}


                if (pdcType=="P")
                {

                    pdcReportModelList = dbContext
                                .Paymentvouchers.Include(i => i.AccountLedger)
                                .Include(i => i.Currency)
                                .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.PaymentTypeId == (int)PaymentType.PDC
                                    //&&(w.ParticularLedgerId == ledgerId || ledgerId ==0)
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
                    pdcReportModelList = dbContext.
                               Receiptvouchers.Include(i => i.AccountLedger)
                               .Include(i => i.Currency)
                               .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                    && w.PaymentTypeId == (int)PaymentType.PDC
                                     //&&(w.ParticularLedgerId == ledgerId || ledgerId ==0)
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
