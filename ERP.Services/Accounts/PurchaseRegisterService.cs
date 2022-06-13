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
    public class PurchaseRegisterService : IPurchaseRegister
    {
        private readonly ErpDbContext _dbContext;

        public PurchaseRegisterService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataTableResultModel<PurchaseRegisterModel>> GetReport(SearchFilterPurchaseRegisterModel searchFilterModel, DateTime fromDate_FY, DateTime toDate_FY)
        {

            int customerLedgerId;
            int accountLedgerId;

            customerLedgerId=(int)(searchFilterModel.SupplierLedgerId==null ? 0 : searchFilterModel.SupplierLedgerId);
            accountLedgerId=(int)(searchFilterModel.AccountLedgerId==null ? 0 : searchFilterModel.AccountLedgerId);

            if (searchFilterModel.FromDate < fromDate_FY) { searchFilterModel.FromDate = fromDate_FY; }

            if (searchFilterModel.ToDate > toDate_FY) { searchFilterModel.ToDate = toDate_FY; }

            IList<PurchaseRegisterModel> purchaseRegisterModelList = await GetList(customerLedgerId, accountLedgerId, searchFilterModel.FromDate, searchFilterModel.ToDate, searchFilterModel.FinancialYearId, searchFilterModel.CompanyId);

            DataTableResultModel<PurchaseRegisterModel> resultModel = new DataTableResultModel<PurchaseRegisterModel>();

            if (null != purchaseRegisterModelList && purchaseRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseRegisterModel>();
                resultModel.ResultList = purchaseRegisterModelList;
                resultModel.TotalResultCount = purchaseRegisterModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PurchaseRegisterModel>();
                resultModel.ResultList = new List<PurchaseRegisterModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseRegisterModel>> GetList(int customerLedgerId, int accountLedgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            IList<PurchaseRegisterModel> purchaseRegisterModelList = new List<PurchaseRegisterModel>();

            IList<PurchaseRegisterModel> purchaseRegisterModelList_Trans = null;

            purchaseRegisterModelList_Trans = await GetTransactionList(customerLedgerId, accountLedgerId, fromDate, toDate, financialYearId, companyId);

            if (purchaseRegisterModelList_Trans==null)
            {
                purchaseRegisterModelList_Trans= new List<PurchaseRegisterModel>();
            }

            purchaseRegisterModelList = purchaseRegisterModelList_Trans;

            if (purchaseRegisterModelList.Any())
            {
                purchaseRegisterModelList.Add(new PurchaseRegisterModel()
                {
                    SequenceNo = 3,
                    SrNo = purchaseRegisterModelList.Max(w => w.SrNo) + 1,
                    InvoiceNo = "Total Amount",
                    InvoiceDate = toDate,
                    TotalLineItemAmount = purchaseRegisterModelList.Sum(w => w.TotalLineItemAmount),
                    TaxAmount = purchaseRegisterModelList.Sum(w => w.TaxAmount),
                    DiscountAmount = purchaseRegisterModelList.Sum(w => w.DiscountAmount),
                    GrossAmount = purchaseRegisterModelList.Sum(w => w.GrossAmount),
                    NetAmount = purchaseRegisterModelList.Sum(w => w.NetAmount),
                });

                return purchaseRegisterModelList.OrderBy(o => o.SequenceNo).ThenBy(o => o.SrNo).ToList();
            }

            return purchaseRegisterModelList;
        }

        private async Task<IList<PurchaseRegisterModel>> GetTransactionList(int customerLedgerId, int accountLedgerId, DateTime fromDate, DateTime toDate, int financialYearId, int companyId)
        {
            return await Task.Run(() =>
            {
                IList<PurchaseRegisterModel> purchaseRegisterModelList = null;

                purchaseRegisterModelList = _dbContext
                            .Purchaseinvoices
                            .Include(w => w.SupplierLedger).Include(w => w.BillToAddress)
                            .Include(w => w.AccountLedger)
                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                            .Include(w => w.Status).Include(w => w.PreparedByUser)
                            .Where(w => w.StatusId == (int)DocumentStatus.Approved
                                && w.FinancialYearId == financialYearId
                                && w.CompanyId == companyId
                                && w.InvoiceDate >= fromDate
                                && w.InvoiceDate <= toDate
                                &&(w.SupplierLedgerId == customerLedgerId || customerLedgerId ==0)
                                &&(w.AccountLedgerId == accountLedgerId || accountLedgerId ==0)
                                )
                            .ToList()
                            .Select((row, index) => new PurchaseRegisterModel
                            {
                                SequenceNo = 2,
                                SrNo = index + 1,
                                PurchaseInvoiceId = row.PurchaseInvoiceId,
                                InvoiceNo = row.InvoiceNo,
                                InvoiceDate = row.InvoiceDate,
                                SupplierLedgerId = row.SupplierLedgerId,
                                BillToAddressId = row.BillToAddressId,
                                AccountLedgerId = row.AccountLedgerId,
                                SupplierReferenceNo = row.SupplierReferenceNo,
                                SupplierReferenceDate = row.SupplierReferenceDate,
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
                                SupplierLedgerName = null != row.SupplierLedger ? row.SupplierLedger.LedgerName : null,
                                BillToAddress = null != row.BillToAddress ? row.BillToAddress.AddressDescription : null,
                                AccountLedgerName = null != row.AccountLedger ? row.AccountLedger.LedgerName : null,
                                TaxRegisterName = null != row.TaxRegister ? row.TaxRegister.TaxRegisterName : null,
                                CurrencyCode = null != row.Currency ? row.Currency.CurrencyCode : null,
                            })
                            .ToList();

                if (purchaseRegisterModelList==null)
                {
                    purchaseRegisterModelList= new List<PurchaseRegisterModel>();
                }

                return purchaseRegisterModelList; // returns.
            });
        }

    }
}
