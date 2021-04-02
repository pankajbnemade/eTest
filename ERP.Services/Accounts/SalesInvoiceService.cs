using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class SalesInvoiceService : Repository<Salesinvoice>, ISalesInvoice
    {
        ICommon common;
        public SalesInvoiceService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }
        public async Task<GenerateNoModel> GenerateInvoiceNo(int? companyId, int? financialYearId)
        {
            IList<SalesInvoiceModel> salesInvoiceModelList = await GetSalesInvoiceList(0);

            int voucherSetupId = 2;

            int? maxNo = salesInvoiceModelList.Where(w => (w.CompanyId == companyId && w.FinancialYearId == financialYearId)).Max(w => w.MaxNo);

            return await common.GenerateVoucherNo(Convert.ToInt32(maxNo), voucherSetupId, Convert.ToInt32(companyId), Convert.ToInt32(financialYearId));
        }
        /// <summary>
        /// create new sales invoice.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateSalesInvoice(SalesInvoiceModel salesInvoiceModel)
        {
            int salesInvoiceId = 0;
            GenerateNoModel generateNoModel = await GenerateInvoiceNo(salesInvoiceModel.CompanyId, salesInvoiceModel.FinancialYearId);

            salesInvoiceModel.InvoiceNo = generateNoModel.VoucherNo;
            salesInvoiceModel.MaxNo = generateNoModel.MaxNo;
            salesInvoiceModel.VoucherStyleId = generateNoModel.VoucherStyleId;

            // assign values.
            Salesinvoice salesInvoice = new Salesinvoice();
            salesInvoice.InvoiceId = salesInvoiceModel.InvoiceId;
            salesInvoice.InvoiceNo = salesInvoiceModel.InvoiceNo;
            salesInvoice.InvoiceDate = salesInvoiceModel.InvoiceDate;
            salesInvoice.CustomerLedgerId = salesInvoiceModel.CustomerLedgerId;
            salesInvoice.BillToAddressId = salesInvoiceModel.BillToAddressId;
            salesInvoice.AccountLedgerId = salesInvoiceModel.AccountLedgerId;
            salesInvoice.BankLedgerId = salesInvoiceModel.BankLedgerId;
            salesInvoice.CustomerReferenceNo = salesInvoiceModel.CustomerReferenceNo;
            salesInvoice.CustomerReferenceDate = salesInvoiceModel.CustomerReferenceDate;
            salesInvoice.CreditLimitDays = salesInvoiceModel.CreditLimitDays;
            salesInvoice.PaymentTerm = salesInvoiceModel.PaymentTerm;
            salesInvoice.Remark = salesInvoiceModel.Remark;
            salesInvoice.TaxModelType = salesInvoiceModel.TaxModelType;
            salesInvoice.TaxRegisterId = salesInvoiceModel.TaxRegisterId;
            salesInvoice.CurrencyId = salesInvoiceModel.CurrencyId;
            salesInvoice.ExchangeRate = salesInvoiceModel.ExchangeRate;
            salesInvoice.TotalLineItemAmountFc = 0;
            salesInvoice.TotalLineItemAmount = 0;
            salesInvoice.GrossAmountFc = 0;
            salesInvoice.GrossAmount = 0;
            salesInvoice.NetAmountFc = 0;
            salesInvoice.NetAmount = 0;
            salesInvoice.NetAmountFcinWord = "";
            salesInvoice.TaxAmountFc = 0;
            salesInvoice.TaxAmount = 0;

            salesInvoice.DiscountPercentageOrAmount = salesInvoiceModel.DiscountPercentageOrAmount;
            salesInvoice.DiscountPercentage = salesInvoiceModel.DiscountPercentage;
            salesInvoice.DiscountAmountFc = 0;
            salesInvoice.DiscountAmount = 0;
            salesInvoice.StatusId = salesInvoiceModel.StatusId;
            salesInvoice.CompanyId = salesInvoiceModel.CompanyId;
            salesInvoice.FinancialYearId = salesInvoiceModel.FinancialYearId;
            salesInvoice.MaxNo = salesInvoiceModel.MaxNo;
            salesInvoice.VoucherStyleId = salesInvoiceModel.VoucherStyleId;
            //salesInvoice.PreparedByUserId = salesInvoiceModel.PreparedByUserId;
            //salesInvoice.UpdatedByUserId = salesInvoiceModel.UpdatedByUserId;
            //salesInvoice.PreparedDateTime = salesInvoiceModel.PreparedDateTime;
            //salesInvoice.UpdatedDateTime = salesInvoiceModel.UpdatedDateTime;

            salesInvoiceId = await Create(salesInvoice);
            if (salesInvoiceId != 0)
            {
                await UpdateSalesInvoiceMasterAmount(salesInvoiceId);
            }
            return salesInvoiceId; // returns.
        }

        /// <summary>
        /// update sales invoice.
        /// </summary>
        /// <param name="salesInvoiceModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateSalesInvoice(SalesInvoiceModel salesInvoiceModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.InvoiceId == salesInvoiceModel.InvoiceId);
            if (null != salesInvoice)
            {
                // assign values.
                salesInvoice.InvoiceId = salesInvoiceModel.InvoiceId;
                salesInvoice.InvoiceNo = salesInvoiceModel.InvoiceNo;
                salesInvoice.InvoiceDate = salesInvoiceModel.InvoiceDate;
                salesInvoice.CustomerLedgerId = salesInvoiceModel.CustomerLedgerId;
                salesInvoice.BillToAddressId = salesInvoiceModel.BillToAddressId;
                salesInvoice.AccountLedgerId = salesInvoiceModel.AccountLedgerId;
                salesInvoice.BankLedgerId = salesInvoiceModel.BankLedgerId;
                salesInvoice.CustomerReferenceNo = salesInvoiceModel.CustomerReferenceNo;
                salesInvoice.CustomerReferenceDate = salesInvoiceModel.CustomerReferenceDate;
                salesInvoice.CreditLimitDays = salesInvoiceModel.CreditLimitDays;
                salesInvoice.PaymentTerm = salesInvoiceModel.PaymentTerm;
                salesInvoice.Remark = salesInvoiceModel.Remark;
                salesInvoice.TaxModelType = salesInvoiceModel.TaxModelType;
                salesInvoice.TaxRegisterId = salesInvoiceModel.TaxRegisterId;
                salesInvoice.CurrencyId = salesInvoiceModel.CurrencyId;
                salesInvoice.ExchangeRate = salesInvoiceModel.ExchangeRate;

                salesInvoice.TotalLineItemAmountFc = 0;
                salesInvoice.TotalLineItemAmount = 0;
                salesInvoice.GrossAmountFc = 0;
                salesInvoice.GrossAmount = 0;
                salesInvoice.NetAmountFc = 0;
                salesInvoice.NetAmount = 0;
                salesInvoice.NetAmountFcinWord = "";
                salesInvoice.TaxAmountFc = 0;
                salesInvoice.TaxAmount = 0;

                salesInvoice.DiscountPercentageOrAmount = salesInvoiceModel.DiscountPercentageOrAmount;
                salesInvoice.DiscountPercentage = salesInvoiceModel.DiscountPercentage;

                salesInvoice.DiscountAmountFc = 0;
                salesInvoice.DiscountAmount = 0;

                //salesInvoice.StatusId = salesInvoiceModel.StatusId;
                //salesInvoice.CompanyId = salesInvoiceModel.CompanyId;

                //salesInvoice.FinancialYearId = salesInvoiceModel.FinancialYearId;
                //salesInvoice.MaxNo = salesInvoiceModel.MaxNo;

                isUpdated = await Update(salesInvoice);
            }

            if (isUpdated != false)
            {
                await UpdateSalesInvoiceMasterAmount(salesInvoice.InvoiceId);
            }
            return isUpdated; // returns.
        }

        /// <summary>
        /// delete sales invoice.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteSalesInvoice(int invoiceId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != salesInvoice)
            {
                isDeleted = await Delete(salesInvoice);
            }

            return isDeleted; // returns.
        }


        public async Task<bool> UpdateSalesInvoiceMasterAmount(int? salesInvoiceId)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.InvoiceId == salesInvoiceId);

            if (null != salesInvoice)
            {
                salesInvoice.TotalLineItemAmountFc = salesInvoice.Salesinvoicedetails.Sum(w => w.GrossAmountFc);
                salesInvoice.TotalLineItemAmount = salesInvoice.TotalLineItemAmountFc * salesInvoice.ExchangeRate;

                if (salesInvoice.DiscountPercentageOrAmount == "Percentage")
                {
                    salesInvoice.DiscountAmountFc = (salesInvoice.TotalLineItemAmountFc * salesInvoice.DiscountPercentage) / 100;
                }
                else
                {
                    salesInvoice.DiscountAmountFc = salesInvoice.DiscountPercentage;
                }

                salesInvoice.DiscountAmount = salesInvoice.DiscountAmountFc * salesInvoice.ExchangeRate;

                salesInvoice.GrossAmountFc = salesInvoice.TotalLineItemAmountFc + salesInvoice.DiscountAmountFc;
                salesInvoice.GrossAmount = salesInvoice.GrossAmountFc * salesInvoice.ExchangeRate;

                if (salesInvoice.TaxModelType == "Line Wise")
                {
                    salesInvoice.TaxAmountFc = salesInvoice.Salesinvoicedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    salesInvoice.TaxAmountFc = salesInvoice.Salesinvoicetaxes.Sum(w => w.TaxAmountFc);
                }

                salesInvoice.TaxAmount = salesInvoice.TaxAmountFc * salesInvoice.ExchangeRate;

                salesInvoice.NetAmountFc = salesInvoice.GrossAmountFc + salesInvoice.DiscountAmountFc;
                salesInvoice.NetAmount = salesInvoice.NetAmountFc * salesInvoice.ExchangeRate;

                salesInvoice.NetAmountFcinWord = await common.AmountInWord_Million(salesInvoice.NetAmountFc.ToString(), salesInvoice.Currency.CurrencyCode, salesInvoice.Currency.Denomination);

                isUpdated = await Update(salesInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get sales invoice based on invoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<SalesInvoiceModel> GetSalesInvoiceById(int invoiceId)
        {
            SalesInvoiceModel salesInvoiceModel = null;

            IList<SalesInvoiceModel> salesInvoiceModelList = await GetSalesInvoiceList(invoiceId);

            if (null != salesInvoiceModelList && salesInvoiceModelList.Any())
            {
                salesInvoiceModel = salesInvoiceModelList.FirstOrDefault();
            }

            return salesInvoiceModel; // returns.
        }

        /// <summary>
        /// get search sales invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<SalesInvoiceModel>> GetSalesInvoiceList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterSalesInvoiceModel searchFilterModel)
        {
            string searchBy = dataTableAjaxPostModel.search?.value;
            int take = dataTableAjaxPostModel.length;
            int skip = dataTableAjaxPostModel.start;

            string sortBy = string.Empty;
            string sortDir = string.Empty;

            if (dataTableAjaxPostModel.order != null)
            {
                sortBy = dataTableAjaxPostModel.columns[dataTableAjaxPostModel.order[0].column].data;
                sortDir = dataTableAjaxPostModel.order[0].dir.ToLower();
            }

            // search the dbase taking into consideration table sorting and paging
            DataTableResultModel<SalesInvoiceModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        #region Private Methods

        /// <summary>
        /// get records from database.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        private async Task<DataTableResultModel<SalesInvoiceModel>> GetDataFromDbase(SearchFilterSalesInvoiceModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<SalesInvoiceModel> resultModel = new DataTableResultModel<SalesInvoiceModel>();

            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.InvoiceId != 0);

            if (!string.IsNullOrEmpty(searchFilterModel.InvoiceNo))
            {
                query = query.Where(w => w.InvoiceNo.Contains(searchFilterModel.InvoiceNo));
            }

            if (null != searchFilterModel.CustomerLedgerId)
            {
                query = query.Where(w => w.CustomerLedgerId == searchFilterModel.CustomerLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.InvoiceDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.InvoiceDate <= searchFilterModel.ToDate);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);
            resultModel.ResultList = await query.Select(s => new SalesInvoiceModel
            {
                InvoiceId = s.InvoiceId,
                InvoiceNo = s.InvoiceNo,
                InvoiceDate = s.InvoiceDate,
                NetAmount = s.NetAmount,
            }).ToListAsync();
            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceModel>> GetSalesInvoiceList(int salesInvoiceId)
        {
            IList<SalesInvoiceModel> salesInvoiceModelList = null;

            // create query.
            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.InvoiceId != 0)
                                            .Include(w => w.CustomerLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger).Include(w => w.BankLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.InvoiceId == salesInvoiceId);

            // get records by query.
            List<Salesinvoice> salesInvoiceList = await query.ToListAsync();

            if (null != salesInvoiceList && salesInvoiceList.Count > 0)
            {
                salesInvoiceModelList = new List<SalesInvoiceModel>();

                foreach (Salesinvoice salesInvoice in salesInvoiceList)
                {
                    salesInvoiceModelList.Add(await AssignValueToModel(salesInvoice));
                }
            }

            return salesInvoiceModelList; // returns.
        }

        private async Task<SalesInvoiceModel> AssignValueToModel(Salesinvoice salesInvoice)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();

                salesInvoiceModel.InvoiceId = salesInvoice.InvoiceId;
                salesInvoiceModel.InvoiceNo = salesInvoice.InvoiceNo;
                salesInvoiceModel.InvoiceDate = salesInvoice.InvoiceDate;
                salesInvoiceModel.CustomerLedgerId = salesInvoice.CustomerLedgerId;
                salesInvoiceModel.BillToAddressId = salesInvoice.BillToAddressId;
                salesInvoiceModel.AccountLedgerId = salesInvoice.AccountLedgerId;
                salesInvoiceModel.BankLedgerId = salesInvoice.BankLedgerId;
                salesInvoiceModel.CustomerReferenceNo = salesInvoice.CustomerReferenceNo;
                salesInvoiceModel.CustomerReferenceDate = salesInvoice.CustomerReferenceDate;
                salesInvoiceModel.CreditLimitDays = salesInvoice.CreditLimitDays;
                salesInvoiceModel.PaymentTerm = salesInvoice.PaymentTerm;
                salesInvoiceModel.Remark = salesInvoice.Remark;
                salesInvoiceModel.TaxModelType = salesInvoice.TaxModelType;
                salesInvoiceModel.TaxRegisterId = salesInvoice.TaxRegisterId;
                salesInvoiceModel.CurrencyId = salesInvoice.CurrencyId;
                salesInvoiceModel.ExchangeRate = salesInvoice.ExchangeRate;
                salesInvoiceModel.TotalLineItemAmountFc = salesInvoice.TotalLineItemAmountFc;
                salesInvoiceModel.TotalLineItemAmount = salesInvoice.TotalLineItemAmount;
                salesInvoiceModel.GrossAmountFc = salesInvoice.GrossAmountFc;
                salesInvoiceModel.GrossAmount = salesInvoice.GrossAmount;
                salesInvoiceModel.NetAmountFc = salesInvoice.NetAmountFc;
                salesInvoiceModel.NetAmount = salesInvoice.NetAmount;
                salesInvoiceModel.NetAmountFcinWord = salesInvoice.NetAmountFcinWord;
                salesInvoiceModel.TaxAmountFc = salesInvoice.TaxAmountFc;
                salesInvoiceModel.TaxAmount = salesInvoice.TaxAmount;
                salesInvoiceModel.DiscountPercentageOrAmount = salesInvoice.DiscountPercentageOrAmount;
                salesInvoiceModel.DiscountPercentage = salesInvoice.DiscountPercentage;
                salesInvoiceModel.DiscountAmountFc = salesInvoice.DiscountAmountFc;
                salesInvoiceModel.DiscountAmount = salesInvoice.DiscountAmount;
                salesInvoiceModel.StatusId = salesInvoice.StatusId;
                salesInvoiceModel.CompanyId = salesInvoice.CompanyId;
                salesInvoiceModel.FinancialYearId = salesInvoice.FinancialYearId;
                salesInvoiceModel.MaxNo = salesInvoice.MaxNo;
                salesInvoiceModel.VoucherStyleId = salesInvoice.VoucherStyleId;
                salesInvoiceModel.PreparedByUserId = salesInvoice.PreparedByUserId;
                salesInvoiceModel.UpdatedByUserId = salesInvoice.UpdatedByUserId;
                salesInvoiceModel.PreparedDateTime = salesInvoice.PreparedDateTime;
                salesInvoiceModel.UpdatedDateTime = salesInvoice.UpdatedDateTime;

                if (null != salesInvoice.CustomerLedger)
                {
                    salesInvoiceModel.CustomerLedgerName = salesInvoice.CustomerLedger.LedgerName;
                }

                if (null != salesInvoice.BillToAddress)
                {
                    salesInvoiceModel.BillToAddress = salesInvoice.BillToAddress.AddressDescription;
                }

                if (null != salesInvoice.AccountLedger)
                {
                    salesInvoiceModel.AccountLedgerName = salesInvoice.AccountLedger.LedgerName;
                }

                if (null != salesInvoice.BankLedger)
                {
                    salesInvoiceModel.BankLedgerName = salesInvoice.BankLedger.LedgerName;
                }

                if (null != salesInvoice.TaxRegister)
                {
                    salesInvoiceModel.TaxRegisterName = salesInvoice.TaxRegister.TaxRegisterName;
                }

                if (null != salesInvoice.Currency)
                {
                    salesInvoiceModel.CurrencyName = salesInvoice.Currency.CurrencyName;
                }

                if (null != salesInvoice.Status)
                {
                    salesInvoiceModel.StatusName = salesInvoice.Status.StatusName;
                }

                if (null != salesInvoice.PreparedByUser)
                {
                    salesInvoiceModel.PreparedByName = salesInvoice.PreparedByUser.UserName;
                }

                return salesInvoiceModel;
            });

        }

        #endregion Private Methods
    }
}
