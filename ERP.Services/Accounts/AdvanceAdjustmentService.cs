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
using System.Linq.Dynamic.Core;

namespace ERP.Services.Accounts
{
    public class AdvanceAdjustmentService : Repository<Advanceadjustment>, IAdvanceAdjustment
    {
        private readonly ICommon _common;

        public AdvanceAdjustmentService(ErpDbContext dbContext, ICommon common) : base(dbContext)
        {
            _common = common;
        }

        public async Task<GenerateNoModel> GenerateAdvanceAdjustmentNo(int companyId, int financialYearId)
        {
            int voucherSetupId = 10;

            int? maxNo = await GetQueryByCondition(w => w.CompanyId == companyId && w.FinancialYearId == financialYearId).MaxAsync(m => (int?)m.MaxNo);

            maxNo = maxNo == null ? 0 : maxNo;

            GenerateNoModel generateNoModel = await _common.GenerateVoucherNo((int)maxNo, voucherSetupId, companyId, financialYearId);

            return generateNoModel; // returns.
        }

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
            advanceAdjustment.ParticularLedgerId = advanceAdjustmentModel.ParticularLedgerId;
            advanceAdjustment.PaymentVoucherDetId = advanceAdjustmentModel.PaymentVoucherDetId;
            advanceAdjustment.ReceiptVoucherDetId = advanceAdjustmentModel.ReceiptVoucherDetId;
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

        public async Task<bool> UpdateAdvanceAdjustment(AdvanceAdjustmentModel advanceAdjustmentModel)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustment advanceAdjustment = await GetByIdAsync(w => w.AdvanceAdjustmentId == advanceAdjustmentModel.AdvanceAdjustmentId);

            if (null != advanceAdjustment)
            {
                advanceAdjustment.AdvanceAdjustmentDate = advanceAdjustmentModel.AdvanceAdjustmentDate;
                advanceAdjustment.ParticularLedgerId = advanceAdjustmentModel.ParticularLedgerId;
                advanceAdjustment.PaymentVoucherDetId = advanceAdjustmentModel.PaymentVoucherDetId;
                advanceAdjustment.ReceiptVoucherDetId = advanceAdjustmentModel.ReceiptVoucherDetId;
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

        public async Task<bool> UpdateStatusAdvanceAdjustment(int advanceAdjustmentId, int statusId)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustment advanceAdjustment = await GetByIdAsync(w => w.AdvanceAdjustmentId == advanceAdjustmentId);

            if (null != advanceAdjustment)
            {
                advanceAdjustment.StatusId = statusId;
                isUpdated = await Update(advanceAdjustment);
            }

            return isUpdated; // returns.
        }

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

        public async Task<bool> UpdateAdvanceAdjustmentMasterAmount(int advanceAdjustmentId)
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
                advanceAdjustment.Amount = advanceAdjustment.AmountFc / advanceAdjustment.ExchangeRate;

                advanceAdjustment.AmountFcinWord = await _common.AmountInWord_Million(advanceAdjustment.AmountFc.ToString(), advanceAdjustment.Currency.CurrencyCode, advanceAdjustment.Currency.Denomination);
                
                if (advanceAdjustment.StatusId == (int)DocumentStatus.Approved || advanceAdjustment.StatusId == (int)DocumentStatus.ApprovalRequested || advanceAdjustment.StatusId == (int)DocumentStatus.Cancelled)
                {
                    advanceAdjustment.StatusId = (int)DocumentStatus.Inprocess;
                }

                isUpdated = await Update(advanceAdjustment);
            }

