using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace ERP.Services.Accounts
{
    public class PaymentVoucherService : Repository<Paymentvoucher>, IPaymentVoucher
    {
        ICommon common;

        public PaymentVoucherService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        public async Task<GenerateNoModel> GeneratePaymentVoucherNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 7;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

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

            paymentVoucher.StatusId = (int)DocumentStatus.Inprocess;
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

        public async Task<bool> UpdateStatusPaymentVoucher(int paymentVoucherId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucher paymentVoucher = await GetByIdAsync(w => w.PaymentVoucherId == paymentVoucherId);

            if (null != paymentVoucher)
            {
                paymentVoucher.StatusId = statusId;
                isUpdated = await Update(paymentVoucher);
            }

            return isUpdated; // returns.
        }

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

        public async Task<bool> UpdatePaymentVoucherMasterAmount(int paymentVoucherId)
        {
            bool isUpdated = false;

            Paymentvoucher paymentVoucher = null;

            //////// get record.

            paymentVoucher = await GetQueryByCondition(w => w.PaymentVoucherId == paymentVoucherId)
                                                       .Include(w => w.Paymentvoucherdetails)
                                                       .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != paymentVoucher)
            {
                paymentVoucher.AmountFc = paymentVoucher.Paymentvoucherdetails.Sum(w => w.AmountFc);
                paymentVoucher.Amount = paymentVoucher.AmountFc * paymentVoucher.ExchangeRate;

                paymentVoucher.AmountFcinWord = await common.AmountInWord_Million(paymentVoucher.AmountFc.ToString(), paymentVoucher.Currency.CurrencyCode, paymentVoucher.Currency.Denomination);

                if (paymentVoucher.StatusId == (int)DocumentStatus.Approved || paymentVoucher.StatusId == (int)DocumentStatus.ApprovalRequested || paymentVoucher.StatusId == (int)DocumentStatus.Cancelled)
                {
                    paymentVoucher.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(paymentVoucher);
            }

            return isUpdated; // returns.
        }

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

        private async Task<DataTableResultModel<PaymentVoucherModel>> GetDataFromDbase(SearchFilterPaymentVoucherModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<PaymentVoucherModel> resultModel = new DataTableResultModel<PaymentVoucherModel>();

            IQueryable<Paymentvoucher> query = GetQueryByCondition(w => w.PaymentVoucherId != 0)
                                                .Include(w => w.AccountLedger).Include(w => w.Currency)
                                                .Include(w => w.PreparedByUser).Include(w => w.Status); ;

            if (!string.IsNullOrEmpty(searchFilterModel.VoucherNo))
            {
                query = query.Where(w => w.VoucherNo.Contains(searchFilterModel.VoucherNo));
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.VoucherDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.VoucherDate <= searchFilterModel.ToDate);
            }
            
            if (!string.IsNullOrEmpty(searchFilterModel.TypeCorB))
            {
                query = query.Where(w => w.TypeCorB == searchFilterModel.TypeCorB);
            }

            if (null != searchFilterModel.LedgerId)
            {
                query = query.Where(w => w.AccountLedgerId == searchFilterModel.LedgerId);
            }


            if (!string.IsNullOrEmpty(searchFilterModel.ChequeNo))
            {
                query = query.Where(w => w.ChequeNo.Contains(searchFilterModel.ChequeNo));
            }


            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            
            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.VoucherNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new PaymentVoucherModel
            {
                PaymentVoucherId = s.PaymentVoucherId,
                VoucherNo = s.VoucherNo,
                VoucherDate = s.VoucherDate,
                AmountFc = s.AmountFc,
                TypeCorBName = EnumHelper.GetEnumDescription<TypeCorB>(s.TypeCorB),
                AccountLedgerName = s.AccountLedger.LedgerName,
                ChequeNo = s.ChequeNo,
                CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.Currency.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

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
            //if (0 != paymentVoucherId)
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
                paymentVoucherModel.AmountFcInWord = paymentVoucher.AmountFcinWord;

                paymentVoucherModel.StatusId = paymentVoucher.StatusId;
                paymentVoucherModel.CompanyId = Convert.ToInt32(paymentVoucher.CompanyId);
                paymentVoucherModel.FinancialYearId = Convert.ToInt32(paymentVoucher.FinancialYearId);
                paymentVoucherModel.MaxNo = paymentVoucher.MaxNo;
                paymentVoucherModel.VoucherStyleId = paymentVoucher.VoucherStyleId;

                // ###
                paymentVoucherModel.AccountLedgerName = null != paymentVoucher.AccountLedger ? paymentVoucher.AccountLedger.LedgerName : null;
                paymentVoucherModel.CurrencyCode = null != paymentVoucher.Currency ? paymentVoucher.Currency.CurrencyCode : null;
                paymentVoucherModel.StatusName = null != paymentVoucher.Status ? paymentVoucher.Status.StatusName : null;
                paymentVoucherModel.PreparedByName = null != paymentVoucher.PreparedByUser ? paymentVoucher.PreparedByUser.UserName : null;

                paymentVoucherModel.TypeCorBName = EnumHelper.GetEnumDescription<TypeCorB>(paymentVoucher.TypeCorB);
                paymentVoucherModel.PaymentTypeName = EnumHelper.GetEnumDescription<PaymentType>(((PaymentType)paymentVoucher.PaymentTypeId).ToString());

                return paymentVoucherModel;
            });

        }

        #endregion Private Methods
    }
}
