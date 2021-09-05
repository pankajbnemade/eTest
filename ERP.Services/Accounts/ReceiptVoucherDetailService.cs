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
    public class ReceiptVoucherDetailService : Repository<Receiptvoucherdetail>, IReceiptVoucherDetail
    {
        IReceiptVoucher receiptVoucher;

        public ReceiptVoucherDetailService(ErpDbContext dbContext, IReceiptVoucher _receiptVoucher) : base(dbContext)
        {
            receiptVoucher = _receiptVoucher;
        }

        public async Task<int> CreateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel)
        {
            int receiptVoucherDetailId = 0;

            // assign values.
            Receiptvoucherdetail receiptVoucherDetail = new Receiptvoucherdetail();

            receiptVoucherDetail.ReceiptVoucherId = receiptVoucherDetailModel.ReceiptVoucherId;
            receiptVoucherDetail.ParticularLedgerId = receiptVoucherDetailModel.ParticularLedgerId;
            receiptVoucherDetail.TransactionTypeId = receiptVoucherDetailModel.TransactionTypeId;
            receiptVoucherDetail.AmountFc = receiptVoucherDetailModel.AmountFc;
            receiptVoucherDetail.Amount = 0;
            receiptVoucherDetail.Narration = receiptVoucherDetailModel.Narration;
            receiptVoucherDetail.SalesInvoiceId = receiptVoucherDetailModel.SalesInvoiceId;
            receiptVoucherDetail.DebitNoteId = receiptVoucherDetailModel.DebitNoteId;
            receiptVoucherDetail.CreditNoteId = receiptVoucherDetailModel.CreditNoteId;

            if (receiptVoucherDetailId != 0)
            {
                await UpdateReceiptVoucherDetailAmount(receiptVoucherDetailId);
            }

            await Create(receiptVoucherDetail);

            receiptVoucherDetailId = receiptVoucherDetail.ReceiptVoucherDetId;

            return receiptVoucherDetailId; // returns.
        }

        public async Task<bool> UpdateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucherdetail receiptVoucherDetail = await GetByIdAsync(w => w.ReceiptVoucherDetId == receiptVoucherDetailModel.ReceiptVoucherDetId);

            if (null != receiptVoucherDetail)
            {

                // assign values.
                receiptVoucherDetail.ParticularLedgerId = receiptVoucherDetailModel.ParticularLedgerId;
                receiptVoucherDetail.TransactionTypeId = receiptVoucherDetailModel.TransactionTypeId;
                receiptVoucherDetail.AmountFc = receiptVoucherDetailModel.AmountFc;
                receiptVoucherDetail.Amount = 0;
                receiptVoucherDetail.Narration = receiptVoucherDetailModel.Narration;
                receiptVoucherDetail.SalesInvoiceId = receiptVoucherDetailModel.SalesInvoiceId;
                receiptVoucherDetail.DebitNoteId = receiptVoucherDetailModel.DebitNoteId;
                receiptVoucherDetail.CreditNoteId = receiptVoucherDetailModel.CreditNoteId;

                isUpdated = await Update(receiptVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdateReceiptVoucherDetailAmount(receiptVoucherDetailModel.ReceiptVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateReceiptVoucherDetailAmount(int? receiptVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucherdetail receiptVoucherDetail = await GetQueryByCondition(w => w.ReceiptVoucherDetId == receiptVoucherDetailId)
                                                                 .Include(w => w.ReceiptVoucher).FirstOrDefaultAsync();

            if (null != receiptVoucherDetail)
            {
                receiptVoucherDetail.Amount = receiptVoucherDetail.AmountFc * receiptVoucherDetail.ReceiptVoucher.ExchangeRate;

                isUpdated = await Update(receiptVoucherDetail);
            }

            if (isUpdated != false)
            {
                await receiptVoucher.UpdateReceiptVoucherMasterAmount(receiptVoucherDetail.ReceiptVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteReceiptVoucherDetail(int receiptVoucherDetailId)
        {
            bool isDeleted = false;

            // get record.
            Receiptvoucherdetail receiptVoucherDetail = await GetByIdAsync(w => w.ReceiptVoucherDetId == receiptVoucherDetailId);

            if (null != receiptVoucherDetail)
            {
                isDeleted = await Delete(receiptVoucherDetail);
            }

            if (isDeleted != false)
            {
                await receiptVoucher.UpdateReceiptVoucherMasterAmount(receiptVoucherDetail.ReceiptVoucherId);
            }

            return isDeleted; // returns.
        }

        public async Task<ReceiptVoucherDetailModel> GetReceiptVoucherDetailById(int receiptVoucherDetailId)
        {
            ReceiptVoucherDetailModel receiptVoucherDetailModel = null;

            IList<ReceiptVoucherDetailModel> receiptVoucherModelDetailList = await GetReceiptVoucherDetailList(receiptVoucherDetailId, 0);

            if (null != receiptVoucherModelDetailList && receiptVoucherModelDetailList.Any())
            {
                receiptVoucherDetailModel = receiptVoucherModelDetailList.FirstOrDefault();
            }

            return receiptVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailByReceiptVoucherId(int receiptVoucherId)
        {
            DataTableResultModel<ReceiptVoucherDetailModel> resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = await GetReceiptVoucherDetailList(0, receiptVoucherId);

            if (null != receiptVoucherDetailModelList && receiptVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();
                resultModel.ResultList = receiptVoucherDetailModelList;
                resultModel.TotalResultCount = receiptVoucherDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailList()
        {
            DataTableResultModel<ReceiptVoucherDetailModel> resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = await GetReceiptVoucherDetailList(0, 0);

            if (null != receiptVoucherDetailModelList && receiptVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();
                resultModel.ResultList = receiptVoucherDetailModelList;
                resultModel.TotalResultCount = receiptVoucherDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailList(int receiptVoucherDetailId, int receiptVoucherId)
        {
            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = null;

            // create query.
            IQueryable<Receiptvoucherdetail> query = GetQueryByCondition(w => w.ReceiptVoucherDetId != 0);

            // apply filters.
            if (0 != receiptVoucherDetailId)
                query = query.Where(w => w.ReceiptVoucherDetId == receiptVoucherDetailId);

            if (0 != receiptVoucherId)
                query = query.Where(w => w.ReceiptVoucherId == receiptVoucherId);

            // get records by query.
            List<Receiptvoucherdetail> receiptVoucherDetailList = await query.ToListAsync();

            if (null != receiptVoucherDetailList && receiptVoucherDetailList.Count > 0)
            {
                receiptVoucherDetailModelList = new List<ReceiptVoucherDetailModel>();

                foreach (Receiptvoucherdetail receiptVoucherDetail in receiptVoucherDetailList)
                {
                    receiptVoucherDetailModelList.Add(await AssignValueToModel(receiptVoucherDetail));
                }
            }

            return receiptVoucherDetailModelList; // returns.
        }

        private async Task<ReceiptVoucherDetailModel> AssignValueToModel(Receiptvoucherdetail receiptVoucherDetail)
        {
            return await Task.Run(() =>
            {
                ReceiptVoucherDetailModel receiptVoucherDetailModel = new ReceiptVoucherDetailModel();

                receiptVoucherDetailModel.ReceiptVoucherDetId = receiptVoucherDetail.ReceiptVoucherDetId;
                receiptVoucherDetailModel.ReceiptVoucherId = receiptVoucherDetail.ReceiptVoucherId;
                receiptVoucherDetailModel.ParticularLedgerId = receiptVoucherDetail.ParticularLedgerId;
                receiptVoucherDetailModel.TransactionTypeId = receiptVoucherDetail.TransactionTypeId;
                receiptVoucherDetailModel.AmountFc = receiptVoucherDetail.AmountFc;
                receiptVoucherDetailModel.Amount = receiptVoucherDetail.Amount;
                receiptVoucherDetailModel.Narration = receiptVoucherDetail.Narration;

                receiptVoucherDetailModel.SalesInvoiceId = receiptVoucherDetail.SalesInvoiceId;
                receiptVoucherDetailModel.CreditNoteId = receiptVoucherDetail.CreditNoteId;
                receiptVoucherDetailModel.DebitNoteId = receiptVoucherDetail.DebitNoteId;

                //--####
                //receiptVoucherDetailModel.TransactionTypeName = null != receiptVoucherDetail.UnitOfMeasurement ? receiptVoucherDetail.UnitOfMeasurement.UnitOfMeasurementName : null;

                return receiptVoucherDetailModel;
            });
        }

    }
}