            return isUpdated; // returns.
        }

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

        private async Task<DataTableResultModel<AdvanceAdjustmentModel>> GetDataFromDbase(SearchFilterAdvanceAdjustmentModel searchFilterModel, string searchBy, int take, int skip, string sortBy, string sortDir)
        {
            DataTableResultModel<AdvanceAdjustmentModel> resultModel = new DataTableResultModel<AdvanceAdjustmentModel>();

            IQueryable<Advanceadjustment> query = GetQueryByCondition(w => w.AdvanceAdjustmentId != 0)
                                                    .Include(w => w.ParticularLedger).Include(w => w.Currency).Include(w => w.Status)
                                                    .Include(w => w.PaymentVoucherDet).ThenInclude(w => w.PaymentVoucher)
                                                    .Include(w => w.ReceiptVoucherDet).ThenInclude(w => w.ReceiptVoucher);
            
            query = query.Where(w => w.CompanyId==searchFilterModel.CompanyId);
            query = query.Where(w => w.FinancialYearId==searchFilterModel.FinancialYearId);

            if (!string.IsNullOrEmpty(searchFilterModel.AdvanceAdjustmentNo))
            {
                query = query.Where(w => w.AdvanceAdjustmentNo.Contains(searchFilterModel.AdvanceAdjustmentNo));
            }

            if (null != searchFilterModel.ParticularLedgerId)
            {
                query = query.Where(w => w.ParticularLedgerId == searchFilterModel.ParticularLedgerId);
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

            //sorting
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDir))
            {
                query = query.OrderBy($"{sortBy} {sortDir}");
            }

            // datatable search
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(w => w.AdvanceAdjustmentNo.ToLower().Contains(searchBy.ToLower()));
            }

            // get records based on pagesize.
            query = query.Skip(skip).Take(take);
            resultModel.ResultList = await query.Select(s => new AdvanceAdjustmentModel
            {
                AdvanceAdjustmentId = s.AdvanceAdjustmentId,
                AdvanceAdjustmentNo = s.AdvanceAdjustmentNo,
                AdvanceAdjustmentDate = s.AdvanceAdjustmentDate,
                ParticularLedgerName = s.ParticularLedger.LedgerName,
                VoucherNo = s.PaymentVoucherDet == null && s.ReceiptVoucherDet != null ? s.ReceiptVoucherDet.ReceiptVoucher.VoucherNo
                                                        : s.PaymentVoucherDet.PaymentVoucher.VoucherNo,
                AmountFc = s.AmountFc,
                CurrencyCode = s.Currency.CurrencyCode,
                PreparedByName = s.PreparedByUser.UserName,
                StatusName = s.Status.StatusName,
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
                                            .Include(w => w.ParticularLedger).Include(w => w.Currency).Include(w => w.Status)
                                            .Include(w => w.PaymentVoucherDet).ThenInclude(w => w.PaymentVoucher)
                                            .Include(w => w.ReceiptVoucherDet).ThenInclude(w => w.ReceiptVoucher);

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
                advanceAdjustmentModel.ParticularLedgerId = advanceAdjustment.ParticularLedgerId;
                advanceAdjustmentModel.PaymentVoucherDetId = advanceAdjustment.PaymentVoucherDetId;
                advanceAdjustmentModel.ReceiptVoucherDetId = advanceAdjustment.ReceiptVoucherDetId;
                advanceAdjustmentModel.CurrencyId = advanceAdjustment.CurrencyId;
                advanceAdjustmentModel.ExchangeRate = advanceAdjustment.ExchangeRate;
                advanceAdjustmentModel.AmountFc = advanceAdjustment.AmountFc;
                advanceAdjustmentModel.Narration = advanceAdjustment.Narration;
                advanceAdjustmentModel.AmountFc = advanceAdjustment.AmountFc;
                advanceAdjustmentModel.Amount = advanceAdjustment.Amount;
                advanceAdjustmentModel.AmountFcInWord = advanceAdjustment.AmountFcinWord;

                advanceAdjustmentModel.StatusId = advanceAdjustment.StatusId;
                advanceAdjustmentModel.CompanyId = advanceAdjustment.CompanyId;
                advanceAdjustmentModel.FinancialYearId = advanceAdjustment.FinancialYearId;
                advanceAdjustmentModel.MaxNo = advanceAdjustment.MaxNo;
                advanceAdjustmentModel.VoucherStyleId = advanceAdjustment.VoucherStyleId;

                // ###

                if (null != advanceAdjustment.PaymentVoucherDetId && null == advanceAdjustment.ReceiptVoucherDetId)
                {
                    advanceAdjustmentModel.VoucherDetId = (int)advanceAdjustment.PaymentVoucherDetId;
                    advanceAdjustmentModel.VoucherNo = null != advanceAdjustment.PaymentVoucherDet.PaymentVoucher.VoucherNo ? advanceAdjustment.PaymentVoucherDet.PaymentVoucher.VoucherNo : "";
                }
                else if (null == advanceAdjustment.PaymentVoucherDetId && null != advanceAdjustment.ReceiptVoucherDetId)
                {
                    advanceAdjustmentModel.VoucherDetId = (int)advanceAdjustment.ReceiptVoucherDetId;
                    advanceAdjustmentModel.VoucherNo = null != advanceAdjustment.ReceiptVoucherDet.ReceiptVoucher.VoucherNo ? advanceAdjustment.ReceiptVoucherDet.ReceiptVoucher.VoucherNo : "";
                }
                else
                {
                    advanceAdjustmentModel.VoucherDetId = 0;
                    advanceAdjustmentModel.VoucherNo = "";
                }

                //advanceAdjustmentModel.VoucherDetId = null != advanceAdjustment.PaymentVoucherDetId && null == advanceAdjustment.ReceiptVoucherDetId
                //                                    ? (int)advanceAdjustment.PaymentVoucherDetId
                //                                    : (null == advanceAdjustment.PaymentVoucherDetId && null != advanceAdjustment.ReceiptVoucherDetId
                //                                    ? (int)advanceAdjustment.ReceiptVoucherDetId
                //                                    : 0);

                //advanceAdjustmentModel.VoucherNo = null != advanceAdjustment.PaymentVoucherDet.PaymentVoucher
                //                                    ? advanceAdjustment.PaymentVoucherDet.PaymentVoucher.VoucherNo
                //                                    : (null != advanceAdjustment.ReceiptVoucherDet.ReceiptVoucher
                //                                    ? advanceAdjustment.ReceiptVoucherDet.ReceiptVoucher.VoucherNo
                //                                    : "");

                advanceAdjustmentModel.CurrencyCode = null != advanceAdjustment.Currency ? advanceAdjustment.Currency.CurrencyCode : null;
                advanceAdjustmentModel.ParticularLedgerName = null != advanceAdjustment.ParticularLedger ? advanceAdjustment.ParticularLedger.LedgerName : null;
                advanceAdjustmentModel.StatusName = null != advanceAdjustment.Status ? advanceAdjustment.Status.StatusName : null;
                advanceAdjustmentModel.PreparedByName = null != advanceAdjustment.PreparedByUser ? advanceAdjustment.PreparedByUser.UserName : null;

                return advanceAdjustmentModel;
            });

        }

        #endregion Private Methods
    }
}
