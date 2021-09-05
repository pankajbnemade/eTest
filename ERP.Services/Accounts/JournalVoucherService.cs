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
    public class JournalVoucherService : Repository<Journalvoucher>, IJournalVoucher
    {
        ICommon common;
        public JournalVoucherService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
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
        public async Task<GenerateNoModel> GenerateJournalVoucherNo(int companyId, int financialYearId)
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
        /// <param name="journalVoucherModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreateJournalVoucher(JournalVoucherModel journalVoucherModel)
        {
            int journalVoucherId = 0;

            GenerateNoModel generateNoModel = await GenerateJournalVoucherNo(journalVoucherModel.CompanyId, journalVoucherModel.FinancialYearId);

            // assign values.
            Journalvoucher journalVoucher = new Journalvoucher();

            journalVoucher.VoucherNo = generateNoModel.VoucherNo;
            journalVoucher.MaxNo = generateNoModel.MaxNo;
            journalVoucher.VoucherStyleId = generateNoModel.VoucherStyleId;

            journalVoucher.VoucherDate = journalVoucherModel.VoucherDate;
            journalVoucher.CurrencyId = journalVoucherModel.CurrencyId;
            journalVoucher.ExchangeRate = journalVoucherModel.ExchangeRate;
            journalVoucher.Narration = journalVoucherModel.Narration;
            journalVoucher.AmountFc = journalVoucherModel.AmountFc;
            journalVoucher.Amount = 0;
            journalVoucher.AmountFcinWord = "";

            journalVoucher.DebitAmountFc = 0;
                journalVoucher.DebitAmount = 0;
                journalVoucher.CreditAmountFc = 0;
                journalVoucher.CreditAmount = 0;

            journalVoucher.StatusId = 1;
            journalVoucher.CompanyId = journalVoucherModel.CompanyId;
            journalVoucher.FinancialYearId = journalVoucherModel.FinancialYearId;

            await Create(journalVoucher);
            journalVoucherId = journalVoucher.JournalVoucherId;

            if (journalVoucherId != 0)
            {
                await UpdateJournalVoucherMasterAmount(journalVoucherId);
            }

            return journalVoucherId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="journalVoucherModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateJournalVoucher(JournalVoucherModel journalVoucherModel)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucher journalVoucher = await GetByIdAsync(w => w.JournalVoucherId == journalVoucherModel.JournalVoucherId);

            if (null != journalVoucher)
            {
                journalVoucher.VoucherDate = journalVoucherModel.VoucherDate;
                journalVoucher.CurrencyId = journalVoucherModel.CurrencyId;
                journalVoucher.ExchangeRate = journalVoucherModel.ExchangeRate;
                journalVoucher.Narration = journalVoucherModel.Narration;
                journalVoucher.AmountFc = journalVoucherModel.AmountFc;
                journalVoucher.Amount = 0;
                journalVoucher.AmountFcinWord = "";

                journalVoucher.DebitAmountFc = 0;
                journalVoucher.DebitAmount = 0;
                journalVoucher.CreditAmountFc = 0;
                journalVoucher.CreditAmount = 0;

                isUpdated = await Update(journalVoucher);
            }

            if (isUpdated != false)
            {
                await UpdateJournalVoucherMasterAmount(journalVoucher.JournalVoucherId);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="journalVoucherId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteJournalVoucher(int JournalVoucherId)
        {
            bool isDeleted = false;

            // get record.
            Journalvoucher journalVoucher = await GetByIdAsync(w => w.JournalVoucherId == JournalVoucherId);

            if (null != journalVoucher)
            {
                isDeleted = await Delete(journalVoucher);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateJournalVoucherMasterAmount(int? journalVoucherId)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucher journalVoucher = await GetQueryByCondition(w => w.JournalVoucherId == journalVoucherId)
                                                    .Include(w => w.Journalvoucherdetails)
                                                    .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != journalVoucher)
            {
                journalVoucher.DebitAmountFc = journalVoucher.Journalvoucherdetails.Sum(w => w.DebitAmountFc);
                journalVoucher.DebitAmount = journalVoucher.DebitAmountFc * journalVoucher.ExchangeRate;

                journalVoucher.CreditAmountFc = journalVoucher.Journalvoucherdetails.Sum(w => w.CreditAmountFc);
                journalVoucher.CreditAmount = journalVoucher.CreditAmountFc * journalVoucher.ExchangeRate;
                journalVoucher.Amount = journalVoucher.AmountFc * journalVoucher.ExchangeRate;

                journalVoucher.AmountFcinWord = await common.AmountInWord_Million(journalVoucher.AmountFc.ToString(), journalVoucher.Currency.CurrencyCode, journalVoucher.Currency.Denomination);

                isUpdated = await Update(journalVoucher);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// get purchase invoice based on JournalVoucherId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<JournalVoucherModel> GetJournalVoucherById(int journalVoucherId)
        {
            JournalVoucherModel journalVoucherModel = null;

            IList<JournalVoucherModel> journalVoucherModelList = await GetJournalVoucherList(journalVoucherId);

            if (null != journalVoucherModelList && journalVoucherModelList.Any())
            {
                journalVoucherModel = journalVoucherModelList.FirstOrDefault();
            }

            return journalVoucherModel; // returns.
        }

        /// <summary>
        /// get search purchase invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<JournalVoucherModel>> GetJournalVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterJournalVoucherModel searchFilterModel)
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
            DataTableResultModel<JournalVoucherModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

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
        private async Task<DataTableResultModel<JournalVoucherModel>> GetDataFromDbase(SearchFilterJournalVoucherModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<JournalVoucherModel> resultModel = new DataTableResultModel<JournalVoucherModel>();

            IQueryable<Journalvoucher> query = GetQueryByCondition(w => w.JournalVoucherId != 0);

            if (!string.IsNullOrEmpty(searchFilterModel.VoucherNo))
            {
                query = query.Where(w => w.VoucherNo.Contains(searchFilterModel.VoucherNo));
            }

            //if (null != searchFilterModel.AccountLedgerId)
            //{
            //    query = query.Where(w => w.AccountLedgerId == searchFilterModel.AccountLedgerId);
            //}

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
            resultModel.ResultList = await query.Select(s => new JournalVoucherModel
            {
                JournalVoucherId = s.JournalVoucherId,
                VoucherNo = s.VoucherNo,
                VoucherDate = s.VoucherDate,
                AmountFc = s.AmountFc,
            }).ToListAsync();
            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<JournalVoucherModel>> GetJournalVoucherList(int journalVoucherId)
        {
            IList<JournalVoucherModel> journalVoucherModelList = null;

            // create query.
            IQueryable<Journalvoucher> query = GetQueryByCondition(w => w.JournalVoucherId != 0)
                                            //.Include(w => w.AccountLedger)
                                            .Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            if (0 != journalVoucherId)
                query = query.Where(w => w.JournalVoucherId == journalVoucherId);

            // get records by query.
            List<Journalvoucher> journalVoucherList = await query.ToListAsync();

            if (null != journalVoucherList && journalVoucherList.Count > 0)
            {
                journalVoucherModelList = new List<JournalVoucherModel>();

                foreach (Journalvoucher journalVoucher in journalVoucherList)
                {
                    journalVoucherModelList.Add(await AssignValueToModel(journalVoucher));
                }
            }

            return journalVoucherModelList; // returns.
        }

        private async Task<JournalVoucherModel> AssignValueToModel(Journalvoucher journalVoucher)
        {
            return await Task.Run(() =>
            {
                JournalVoucherModel journalVoucherModel = new JournalVoucherModel();

                journalVoucherModel.JournalVoucherId = journalVoucher.JournalVoucherId;
                journalVoucherModel.VoucherNo = journalVoucher.VoucherNo;
                journalVoucherModel.VoucherDate = journalVoucher.VoucherDate;
                journalVoucherModel.CurrencyId = journalVoucher.CurrencyId;
                journalVoucherModel.ExchangeRate = journalVoucher.ExchangeRate;
                journalVoucherModel.Narration = journalVoucher.Narration;
                journalVoucherModel.AmountFc = journalVoucher.AmountFc;
                journalVoucherModel.Amount = journalVoucher.Amount;
                journalVoucherModel.AmountFcinWord = journalVoucher.AmountFcinWord;

                journalVoucherModel.StatusId = journalVoucher.StatusId;
                journalVoucherModel.CompanyId = Convert.ToInt32(journalVoucher.CompanyId);
                journalVoucherModel.FinancialYearId = Convert.ToInt32(journalVoucher.FinancialYearId);
                journalVoucherModel.MaxNo = journalVoucher.MaxNo;
                journalVoucherModel.VoucherStyleId = journalVoucher.VoucherStyleId;

                // ###
                journalVoucherModel.CurrencyName = null != journalVoucher.Currency ? journalVoucher.Currency.CurrencyName : null;
                journalVoucherModel.StatusName = null != journalVoucher.Status ? journalVoucher.Status.StatusName : null;
                journalVoucherModel.PreparedByName = null != journalVoucher.PreparedByUser ? journalVoucher.PreparedByUser.UserName : null;

                return journalVoucherModel;
            });

        }

        #endregion Private Methods
    }
}
