using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class AdvanceAdjustmentDetailService : Repository<Advanceadjustmentdetail>, IAdvanceAdjustmentDetail
    {
        IAdvanceAdjustment advanceAdjustment;

        public AdvanceAdjustmentDetailService(ErpDbContext dbContext, IAdvanceAdjustment _advanceAdjustment) : base(dbContext)
        {
            advanceAdjustment = _advanceAdjustment;
        }

        public async Task<int> CreateAdvanceAdjustmentDetail(AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel)
        {
            int advanceAdjustmentDetailId = 0;

            // assign values.
            Advanceadjustmentdetail advanceAdjustmentDetail = new Advanceadjustmentdetail();

            advanceAdjustmentDetail.AdvanceAdjustmentId = advanceAdjustmentDetailModel.AdvanceAdjustmentId;
            advanceAdjustmentDetail.AmountFc = advanceAdjustmentDetailModel.AmountFc;
            advanceAdjustmentDetail.Amount = 0;
            advanceAdjustmentDetail.Narration = advanceAdjustmentDetailModel.Narration;
            advanceAdjustmentDetail.PurchaseInvoiceId = advanceAdjustmentDetailModel.PurchaseInvoiceId;
            advanceAdjustmentDetail.DebitNoteId = advanceAdjustmentDetailModel.DebitNoteId;
            advanceAdjustmentDetail.CreditNoteId = advanceAdjustmentDetailModel.CreditNoteId;

            if (advanceAdjustmentDetailId != 0)
            {
                await UpdateAdvanceAdjustmentDetailAmount(advanceAdjustmentDetailId);
            }

            await Create(advanceAdjustmentDetail);

            advanceAdjustmentDetailId = advanceAdjustmentDetail.AdvanceAdjustmentDetId;

            return advanceAdjustmentDetailId; // returns.
        }

        public async Task<bool> UpdateAdvanceAdjustmentDetail(AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustmentdetail advanceAdjustmentDetail = await GetByIdAsync(w => w.AdvanceAdjustmentDetId == advanceAdjustmentDetailModel.AdvanceAdjustmentDetId);

            if (null != advanceAdjustmentDetail)
            {

                // assign values.
                advanceAdjustmentDetail.AmountFc = advanceAdjustmentDetailModel.AmountFc;
                advanceAdjustmentDetail.Amount = 0;
                advanceAdjustmentDetail.Narration = advanceAdjustmentDetailModel.Narration;
                advanceAdjustmentDetail.PurchaseInvoiceId = advanceAdjustmentDetailModel.PurchaseInvoiceId;
                advanceAdjustmentDetail.DebitNoteId = advanceAdjustmentDetailModel.DebitNoteId;
                advanceAdjustmentDetail.CreditNoteId = advanceAdjustmentDetailModel.CreditNoteId;

                isUpdated = await Update(advanceAdjustmentDetail);
            }

            if (isUpdated != false)
            {
                await UpdateAdvanceAdjustmentDetailAmount(advanceAdjustmentDetailModel.AdvanceAdjustmentDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateAdvanceAdjustmentDetailAmount(int? advanceAdjustmentDetailId)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustmentdetail advanceAdjustmentDetail = await GetQueryByCondition(w => w.AdvanceAdjustmentDetId == advanceAdjustmentDetailId)
                                                                    .Include(w => w.AdvanceAdjustment)
                                                                    .FirstOrDefaultAsync();

            if (null != advanceAdjustmentDetail)
            {
                advanceAdjustmentDetail.Amount = advanceAdjustmentDetail.AmountFc * advanceAdjustmentDetail.AdvanceAdjustment.ExchangeRate;

                isUpdated = await Update(advanceAdjustmentDetail);
            }

            if (isUpdated != false)
            {
                await advanceAdjustment.UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentDetail.AdvanceAdjustmentId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteAdvanceAdjustmentDetail(int advanceAdjustmentDetailId)
        {
            bool isDeleted = false;

            // get record.
            Advanceadjustmentdetail advanceAdjustmentDetail = await GetByIdAsync(w => w.AdvanceAdjustmentDetId == advanceAdjustmentDetailId);

            if (null != advanceAdjustmentDetail)
            {
                isDeleted = await Delete(advanceAdjustmentDetail);
            }

            if (isDeleted != false)
            {
                await advanceAdjustment.UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentDetail.AdvanceAdjustmentId);
            }

            return isDeleted; // returns.
        }

        public async Task<AdvanceAdjustmentDetailModel> GetAdvanceAdjustmentDetailById(int advanceAdjustmentDetailId)
        {
            AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel = null;

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentModelDetailList = await GetAdvanceAdjustmentDetailList(advanceAdjustmentDetailId, 0);

            if (null != advanceAdjustmentModelDetailList && advanceAdjustmentModelDetailList.Any())
            {
                advanceAdjustmentDetailModel = advanceAdjustmentModelDetailList.FirstOrDefault();
            }

            return advanceAdjustmentDetailModel; // returns.
        }

        public async Task<DataTableResultModel<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailByAdvanceAdjustmentId(int advanceAdjustmentId)
        {
            DataTableResultModel<AdvanceAdjustmentDetailModel> resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = await GetAdvanceAdjustmentDetailList(0, advanceAdjustmentId);

            if (null != advanceAdjustmentDetailModelList && advanceAdjustmentDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();
                resultModel.ResultList = advanceAdjustmentDetailModelList;
                resultModel.TotalResultCount = advanceAdjustmentDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();
                resultModel.ResultList = new List<AdvanceAdjustmentDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<IList<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailByAdjustmentId(int advanceAdjustmentId, int addRow_Blank)
        {
            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = await GetAdvanceAdjustmentDetailList(0, advanceAdjustmentId);

            //if (null != advanceAdjustmentDetailModelList && advanceAdjustmentDetailModelList.Any())
            //{
            //    if (addRow_Blank == 1)
            //    {
            //        advanceAdjustmentDetailModelList.Add(await AddRow_Blank(advanceAdjustmentId));
            //    }
            //}
            //else
            //{
            //    advanceAdjustmentDetailModelList = new List<AdvanceAdjustmentDetailModel>();

            //    if (addRow_Blank == 1)
            //    {
            //        advanceAdjustmentDetailModelList.Add(await AddRow_Blank(advanceAdjustmentId));
            //    }

            //}

            return advanceAdjustmentDetailModelList; // returns.
        }

        public async Task<DataTableResultModel<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailList()
        {
            DataTableResultModel<AdvanceAdjustmentDetailModel> resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();

            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = await GetAdvanceAdjustmentDetailList(0, 0);

            if (null != advanceAdjustmentDetailModelList && advanceAdjustmentDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();
                resultModel.ResultList = advanceAdjustmentDetailModelList;
                resultModel.TotalResultCount = advanceAdjustmentDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        /// <summary>
        /// get advance adjustment List based on particularLedgerId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<IList<AdvanceAdjustmentDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId)
        {
            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = null;

            // create query.
            IQueryable<Advanceadjustmentdetail> query = GetQueryByCondition(w => w.AdvanceAdjustmentDetId != 0)
                                                        .Include(w => w.AdvanceAdjustment);

            // apply filters.
            if (0 != particularLedgerId)
                query = query.Where(w => w.AdvanceAdjustment.ParticularLedgerId == particularLedgerId);

            // get records by query.
            List<Advanceadjustmentdetail> advanceAdjustmentDetailList = await query.ToListAsync();

            advanceAdjustmentDetailModelList = new List<AdvanceAdjustmentDetailModel>();

            if (null != advanceAdjustmentDetailList && advanceAdjustmentDetailList.Count > 0)
            {
                foreach (Advanceadjustmentdetail advanceAdjustmentDetail in advanceAdjustmentDetailList)
                {
                    advanceAdjustmentDetailModelList.Add(await AssignValueToModel(advanceAdjustmentDetail));
                }
            }


            return advanceAdjustmentDetailModelList; // returns.
        }

        private async Task<IList<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailList(int advanceAdjustmentDetailId, int advanceAdjustmentId)
        {
            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = null;

            // create query.
            IQueryable<Advanceadjustmentdetail> query = GetQueryByCondition(w => w.AdvanceAdjustmentDetId != 0);

            // apply filters.
            if (0 != advanceAdjustmentDetailId)
                query = query.Where(w => w.AdvanceAdjustmentDetId == advanceAdjustmentDetailId);

            if (0 != advanceAdjustmentId)
                query = query.Where(w => w.AdvanceAdjustmentId == advanceAdjustmentId);

            // get records by query.
            List<Advanceadjustmentdetail> advanceAdjustmentDetailList = await query.ToListAsync();

            if (null != advanceAdjustmentDetailList && advanceAdjustmentDetailList.Count > 0)
            {
                advanceAdjustmentDetailModelList = new List<AdvanceAdjustmentDetailModel>();

                foreach (Advanceadjustmentdetail advanceAdjustmentDetail in advanceAdjustmentDetailList)
                {
                    advanceAdjustmentDetailModelList.Add(await AssignValueToModel(advanceAdjustmentDetail));
                }
            }

            return advanceAdjustmentDetailModelList; // returns.
        }

        private async Task<AdvanceAdjustmentDetailModel> AssignValueToModel(Advanceadjustmentdetail advanceAdjustmentDetail)
        {
            return await Task.Run(() =>
            {
                AdvanceAdjustmentDetailModel advanceAdjustmentDetailModel = new AdvanceAdjustmentDetailModel();

                advanceAdjustmentDetailModel.AdvanceAdjustmentDetId = advanceAdjustmentDetail.AdvanceAdjustmentDetId;
                advanceAdjustmentDetailModel.AdvanceAdjustmentId = advanceAdjustmentDetail.AdvanceAdjustmentId;
                advanceAdjustmentDetailModel.AmountFc = advanceAdjustmentDetail.AmountFc;
                advanceAdjustmentDetailModel.Amount = advanceAdjustmentDetail.Amount;
                advanceAdjustmentDetailModel.Narration = advanceAdjustmentDetail.Narration;

                advanceAdjustmentDetailModel.PurchaseInvoiceId = advanceAdjustmentDetail.PurchaseInvoiceId;
                advanceAdjustmentDetailModel.CreditNoteId = advanceAdjustmentDetail.CreditNoteId;
                advanceAdjustmentDetailModel.DebitNoteId = advanceAdjustmentDetail.DebitNoteId;

                //--####
                //advanceAdjustmentDetailModel.TransactionTypeName = null != advanceAdjustmentDetail.UnitOfMeasurement ? advanceAdjustmentDetail.UnitOfMeasurement.UnitOfMeasurementName : null;

                return advanceAdjustmentDetailModel;
            });
        }

    }
}
