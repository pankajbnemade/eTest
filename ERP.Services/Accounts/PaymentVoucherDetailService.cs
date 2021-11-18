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
        ILedger ledger;

        public PaymentVoucherDetailService(ErpDbContext dbContext, IPaymentVoucher _paymentVoucher, ILedger _ledger) : base(dbContext)
        {
            paymentVoucher = _paymentVoucher;
            ledger = _ledger;
        }

        public async Task<int> CreatePaymentVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel)
        {
            int paymentVoucherDetailId = 0;

            Paymentvoucherdetail paymentVoucherDetail = new Paymentvoucherdetail();

            paymentVoucherDetail.PaymentVoucherId = paymentVoucherDetailModel.PaymentVoucherId;
            paymentVoucherDetail.ParticularLedgerId = paymentVoucherDetailModel.ParticularLedgerId;
            paymentVoucherDetail.TransactionTypeId = paymentVoucherDetailModel.TransactionTypeId;
            paymentVoucherDetail.AmountFc = paymentVoucherDetailModel.AmountFc;
            paymentVoucherDetail.Amount = 0;

            paymentVoucherDetail.Narration = paymentVoucherDetailModel.Narration;
            paymentVoucherDetail.PurchaseInvoiceId = paymentVoucherDetailModel.PurchaseInvoiceId;
            paymentVoucherDetail.DebitNoteId = paymentVoucherDetailModel.DebitNoteId;

            await Create(paymentVoucherDetail);

            paymentVoucherDetailId = paymentVoucherDetail.PaymentVoucherDetId;

            if (paymentVoucherDetailId != 0)
            {
                await UpdatePaymentVoucherDetailAmount(paymentVoucherDetailId);
            }

            return paymentVoucherDetailId; // returns.
        }

        public async Task<bool> UpdatePaymentVoucherDetail(PaymentVoucherDetailModel paymentVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucherdetail paymentVoucherDetail = await GetByIdAsync(w => w.PaymentVoucherDetId == paymentVoucherDetailModel.PaymentVoucherDetId);

            if (null != paymentVoucherDetail)
            {
                paymentVoucherDetail.ParticularLedgerId = paymentVoucherDetailModel.ParticularLedgerId;
                paymentVoucherDetail.TransactionTypeId = paymentVoucherDetailModel.TransactionTypeId;
                paymentVoucherDetail.AmountFc = paymentVoucherDetailModel.AmountFc;
                paymentVoucherDetail.Amount = 0;
                paymentVoucherDetail.Narration = paymentVoucherDetailModel.Narration == null ? "" : paymentVoucherDetailModel.Narration;
                //paymentVoucherDetail.PurchaseInvoiceId = paymentVoucherDetailModel.PurchaseInvoiceId;
                //paymentVoucherDetail.DebitNoteId = paymentVoucherDetailModel.DebitNoteId;

                isUpdated = await Update(paymentVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdatePaymentVoucherDetailAmount(paymentVoucherDetailModel.PaymentVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdatePaymentVoucherDetailAmount(int paymentVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Paymentvoucherdetail paymentVoucherDetail = await GetQueryByCondition(w => w.PaymentVoucherDetId == paymentVoucherDetailId)
                                                                 .Include(w => w.PaymentVoucher).FirstOrDefaultAsync();

            if (null != paymentVoucherDetail)
            {
                paymentVoucherDetail.Amount = paymentVoucherDetail.AmountFc / paymentVoucherDetail.PaymentVoucher.ExchangeRate;

                isUpdated = await Update(paymentVoucherDetail);
            }

            //if (isUpdated != false)
            //{
            //    await paymentVoucher.UpdatePaymentVoucherMasterAmount(paymentVoucherDetail.PaymentVoucherId);
            //}

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

        public async Task<PaymentVoucherDetailModel> GetPaymentVoucherDetailById(int paymentVoucherDetailId, int paymentVoucherId)
        {
            PaymentVoucherDetailModel paymentVoucherDetailModel = null;

            IList<PaymentVoucherDetailModel> paymentVoucherModelDetailList = await GetPaymentVoucherDetailList(paymentVoucherDetailId, paymentVoucherId);

            if (null != paymentVoucherModelDetailList && paymentVoucherModelDetailList.Any())
            {
                paymentVoucherDetailModel = paymentVoucherModelDetailList.FirstOrDefault();
            }

            return paymentVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<PaymentVoucherDetailModel>> GetPaymentVoucherDetailByPaymentVoucherId(int paymentVoucherId, int addRow_Blank)
        {
            DataTableResultModel<PaymentVoucherDetailModel> resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();

            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = await GetPaymentVoucherDetailList(0, paymentVoucherId);

            if (null != paymentVoucherDetailModelList && paymentVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    paymentVoucherDetailModelList.Add(await AddRow_Blank(paymentVoucherId));
                }

                resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();
                resultModel.ResultList = paymentVoucherDetailModelList;
                resultModel.TotalResultCount = paymentVoucherDetailModelList.Count();
            }
            else
            {
                paymentVoucherDetailModelList = new List<PaymentVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    paymentVoucherDetailModelList.Add(await AddRow_Blank(paymentVoucherId));
                }

                resultModel = new DataTableResultModel<PaymentVoucherDetailModel>();
                resultModel.ResultList = paymentVoucherDetailModelList;
                resultModel.TotalResultCount = paymentVoucherDetailModelList.Count();
            }


            return resultModel; // returns.
        }

        public async Task<IList<PaymentVoucherDetailModel>> GetPaymentVoucherDetailByVoucherId(int paymentVoucherId, int addRow_Blank)
        {
            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = await GetPaymentVoucherDetailList(0, paymentVoucherId);

            if (null != paymentVoucherDetailModelList && paymentVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    paymentVoucherDetailModelList.Add(await AddRow_Blank(paymentVoucherId));
                }
            }
            else
            {
                paymentVoucherDetailModelList = new List<PaymentVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    paymentVoucherDetailModelList.Add(await AddRow_Blank(paymentVoucherId));
                }

            }

            return paymentVoucherDetailModelList; // returns.
        }

        private async Task<PaymentVoucherDetailModel> AddRow_Blank(int paymentVoucherId)
        {
            PaymentVoucherDetailModel paymentVoucherDetailModel = new PaymentVoucherDetailModel();

            return await Task.Run(() =>
            {
                paymentVoucherDetailModel.PaymentVoucherId = paymentVoucherId;
                paymentVoucherDetailModel.ParticularLedgerId = 0;
                paymentVoucherDetailModel.TransactionTypeId = 0;
                paymentVoucherDetailModel.AmountFc = 0;
                paymentVoucherDetailModel.Amount = 0;
                paymentVoucherDetailModel.Narration = "";
                paymentVoucherDetailModel.PurchaseInvoiceId = null;
                paymentVoucherDetailModel.DebitNoteId = null;
                paymentVoucherDetailModel.InvoiceNo = "";
                paymentVoucherDetailModel.InvoiceType = "";
                paymentVoucherDetailModel.ParticularLedgerName = "";
                paymentVoucherDetailModel.TransactionTypeName = "";

                return paymentVoucherDetailModel;
            });
        }

        public async Task<IList<PaymentVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId)
        {
            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = null;

            // create query.
            IQueryable<Paymentvoucherdetail> query = GetQueryByCondition(w => w.PaymentVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger)
                                                    .Include(w => w.PurchaseInvoice).Include(w => w.DebitNote); ;

            // apply filters.
            if (0 != particularLedgerId)
                query = query.Where(w => w.ParticularLedgerId == particularLedgerId);

            // get records by query.
            List<Paymentvoucherdetail> paymentVoucherDetailList = await query.ToListAsync();

            paymentVoucherDetailModelList = new List<PaymentVoucherDetailModel>();

            if (null != paymentVoucherDetailList && paymentVoucherDetailList.Count > 0)
            {
                foreach (Paymentvoucherdetail paymentVoucherDetail in paymentVoucherDetailList)
                {
                    paymentVoucherDetailModelList.Add(await AssignValueToModel(paymentVoucherDetail));
                }
            }

            return paymentVoucherDetailModelList; // returns.
        }

        private async Task<IList<PaymentVoucherDetailModel>> GetPaymentVoucherDetailList(int paymentVoucherDetailId, int paymentVoucherId)
        {
            IList<PaymentVoucherDetailModel> paymentVoucherDetailModelList = null;

            // create query.
            IQueryable<Paymentvoucherdetail> query = GetQueryByCondition(w => w.PaymentVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger)
                                                    .Include(w => w.PurchaseInvoice).Include(w => w.DebitNote);

            // apply filters.
            if (0 != paymentVoucherDetailId)
                query = query.Where(w => w.PaymentVoucherDetId == paymentVoucherDetailId);

            query = query.Where(w => w.PaymentVoucherId == paymentVoucherId);

            query = query.OrderBy(w => w.ParticularLedger.LedgerName);

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

                paymentVoucherDetailModel.PurchaseInvoiceId = paymentVoucherDetail.PurchaseInvoiceId;
                paymentVoucherDetailModel.DebitNoteId = paymentVoucherDetail.DebitNoteId;

                paymentVoucherDetailModel.TransactionTypeName = EnumHelper.GetEnumDescription<TransactionType>(((TransactionType)paymentVoucherDetail.TransactionTypeId).ToString());
                paymentVoucherDetailModel.ParticularLedgerName = null != paymentVoucherDetail.ParticularLedger ? paymentVoucherDetail.ParticularLedger.LedgerName : null;

                if (paymentVoucherDetailModel.PurchaseInvoiceId != 0 && paymentVoucherDetailModel.PurchaseInvoiceId != null)
                {
                    paymentVoucherDetailModel.InvoiceType = "Purchase Invoice";
                    paymentVoucherDetailModel.InvoiceNo = null != paymentVoucherDetail.PurchaseInvoice ? paymentVoucherDetail.PurchaseInvoice.InvoiceNo : null;
                }
                else if (paymentVoucherDetailModel.DebitNoteId != 0 && paymentVoucherDetailModel.DebitNoteId != null)
                {
                    paymentVoucherDetailModel.InvoiceType = "Debit Note";
                    paymentVoucherDetailModel.InvoiceNo = null != paymentVoucherDetail.DebitNote ? paymentVoucherDetail.DebitNote.DebitNoteNo : null;
                }

                return paymentVoucherDetailModel;
            });
        }

        public async Task<AdvanceAdjustmentVoucherDetailModel> GetVoucherDetail(int paymentVoucherDetId)
        {
            // create query.
            IQueryable<Paymentvoucherdetail> query = GetQueryByCondition(w => w.PaymentVoucherDetId == paymentVoucherDetId)
                                                    .Include(w => w.PaymentVoucher);
            // apply filters.
            if (0 != paymentVoucherDetId)
                query = query.Where(w => w.PaymentVoucherDetId == paymentVoucherDetId);

            // get records by query.
            List<Paymentvoucherdetail> paymentVoucherDetailList = await query.ToListAsync();

            Paymentvoucherdetail paymentVoucherDetail = null;

            AdvanceAdjustmentVoucherDetailModel advanceAdjustmentVoucherDetailModel = new AdvanceAdjustmentVoucherDetailModel();

            if (null != paymentVoucherDetailList && paymentVoucherDetailList.Any())
            {
                paymentVoucherDetail = paymentVoucherDetailList.FirstOrDefault();

                advanceAdjustmentVoucherDetailModel.CurrencyId = paymentVoucherDetail.PaymentVoucher.CurrencyId;
                advanceAdjustmentVoucherDetailModel.ExchangeRate = paymentVoucherDetail.PaymentVoucher.ExchangeRate;
            }

            return advanceAdjustmentVoucherDetailModel;
        }

        public async Task<IList<SelectListModel>> GetVocuherSelectList(int particularLedgerId, DateTime advanceAdjustmentDate, int voucherDetId, decimal amountFc)
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.PaymentVoucherDetId != 0))
            {
                var query = GetQueryByCondition(w => w.PaymentVoucherDetId != 0)
                                                .Include(w => w.PaymentVoucher)
                                                .Include(w => w.Advanceadjustments)
                                                .ThenInclude(w => w.Advanceadjustmentdetails)
                                                .Where
                                                (
                                                    w => w.ParticularLedgerId == particularLedgerId
                                                    && w.TransactionTypeId == (int)TransactionType.Advance
                                                    && w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved
                                                    && w.PaymentVoucher.VoucherDate <= advanceAdjustmentDate
                                                    && (
                                                            w.AmountFc > w.Advanceadjustments.Sum(a => a.Advanceadjustmentdetails.Sum(ad => ad.AmountFc))
                                                        || voucherDetId == w.PaymentVoucherDetId
                                                        )
                                                )
                                                .Select(vd => new
                                                {
                                                    VoucherNo = vd.PaymentVoucher.VoucherNo,
                                                    VoucherDetId = vd.PaymentVoucherDetId,
                                                    AmountFc = voucherDetId == vd.PaymentVoucherDetId
                                                            ? (vd.AmountFc - vd.Advanceadjustments.Sum(a => a.Advanceadjustmentdetails.Sum(ad => ad.AmountFc)))
                                                            : (vd.AmountFc - vd.Advanceadjustments.Sum(a => a.Advanceadjustmentdetails.Sum(ad => ad.AmountFc))) + amountFc,
                                                });

                resultModel = await query.Select(s => new SelectListModel
                {
                    DisplayText = s.VoucherNo + " (" + s.AmountFc.ToString() + ")",
                    Value = s.VoucherDetId.ToString()
                }).OrderBy(w => w.DisplayText).ToListAsync();

            }

            return resultModel; // returns.
        }


        public async Task<decimal> GetVoucherAvailableAmount(int paymentVoucherDetId)
        {
            // create query.
            IQueryable<Paymentvoucherdetail> query = GetQueryByCondition(w => w.PaymentVoucherDetId == paymentVoucherDetId)
                                                    .Include(w => w.Advanceadjustments)
                                                    .ThenInclude(w => w.Advanceadjustmentdetails);
            // get records by query.
            List<Paymentvoucherdetail> paymentVoucherDetailList = await query.ToListAsync();

            Paymentvoucherdetail paymentVoucherDetail = null;

            decimal availableAmountFc = 0;

            if (null != paymentVoucherDetailList && paymentVoucherDetailList.Any())
            {
                paymentVoucherDetail = paymentVoucherDetailList.FirstOrDefault();

                availableAmountFc = paymentVoucherDetail.AmountFc - paymentVoucherDetail.Advanceadjustments.Sum(a => a.Advanceadjustmentdetails.Sum(ad => ad.AmountFc));
            }

            return availableAmountFc;
        }

        public async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            // create query.
            IQueryable<Paymentvoucherdetail> query = GetQueryByCondition(w => w.PaymentVoucherDetId != 0)
                                                .Include(i => i.PaymentVoucher).ThenInclude(i => i.Currency)
                                                .Where((w => w.PaymentVoucher.StatusId == (int)DocumentStatus.Approved && w.PaymentVoucher.FinancialYearId == yearId && w.PaymentVoucher.CompanyId == companyId));

            query = query.Where(w => w.ParticularLedgerId == ledgerId);

            query = query.Where(w => w.PaymentVoucher.VoucherDate >= fromDate && w.PaymentVoucher.VoucherDate <= toDate);

            // get records by query.
            List<Paymentvoucherdetail> paymentVoucherDetailList = await query.ToListAsync();

            generalLedgerModelList = new List<GeneralLedgerModel>();

            if (null != paymentVoucherDetailList && paymentVoucherDetailList.Count > 0)
            {
                foreach (Paymentvoucherdetail paymentVoucherDetail in paymentVoucherDetailList)
                {
                    generalLedgerModelList.Add(new GeneralLedgerModel()
                    {
                        DocumentId = paymentVoucherDetail.PaymentVoucher.PaymentVoucherId,
                        DocumentType = "Payment Voucher",
                        DocumentNo = paymentVoucherDetail.PaymentVoucher.VoucherNo,
                        DocumentDate = paymentVoucherDetail.PaymentVoucher.VoucherDate,
                        Amount_FC = paymentVoucherDetail.AmountFc,
                        Amount = paymentVoucherDetail.Amount,
                        DebitAmount_FC = paymentVoucherDetail.AmountFc,
                        DebitAmount = paymentVoucherDetail.Amount,
                        PaymentVoucherId = paymentVoucherDetail.PaymentVoucher.PaymentVoucherId,
                        CurrencyId = paymentVoucherDetail.PaymentVoucher.CurrencyId,
                        CurrencyCode = paymentVoucherDetail.PaymentVoucher.Currency.CurrencyCode,
                        ExchangeRate = paymentVoucherDetail.PaymentVoucher.ExchangeRate,
                        PartyReferenceNo = paymentVoucherDetail.PaymentVoucher.ChequeNo,
                    });
                }
            }

            return generalLedgerModelList; // returns.
        }

    }
}
