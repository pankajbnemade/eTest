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
    public class JournalVoucherService : Repository<Journalvoucher>, IJournalVoucher
    {
        ICommon common;

        public JournalVoucherService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        public async Task<GenerateNoModel> GenerateJournalVoucherNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 8;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

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

            journalVoucher.CreditAmountFc = 0;
            journalVoucher.CreditAmount = 0;
            journalVoucher.DebitAmountFc = 0;
            journalVoucher.DebitAmount = 0;

            journalVoucher.StatusId = (int)DocumentStatus.Inprocess;
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

                journalVoucher.CreditAmountFc = 0;
                journalVoucher.CreditAmount = 0;
                journalVoucher.DebitAmountFc = 0;
                journalVoucher.DebitAmount = 0;

                isUpdated = await Update(journalVoucher);
            }

            if (isUpdated != false)
            {
                await UpdateJournalVoucherMasterAmount(journalVoucher.JournalVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateStatusJournalVoucher(int journalVoucherId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Journalvoucher journalVoucher = await GetByIdAsync(w => w.JournalVoucherId == journalVoucherId);

            if (null != journalVoucher)
            {
                journalVoucher.StatusId = statusId;
                isUpdated = await Update(journalVoucher);
            }

            return isUpdated; // returns.
        }

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

        public async Task<bool> UpdateJournalVoucherMasterAmount(int journalVoucherId)
        {
            bool isUpdated = false;

            Journalvoucher journalVoucher = null;

            //////// get record.

            journalVoucher = await GetQueryByCondition(w => w.JournalVoucherId == journalVoucherId)
                                                       .Include(w => w.Journalvoucherdetails)
                                                       .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != journalVoucher)
            {
                journalVoucher.CreditAmountFc = journalVoucher.Journalvoucherdetails.Sum(w => w.CreditAmountFc);
                journalVoucher.CreditAmount = journalVoucher.CreditAmountFc / journalVoucher.ExchangeRate;

                journalVoucher.DebitAmountFc = journalVoucher.Journalvoucherdetails.Sum(w => w.DebitAmountFc);
                journalVoucher.DebitAmount = journalVoucher.DebitAmountFc / journalVoucher.ExchangeRate;

                journalVoucher.AmountFcinWord = await common.AmountInWord_Million(journalVoucher.AmountFc.ToString(), journalVoucher.Currency.CurrencyCode, journalVoucher.Currency.Denomination);

                if (journalVoucher.StatusId == (int)DocumentStatus.Approved || journalVoucher.StatusId == (int)DocumentStatus.ApprovalRequested || journalVoucher.StatusId == (int)DocumentStatus.Cancelled)
                {
                    journalVoucher.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(journalVoucher);
            }

            return isUpdated; // returns.
        }

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

        private async Task<DataTableResultModel<JournalVoucherModel>> GetDataFromDbase(SearchFilterJournalVoucherModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<JournalVoucherModel> resultModel = new DataTableResultModel<JournalVoucherModel>();

            IQueryable<Journalvoucher> query = GetQueryByCondition(w => w.JournalVoucherId != 0)
                                                .Include(w => w.Currency)
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

            // get total count.
            resultModel.TotalResultCount = await query.CountAsync();


            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.VoucherNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);

            resultModel.ResultList = await query.Select(s => new JournalVoucherModel
            {
                JournalVoucherId = s.JournalVoucherId,
                VoucherNo = s.VoucherNo,
                VoucherDate = s.VoucherDate,
                AmountFc = s.AmountFc,
                CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<JournalVoucherModel>> GetJournalVoucherList(int journalVoucherId)
        {
            IList<JournalVoucherModel> journalVoucherModelList = null;

            // create query.
            IQueryable<Journalvoucher> query = GetQueryByCondition(w => w.JournalVoucherId != 0)
                                            .Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            //if (0 != journalVoucherId)
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
                journalVoucherModel.AmountFcInWord = journalVoucher.AmountFcinWord;

                journalVoucherModel.CreditAmountFc = journalVoucher.CreditAmountFc;
                journalVoucherModel.CreditAmount = journalVoucher.CreditAmount;
                journalVoucherModel.DebitAmountFc = journalVoucher.DebitAmountFc;
                journalVoucherModel.DebitAmount = journalVoucher.DebitAmount;

                journalVoucherModel.StatusId = journalVoucher.StatusId;
                journalVoucherModel.CompanyId = Convert.ToInt32(journalVoucher.CompanyId);
                journalVoucherModel.FinancialYearId = Convert.ToInt32(journalVoucher.FinancialYearId);
                journalVoucherModel.MaxNo = journalVoucher.MaxNo;
                journalVoucherModel.VoucherStyleId = journalVoucher.VoucherStyleId;

                // ###
                journalVoucherModel.CurrencyCode = null != journalVoucher.Currency ? journalVoucher.Currency.CurrencyCode : null;
                journalVoucherModel.StatusName = null != journalVoucher.Status ? journalVoucher.Status.StatusName : null;
                journalVoucherModel.PreparedByName = null != journalVoucher.PreparedByUser ? journalVoucher.PreparedByUser.UserName : null;

                return journalVoucherModel;
            });

        }

        #endregion Private Methods
    }
}
