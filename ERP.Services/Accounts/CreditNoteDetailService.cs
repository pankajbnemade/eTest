using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class CreditNoteDetailService : Repository<Creditnotedetail>, ICreditNoteDetail
    {
        private readonly ICreditNote _creditNote;

        public CreditNoteDetailService(ErpDbContext dbContext, ICreditNote creditNote) : base(dbContext)
        {
            _creditNote = creditNote;
        }

        public async Task<int> GenerateSrNo(int creditNoteId)
        {
            int srNo = 0;

            if (await Any(w => w.CreditNoteDetId != 0 && w.CreditNoteId == creditNoteId))
            {
                srNo = await GetQueryByCondition(w => w.CreditNoteDetId != 0 && w.CreditNoteId == creditNoteId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateCreditNoteDetail(CreditNoteDetailModel creditNoteDetailModel)
        {
            int creditNoteDetailId = 0;

            // assign values.
            Creditnotedetail creditNoteDetail = new Creditnotedetail();

            creditNoteDetail.CreditNoteId = creditNoteDetailModel.CreditNoteId;
            creditNoteDetail.SrNo = creditNoteDetailModel.SrNo;
            creditNoteDetail.Description = creditNoteDetailModel.Description;
            creditNoteDetail.UnitOfMeasurementId = creditNoteDetailModel.UnitOfMeasurementId;
            creditNoteDetail.Quantity = creditNoteDetailModel.Quantity;
            creditNoteDetail.PerUnit = creditNoteDetailModel.PerUnit;
            creditNoteDetail.UnitPrice = creditNoteDetailModel.UnitPrice;
            creditNoteDetail.GrossAmountFc = 0;
            creditNoteDetail.GrossAmount = 0;
            creditNoteDetail.TaxAmountFc = 0;
            creditNoteDetail.TaxAmount = 0;
            creditNoteDetail.NetAmountFc = 0;
            creditNoteDetail.NetAmount = 0;

            await Create(creditNoteDetail);

            creditNoteDetailId = creditNoteDetail.CreditNoteDetId;

            if (creditNoteDetailId != 0)
            {
                await UpdateCreditNoteDetailAmount(creditNoteDetailId);
            }

            return creditNoteDetailId; // returns.
        }

        public async Task<bool> UpdateCreditNoteDetail(CreditNoteDetailModel creditNoteDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Creditnotedetail creditNoteDetail = await GetByIdAsync(w => w.CreditNoteDetId == creditNoteDetailModel.CreditNoteDetId);

            if (null != creditNoteDetail)
            {

                // assign values.
                creditNoteDetail.CreditNoteId = creditNoteDetailModel.CreditNoteId;
                creditNoteDetail.SrNo = creditNoteDetailModel.SrNo;
                creditNoteDetail.Description = creditNoteDetailModel.Description;
                creditNoteDetail.UnitOfMeasurementId = creditNoteDetailModel.UnitOfMeasurementId;
                creditNoteDetail.Quantity = creditNoteDetailModel.Quantity;
                creditNoteDetail.PerUnit = creditNoteDetailModel.PerUnit;
                creditNoteDetail.UnitPrice = creditNoteDetailModel.UnitPrice;
                creditNoteDetail.GrossAmountFc = 0;
                creditNoteDetail.GrossAmount = 0;
                creditNoteDetail.TaxAmountFc = 0;
                creditNoteDetail.TaxAmount = 0;
                creditNoteDetail.NetAmountFc = 0;
                creditNoteDetail.NetAmount = 0;

                isUpdated = await Update(creditNoteDetail);
            }

            if (isUpdated != false)
            {
                await UpdateCreditNoteDetailAmount(creditNoteDetailModel.CreditNoteDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateCreditNoteDetailAmount(int? creditNoteDetailId)
        {
            bool isUpdated = false;

            // get record.
            Creditnotedetail creditNoteDetail = await GetQueryByCondition(w => w.CreditNoteDetId == creditNoteDetailId)
                                                                 .Include(w => w.CreditNote).Include(w => w.Creditnotedetailtaxes).FirstOrDefaultAsync();

            if (null != creditNoteDetail)
            {
                creditNoteDetail.GrossAmountFc = creditNoteDetail.Quantity * creditNoteDetail.PerUnit * creditNoteDetail.UnitPrice;
                creditNoteDetail.GrossAmount = creditNoteDetail.GrossAmountFc / creditNoteDetail.CreditNote.ExchangeRate;
                creditNoteDetail.TaxAmountFc = creditNoteDetail.Creditnotedetailtaxes.Sum(s => s.TaxAmountFc);
                creditNoteDetail.TaxAmount = creditNoteDetail.TaxAmountFc / creditNoteDetail.CreditNote.ExchangeRate;
                creditNoteDetail.NetAmountFc = creditNoteDetail.TaxAmountFc + creditNoteDetail.GrossAmountFc;
                creditNoteDetail.NetAmount = creditNoteDetail.NetAmountFc / creditNoteDetail.CreditNote.ExchangeRate;

                isUpdated = await Update(creditNoteDetail);
            }

            if (isUpdated != false)
            {
                await _creditNote.UpdateCreditNoteMasterAmount(creditNoteDetail.CreditNoteId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteCreditNoteDetail(int creditNoteDetailId)
        {
            bool isDeleted = false;

            // get record.
            Creditnotedetail creditNoteDetail = await GetByIdAsync(w => w.CreditNoteDetId == creditNoteDetailId);

            if (null != creditNoteDetail)
            {
                isDeleted = await Delete(creditNoteDetail);
            }

            if (isDeleted != false)
            {
                await _creditNote.UpdateCreditNoteMasterAmount(creditNoteDetail.CreditNoteId);
            }

            return isDeleted; // returns.
        }

        public async Task<CreditNoteDetailModel> GetCreditNoteDetailById(int creditNoteDetailId)
        {
            CreditNoteDetailModel creditNoteDetailModel = null;

            IList<CreditNoteDetailModel> creditNoteModelDetailList = await GetCreditNoteDetailList(creditNoteDetailId, 0);

            if (null != creditNoteModelDetailList && creditNoteModelDetailList.Any())
            {
                creditNoteDetailModel = creditNoteModelDetailList.FirstOrDefault();
            }

            return creditNoteDetailModel; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteDetailModel>> GetCreditNoteDetailByCreditNoteId(int creditNoteId)
        {
            DataTableResultModel<CreditNoteDetailModel> resultModel = new DataTableResultModel<CreditNoteDetailModel>();

            IList<CreditNoteDetailModel> creditNoteDetailModelList = await GetCreditNoteDetailList(0, creditNoteId);

            if (null != creditNoteDetailModelList && creditNoteDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteDetailModel>();
                resultModel.ResultList = creditNoteDetailModelList;
                resultModel.TotalResultCount = creditNoteDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<CreditNoteDetailModel>();
                resultModel.ResultList = new List<CreditNoteDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<IList<CreditNoteDetailModel>> GetCreditNoteDetailListByCreditNoteId(int creditNoteId)
        {
            IList<CreditNoteDetailModel> creditNoteDetailModelList = await GetCreditNoteDetailList(0, creditNoteId);

            return creditNoteDetailModelList; // returns.
        }

        public async Task<DataTableResultModel<CreditNoteDetailModel>> GetCreditNoteDetailList()
        {
            DataTableResultModel<CreditNoteDetailModel> resultModel = new DataTableResultModel<CreditNoteDetailModel>();

            IList<CreditNoteDetailModel> creditNoteDetailModelList = await GetCreditNoteDetailList(0, 0);

            if (null != creditNoteDetailModelList && creditNoteDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<CreditNoteDetailModel>();
                resultModel.ResultList = creditNoteDetailModelList;
                resultModel.TotalResultCount = creditNoteDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CreditNoteDetailModel>> GetCreditNoteDetailList(int creditNoteDetailId, int creditNoteId)
        {
            IList<CreditNoteDetailModel> creditNoteDetailModelList = null;

            // create query.
            IQueryable<Creditnotedetail> query = GetQueryByCondition(w => w.CreditNoteDetId != 0)
                                                        .Include(w => w.UnitOfMeasurement).Include(w => w.CreditNote);

            // apply filters.
            if (0 != creditNoteDetailId)
                query = query.Where(w => w.CreditNoteDetId == creditNoteDetailId);

            if (0 != creditNoteId)
                query = query.Where(w => w.CreditNoteId == creditNoteId);

            // get records by query.
            List<Creditnotedetail> creditNoteDetailList = await query.ToListAsync();

            if (null != creditNoteDetailList && creditNoteDetailList.Count > 0)
            {
                creditNoteDetailModelList = new List<CreditNoteDetailModel>();

                foreach (Creditnotedetail creditNoteDetail in creditNoteDetailList)
                {
                    creditNoteDetailModelList.Add(await AssignValueToModel(creditNoteDetail));
                }
            }

            return creditNoteDetailModelList; // returns.
        }

        private async Task<CreditNoteDetailModel> AssignValueToModel(Creditnotedetail creditNoteDetail)
        {
            return await Task.Run(() =>
            {
                CreditNoteDetailModel creditNoteDetailModel = new CreditNoteDetailModel();

                creditNoteDetailModel.CreditNoteDetId = creditNoteDetail.CreditNoteDetId;
                creditNoteDetailModel.CreditNoteId = creditNoteDetail.CreditNoteId;
                creditNoteDetailModel.SrNo = creditNoteDetail.SrNo;
                creditNoteDetailModel.Description = creditNoteDetail.Description;
                creditNoteDetailModel.UnitOfMeasurementId = creditNoteDetail.UnitOfMeasurementId;
                creditNoteDetailModel.Quantity = creditNoteDetail.Quantity;
                creditNoteDetailModel.PerUnit = creditNoteDetail.PerUnit;
                creditNoteDetailModel.UnitPrice = creditNoteDetail.UnitPrice;
                creditNoteDetailModel.GrossAmountFc = creditNoteDetail.GrossAmountFc;
                creditNoteDetailModel.GrossAmount = creditNoteDetail.GrossAmount;
                creditNoteDetailModel.UnitPrice = creditNoteDetail.UnitPrice;
                creditNoteDetailModel.TaxAmountFc = creditNoteDetail.TaxAmountFc;
                creditNoteDetailModel.TaxAmount = creditNoteDetail.TaxAmount;
                creditNoteDetailModel.NetAmountFc = creditNoteDetail.NetAmountFc;
                creditNoteDetailModel.NetAmount = creditNoteDetail.NetAmount;

                //--####
                creditNoteDetailModel.UnitOfMeasurementName = null != creditNoteDetail.UnitOfMeasurement ? creditNoteDetail.UnitOfMeasurement.UnitOfMeasurementName : null;
                creditNoteDetailModel.IsTaxDetVisible = null != creditNoteDetail.CreditNote ?
                                                            (creditNoteDetail.CreditNote.TaxModelType == TaxModelType.LineWise.ToString() ? true : false)
                                                            : false;

                return creditNoteDetailModel;
            });
        }

    }
}
