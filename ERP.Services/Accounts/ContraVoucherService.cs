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
    public class ContraVoucherService : Repository<Contravoucher>, IContraVoucher
    {
        ICommon common;

        public ContraVoucherService(ErpDbContext dbContext, ICommon _common) : base(dbContext)
        {
            common = _common;
        }

        public async Task<GenerateNoModel> GenerateContraVoucherNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 9;
            // get maxno.
            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

        public async Task<int> CreateContraVoucher(ContraVoucherModel contraVoucherModel)
        {
            int contraVoucherId = 0;

            GenerateNoModel generateNoModel = await GenerateContraVoucherNo(contraVoucherModel.CompanyId, contraVoucherModel.FinancialYearId);

            // assign values.
            Contravoucher contraVoucher = new Contravoucher();

            contraVoucher.VoucherNo = generateNoModel.VoucherNo;
            contraVoucher.MaxNo = generateNoModel.MaxNo;
            contraVoucher.VoucherStyleId = generateNoModel.VoucherStyleId;

            contraVoucher.VoucherDate = contraVoucherModel.VoucherDate;
            contraVoucher.CurrencyId = contraVoucherModel.CurrencyId;
            contraVoucher.ExchangeRate = contraVoucherModel.ExchangeRate;


            contraVoucher.ChequeNo = contraVoucherModel.ChequeNo;
            contraVoucher.ChequeDate = contraVoucherModel.ChequeDate;
            contraVoucher.AmountFc = contraVoucherModel.AmountFc;
            contraVoucher.AmountFcinWord = "";
            contraVoucher.Amount = 0;

            contraVoucher.Narration = contraVoucherModel.Narration;

            contraVoucher.CreditAmountFc = 0;
            contraVoucher.CreditAmount = 0;
            contraVoucher.DebitAmountFc = 0;
            contraVoucher.DebitAmount = 0;

            contraVoucher.StatusId = (int)DocumentStatus.Inprocess;
            contraVoucher.CompanyId = contraVoucherModel.CompanyId;
            contraVoucher.FinancialYearId = contraVoucherModel.FinancialYearId;

            await Create(contraVoucher);

            contraVoucherId = contraVoucher.ContraVoucherId;

            if (contraVoucherId != 0)
            {
                await UpdateContraVoucherMasterAmount(contraVoucherId);
            }

            return contraVoucherId; // returns.
        }

        public async Task<bool> UpdateContraVoucher(ContraVoucherModel contraVoucherModel)
        {
            bool isUpdated = false;

            // get record.
            Contravoucher contraVoucher = await GetByIdAsync(w => w.ContraVoucherId == contraVoucherModel.ContraVoucherId);

            if (null != contraVoucher)
            {
                contraVoucher.VoucherDate = contraVoucherModel.VoucherDate;
                contraVoucher.CurrencyId = contraVoucherModel.CurrencyId;
                contraVoucher.ExchangeRate = contraVoucherModel.ExchangeRate;

                contraVoucher.Narration = contraVoucherModel.Narration;

                contraVoucher.ChequeNo = contraVoucherModel.ChequeNo;
                contraVoucher.ChequeDate = contraVoucherModel.ChequeDate;
                contraVoucher.AmountFc = contraVoucherModel.AmountFc;
                contraVoucher.Amount = 0;
                contraVoucher.AmountFcinWord = "";

                contraVoucher.CreditAmountFc = 0;
                contraVoucher.CreditAmount = 0;
                contraVoucher.DebitAmountFc = 0;
                contraVoucher.DebitAmount = 0;

                isUpdated = await Update(contraVoucher);
            }

            if (isUpdated != false)
            {
                await UpdateContraVoucherMasterAmount(contraVoucher.ContraVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateStatusContraVoucher(int contraVoucherId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Contravoucher contraVoucher = await GetByIdAsync(w => w.ContraVoucherId == contraVoucherId);

            if (null != contraVoucher)
            {
                contraVoucher.StatusId = statusId;
                isUpdated = await Update(contraVoucher);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteContraVoucher(int ContraVoucherId)
        {
            bool isDeleted = false;

            // get record.
            Contravoucher contraVoucher = await GetByIdAsync(w => w.ContraVoucherId == ContraVoucherId);

            if (null != contraVoucher)
            {
                isDeleted = await Delete(contraVoucher);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> UpdateContraVoucherMasterAmount(int contraVoucherId)
        {
            bool isUpdated = false;

            Contravoucher contraVoucher = null;

            //////// get record.

            contraVoucher = await GetQueryByCondition(w => w.ContraVoucherId == contraVoucherId)
                                                       .Include(w => w.Contravoucherdetails)
                                                       .Include(w => w.Currency).FirstOrDefaultAsync();

            if (null != contraVoucher)
            {
                contraVoucher.CreditAmountFc = contraVoucher.Contravoucherdetails.Sum(w => w.CreditAmountFc);
                contraVoucher.CreditAmount = contraVoucher.CreditAmountFc / contraVoucher.ExchangeRate;

                contraVoucher.DebitAmountFc = contraVoucher.Contravoucherdetails.Sum(w => w.DebitAmountFc);
                contraVoucher.DebitAmount = contraVoucher.DebitAmountFc / contraVoucher.ExchangeRate;

                contraVoucher.AmountFcinWord = await common.AmountInWord_Million(contraVoucher.AmountFc.ToString(), contraVoucher.Currency.CurrencyCode, contraVoucher.Currency.Denomination);

                if (contraVoucher.StatusId == (int)DocumentStatus.Approved || contraVoucher.StatusId == (int)DocumentStatus.ApprovalRequested || contraVoucher.StatusId == (int)DocumentStatus.Cancelled)
                {
                    contraVoucher.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(contraVoucher);
            }

            return isUpdated; // returns.
        }

        public async Task<ContraVoucherModel> GetContraVoucherById(int contraVoucherId)
        {
            ContraVoucherModel contraVoucherModel = null;

            IList<ContraVoucherModel> contraVoucherModelList = await GetContraVoucherList(contraVoucherId);

            if (null != contraVoucherModelList && contraVoucherModelList.Any())
            {
                contraVoucherModel = contraVoucherModelList.FirstOrDefault();
            }

            return contraVoucherModel; // returns.
        }

        public async Task<DataTableResultModel<ContraVoucherModel>> GetContraVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterContraVoucherModel searchFilterModel)
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
            DataTableResultModel<ContraVoucherModel> resultModel = await GetDataFromDbase(searchFilterModel, searchBy, take, skip, sortBy, sortDir);

            return resultModel; // returns.
        }

        #region Private Methods

        private async Task<DataTableResultModel<ContraVoucherModel>> GetDataFromDbase(SearchFilterContraVoucherModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<ContraVoucherModel> resultModel = new DataTableResultModel<ContraVoucherModel>();

            IQueryable<Contravoucher> query = GetQueryByCondition(w => w.ContraVoucherId != 0)
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

            resultModel.ResultList = await query.Select(s => new ContraVoucherModel
            {
                ContraVoucherId = s.ContraVoucherId,
                VoucherNo = s.VoucherNo,
                VoucherDate = s.VoucherDate,
                AmountFc = s.AmountFc,
                ChequeNo = s.ChequeNo,
                 CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
            }).OrderBy($"{sortBy} {sortDir}").ToListAsync();

            // get filter record count.
            resultModel.FilterResultCount = await query.CountAsync();

            return resultModel; // returns.
        }

        private async Task<IList<ContraVoucherModel>> GetContraVoucherList(int contraVoucherId)
        {
            IList<ContraVoucherModel> contraVoucherModelList = null;

            // create query.
            IQueryable<Contravoucher> query = GetQueryByCondition(w => w.ContraVoucherId != 0)
                                            .Include(w => w.Currency)
                                            .Include(w => w.Status).Include(w => w.PreparedByUser);

            // apply filters.
            //if (0 != contraVoucherId)
            query = query.Where(w => w.ContraVoucherId == contraVoucherId);

            // get records by query.
            List<Contravoucher> contraVoucherList = await query.ToListAsync();

            if (null != contraVoucherList && contraVoucherList.Count > 0)
            {
                contraVoucherModelList = new List<ContraVoucherModel>();

                foreach (Contravoucher contraVoucher in contraVoucherList)
                {
                    contraVoucherModelList.Add(await AssignValueToModel(contraVoucher));
                }
            }

            return contraVoucherModelList; // returns.
        }

        private async Task<ContraVoucherModel> AssignValueToModel(Contravoucher contraVoucher)
        {
            return await Task.Run(() =>
            {
                ContraVoucherModel contraVoucherModel = new ContraVoucherModel();

                contraVoucherModel.ContraVoucherId = contraVoucher.ContraVoucherId;
                contraVoucherModel.VoucherNo = contraVoucher.VoucherNo;
                contraVoucherModel.VoucherDate = contraVoucher.VoucherDate;
                contraVoucherModel.CurrencyId = contraVoucher.CurrencyId;
                contraVoucherModel.ExchangeRate = contraVoucher.ExchangeRate;
                contraVoucherModel.Narration = contraVoucher.Narration;

                contraVoucherModel.ChequeNo = contraVoucher.ChequeNo;
                contraVoucherModel.ChequeDate = contraVoucher.ChequeDate;
                contraVoucherModel.AmountFc = contraVoucher.AmountFc;
                contraVoucherModel.Amount = contraVoucher.Amount;
                contraVoucherModel.AmountFcInWord = contraVoucher.AmountFcinWord;

                contraVoucherModel.CreditAmountFc = contraVoucher.CreditAmountFc;
                contraVoucherModel.CreditAmount = contraVoucher.CreditAmount;
                contraVoucherModel.DebitAmountFc = contraVoucher.DebitAmountFc;
                contraVoucherModel.DebitAmount = contraVoucher.DebitAmount;

                contraVoucherModel.StatusId = contraVoucher.StatusId;
                contraVoucherModel.CompanyId = Convert.ToInt32(contraVoucher.CompanyId);
                contraVoucherModel.FinancialYearId = Convert.ToInt32(contraVoucher.FinancialYearId);
                contraVoucherModel.MaxNo = contraVoucher.MaxNo;
                contraVoucherModel.VoucherStyleId = contraVoucher.VoucherStyleId;

                // ###
                contraVoucherModel.CurrencyCode = null != contraVoucher.Currency ? contraVoucher.Currency.CurrencyCode : null;
                contraVoucherModel.StatusName = null != contraVoucher.Status ? contraVoucher.Status.StatusName : null;
                contraVoucherModel.PreparedByName = null != contraVoucher.PreparedByUser ? contraVoucher.PreparedByUser.UserName : null;

                return contraVoucherModel;
            });

        }

        #endregion Private Methods
    }
}
