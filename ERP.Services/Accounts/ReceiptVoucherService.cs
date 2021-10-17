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
    public class ReceiptVoucherService : Repository<Receiptvoucher>, IReceiptVoucher
    {
        ICommon common;

        public ReceiptVoucherService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        public async Task<GenerateNoModel> GenerateReceiptVoucherNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 6;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        public async Task<int> CreateReceiptVoucher(ReceiptVoucherModel receiptVoucherModel)
        {
            int receiptVoucherId = 0;

            GenerateNoModel generateNoModel = await GenerateReceiptVoucherNo(receiptVoucherModel.CompanyId, receiptVoucherModel.FinancialYearId);

            // assign values.
            Receiptvoucher receiptVoucher = new Receiptvoucher();

            receiptVoucher.VoucherNo = generateNoModel.VoucherNo;
            receiptVoucher.MaxNo = generateNoModel.MaxNo;
            receiptVoucher.VoucherStyleId = generateNoModel.VoucherStyleId;

            receiptVoucher.VoucherDate = receiptVoucherModel.VoucherDate;
            receiptVoucher.AccountLedgerId = receiptVoucherModel.AccountLedgerId;
            receiptVoucher.TypeCorB = receiptVoucherModel.TypeCorB;
            receiptVoucher.PaymentTypeId = receiptVoucherModel.PaymentTypeId;
            receiptVoucher.CurrencyId = receiptVoucherModel.CurrencyId;
            receiptVoucher.ExchangeRate = receiptVoucherModel.ExchangeRate;

            receiptVoucher.ChequeNo = receiptVoucherModel.ChequeNo;
            receiptVoucher.ChequeDate = receiptVoucherModel.ChequeDate;
            receiptVoucher.ChequeAmountFc = receiptVoucherModel.ChequeAmountFc;
            receiptVoucher.Narration = receiptVoucherModel.Narration;

            receiptVoucher.AmountFc = 0;
            receiptVoucher.Amount = 0;
            receiptVoucher.AmountFcinWord = "";

            receiptVoucher.StatusId = (int)DocumentStatus.Inprocess;
            receiptVoucher.CompanyId = receiptVoucherModel.CompanyId;
            receiptVoucher.FinancialYearId = receiptVoucherModel.FinancialYearId;

            await Create(receiptVoucher);
            receiptVoucherId = receiptVoucher.ReceiptVoucherId;

            if (receiptVoucherId != 0)
            {
                await UpdateReceiptVoucherMasterAmount(receiptVoucherId);
            }

            return receiptVoucherId; // returns.
        }

        public async Task<bool> UpdateReceiptVoucher(ReceiptVoucherModel receiptVoucherModel)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucher receiptVoucher = await GetByIdAsync(w => w.ReceiptVoucherId == receiptVoucherModel.ReceiptVoucherId);

            if (null != receiptVoucher)
            {
                receiptVoucher.VoucherDate = receiptVoucherModel.VoucherDate;
                receiptVoucher.AccountLedgerId = receiptVoucherModel.AccountLedgerId;
                receiptVoucher.TypeCorB = receiptVoucherModel.TypeCorB;
                receiptVoucher.PaymentTypeId = receiptVoucherModel.PaymentTypeId;
                receiptVoucher.CurrencyId = receiptVoucherModel.CurrencyId;
                receiptVoucher.ExchangeRate = receiptVoucherModel.ExchangeRate;

                receiptVoucher.ChequeNo = receiptVoucherModel.ChequeNo;
                receiptVoucher.ChequeDate = receiptVoucherModel.ChequeDate;
                receiptVoucher.ChequeAmountFc = receiptVoucherModel.ChequeAmountFc;
                receiptVoucher.Narration = receiptVoucherModel.Narration;

                receiptVoucher.AmountFc = 0;
                receiptVoucher.Amount = 0;
                receiptVoucher.AmountFcinWord = "";

                isUpdated = await Update(receiptVoucher);
            }

            if (isUpdated != false)
            {
                await UpdateReceiptVoucherMasterAmount(receiptVoucher.ReceiptVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateStatusReceiptVoucher(int receiptVoucherId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucher receiptVoucher = await GetByIdAsync(w => w.ReceiptVoucherId == receiptVoucherId);

            if (null != receiptVoucher)
            {
                receiptVoucher.StatusId = statusId;
                isUpdated = await Update(receiptVoucher);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteReceiptVoucher(int ReceiptVoucherId)
        {
            bool isDeleted = false;

            // get record.
            Receiptvoucher receiptVoucher = await GetByIdAsync(w => w.ReceiptVoucherId == ReceiptVoucherId);

            if (null != receiptVoucher)
            {
                isDeleted = await Delete(receiptVoucher);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateReceiptVoucherMasterAmount(int receiptVoucherId)
        {
            bool isUpdated = false;

            Receiptvoucher receiptVoucher = null;

            //////// get record.

            receiptVoucher = await GetQueryByCondition(w => w.ReceiptVoucherId == receiptVoucherId)
                                                       .Include(w => w.Receiptvoucherdetails)
                                                       .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != receiptVoucher)
            {
                receiptVoucher.AmountFc = receiptVoucher.Receiptvoucherdetails.Sum(w => w.AmountFc);
                receiptVoucher.Amount = receiptVoucher.AmountFc * receiptVoucher.ExchangeRate;

                receiptVoucher.AmountFcinWord = await common.AmountInWord_Million(receiptVoucher.AmountFc.ToString(), receiptVoucher.Currency.CurrencyCode, receiptVoucher.Currency.Denomination);

                if (receiptVoucher.StatusId == (int)DocumentStatus.Approved || receiptVoucher.StatusId == (int)DocumentStatus.ApprovalRequested || receiptVoucher.StatusId == (int)DocumentStatus.Cancelled)
                {
                    receiptVoucher.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(receiptVoucher);
            }

            return isUpdated; // returns.
        }

        public async Task<ReceiptVoucherModel> GetReceiptVoucherById(int receiptVoucherId)
        {
            ReceiptVoucherModel receiptVoucherModel = null;

            IList<ReceiptVoucherModel> receiptVoucherModelList = await GetReceiptVoucherList(receiptVoucherId);

            if (null != receiptVoucherModelList && receiptVoucherModelList.Any())
            {
                receiptVoucherModel = receiptVoucherModelList.FirstOrDefault();
            }

            return receiptVoucherModel; // returns.
        }

        public async Task<DataTableResultModel<ReceiptVoucherModel>> GetReceiptVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterReceiptVoucherModel searchFilterModel)
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
            DataTableResultModel<ReceiptVoucherModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        #region Private Methods

        private async Task<DataTableResultModel<ReceiptVoucherModel>> GetDataFromDbase(SearchFilterReceiptVoucherModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<ReceiptVoucherModel> resultModel = new DataTableResultModel<ReceiptVoucherModel>();

            IQueryable<Receiptvoucher> query = GetQueryByCondition(w => w.ReceiptVoucherId != 0)
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

            resultModel.ResultList = await query.Select(s => new ReceiptVoucherModel
            {
                ReceiptVoucherId = s.ReceiptVoucherId,
                VoucherNo = s.VoucherNo,
                VoucherDate = s.VoucherDate,
                AmountFc = s.AmountFc,
                TypeCorBName = EnumHelper.GetEnumDescription<TypeCorB>(s.TypeCorB),
                AccountLedgerName = s.AccountLedger.LedgerName,
                ChequeNo = s.ChequeNo,
                CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<ReceiptVoucherModel>> GetReceiptVoucherList(int receiptVoucherId)
        {
            IList<ReceiptVoucherModel> receiptVoucherModelList = null;

            // create query.
            IQueryable<Receiptvoucher> query = GetQueryByCondition(w => w.ReceiptVoucherId != 0)
                                            .Include(w => w.AccountLedger)
                                            .Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            //if (0 != receiptVoucherId)
            query = query.Where(w => w.ReceiptVoucherId == receiptVoucherId);

            // get records by query.
            List<Receiptvoucher> receiptVoucherList = await query.ToListAsync();

            if (null != receiptVoucherList && receiptVoucherList.Count > 0)
            {
                receiptVoucherModelList = new List<ReceiptVoucherModel>();

                foreach (Receiptvoucher receiptVoucher in receiptVoucherList)
                {
                    receiptVoucherModelList.Add(await AssignValueToModel(receiptVoucher));
                }
            }

            return receiptVoucherModelList; // returns.
        }

        private async Task<ReceiptVoucherModel> AssignValueToModel(Receiptvoucher receiptVoucher)
        {
            return await Task.Run(() =>
            {
                ReceiptVoucherModel receiptVoucherModel = new ReceiptVoucherModel();

                receiptVoucherModel.ReceiptVoucherId = receiptVoucher.ReceiptVoucherId;
                receiptVoucherModel.VoucherNo = receiptVoucher.VoucherNo;
                receiptVoucherModel.VoucherDate = receiptVoucher.VoucherDate;
                receiptVoucherModel.AccountLedgerId = receiptVoucher.AccountLedgerId;
                receiptVoucherModel.TypeCorB = receiptVoucher.TypeCorB;
                receiptVoucherModel.PaymentTypeId = receiptVoucher.PaymentTypeId;
                receiptVoucherModel.CurrencyId = receiptVoucher.CurrencyId;
                receiptVoucherModel.ExchangeRate = receiptVoucher.ExchangeRate;
                receiptVoucherModel.ChequeNo = receiptVoucher.ChequeNo;
                receiptVoucherModel.ChequeDate = receiptVoucher.ChequeDate;
                receiptVoucherModel.ChequeAmountFc = receiptVoucher.ChequeAmountFc;
                receiptVoucherModel.Narration = receiptVoucher.Narration;
                receiptVoucherModel.AmountFc = receiptVoucher.AmountFc;
                receiptVoucherModel.Amount = receiptVoucher.Amount;
                receiptVoucherModel.AmountFcInWord = receiptVoucher.AmountFcinWord;

                receiptVoucherModel.StatusId = receiptVoucher.StatusId;
                receiptVoucherModel.CompanyId = Convert.ToInt32(receiptVoucher.CompanyId);
                receiptVoucherModel.FinancialYearId = Convert.ToInt32(receiptVoucher.FinancialYearId);
                receiptVoucherModel.MaxNo = receiptVoucher.MaxNo;
                receiptVoucherModel.VoucherStyleId = receiptVoucher.VoucherStyleId;

                // ###
                receiptVoucherModel.AccountLedgerName = null != receiptVoucher.AccountLedger ? receiptVoucher.AccountLedger.LedgerName : null;
                receiptVoucherModel.CurrencyCode = null != receiptVoucher.Currency ? receiptVoucher.Currency.CurrencyCode : null;
                receiptVoucherModel.StatusName = null != receiptVoucher.Status ? receiptVoucher.Status.StatusName : null;
                receiptVoucherModel.PreparedByName = null != receiptVoucher.PreparedByUser ? receiptVoucher.PreparedByUser.UserName : null;

                receiptVoucherModel.TypeCorBName = EnumHelper.GetEnumDescription<TypeCorB>(receiptVoucher.TypeCorB);
                receiptVoucherModel.PaymentTypeName = EnumHelper.GetEnumDescription<PaymentType>(((PaymentType)receiptVoucher.PaymentTypeId).ToString());

                return receiptVoucherModel;
            });

        }

        #endregion Private Methods
    }
}
