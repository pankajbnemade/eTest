using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class PaymentVoucherService : Repository<Paymentvoucher>, IPaymentVoucher
    {
        ICommon common;
        public PaymentVoucherService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
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
        public async Task<GenerateNoModel> GeneratePaymentVoucherNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 7;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => m.MaxNo);

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo(Convert.ToInt32(maxNo), voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        /// <summary>
        /// create new purchase invoice.
        /// </summary>
        /// <param name="paymentVoucherModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreatePaymentVoucher(PaymentVoucherModel paymentVoucherModel)
        {
            int paymentVoucherId = 0;

            GenerateNoModel generateNoModel = await GeneratePaymentVoucherNo(paymentVoucherModel.CompanyId, paymentVoucherModel.FinancialYearId);

            // assign values.
            Paymentvoucher paymentVoucher = new Paymentvoucher();

            paymentVoucher.VoucherNo = generateNoModel.VoucherNo;
            paymentVoucher.MaxNo = generateNoModel.MaxNo;
            paymentVoucher.VoucherStyleId = generateNoModel.VoucherStyleId;

            paymentVoucher.VoucherDate = paymentVoucherModel.VoucherDate;
            paymentVoucher.AccountLedgerId = paymentVoucherModel.AccountLedgerId;
            paymentVoucher.TypeCorB = paymentVoucherModel.TypeCorB;
            paymentVoucher.PaymentTypeId = paymentVoucherModel.PaymentTypeId;
            paymentVoucher.CurrencyId = paymentVoucherModel.CurrencyId;
            paymentVoucher.ExchangeRate = paymentVoucherModel.ExchangeRate;

            paymentVoucher.ChequeNo = paymentVoucherModel.ChequeNo;
            paymentVoucher.ChequeDate = paymentVoucherModel.ChequeDate;
            paymentVoucher.ChequeAmountFc = paymentVoucherModel.ChequeAmountFc;
            paymentVoucher.Narration = paymentVoucherModel.Narration;

            paymentVoucher.AmountFc = 0;
            paymentVoucher.Amount = 0;
            paymentVoucher.AmountFcinWord = "";

            paymentVoucher.StatusId = 1;
            paymentVoucher.CompanyId = paymentVoucherModel.CompanyId;
            paymentVoucher.FinancialYearId = paymentVoucherModel.FinancialYearId;

            await Create(paymentVoucher);
            paymentVoucherId = paymentVoucher.PaymentVoucherId;

            if (paymentVoucherId != 0)
            {
                await UpdatePaymentVoucherMasterAmount(paymentVoucherId);
            }

            return paymentVoucherId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="paymentVoucherModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdatePaymentVoucher(PaymentVoucherModel paymentVoucherModel)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucher paymentVoucher = await GetByIdAsync(w => w.PaymentVoucherId == paymentVoucherModel.PaymentVoucherId);

            if (null != paymentVoucher)
            {
                paymentVoucher.VoucherDate = paymentVoucherModel.VoucherDate;
                paymentVoucher.AccountLedgerId = paymentVoucherModel.AccountLedgerId;
                paymentVoucher.TypeCorB = paymentVoucherModel.TypeCorB;
                paymentVoucher.PaymentTypeId = paymentVoucherModel.PaymentTypeId;
                paymentVoucher.CurrencyId = paymentVoucherModel.CurrencyId;
                paymentVoucher.ExchangeRate = paymentVoucherModel.ExchangeRate;

                paymentVoucher.ChequeNo = paymentVoucherModel.ChequeNo;
                paymentVoucher.ChequeDate = paymentVoucherModel.ChequeDate;
                paymentVoucher.ChequeAmountFc = paymentVoucherModel.ChequeAmountFc;
                paymentVoucher.Narration = paymentVoucherModel.Narration;

                paymentVoucher.AmountFc = 0;
                paymentVoucher.Amount = 0;
                paymentVoucher.AmountFcinWord = "";

                isUpdated = await Update(paymentVoucher);
            }

            if (isUpdated != false)
            {
                await UpdatePaymentVoucherMasterAmount(paymentVoucher.PaymentVoucherId);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeletePaymentVoucher(int PaymentVoucherId)
        {
            bool isDeleted = false;

            // get record.
            Paymentvoucher paymentVoucher = await GetByIdAsync(w => w.PaymentVoucherId == PaymentVoucherId);
            
            if (null != paymentVoucher)
            {
                isDeleted = await Delete(paymentVoucher);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdatePaymentVoucherMasterAmount(int? paymentVoucherId)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucher paymentVoucher = await GetQueryByCondition(w => w.PaymentVoucherId == paymentVoucherId)
                                                    .Include(w => w.Paymentvoucherdetails)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != paymentVoucher)
            {
                paymentVoucher.AmountFc = paymentVoucher.Paymentvoucherdetails.Sum(w => w.AmountFc);
                paymentVoucher.Amount = paymentVoucher.AmountFc * paymentVoucher.ExchangeRate;

                paymentVoucher.AmountFcinWord = await common.AmountInWord_Million(paymentVoucher.AmountFc.ToString(), paymentVoucher.Currency.CurrencyCode, paymentVoucher.Currency.Denomination);

                isUpdated = await Update(paymentVoucher);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase invoice based on PaymentVoucherId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<PaymentVoucherModel> GetPaymentVoucherById(int paymentVoucherId)
        {
            PaymentVoucherModel paymentVoucherModel = null;

            IList<PaymentVoucherModel> paymentVoucherModelList = await GetPaymentVoucherList(paymentVoucherId);

            if (null != paymentVoucherModelList && paymentVoucherModelList.Any())
            {
                paymentVoucherModel = paymentVoucherModelList.FirstOrDefault();
            }

            return paymentVoucherModel; // returns.
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<PaymentVoucherModel>> GetPaymentVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterPaymentVoucherModel searchFilterModel)
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
            DataTableResultModel<PaymentVoucherModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

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
        private async Task<DataTableResultModel<PaymentVoucherModel>> GetDataFromDbase(SearchFilterPaymentVoucherModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<PaymentVoucherModel> resultModel = new DataTableResultModel<PaymentVoucherModel>();

            IQueryable<Paymentvoucher> query = GetQueryByCondition(w => w.PaymentVoucherId != 0);

            if (!string.IsNullOrEmpty(searchFilterModel.VoucherNo))
            {
                query = query.Where(w => w.VoucherNo.Contains(searchFilterModel.VoucherNo));
            }

            if (null != searchFilterModel.AccountLedgerId)
            {
                query = query.Where(w => w.AccountLedgerId == searchFilterModel.AccountLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.VoucherDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.VoucherDate <= searchFilterModel.ToDate);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);
            resultModel.ResultList = await query.Select(s => new PaymentVoucherModel
            {
                PaymentVoucherId = s.PaymentVoucherId,
                VoucherNo = s.VoucherNo,
                VoucherDate = s.VoucherDate,
                AmountFc = s.AmountFc,
            }).ToListAsync();
            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<PaymentVoucherModel>> GetPaymentVoucherList(int paymentVoucherId)
        {
            IList<PaymentVoucherModel> paymentVoucherModelList = null;

            // create query.
            IQueryable<Paymentvoucher> query = GetQueryByCondition(w => w.PaymentVoucherId != 0)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != paymentVoucherId)
                query = query.Where(w => w.PaymentVoucherId == paymentVoucherId);

            // get records by query.
            List<Paymentvoucher> paymentVoucherList = await query.ToListAsync();

            if (null != paymentVoucherList && paymentVoucherList.Count > 0)
            {
                paymentVoucherModelList = new List<PaymentVoucherModel>();

                foreach (Paymentvoucher paymentVoucher in paymentVoucherList)
                {
                    paymentVoucherModelList.Add(await AssignValueToModel(paymentVoucher));
                }
            }

            return paymentVoucherModelList; // returns.
        }

        private async Task<PaymentVoucherModel> AssignValueToModel(Paymentvoucher paymentVoucher)
        {
            return await Task.Run(() =>
            {
                PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();

                paymentVoucherModel.PaymentVoucherId = paymentVoucher.PaymentVoucherId;
                paymentVoucherModel.VoucherNo = paymentVoucher.VoucherNo;
                paymentVoucherModel.VoucherDate = paymentVoucher.VoucherDate;
                paymentVoucherModel.AccountLedgerId = paymentVoucher.AccountLedgerId;
                paymentVoucherModel.TypeCorB = paymentVoucher.TypeCorB;
                paymentVoucherModel.PaymentTypeId = paymentVoucher.PaymentTypeId;
                paymentVoucherModel.CurrencyId = paymentVoucher.CurrencyId;
                paymentVoucherModel.ExchangeRate = paymentVoucher.ExchangeRate;
                paymentVoucherModel.ChequeNo = paymentVoucher.ChequeNo;
                paymentVoucherModel.ChequeDate = paymentVoucher.ChequeDate;
                paymentVoucherModel.ChequeAmountFc = paymentVoucher.ChequeAmountFc;
                paymentVoucherModel.Narration = paymentVoucher.Narration;
                paymentVoucherModel.AmountFc = paymentVoucher.AmountFc;
                paymentVoucherModel.Amount = paymentVoucher.Amount;
                paymentVoucherModel.AmountFcinWord = paymentVoucher.AmountFcinWord;

                paymentVoucherModel.StatusId = paymentVoucher.StatusId;
                paymentVoucherModel.CompanyId = Convert.ToInt32(paymentVoucher.CompanyId);
                paymentVoucherModel.FinancialYearId = Convert.ToInt32(paymentVoucher.FinancialYearId);
                paymentVoucherModel.MaxNo = paymentVoucher.MaxNo;
                paymentVoucherModel.VoucherStyleId = paymentVoucher.VoucherStyleId;
                
                // ###
                paymentVoucherModel.AccountLedgerName = null != paymentVoucher.AccountLedger ? paymentVoucher.AccountLedger.LedgerName : null;
                paymentVoucherModel.CurrencyName = null != paymentVoucher.Currency ? paymentVoucher.Currency.CurrencyName : null;
                paymentVoucherModel.StatusName = null != paymentVoucher.Status ? paymentVoucher.Status.StatusName : null;
                paymentVoucherModel.PreparedByName = null != paymentVoucher.PreparedByUser ? paymentVoucher.PreparedByUser.UserName : null;
                //paymentVoucherModel.PaymentTypeName = null != paymentVoucher.PreparedByUser ? paymentVoucher.PreparedByUser.UserName : null;
                paymentVoucherModel.TypeCorB = "C" != paymentVoucher.TypeCorB ? "Cash" : "Bank";

                return paymentVoucherModel;
            });

        }

        #endregion Private Methods
    }
}
