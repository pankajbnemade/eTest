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
        private readonly IAdvanceAdjustment _advanceAdjustment;

        public AdvanceAdjustmentDetailService(ErpDbContext dbContext, IAdvanceAdjustment advanceAdjustment) : base(dbContext)
        {
            _advanceAdjustment = advanceAdjustment;
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
            advanceAdjustmentDetail.SalesInvoiceId = advanceAdjustmentDetailModel.SalesInvoiceId;
            advanceAdjustmentDetail.PurchaseInvoiceId = advanceAdjustmentDetailModel.PurchaseInvoiceId;
            advanceAdjustmentDetail.DebitNoteId = advanceAdjustmentDetailModel.DebitNoteId;
            advanceAdjustmentDetail.CreditNoteId = advanceAdjustmentDetailModel.CreditNoteId;

            await Create(advanceAdjustmentDetail);

            advanceAdjustmentDetailId = advanceAdjustmentDetail.AdvanceAdjustmentDetId;

            if (advanceAdjustmentDetailId != 0)
            {
                await UpdateAdvanceAdjustmentDetailAmount(advanceAdjustmentDetailId);
            }

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
                //advanceAdjustmentDetail.SalesInvoiceId = advanceAdjustmentDetailModel.SalesInvoiceId;
                // advanceAdjustmentDetail.PurchaseInvoiceId = advanceAdjustmentDetailModel.PurchaseInvoiceId;
                // advanceAdjustmentDetail.DebitNoteId = advanceAdjustmentDetailModel.DebitNoteId;
                // advanceAdjustmentDetail.CreditNoteId = advanceAdjustmentDetailModel.CreditNoteId;

                isUpdated = await Update(advanceAdjustmentDetail);
            }

            if (isUpdated != false)
            {
                await UpdateAdvanceAdjustmentDetailAmount(advanceAdjustmentDetailModel.AdvanceAdjustmentDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateAdvanceAdjustmentDetailAmount(int advanceAdjustmentDetailId)
        {
            bool isUpdated = false;

            // get record.
            Advanceadjustmentdetail advanceAdjustmentDetail = await GetQueryByCondition(w => w.AdvanceAdjustmentDetId == advanceAdjustmentDetailId)
                                                                    .Include(w => w.AdvanceAdjustment)
                                                                    .FirstOrDefaultAsync();

            if (null != advanceAdjustmentDetail)
            {
                advanceAdjustmentDetail.Amount = advanceAdjustmentDetail.AmountFc / advanceAdjustmentDetail.AdvanceAdjustment.ExchangeRate;

                isUpdated = await Update(advanceAdjustmentDetail);
            }

            //if (isUpdated != false)
            //{
            //    await advanceAdjustment.UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentDetail.AdvanceAdjustmentId);
            //}

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
                await _advanceAdjustment.UpdateAdvanceAdjustmentMasterAmount(advanceAdjustmentDetail.AdvanceAdjustmentId);
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

        public async Task<DataTableResultModel<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailByAdvanceAdjustmentId(int advanceAdjustmentId, int addRow_Blank)
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

        public async Task<IList<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailByAdjustmentId(int advanceAdjustmentId)
        {
            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = await GetAdvanceAdjustmentDetailList(0, advanceAdjustmentId);

            return advanceAdjustmentDetailModelList; // returns.
        }

        //public async Task<DataTableResultModel<AdvanceAdjustmentDetailModel>> GetAdvanceAdjustmentDetailList()
        //{
        //    DataTableResultModel<AdvanceAdjustmentDetailModel> resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();

        //    IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = await GetAdvanceAdjustmentDetailList(0, 0);

        //    if (null != advanceAdjustmentDetailModelList && advanceAdjustmentDetailModelList.Any())
        //    {
        //        resultModel = new DataTableResultModel<AdvanceAdjustmentDetailModel>();
        //        resultModel.ResultList = advanceAdjustmentDetailModelList;
        //        resultModel.TotalResultCount = advanceAdjustmentDetailModelList.Count();
        //    }

        //    return resultModel; // returns.
        //}

        public async Task<IList<AdvanceAdjustmentDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId)
        {
            IList<AdvanceAdjustmentDetailModel> advanceAdjustmentDetailModelList = null;

            // create query.
            IQueryable<Advanceadjustmentdetail> query = GetQueryByCondition(w => w.AdvanceAdjustmentDetId != 0)
                                                        .Include(w => w.AdvanceAdjustment)
                                                        .Include(w => w.SalesInvoice).Include(w => w.CreditNote)
                                                        .Include(w => w.PurchaseInvoice).Include(w => w.DebitNote);

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
            IQueryable<Advanceadjustmentdetail> query = GetQueryByCondition(w => w.AdvanceAdjustmentDetId != 0)
                                                        .Include(w => w.SalesInvoice).Include(w => w.CreditNote)
                                                        .Include(w => w.PurchaseInvoice).Include(w => w.DebitNote);

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
                advanceAdjustmentDetailModel.SalesInvoiceId = advanceAdjustmentDetail.SalesInvoiceId;
                advanceAdjustmentDetailModel.CreditNoteId = advanceAdjustmentDetail.CreditNoteId;
                advanceAdjustmentDetailModel.DebitNoteId = advanceAdjustmentDetail.DebitNoteId;

                //--####
                if (advanceAdjustmentDetailModel.SalesInvoiceId != 0 && advanceAdjustmentDetailModel.SalesInvoiceId != null)
                {
                    advanceAdjustmentDetailModel.InvoiceType = "Sales Invoice";
                    advanceAdjustmentDetailModel.InvoiceNo = null != advanceAdjustmentDetail.SalesInvoice ? advanceAdjustmentDetail.SalesInvoice.InvoiceNo : null;
                }
                else if (advanceAdjustmentDetailModel.PurchaseInvoiceId != 0 && advanceAdjustmentDetailModel.PurchaseInvoiceId != null)
                {
                    advanceAdjustmentDetailModel.InvoiceType = "Purchase Invoice";
                    advanceAdjustmentDetailModel.InvoiceNo = null != advanceAdjustmentDetail.PurchaseInvoice ? advanceAdjustmentDetail.PurchaseInvoice.InvoiceNo : null;
                }
                else if (advanceAdjustmentDetailModel.CreditNoteId != 0 && advanceAdjustmentDetailModel.CreditNoteId != null)
                {
                    advanceAdjustmentDetailModel.InvoiceType = "Credit Note";
                    advanceAdjustmentDetailModel.InvoiceNo = null != advanceAdjustmentDetail.CreditNote ? advanceAdjustmentDetail.CreditNote.CreditNoteNo : null;
                }
                else if (advanceAdjustmentDetailModel.DebitNoteId != 0 && advanceAdjustmentDetailModel.DebitNoteId != null)
                {
                    advanceAdjustmentDetailModel.InvoiceType = "Debit Note";
                    advanceAdjustmentDetailModel.InvoiceNo = null != advanceAdjustmentDetail.DebitNote ? advanceAdjustmentDetail.DebitNote.DebitNoteNo : null;
                }

                return advanceAdjustmentDetailModel;
            });
        }

    }
}
