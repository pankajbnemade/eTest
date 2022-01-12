using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class SalesInvoiceService : Repository<Salesinvoice>, ISalesInvoice
    {
        private readonly ICommon common;

        public SalesInvoiceService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        public async Task<GenerateNoModel> GenerateInvoiceNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 2;

            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
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

            // assign values.
            Salesinvoice salesInvoice = new Salesinvoice();

            salesInvoice.InvoiceNo = generateNoModel.VoucherNo;
            salesInvoice.MaxNo = generateNoModel.MaxNo;
            salesInvoice.VoucherStyleId = generateNoModel.VoucherStyleId;

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
            salesInvoice.DiscountPerOrAmountFc = salesInvoiceModel.DiscountPerOrAmountFc;
            salesInvoice.DiscountAmountFc = 0;
            salesInvoice.DiscountAmount = 0;

            salesInvoice.StatusId = (int)DocumentStatus.Inprocess;
            salesInvoice.CompanyId = salesInvoiceModel.CompanyId;
            salesInvoice.FinancialYearId = salesInvoiceModel.FinancialYearId;

            await Create(salesInvoice);

            salesInvoiceId = salesInvoice.SalesInvoiceId;

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
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.SalesInvoiceId == salesInvoiceModel.SalesInvoiceId);

            if (null != salesInvoice)
            {
                // assign values.
                //salesInvoice.SalesInvoiceId = salesInvoiceModel.SalesInvoiceId;
                //salesInvoice.InvoiceNo = salesInvoiceModel.InvoiceNo;
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
                salesInvoice.DiscountPerOrAmountFc = salesInvoiceModel.DiscountPerOrAmountFc;

                salesInvoice.DiscountAmountFc = 0;
                salesInvoice.DiscountAmount = 0;

                isUpdated = await Update(salesInvoice);
            }

            if (isUpdated != false)
            {
                await UpdateSalesInvoiceMasterAmount(salesInvoice.SalesInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateStatusSalesInvoice(int salesInvoiceId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.SalesInvoiceId == salesInvoiceId);

            if (null != salesInvoice)
            {
                salesInvoice.StatusId = statusId;
                isUpdated = await Update(salesInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete sales invoice.
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteSalesInvoice(int salesInvoiceId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoice salesInvoice = await GetByIdAsync(w => w.SalesInvoiceId == salesInvoiceId);

            if (null != salesInvoice)
            {
                isDeleted = await Delete(salesInvoice);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceMasterAmount(int salesInvoiceId)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoice salesInvoice = await GetQueryByCondition(w => w.SalesInvoiceId == salesInvoiceId)
                                                    .Include(w => w.Salesinvoicedetails).Include(w => w.Salesinvoicetaxes)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != salesInvoice)
            {
                salesInvoice.TotalLineItemAmountFc = salesInvoice.Salesinvoicedetails.Sum(w => w.GrossAmountFc);
                salesInvoice.TotalLineItemAmount = salesInvoice.TotalLineItemAmountFc / salesInvoice.ExchangeRate;

                if (DiscountType.Percentage.ToString() == salesInvoice.DiscountPercentageOrAmount)
                {
                    salesInvoice.DiscountAmountFc = (salesInvoice.TotalLineItemAmountFc * salesInvoice.DiscountPerOrAmountFc) / 100;
                }
                else
                {
                    salesInvoice.DiscountAmountFc = salesInvoice.DiscountPerOrAmountFc;
                }

                salesInvoice.DiscountAmount = salesInvoice.DiscountAmountFc / salesInvoice.ExchangeRate;
                salesInvoice.GrossAmountFc = salesInvoice.TotalLineItemAmountFc - salesInvoice.DiscountAmountFc;
                salesInvoice.GrossAmount = salesInvoice.GrossAmountFc / salesInvoice.ExchangeRate;

                if (TaxModelType.LineWise.ToString() == salesInvoice.TaxModelType)
                {
                    salesInvoice.TaxAmountFc = salesInvoice.Salesinvoicedetails.Sum(w => w.TaxAmountFc);
                }
                else
                {
                    salesInvoice.TaxAmountFc = salesInvoice.Salesinvoicetaxes.Sum(w => w.TaxAmountFc);
                }

                salesInvoice.TaxAmount = salesInvoice.TaxAmountFc / salesInvoice.ExchangeRate;
                salesInvoice.NetAmountFc = salesInvoice.GrossAmountFc + salesInvoice.TaxAmountFc;
                salesInvoice.NetAmount = salesInvoice.NetAmountFc / salesInvoice.ExchangeRate;

                salesInvoice.NetAmountFcinWord = await common.AmountInWord_Million(salesInvoice.NetAmountFc.ToString(), salesInvoice.Currency.CurrencyCode, salesInvoice.Currency.Denomination);

                if (salesInvoice.StatusId == (int)DocumentStatus.Approved || salesInvoice.StatusId == (int)DocumentStatus.ApprovalRequested || salesInvoice.StatusId == (int)DocumentStatus.Cancelled)
                {
                    salesInvoice.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(salesInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get sales invoice based on salesInvoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<SalesInvoiceModel> GetSalesInvoiceById(int salesInvoiceId)
        {
            SalesInvoiceModel salesInvoiceModel = null;

            IList<SalesInvoiceModel> salesInvoiceModelList = await GetSalesInvoiceList(salesInvoiceId);

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

        /// <summary>
        /// get sales invoice List based on customerLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<OutstandingInvoiceModel>> GetSalesInvoiceListByCustomerLedgerId(int customerLedgerId, DateTime? voucherDate)
        {
            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = null;

            // create query.
            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.SalesInvoiceId != 0
                                                                && w.StatusId == (int)DocumentStatus.Approved);

            // apply filters.
            if (0 != customerLedgerId)
                query = query.Where(w => w.CustomerLedgerId == customerLedgerId);

            if (null != voucherDate)
            {
                query = query.Where(w => w.InvoiceDate <= voucherDate);
            }

            // get records by query.
            List<Salesinvoice> salesInvoiceList = await query.ToListAsync();

            outstandingInvoiceModelList = new List<OutstandingInvoiceModel>();

            if (null != salesInvoiceList && salesInvoiceList.Count > 0)
            {
                foreach (Salesinvoice salesInvoice in salesInvoiceList)
                {
                    outstandingInvoiceModelList.Add(new OutstandingInvoiceModel()
                    {
                        InvoiceId = salesInvoice.SalesInvoiceId,
                        InvoiceType = "Sales Invoice",
                        InvoiceNo = salesInvoice.InvoiceNo,
                        InvoiceDate = salesInvoice.InvoiceDate,
                        InvoiceAmount = salesInvoice.NetAmount,
                        OutstandingAmount = salesInvoice.NetAmount,
                        SalesInvoiceId = salesInvoice.SalesInvoiceId,
                        PurchaseInvoiceId = null,
                        CreditNoteId = null,
                        DebitNoteId = null
                    });
                }
            }

            return outstandingInvoiceModelList; // returns.
        }

        public async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            // create query.
            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.StatusId == (int)DocumentStatus.Approved && w.FinancialYearId == yearId && w.CompanyId == companyId)
                                                .Include(i => i.Currency);

            query = query.Where(w => w.CustomerLedgerId == ledgerId);

            query = query.Where(w => w.InvoiceDate >= fromDate && w.InvoiceDate <= toDate);

            // get records by query.
            List<Salesinvoice> salesInvoiceList = await query.ToListAsync();

            generalLedgerModelList = new List<GeneralLedgerModel>();

            if (null != salesInvoiceList && salesInvoiceList.Count > 0)
            {
                foreach (Salesinvoice salesInvoice in salesInvoiceList)
                {
                    generalLedgerModelList.Add(new GeneralLedgerModel()
                    {
                        DocumentId = salesInvoice.SalesInvoiceId,
                        DocumentType = "Sales Invoice",
                        DocumentNo = salesInvoice.InvoiceNo,
                        DocumentDate = salesInvoice.InvoiceDate,
                        Amount_FC = salesInvoice.NetAmountFc,
                        Amount = salesInvoice.NetAmount,
                        DebitAmount_FC = salesInvoice.NetAmountFc,
                        DebitAmount = salesInvoice.NetAmount,
                        SalesInvoiceId = salesInvoice.SalesInvoiceId,
                        CurrencyId = salesInvoice.CurrencyId,
                        CurrencyCode = salesInvoice.Currency.CurrencyCode,
                        ExchangeRate = salesInvoice.ExchangeRate,
                        PartyReferenceNo = salesInvoice.CustomerReferenceNo,
                    });
                }
            }

            return generalLedgerModelList; // returns.
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

            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.SalesInvoiceId != 0)
                                                .Include(w => w.CustomerLedger).Include(w => w.Currency)
                                                .Include(w => w.PreparedByUser).Include(w => w.Status);

            query = query.Where(w => w.CompanyId==searchFilterModel.CompanyId);
            query = query.Where(w => w.FinancialYearId==searchFilterModel.FinancialYearId);

            //sortBy
            if (string.IsNullOrEmpty(sortBy) || sortBy == "0")
            {
                sortBy = "InvoiceNo";
            }

            //sortDir
            if (string.IsNullOrEmpty(sortDir) || sortDir == "")
            {
                sortDir = "asc";
            }

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

            if (!string.IsNullOrEmpty(searchFilterModel.CustomerReferenceNo))
            {
                query = query.Where(w => w.CustomerReferenceNo.Contains(searchFilterModel.CustomerReferenceNo));
            }

            if (null != searchFilterModel.AccountLedgerId)
            {
                query = query.Where(w => w.AccountLedgerId == searchFilterModel.AccountLedgerId);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.InvoiceNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new SalesInvoiceModel
            {
                SalesInvoiceId = s.SalesInvoiceId,
                InvoiceNo = s.InvoiceNo,
                InvoiceDate = s.InvoiceDate,
                NetAmountFc = s.NetAmountFc,
                CustomerReferenceNo = s.CustomerReferenceNo,
                CustomerReferenceDate = s.CustomerReferenceDate,
                CustomerLedgerName = s.CustomerLedger.LedgerName,
                CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceModel>> GetSalesInvoiceList(int salesInvoiceId)
        {
            IList<SalesInvoiceModel> salesInvoiceModelList = null;

            // create query.
            IQueryable<Salesinvoice> query = GetQueryByCondition(w => w.SalesInvoiceId != 0)
                                            .Include(w => w.CustomerLedger).Include(w => w.BillToAddress)
                                            .Include(w => w.AccountLedger).Include(w => w.BankLedger)
                                            .Include(w => w.TaxRegister).Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.SalesInvoiceId == salesInvoiceId);

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

                salesInvoiceModel.SalesInvoiceId = salesInvoice.SalesInvoiceId;
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
                salesInvoiceModel.NetAmountFcInWord = salesInvoice.NetAmountFcinWord;
                salesInvoiceModel.TaxAmountFc = salesInvoice.TaxAmountFc;
                salesInvoiceModel.TaxAmount = salesInvoice.TaxAmount;
                salesInvoiceModel.DiscountPercentageOrAmount = salesInvoice.DiscountPercentageOrAmount;
                salesInvoiceModel.DiscountPerOrAmountFc = salesInvoice.DiscountPerOrAmountFc;
                salesInvoiceModel.DiscountAmountFc = salesInvoice.DiscountAmountFc;
                salesInvoiceModel.DiscountAmount = salesInvoice.DiscountAmount;
                salesInvoiceModel.StatusId = salesInvoice.StatusId;
                salesInvoiceModel.CompanyId = Convert.ToInt32(salesInvoice.CompanyId);
                salesInvoiceModel.FinancialYearId = Convert.ToInt32(salesInvoice.FinancialYearId);
                salesInvoiceModel.MaxNo = salesInvoice.MaxNo;
                salesInvoiceModel.VoucherStyleId = salesInvoice.VoucherStyleId;



                //salesInvoiceModel.PreparedByUserId = salesInvoice.PreparedByUserId;
                //salesInvoiceModel.UpdatedByUserId = salesInvoice.UpdatedByUserId;



                //salesInvoiceModel.PreparedDateTime = salesInvoice.PreparedDateTime;
                //salesInvoiceModel.UpdatedDateTime = salesInvoice.UpdatedDateTime;



                // ###
                salesInvoiceModel.CustomerLedgerName = null != salesInvoice.CustomerLedger ? salesInvoice.CustomerLedger.LedgerName : null;
                salesInvoiceModel.BillToAddress = null != salesInvoice.BillToAddress ? salesInvoice.BillToAddress.AddressDescription : null;
                salesInvoiceModel.AccountLedgerName = null != salesInvoice.AccountLedger ? salesInvoice.AccountLedger.LedgerName : null;
                salesInvoiceModel.BankLedgerName = null != salesInvoice.BankLedger ? salesInvoice.BankLedger.LedgerName : null;
                salesInvoiceModel.TaxRegisterName = null != salesInvoice.TaxRegister ? salesInvoice.TaxRegister.TaxRegisterName : null;
                salesInvoiceModel.CurrencyCode = null != salesInvoice.Currency ? salesInvoice.Currency.CurrencyCode : null;
                salesInvoiceModel.StatusName = null != salesInvoice.Status ? salesInvoice.Status.StatusName : null;
                salesInvoiceModel.PreparedByName = null != salesInvoice.PreparedByUser ? salesInvoice.PreparedByUser.UserName : null;

                return salesInvoiceModel;
            });

        }

        #endregion Private Methods


    }


}
