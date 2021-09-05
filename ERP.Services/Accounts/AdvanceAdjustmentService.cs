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
    public class AdvanceAdjustmentService : Repository<Advanceadjustment>, IAdvanceAdjustment
    {
        ICommon common;
        public AdvanceAdjustmentService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
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
        public async Task<GenerateNoModel> GenerateAdvanceAdjustmentNo(int companyId, int financialYearId)
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
        /// <param name="advanceAdjustmentModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel)
        {
            int advanceAdjustmentId = 0;

            GenerateNoModel generateNoModel = await GenerateAdvanceAdjustmentNo(advanceAdjustmentModel.CompanyId, advanceAdjustmentModel.FinancialYearId);

            // assign values.
            Advanceadjustment advanceAdjustment = new Advanceadjustment();

            advanceAdjustment.AdvanceAdjustmentNo = generateNoModel.VoucherNo;
            advanceAdjustment.MaxNo = generateNoModel.MaxNo;
            advanceAdjustment.VoucherStyleId = generateNoModel.VoucherStyleId;

            advanceAdjustment.AdvanceAdjustmentDate = advanceAdjustmentModel.AdvanceAdjustmentDate;
            advanceAdjustment.AccountLedgerId = advanceAdjustmentModel.AccountLedgerId;
            advanceAdjustment.PaymentVoucherId = advanceAdjustmentModel.PaymentVoucherId;
            advanceAdjustment.ReceiptVoucherId = advanceAdjustmentModel.ReceiptVoucherId;
            advanceAdjustment.CurrencyId = advanceAdjustmentModel.CurrencyId;
            advanceAdjustment.ExchangeRate = advanceAdjustmentModel.ExchangeRate;
            advanceAdjustment.Narration = advanceAdjustmentModel.Narration;
            advanceAdjustment.AmountFc = advanceAdjustmentModel.AmountFc;
            advanceAdjustment.Amount = 0;
            advanceAdjustment.AmountFcinWord = "";

            advanceAdjustment.StatusId = 1;
            advanceAdjustment.CompanyId = advanceAdjustmentModel.CompanyId;
            advanceAdjustment.FinancialYearId = advanceAdjustmentModel.FinancialYearId;

            await Create(advanceAdjustment);
            advanceAdjustmentId = advanceAdjustment.AdvanceAdjustmentId;

            if (advanceAdjustmentId != 0)
            {
                await UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentId);
            }

            return advanceAdjustmentId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="advanceAdjustmentModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustment advanceAdjustment = await GetByIdAsync(w => w.AdvanceAdjustmentId == advanceAdjustmentModel.AdvanceAdjustmentId);

            if (null != advanceAdjustment)
            {
                advanceAdjustment.AdvanceAdjustmentDate = advanceAdjustmentModel.AdvanceAdjustmentDate;
                advanceAdjustment.AccountLedgerId = advanceAdjustmentModel.AccountLedgerId;
                advanceAdjustment.PaymentVoucherId = advanceAdjustmentModel.PaymentVoucherId;
                advanceAdjustment.ReceiptVoucherId = advanceAdjustmentModel.ReceiptVoucherId;
                advanceAdjustment.CurrencyId = advanceAdjustmentModel.CurrencyId;
                advanceAdjustment.ExchangeRate = advanceAdjustmentModel.ExchangeRate;
                advanceAdjustment.Narration = advanceAdjustmentModel.Narration;
                advanceAdjustment.AmountFc = advanceAdjustmentModel.AmountFc;
                advanceAdjustment.Amount = 0;
                advanceAdjustment.AmountFcinWord = "";

                advanceAdjustment.AmountFc = 0;
                advanceAdjustment.Amount = 0;
                advanceAdjustment.AmountFcinWord = "";

                isUpdated = await Update(advanceAdjustment);
            }

            if (isUpdated != false)
            {
                await UpdateAdvanceAdjustmentMasterAmount(advanceAdjustment.AdvanceAdjustmentId);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="advanceAdjustmentId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteAdvanceAdjustment(int AdvanceAdjustmentId)
        {
            bool isDeleted = false;

            // get record.
            Advanceadjustment advanceAdjustment = await GetByIdAsync(w => w.AdvanceAdjustmentId == AdvanceAdjustmentId);

            if (null != advanceAdjustment)
            {
                isDeleted = await Delete(advanceAdjustment);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateAdvanceAdjustmentMasterAmount(int? advanceAdjustmentId)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustment advanceAdjustment = await GetQueryByCondition(w => w.AdvanceAdjustmentId == advanceAdjustmentId)
                                                    .Include(w => w.Advanceadjustmentdetails)
                                                    .Include(w => w.Currency)
                                                    .FirstOrDefaultAsync();

            if (null != advanceAdjustment)
            {
                advanceAdjustment.AmountFc = advanceAdjustment.Advanceadjustmentdetails.Sum(w => w.AmountFc);
                advanceAdjustment.Amount = advanceAdjustment.AmountFc * advanceAdjustment.ExchangeRate;

                advanceAdjustment.AmountFcinWord = await common.AmountInWord_Million(advanceAdjustment.AmountFc.ToString(), advanceAdjustment.PaymentVoucher.Currency.CurrencyCode, advanceAdjustment.PaymentVoucher.Currency.Denomination);

                isUpdated = await Update(advanceAdjustment);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase invoice based on AdvanceAdjustmentId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<AdvanceAdjustmentModel> GetAdvanceAdjustmentById(int advanceAdjustmentId)
        {
            AdvanceAdjustmentModel advanceAdjustmentModel = null;

            IList<AdvanceAdjustmentModel> advanceAdjustmentModelList = await GetAdvanceAdjustmentList(advanceAdjustmentId);

            if (null != advanceAdjustmentModelList && advanceAdjustmentModelList.Any())
            {
                advanceAdjustmentModel = advanceAdjustmentModelList.FirstOrDefault();
            }

            return advanceAdjustmentModel; // returns.
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<AdvanceAdjustmentModel>> GetAdvanceAdjustmentList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterAdvanceAdjustmentModel searchFilterModel)
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
            DataTableResultModel<AdvanceAdjustmentModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

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
        private async Task<DataTableResultModel<AdvanceAdjustmentModel>> GetDataFromDbase(SearchFilterAdvanceAdjustmentModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<AdvanceAdjustmentModel> resultModel = new DataTableResultModel<AdvanceAdjustmentModel>();

            IQueryable<Advanceadjustment> query = GetQueryByCondition(w => w.AdvanceAdjustmentId != 0);

            if (!string.IsNullOrEmpty(searchFilterModel.AdvanceAdjustmentNo))
            {
                query = query.Where(w => w.AdvanceAdjustmentNo.Contains(searchFilterModel.AdvanceAdjustmentNo));
            }

            if (null != searchFilterModel.AccountLedgerId)
            {
                query = query.Where(w => w.AccountLedgerId == searchFilterModel.AccountLedgerId);
            }

            if (null != searchFilterModel.FromDate)
            {
                query = query.Where(w => w.AdvanceAdjustmentDate >= searchFilterModel.FromDate);
            }

            if (null != searchFilterModel.ToDate)
            {
                query = query.Where(w => w.AdvanceAdjustmentDate <= searchFilterModel.ToDate);
            }

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);
            resultModel.ResultList = await query.Select(s => new AdvanceAdjustmentModel
            {
                AdvanceAdjustmentId = s.AdvanceAdjustmentId,
                AdvanceAdjustmentNo = s.AdvanceAdjustmentNo,
                AdvanceAdjustmentDate = s.AdvanceAdjustmentDate,
                AmountFc = s.AmountFc,
            }).ToListAsync();
            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<AdvanceAdjustmentModel>> GetAdvanceAdjustmentList(int advanceAdjustmentId)
        {
            IList<AdvanceAdjustmentModel> advanceAdjustmentModelList = null;

            // create query.
            IQueryable<Advanceadjustment> query = GetQueryByCondition(w => w.AdvanceAdjustmentId != 0)
                                            .Include(w => w.AccountLedger)
                                            //.Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != advanceAdjustmentId)
                query = query.Where(w => w.AdvanceAdjustmentId == advanceAdjustmentId);

            // get records by query.
            List<Advanceadjustment> advanceAdjustmentList = await query.ToListAsync();

            if (null != advanceAdjustmentList && advanceAdjustmentList.Count > 0)
            {
                advanceAdjustmentModelList = new List<AdvanceAdjustmentModel>();

                foreach (Advanceadjustment advanceAdjustment in advanceAdjustmentList)
                {
                    advanceAdjustmentModelList.Add(await AssignValueToModel(advanceAdjustment));
                }
            }

            return advanceAdjustmentModelList; // returns.
        }

        private async Task<AdvanceAdjustmentModel> AssignValueToModel(Advanceadjustment advanceAdjustment)
        {
            return await Task.Run(() =>
            {
                AdvanceAdjustmentModel advanceAdjustmentModel = new AdvanceAdjustmentModel();

                advanceAdjustmentModel.AdvanceAdjustmentId = advanceAdjustment.AdvanceAdjustmentId;
                advanceAdjustmentModel.AdvanceAdjustmentNo = advanceAdjustment.AdvanceAdjustmentNo;
                advanceAdjustmentModel.AdvanceAdjustmentDate = advanceAdjustment.AdvanceAdjustmentDate;
                advanceAdjustmentModel.AccountLedgerId = advanceAdjustment.AccountLedgerId;
                advanceAdjustmentModel.PaymentVoucherId = advanceAdjustment.PaymentVoucherId;
                advanceAdjustmentModel.ReceiptVoucherId = advanceAdjustment.ReceiptVoucherId;
                advanceAdjustmentModel.CurrencyId = advanceAdjustment.CurrencyId;
                advanceAdjustmentModel.ExchangeRate = advanceAdjustment.ExchangeRate;
                advanceAdjustmentModel.AmountFc = advanceAdjustment.AmountFc;
                advanceAdjustmentModel.Narration = advanceAdjustment.Narration;
                advanceAdjustmentModel.AmountFc = advanceAdjustment.AmountFc;
                advanceAdjustmentModel.Amount = advanceAdjustment.Amount;
                advanceAdjustmentModel.AmountFcinWord = advanceAdjustment.AmountFcinWord;

                advanceAdjustmentModel.StatusId = advanceAdjustment.StatusId;
                advanceAdjustmentModel.CompanyId = Convert.ToInt32(advanceAdjustment.CompanyId);
                advanceAdjustmentModel.FinancialYearId = Convert.ToInt32(advanceAdjustment.FinancialYearId);
                advanceAdjustmentModel.MaxNo = advanceAdjustment.MaxNo;
                advanceAdjustmentModel.VoucherStyleId = advanceAdjustment.VoucherStyleId;

                // ###
                advanceAdjustmentModel.StatusName = null != advanceAdjustment.Status ? advanceAdjustment.Status.StatusName : null;
                advanceAdjustmentModel.PreparedByName = null != advanceAdjustment.PreparedByUser ? advanceAdjustment.PreparedByUser.UserName : null;

                return advanceAdjustmentModel;
            });

        }

        #endregion Private Methods
    }
}
