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
    public class DebitNoteDetailService : Repository<Debitnotedetail>, IDebitNoteDetail
    {
        IDebitNote debitNote;

        public DebitNoteDetailService(ErpDbContext dbContext, IDebitNote _debitNote) : base(dbContext)
        {
            debitNote = _debitNote;
        }

        public async Task<int> GenerateSrNo(int debitNoteId)
        {
            int srNo = 0;

            if (await Any(w => w.DebitNoteDetId != 0 && w.DebitNoteId == debitNoteId))
            {
                srNo = await GetQueryByCondition(w => w.DebitNoteDetId != 0 && w.DebitNoteId == debitNoteId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateDebitNoteDetail(DebitNoteDetailModel debitNoteDetailModel)
        {
            int debitNoteDetailId = 0;

            // assign values.
            Debitnotedetail debitNoteDetail = new Debitnotedetail();

            debitNoteDetail.DebitNoteId = debitNoteDetailModel.DebitNoteId;
            debitNoteDetail.SrNo = debitNoteDetailModel.SrNo;
            debitNoteDetail.Description = debitNoteDetailModel.Description;
            debitNoteDetail.UnitOfMeasurementId = debitNoteDetailModel.UnitOfMeasurementId;
            debitNoteDetail.Quantity = debitNoteDetailModel.Quantity;
            debitNoteDetail.PerUnit = debitNoteDetailModel.PerUnit;
            debitNoteDetail.UnitPrice = debitNoteDetailModel.UnitPrice;
            debitNoteDetail.GrossAmountFc = 0;
            debitNoteDetail.GrossAmount = 0;
            debitNoteDetail.TaxAmountFc = 0;
            debitNoteDetail.TaxAmount = 0;
            debitNoteDetail.NetAmountFc = 0;
            debitNoteDetail.NetAmount = 0;

            if (debitNoteDetailId != 0)
            {
                await UpdateDebitNoteDetailAmount(debitNoteDetailId);
                //await debitNote.UpdateDebitNoteMasterAmount(debitNoteDetail.DebitNoteId);
            }

             await Create(debitNoteDetail);
            debitNoteDetailId = debitNoteDetail.DebitNoteDetId;

            return debitNoteDetailId; // returns.
        }

        public async Task<bool> UpdateDebitNoteDetail(DebitNoteDetailModel debitNoteDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Debitnotedetail debitNoteDetail = await GetByIdAsync(w => w.DebitNoteDetId == debitNoteDetailModel.DebitNoteDetId);

            if (null != debitNoteDetail)
            {

                // assign values.
                debitNoteDetail.DebitNoteId = debitNoteDetailModel.DebitNoteId;
                debitNoteDetail.SrNo = debitNoteDetailModel.SrNo;
                debitNoteDetail.Description = debitNoteDetailModel.Description;
                debitNoteDetail.UnitOfMeasurementId = debitNoteDetailModel.UnitOfMeasurementId;
                debitNoteDetail.Quantity = debitNoteDetailModel.Quantity;
                debitNoteDetail.PerUnit = debitNoteDetailModel.PerUnit;
                debitNoteDetail.UnitPrice = debitNoteDetailModel.UnitPrice;
                debitNoteDetail.GrossAmountFc = 0;
                debitNoteDetail.GrossAmount = 0;
                debitNoteDetail.TaxAmountFc = 0;
                debitNoteDetail.TaxAmount = 0;
                debitNoteDetail.NetAmountFc = 0;
                debitNoteDetail.NetAmount = 0;

                isUpdated = await Update(debitNoteDetail);
            }

            if (isUpdated != false)
            {
                await UpdateDebitNoteDetailAmount(debitNoteDetailModel.DebitNoteDetId);
                //await debitNote.UpdateDebitNoteMasterAmount(debitNoteDetail.DebitNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateDebitNoteDetailAmount(int? debitNoteDetailId)
        {
            bool isUpdated = false;

            // get record.
            Debitnotedetail debitNoteDetail =  await GetQueryByCondition(w => w.DebitNoteDetId == debitNoteDetailId)
                                                                 .Include(w => w.DebitNote).Include(w => w.Debitnotedetailtaxes).FirstOrDefaultAsync();

            if (null != debitNoteDetail)
            {
                debitNoteDetail.GrossAmountFc = debitNoteDetail.Quantity * debitNoteDetail.PerUnit * debitNoteDetail.UnitPrice;
                debitNoteDetail.GrossAmount = debitNoteDetail.GrossAmountFc * debitNoteDetail.DebitNote.ExchangeRate;
                debitNoteDetail.TaxAmountFc = debitNoteDetail.Debitnotedetailtaxes.Sum(s => s.TaxAmountFc);
                debitNoteDetail.TaxAmount = debitNoteDetail.DebitNote.ExchangeRate * debitNoteDetail.TaxAmountFc;
                debitNoteDetail.NetAmountFc = debitNoteDetail.TaxAmountFc + debitNoteDetail.GrossAmountFc;
                debitNoteDetail.NetAmount = debitNoteDetail.DebitNote.ExchangeRate * debitNoteDetail.NetAmountFc;

                isUpdated = await Update(debitNoteDetail);
            }

            if (isUpdated != false)
            {
                await debitNote.UpdateDebitNoteMasterAmount(debitNoteDetail.DebitNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteDebitNoteDetail(int debitNoteDetailId)
        {
            bool isDeleted = false;

            // get record.
            Debitnotedetail debitNoteDetail = await GetByIdAsync(w => w.DebitNoteDetId == debitNoteDetailId);

            if (null != debitNoteDetail)
            {
                isDeleted = await Delete(debitNoteDetail);
            }

            if (isDeleted != false)
            {
                await debitNote.UpdateDebitNoteMasterAmount(debitNoteDetail.DebitNoteId);
            }

            return isDeleted; // returns.
        }

        public async Task<DebitNoteDetailModel> GetDebitNoteDetailById(int debitNoteDetailId)
        {
            DebitNoteDetailModel debitNoteDetailModel = null;

            IList<DebitNoteDetailModel> debitNoteModelDetailList = await GetDebitNoteDetailList(debitNoteDetailId, 0);

            if (null != debitNoteModelDetailList && debitNoteModelDetailList.Any())
            {
                debitNoteDetailModel = debitNoteModelDetailList.FirstOrDefault();
            }

            return debitNoteDetailModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteDetailModel>> GetDebitNoteDetailByDebitNoteId(int debitNoteId)
        {
            DataTableResultModel<DebitNoteDetailModel> resultModel = new DataTableResultModel<DebitNoteDetailModel>();

            IList<DebitNoteDetailModel> debitNoteDetailModelList = await GetDebitNoteDetailList(0, debitNoteId);

            if (null != debitNoteDetailModelList && debitNoteDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteDetailModel>();
                resultModel.ResultList = debitNoteDetailModelList;
                resultModel.TotalResultCount = debitNoteDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<DebitNoteDetailModel>> GetDebitNoteDetailList()
        {
            DataTableResultModel<DebitNoteDetailModel> resultModel = new DataTableResultModel<DebitNoteDetailModel>();

            IList<DebitNoteDetailModel> debitNoteDetailModelList = await GetDebitNoteDetailList(0, 0);

            if (null != debitNoteDetailModelList && debitNoteDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<DebitNoteDetailModel>();
                resultModel.ResultList = debitNoteDetailModelList;
                resultModel.TotalResultCount = debitNoteDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<DebitNoteDetailModel>> GetDebitNoteDetailList(int debitNoteDetailId, int debitNoteId)
        {
            IList<DebitNoteDetailModel> debitNoteDetailModelList = null;

            // create query.
            IQueryable<Debitnotedetail> query = GetQueryByCondition(w => w.DebitNoteDetId != 0)
                                                        .Include(w => w.UnitOfMeasurement);

            // apply filters.
            if (0 != debitNoteDetailId)
                query = query.Where(w => w.DebitNoteDetId == debitNoteDetailId);

            if (0 != debitNoteId)
                query = query.Where(w => w.DebitNoteId == debitNoteId);

            // get records by query.
            List<Debitnotedetail> debitNoteDetailList = await query.ToListAsync();

            if (null != debitNoteDetailList && debitNoteDetailList.Count > 0)
            {
                debitNoteDetailModelList = new List<DebitNoteDetailModel>();

                foreach (Debitnotedetail debitNoteDetail in debitNoteDetailList)
                {
                    debitNoteDetailModelList.Add(await AssignValueToModel(debitNoteDetail));
                }
            }

            return debitNoteDetailModelList; // returns.
        }

        private async Task<DebitNoteDetailModel> AssignValueToModel(Debitnotedetail debitNoteDetail)
        {
            return await Task.Run(() =>
            {
                DebitNoteDetailModel debitNoteDetailModel = new DebitNoteDetailModel();

                debitNoteDetailModel.DebitNoteDetId = debitNoteDetail.DebitNoteDetId;
                debitNoteDetailModel.DebitNoteId = debitNoteDetail.DebitNoteId;
                debitNoteDetailModel.SrNo = debitNoteDetail.SrNo;
                debitNoteDetailModel.Description = debitNoteDetail.Description;
                debitNoteDetailModel.UnitOfMeasurementId = debitNoteDetail.UnitOfMeasurementId;
                debitNoteDetailModel.Quantity = debitNoteDetail.Quantity;
                debitNoteDetailModel.PerUnit = debitNoteDetail.PerUnit;
                debitNoteDetailModel.UnitPrice = debitNoteDetail.UnitPrice;
                debitNoteDetailModel.GrossAmountFc = debitNoteDetail.GrossAmountFc;
                debitNoteDetailModel.GrossAmount = debitNoteDetail.GrossAmount;
                debitNoteDetailModel.UnitPrice = debitNoteDetail.UnitPrice;
                debitNoteDetailModel.TaxAmountFc = debitNoteDetail.TaxAmountFc;
                debitNoteDetailModel.TaxAmount = debitNoteDetail.TaxAmount;
                debitNoteDetailModel.NetAmountFc = debitNoteDetail.NetAmountFc;
                debitNoteDetailModel.NetAmount = debitNoteDetail.NetAmount;

                //--####
                debitNoteDetailModel.UnitOfMeasurementName = null != debitNoteDetail.UnitOfMeasurement ? debitNoteDetail.UnitOfMeasurement.UnitOfMeasurementName : null;

                return debitNoteDetailModel;
            });
        }

    }
}
