using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class PaymentVoucherDetailService : Repository<Paymentvoucherdetail>, IPaymentVoucherDetail
    {
        IPaymentVoucher paymentVoucher;

        public PaymentVoucherDetailService(ErpDbContext dbContext, IPaymentVoucher _paymentVoucher) : base(dbContext)
        {
            paymentVoucher = _paymentVoucher;
        }

        public async Task<int> CreatePaymentVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel)
        {
            int paymentVoucherDetailId = 0;

            // assign values.
            Paymentvoucherdetail paymentVoucherDetail = new Paymentvoucherdetail();

            paymentVoucherDetail.PaymentVoucherId = paymentVoucherDetailModel.PaymentVoucherId;
            paymentVoucherDetail.ParticularLedgerId = paymentVoucherDetailModel.ParticularLedgerId;
            paymentVoucherDetail.TransactionTypeId = paymentVoucherDetailModel.TransactionTypeId;
            paymentVoucherDetail.AmountFc = paymentVoucherDetailModel.AmountFc;
            paymentVoucherDetail.Amount = 0;
            paymentVoucherDetail.Narration = paymentVoucherDetailModel.Narration;
            paymentVoucherDetail.PurchaseInvoiceId = paymentVoucherDetailModel.PurchaseInvoiceId;
            paymentVoucherDetail.DebitNoteId = paymentVoucherDetailModel.DebitNoteId;
            paymentVoucherDetail.CreditNoteId = paymentVoucherDetailModel.CreditNoteId;

            if (paymentVoucherDetailId != 0)
            {
                await UpdatePaymentVoucherDetailAmount(paymentVoucherDetailId);
            }

            await Create(paymentVoucherDetail);

            paymentVoucherDetailId = paymentVoucherDetail.PaymentVoucherDetId;

            return paymentVoucherDetailId; // returns.
        }

        public async Task<bool> UpdatePaymentVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucherdetail paymentVoucherDetail = await GetByIdAsync(w => w.PaymentVoucherDetId == paymentVoucherDetailModel.PaymentVoucherDetId);

            if (null != paymentVoucherDetail)
            {

                // assign values.
                paymentVoucherDetail.ParticularLedgerId = paymentVoucherDetailModel.ParticularLedgerId;
                paymentVoucherDetail.TransactionTypeId = paymentVoucherDetailModel.TransactionTypeId;
                paymentVoucherDetail.AmountFc = paymentVoucherDetailModel.AmountFc;
                paymentVoucherDetail.Amount = 0;
                paymentVoucherDetail.Narration = paymentVoucherDetailModel.Narration;
                paymentVoucherDetail.PurchaseInvoiceId = paymentVoucherDetailModel.PurchaseInvoiceId;
                paymentVoucherDetail.DebitNoteId = paymentVoucherDetailModel.DebitNoteId;
                paymentVoucherDetail.CreditNoteId = paymentVoucherDetailModel.CreditNoteId;

                isUpdated = await Update(paymentVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdatePaymentVoucherDetailAmount(paymentVoucherDetailModel.PaymentVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdatePaymentVoucherDetailAmount(int? paymentVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucherdetail paymentVoucherDetail = await GetQueryByCondition(w => w.PaymentVoucherDetId == paymentVoucherDetailId)
                                                                 .Include(w => w.PaymentVoucher).FirstOrDefaultAsync();

            if (null != paymentVoucherDetail)
            {
                paymentVoucherDetail.Amount = paymentVoucherDetail.AmountFc * paymentVoucherDetail.PaymentVoucher.ExchangeRate;

                isUpdated = await Update(paymentVoucherDetail);
            }

            if (isUpdated != false)
            {
                await paymentVoucher.UpdatePaymentVoucherMasterAmount(paymentVoucherDetail.PaymentVoucherId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeletePaymentVoucherDetail(int paymentVoucherDetailId)
        {
            bool isDeleted = false;

            // get record.
            Paymentvoucherdetail paymentVoucherDetail = await GetByIdAsync(w => w.PaymentVoucherDetId == paymentVoucherDetailId);

            if (null != paymentVoucherDetail)
            {
                isDeleted = await Delete(paymentVoucherDetail);
            }

            if (isDeleted != false)
            {
                await paymentVoucher.UpdatePaymentVoucherMasterAmount(paymentVoucherDetail.PaymentVoucherId);
            }

            return isDeleted; // returns.
        }

        public async Task<PaymentVoucherDetailModel> GetPaymentVoucherDetailById(int paymentVoucherDetailId)
        {
            PaymentVoucherDetailModel paymentVoucherDetailModel = null;

            IList<PaymentVoucherDetailModel> paymentVoucherModelDetailList = await GetPaymentVoucherDetailList(paymentVoucherDetailId, 0);

            if (null != paymentVoucherModelDetailList && paymentVoucherModelDetailList.Any())
            {
                paymentVoucherDetailModel = paymentVoucherModelDetailList.FirstOrDefault();
            }

            return paymentVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<PaymentVoucherDetailModel>> GetPaymentVoucherDetailByPaymentVoucherId(int paymentVoucherId)
        {
            DataTableResultModel<PaymentVoucherDetailModel> resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = await GetPaymentVoucherDetailList(0, paymentVoucherId);

            if (null != paymentVoucherDetailModelList && paymentVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();
                resultModel.ResultList = paymentVoucherDetailModelList;
                resultModel.TotalResultCount = paymentVoucherDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<PaymentVoucherDetailModel>> GetPaymentVoucherDetailList()
        {
            DataTableResultModel<PaymentVoucherDetailModel> resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = await GetPaymentVoucherDetailList(0, 0);

            if (null != paymentVoucherDetailModelList && paymentVoucherDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();
                resultModel.ResultList = paymentVoucherDetailModelList;
                resultModel.TotalResultCount = paymentVoucherDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PaymentVoucherDetailModel>> GetPaymentVoucherDetailList(int paymentVoucherDetailId, int paymentVoucherId)
        {
            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = null;

            // create query.
            IQueryable<Paymentvoucherdetail> query = GetQueryByCondition(w => w.PaymentVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger)
                                                    .Include(w => w.PurchaseInvoice).Include(w => w.CreditNote).Include(w => w.DebitNote);

            // apply filters.
            if (0 != paymentVoucherDetailId)
                query = query.Where(w => w.PaymentVoucherDetId == paymentVoucherDetailId);

            if (0 != paymentVoucherId)
                query = query.Where(w => w.PaymentVoucherId == paymentVoucherId);

            // get records by query.
            List<Paymentvoucherdetail> paymentVoucherDetailList = await query.ToListAsync();

            if (null != paymentVoucherDetailList && paymentVoucherDetailList.Count > 0)
            {
                paymentVoucherDetailModelList = new List<PaymentVoucherDetailModel>();

                foreach (Paymentvoucherdetail paymentVoucherDetail in paymentVoucherDetailList)
                {
                    paymentVoucherDetailModelList.Add(await AssignValueToModel(paymentVoucherDetail));
                }
            }

            return paymentVoucherDetailModelList; // returns.
        }

        private async Task<PaymentVoucherDetailModel> AssignValueToModel(Paymentvoucherdetail paymentVoucherDetail)
        {
            return await Task.Run(() =>
            {
                PaymentVoucherDetailModel paymentVoucherDetailModel = new PaymentVoucherDetailModel();

                paymentVoucherDetailModel.PaymentVoucherDetId = paymentVoucherDetail.PaymentVoucherDetId;
                paymentVoucherDetailModel.PaymentVoucherId = paymentVoucherDetail.PaymentVoucherId;
                paymentVoucherDetailModel.ParticularLedgerId = paymentVoucherDetail.ParticularLedgerId;
                paymentVoucherDetailModel.TransactionTypeId = paymentVoucherDetail.TransactionTypeId;
                paymentVoucherDetailModel.AmountFc = paymentVoucherDetail.AmountFc;
                paymentVoucherDetailModel.Amount = paymentVoucherDetail.Amount;
                paymentVoucherDetailModel.Narration = paymentVoucherDetail.Narration;

                paymentVoucherDetailModel.PurchaseInvoiceId = null != paymentVoucherDetail.PurchaseInvoiceId ? paymentVoucherDetail.PurchaseInvoiceId : 0;
                paymentVoucherDetailModel.CreditNoteId = null != paymentVoucherDetail.CreditNoteId ? paymentVoucherDetail.CreditNoteId : 0;
                paymentVoucherDetailModel.DebitNoteId = null != paymentVoucherDetail.DebitNoteId ? paymentVoucherDetail.DebitNoteId : 0;

                //--####
                paymentVoucherDetailModel.TransactionTypeName = EnumHelper.GetEnumDescription<TransactionType>(((TransactionType)paymentVoucherDetail.TransactionTypeId).ToString());
                paymentVoucherDetailModel.ParticularLedgerName = null != paymentVoucherDetail.ParticularLedger ? paymentVoucherDetail.ParticularLedger.LedgerName : null;

                if (paymentVoucherDetailModel.PurchaseInvoiceId != 0 && paymentVoucherDetailModel.CreditNoteId == 0 && paymentVoucherDetailModel.DebitNoteId == 0)
                {
                    paymentVoucherDetailModel.InvoiceType = "Purchase Invoice";
                    paymentVoucherDetailModel.InvoiceNo = paymentVoucherDetail.PurchaseInvoice.InvoiceNo;
                }
                else if (paymentVoucherDetailModel.PurchaseInvoiceId == 0 && paymentVoucherDetailModel.CreditNoteId != 0 && paymentVoucherDetailModel.DebitNoteId == 0)
                {
                    paymentVoucherDetailModel.InvoiceType = "Credit Note";
                    paymentVoucherDetailModel.InvoiceNo = paymentVoucherDetail.CreditNote.CreditNoteNo;
                }
                else if (paymentVoucherDetailModel.PurchaseInvoiceId == 0 && paymentVoucherDetailModel.CreditNoteId == 0 && paymentVoucherDetailModel.DebitNoteId != 0)
                {
                    paymentVoucherDetailModel.InvoiceType = "Debit Note";
                    paymentVoucherDetailModel.InvoiceNo = paymentVoucherDetail.DebitNote.DebitNoteNo;
                }

                return paymentVoucherDetailModel;
            });
        }

    }
}
