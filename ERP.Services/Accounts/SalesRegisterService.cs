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
    public class SalesRegisterService : ISalesRegister
    {
        ErpDbContext dbContext;

        public SalesRegisterService(ErpDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DataTableResultModel<SalesRegisterModel>> GetReport(SearchFilterSalesRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {

            int customerLedgerId;
            int accountLedgerId;

            customerLedgerId=(int)(searchFilterModel.CustomerLedgerId==null ? 0 : searchFilterModel.CustomerLedgerId);
            accountLedgerId=(int)(searchFilterModel.AccountLedgerId==null ? 0 : searchFilterModel.AccountLedgerId);

            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<SalesRegisterModel> salesRegisterModelList = await GetList(customerLedgerId, accountLedgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<SalesRegisterModel> resultModel = new DataTableResultModel<SalesRegisterModel>();

            if (null != salesRegisterModelList && salesRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesRegisterModel>();
                resultModel.ResultList = salesRegisterModelList;
                resultModel.TotalResultCount = salesRegisterModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<SalesRegisterModel>();
                resultModel.ResultList = new List<SalesRegisterModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesRegisterModel>> GetList(int customerLedgerId, int accountLedgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<SalesRegisterModel> salesRegisterModelList = new List<SalesRegisterModel>();

            IList<SalesRegisterModel> salesRegisterModelList_Trans = null;

            salesRegisterModelList_Trans = await GetTransactionList(customerLedgerId, accountLedgerId, fromDate, toDate, financialYearId, companyId);

            if (salesRegisterModelList_Trans==null)
            {
                salesRegisterModelList_Trans= new List<SalesRegisterModel>();
            }

            salesRegisterModelList = salesRegisterModelList_Trans;

            if (salesRegisterModelList.Any())
            {
                salesRegisterModelList.Add(new SalesRegisterModel()
                {
                    SequenceNo = 3,
                    SrNo = salesRegisterModelList.Max(w => w.SrNo) + 1,
                    InvoiceNo = "Total Amount",
                    InvoiceDate = toDate,
                    TotalLineItemAmount = salesRegisterModelList.Sum(w => w.TotalLineItemAmount),
                    TaxAmount = salesRegisterModelList.Sum(w => w.TaxAmount),
                    DiscountAmount = salesRegisterModelList.Sum(w => w.DiscountAmount),
                    GrossAmount = salesRegisterModelList.Sum(w => w.GrossAmount),
                    NetAmount = salesRegisterModelList.Sum(w => w.NetAmount),
                });

                return salesRegisterModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return salesRegisterModelList;

        }

        private async Task<IList<SalesRegisterModel>> GetTransactionList(int customerLedgerId, int accountLedgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<SalesRegisterModel> salesRegisterModelList = null;

                salesRegisterModelList = dbContext
                            .Salesinvoices
                            .Include(w => w.CustomerLedger).Include(w => w.BillToAddress)
                            .Include(w => w.AccountLedger).Include(w => w.BankLedger)
                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                            .Include(w => w.Status).Include(w => w.PreparedByUser)
                            .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                && w.FinancialYearId == financialYearId
                                && w.CompanyId == companyId
                                && w.InvoiceDate >= fromDate
                                && w.InvoiceDate <= toDate
                                &&(w.CustomerLedgerId == customerLedgerId || customerLedgerId ==0)
                                &&(w.AccountLedgerId == accountLedgerId || accountLedgerId ==0)
                                )
                            .ToList()
                            .Select((row, index) => new SalesRegisterModel
                            {
                                SequenceNo = 2,
                                SrNo = index + 1,
                                SalesInvoiceId = row.SalesInvoiceId,
                                InvoiceNo = row.InvoiceNo,
                                InvoiceDate = row.InvoiceDate,
                                CustomerLedgerId = row.CustomerLedgerId,
                                BillToAddressId = row.BillToAddressId,
                                AccountLedgerId = row.AccountLedgerId,
                                BankLedgerId = row.BankLedgerId,
                                CustomerReferenceNo = row.CustomerReferenceNo,
                                CustomerReferenceDate = row.CustomerReferenceDate,
                                CreditLimitDays = row.CreditLimitDays,
                                PaymentTerm = row.PaymentTerm,
                                Remark = row.Remark,
                                TaxModelType = row.TaxModelType,
                                TaxRegisterId = row.TaxRegisterId,
                                CurrencyId = row.CurrencyId,
                                ExchangeRate = row.ExchangeRate,
                                TotalLineItemAmountFc = row.TotalLineItemAmountFc,
                                TotalLineItemAmount = row.TotalLineItemAmount,
                                GrossAmountFc = row.GrossAmountFc,
                                GrossAmount = row.GrossAmount,
                                NetAmountFc = row.NetAmountFc,
                                NetAmount = row.NetAmount,
                                NetAmountFcInWord = row.NetAmountFcinWord,
                                TaxAmountFc = row.TaxAmountFc,
                                TaxAmount = row.TaxAmount,
                                DiscountPercentageOrAmount = row.DiscountPercentageOrAmount,
                                DiscountPerOrAmountFc = row.DiscountPerOrAmountFc,
                                DiscountAmountFc = row.DiscountAmountFc,
                                DiscountAmount = row.DiscountAmount,
                                CustomerLedgerName = null != row.CustomerLedger ? row.CustomerLedger.LedgerName : null,
                                BillToAddress = null != row.BillToAddress ? row.BillToAddress.AddressDescription : null,
                                AccountLedgerName = null != row.AccountLedger ? row.AccountLedger.LedgerName : null,
                                BankLedgerName = null != row.BankLedger ? row.BankLedger.LedgerName : null,
                                TaxRegisterName = null != row.TaxRegister ? row.TaxRegister.TaxRegisterName : null,
                                CurrencyCode = null != row.Currency ? row.Currency.CurrencyCode : null,
                            })
                            .ToList();

                if (salesRegisterModelList==null)
                {
                    salesRegisterModelList= new List<SalesRegisterModel>();
                }

                return salesRegisterModelList; // returns.
            });
        }

    }
}
