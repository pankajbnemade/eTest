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
    public class ReceiptVoucherDetailService : Repository<Receiptvoucherdetail>, IReceiptVoucherDetail
    {
        private readonly IReceiptVoucher _receiptVoucher;
        private readonly ILedger _ledger;

        public ReceiptVoucherDetailService(ErpDbContext dbContext, IReceiptVoucher receiptVoucher, ILedger ledger) : base(dbContext)
        {
            _receiptVoucher = receiptVoucher;
            _ledger = ledger;
        }

        public async Task<int> CreateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel)
        {
            int receiptVoucherDetailId = 0;

            Receiptvoucherdetail receiptVoucherDetail = new Receiptvoucherdetail();

            receiptVoucherDetail.ReceiptVoucherId = receiptVoucherDetailModel.ReceiptVoucherId;
            receiptVoucherDetail.ParticularLedgerId = receiptVoucherDetailModel.ParticularLedgerId;
            receiptVoucherDetail.TransactionTypeId = receiptVoucherDetailModel.TransactionTypeId;
            receiptVoucherDetail.AmountFc = receiptVoucherDetailModel.AmountFc;
            receiptVoucherDetail.Amount = 0;

            receiptVoucherDetail.Narration = receiptVoucherDetailModel.Narration;
            receiptVoucherDetail.SalesInvoiceId = receiptVoucherDetailModel.SalesInvoiceId;
            receiptVoucherDetail.CreditNoteId = receiptVoucherDetailModel.CreditNoteId;

            await Create(receiptVoucherDetail);

            receiptVoucherDetailId = receiptVoucherDetail.ReceiptVoucherDetId;

            if (receiptVoucherDetailId != 0)
            {
                await UpdateReceiptVoucherDetailAmount(receiptVoucherDetailId);
            }

            return receiptVoucherDetailId; // returns.
        }

        public async Task<bool> UpdateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucherdetail receiptVoucherDetail = await GetByIdAsync(w => w.ReceiptVoucherDetId == receiptVoucherDetailModel.ReceiptVoucherDetId);

            if (null != receiptVoucherDetail)
            {
                receiptVoucherDetail.ParticularLedgerId = receiptVoucherDetailModel.ParticularLedgerId;
                receiptVoucherDetail.TransactionTypeId = receiptVoucherDetailModel.TransactionTypeId;
                receiptVoucherDetail.AmountFc = receiptVoucherDetailModel.AmountFc;
                receiptVoucherDetail.Amount = 0;
                receiptVoucherDetail.Narration = receiptVoucherDetailModel.Narration == null ? "" : receiptVoucherDetailModel.Narration;
                //receiptVoucherDetail.SalesInvoiceId = receiptVoucherDetailModel.SalesInvoiceId;
                //receiptVoucherDetail.CreditNoteId = receiptVoucherDetailModel.CreditNoteId;

                isUpdated = await Update(receiptVoucherDetail);
            }

            if (isUpdated != false)
            {
                await UpdateReceiptVoucherDetailAmount(receiptVoucherDetailModel.ReceiptVoucherDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateReceiptVoucherDetailAmount(int receiptVoucherDetailId)
        {
            bool isUpdated = false;

            // get record.
            Receiptvoucherdetail receiptVoucherDetail = await GetQueryByCondition(w => w.ReceiptVoucherDetId == receiptVoucherDetailId)
                                                                 .Include(w => w.ReceiptVoucher).FirstOrDefaultAsync();

            if (null != receiptVoucherDetail)
            {
                receiptVoucherDetail.Amount = receiptVoucherDetail.AmountFc / receiptVoucherDetail.ReceiptVoucher.ExchangeRate;

                isUpdated = await Update(receiptVoucherDetail);
            }

            //if (isUpdated != false)
            //{
            //    await receiptVoucher.UpdateReceiptVoucherMasterAmount(receiptVoucherDetail.ReceiptVoucherId);
            //}

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
                await _receiptVoucher.UpdateReceiptVoucherMasterAmount(receiptVoucherDetail.ReceiptVoucherId);
            }

            return isDeleted; // returns.
        }

        public async Task<ReceiptVoucherDetailModel> GetReceiptVoucherDetailById(int receiptVoucherDetailId, int receiptVoucherId)
        {
            ReceiptVoucherDetailModel receiptVoucherDetailModel = null;

            IList<ReceiptVoucherDetailModel> receiptVoucherModelDetailList = await GetReceiptVoucherDetailList(receiptVoucherDetailId, receiptVoucherId);

            if (null != receiptVoucherModelDetailList && receiptVoucherModelDetailList.Any())
            {
                receiptVoucherDetailModel = receiptVoucherModelDetailList.FirstOrDefault();
            }

            return receiptVoucherDetailModel; // returns.
        }

        public async Task<DataTableResultModel<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailByReceiptVoucherId(int receiptVoucherId, int addRow_Blank)
        {
            DataTableResultModel<ReceiptVoucherDetailModel> resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = await GetReceiptVoucherDetailList(0, receiptVoucherId);

            if (null != receiptVoucherDetailModelList && receiptVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    receiptVoucherDetailModelList.Add(await AddRow_Blank(receiptVoucherId));
                }

                resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();
                resultModel.ResultList = receiptVoucherDetailModelList;
                resultModel.TotalResultCount = receiptVoucherDetailModelList.Count();
            }
            else
            {
                receiptVoucherDetailModelList = new List<ReceiptVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    receiptVoucherDetailModelList.Add(await AddRow_Blank(receiptVoucherId));
                }

                resultModel = new DataTableResultModel<ReceiptVoucherDetailModel>();
                resultModel.ResultList = receiptVoucherDetailModelList;
                resultModel.TotalResultCount = receiptVoucherDetailModelList.Count();
            }


            return resultModel; // returns.
        }

        public async Task<IList<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailByVoucherId(int receiptVoucherId, int addRow_Blank)
        {
            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = await GetReceiptVoucherDetailList(0, receiptVoucherId);

            if (null != receiptVoucherDetailModelList && receiptVoucherDetailModelList.Any())
            {
                if (addRow_Blank == 1)
                {
                    receiptVoucherDetailModelList.Add(await AddRow_Blank(receiptVoucherId));
                }
            }
            else
            {
                receiptVoucherDetailModelList = new List<ReceiptVoucherDetailModel>();

                if (addRow_Blank == 1)
                {
                    receiptVoucherDetailModelList.Add(await AddRow_Blank(receiptVoucherId));
                }

            }

            return receiptVoucherDetailModelList; // returns.
        }

        private async Task<ReceiptVoucherDetailModel> AddRow_Blank(int receiptVoucherId)
        {
            ReceiptVoucherDetailModel receiptVoucherDetailModel = new ReceiptVoucherDetailModel();

            return await Task.Run(() =>
            {
                receiptVoucherDetailModel.ReceiptVoucherId = receiptVoucherId;
                receiptVoucherDetailModel.ParticularLedgerId = 0;
                receiptVoucherDetailModel.TransactionTypeId = 0;
                receiptVoucherDetailModel.AmountFc = 0;
                receiptVoucherDetailModel.Amount = 0;
                receiptVoucherDetailModel.Narration = "";
                receiptVoucherDetailModel.SalesInvoiceId = null;
                receiptVoucherDetailModel.CreditNoteId = null;
                receiptVoucherDetailModel.InvoiceNo = "";
                receiptVoucherDetailModel.InvoiceType = "";
                receiptVoucherDetailModel.ParticularLedgerName = "";
                receiptVoucherDetailModel.TransactionTypeName = "";

                return receiptVoucherDetailModel;
            });
        }

        public async Task<IList<ReceiptVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId)
        {
            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = null;

            // create query.
            IQueryable<Receiptvoucherdetail> query = GetQueryByCondition(w => w.ReceiptVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger)
                                                    .Include(w => w.SalesInvoice).Include(w => w.CreditNote); ;

            // apply filters.
            if (0 != particularLedgerId)
                query = query.Where(w => w.ParticularLedgerId == particularLedgerId);

            // get records by query.
            List<Receiptvoucherdetail> receiptVoucherDetailList = await query.ToListAsync();

            receiptVoucherDetailModelList = new List<ReceiptVoucherDetailModel>();

            if (null != receiptVoucherDetailList && receiptVoucherDetailList.Count > 0)
            {
                foreach (Receiptvoucherdetail receiptVoucherDetail in receiptVoucherDetailList)
                {
                    receiptVoucherDetailModelList.Add(await AssignValueToModel(receiptVoucherDetail));
                }
            }

            return receiptVoucherDetailModelList; // returns.
        }

        private async Task<IList<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailList(int receiptVoucherDetailId, int receiptVoucherId)
        {
            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = null;

            // create query.
            IQueryable<Receiptvoucherdetail> query = GetQueryByCondition(w => w.ReceiptVoucherDetId != 0)
                                                    .Include(w => w.ParticularLedger)
                                                    .Include(w => w.SalesInvoice).Include(w => w.CreditNote);

            // apply filters.
            if (0 != receiptVoucherDetailId)
                query = query.Where(w => w.ReceiptVoucherDetId == receiptVoucherDetailId);

            query = query.Where(w => w.ReceiptVoucherId == receiptVoucherId);

            query = query.OrderBy(w => w.ParticularLedger.LedgerName);

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

                receiptVoucherDetailModel.TransactionTypeName = EnumHelper.GetEnumDescription<TransactionType>(((TransactionType)receiptVoucherDetail.TransactionTypeId).ToString());
                receiptVoucherDetailModel.ParticularLedgerName = null != receiptVoucherDetail.ParticularLedger ? receiptVoucherDetail.ParticularLedger.LedgerName : null;

                if (receiptVoucherDetailModel.SalesInvoiceId != 0 && receiptVoucherDetailModel.SalesInvoiceId != null)
                {
                    receiptVoucherDetailModel.InvoiceType = "Sales Invoice";
                    receiptVoucherDetailModel.InvoiceNo = null != receiptVoucherDetail.SalesInvoice ? receiptVoucherDetail.SalesInvoice.InvoiceNo : null;
                }
                else if (receiptVoucherDetailModel.CreditNoteId != 0 && receiptVoucherDetailModel.CreditNoteId != null)
                {
                    receiptVoucherDetailModel.InvoiceType = "Credit Note";
                    receiptVoucherDetailModel.InvoiceNo = null != receiptVoucherDetail.CreditNote ? receiptVoucherDetail.CreditNote.CreditNoteNo : null;
                }

                return receiptVoucherDetailModel;
            });
        }

        public async Task<AdvanceAdjustmentVoucherDetailModel> GetVoucherDetail(int receiptVoucherDetId)
        {
            // create query.
            IQueryable<Receiptvoucherdetail> query = GetQueryByCondition(w => w.ReceiptVoucherDetId == receiptVoucherDetId)
                                                    .Include(w => w.ReceiptVoucher);

            // get records by query.
            List<Receiptvoucherdetail> receiptVoucherDetailList = await query.ToListAsync();

            Receiptvoucherdetail receiptVoucherDetail = null;

            AdvanceAdjustmentVoucherDetailModel advanceAdjustmentVoucherDetailModel = new AdvanceAdjustmentVoucherDetailModel();

            if (null != receiptVoucherDetailList && receiptVoucherDetailList.Any())
            {
                receiptVoucherDetail = receiptVoucherDetailList.FirstOrDefault();

                advanceAdjustmentVoucherDetailModel.CurrencyId = receiptVoucherDetail.ReceiptVoucher.CurrencyId;
                advanceAdjustmentVoucherDetailModel.ExchangeRate = receiptVoucherDetail.ReceiptVoucher.ExchangeRate;
            }

            return advanceAdjustmentVoucherDetailModel;
        }

        public async Task<IList<SelectListModel>> GetVocuherSelectList(int particularLedgerId, DateTime advanceAdjustmentDate, int voucherDetId, decimal amountFc)
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.ReceiptVoucherDetId != 0))
            {
                var query = GetQueryByCondition(w => w.ReceiptVoucherDetId != 0)
                                                .Include(w => w.ReceiptVoucher)
                                                .Include(w => w.Advanceadjustments)
                                                .ThenInclude(w => w.Advanceadjustmentdetails)
                                                .Where
                                                (
                                                    w => w.ParticularLedgerId == particularLedgerId
                                                    && w.TransactionTypeId == (int)TransactionType.Advance
                                                    && w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved
                                                    && w.ReceiptVoucher.VoucherDate <= advanceAdjustmentDate
                                                    && (
                                                            w.AmountFc > w.Advanceadjustments.Sum(a => a.Advanceadjustmentdetails.Sum(ad => ad.AmountFc))
                                                        || voucherDetId == w.ReceiptVoucherDetId
                                                        )
                                                )
                                                .Select(vd => new
                                                {
                                                    VoucherNo = vd.ReceiptVoucher.VoucherNo,
                                                    VoucherDetId = vd.ReceiptVoucherDetId,
                                                    AmountFc = voucherDetId == vd.ReceiptVoucherDetId
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

        public async Task<decimal> GetVoucherAvailableAmount(int receiptVoucherDetId)
        {
            // create query.
            IQueryable<Receiptvoucherdetail> query = GetQueryByCondition(w => w.ReceiptVoucherDetId == receiptVoucherDetId)
                                                    .Include(w => w.Advanceadjustments)
                                                    .ThenInclude(w => w.Advanceadjustmentdetails);

            // get records by query.
            List<Receiptvoucherdetail> receiptVoucherDetailList = await query.ToListAsync();

            Receiptvoucherdetail receiptVoucherDetail = null;

            decimal availableAmountFc = 0;

            if (null != receiptVoucherDetailList && receiptVoucherDetailList.Any())
            {
                receiptVoucherDetail = receiptVoucherDetailList.FirstOrDefault();

                availableAmountFc = receiptVoucherDetail.AmountFc - receiptVoucherDetail.Advanceadjustments.Sum(a => a.Advanceadjustmentdetails.Sum(ad => ad.AmountFc));
            }

            return availableAmountFc;
        }

        public async Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId)
        {
            IList<GeneralLedgerModel> generalLedgerModelList = null;

            // create query.
            IQueryable<Receiptvoucherdetail> query = GetQueryByCondition(w => w.ReceiptVoucherDetId != 0)
                                                .Include(i => i.ReceiptVoucher).ThenInclude(i => i.Currency)
                                                .Where((w => w.ReceiptVoucher.StatusId == (int)DocumentStatus.Approved && w.ReceiptVoucher.FinancialYearId == yearId && w.ReceiptVoucher.CompanyId == companyId));

            query = query.Where(w => w.ParticularLedgerId == ledgerId);

            query = query.Where(w => w.ReceiptVoucher.VoucherDate >= fromDate && w.ReceiptVoucher.VoucherDate <= toDate);

            // get records by query.
            List<Receiptvoucherdetail> receiptVoucherDetailList = await query.ToListAsync();

            generalLedgerModelList = new List<GeneralLedgerModel>();

            if (null != receiptVoucherDetailList && receiptVoucherDetailList.Count > 0)
            {
                foreach (Receiptvoucherdetail receiptVoucherDetail in receiptVoucherDetailList)
                {
                    generalLedgerModelList.Add(new GeneralLedgerModel()
                    {
                        DocumentId = receiptVoucherDetail.ReceiptVoucher.ReceiptVoucherId,
                        DocumentType = "Receipt Voucher",
                        DocumentNo = receiptVoucherDetail.ReceiptVoucher.VoucherNo,
                        DocumentDate = receiptVoucherDetail.ReceiptVoucher.VoucherDate,
                        Amount_FC = receiptVoucherDetail.AmountFc,
                        Amount = receiptVoucherDetail.Amount,
                        CreditAmount_FC = receiptVoucherDetail.AmountFc,
                        CreditAmount = receiptVoucherDetail.Amount,
                        ReceiptVoucherId = receiptVoucherDetail.ReceiptVoucher.ReceiptVoucherId,
                        CurrencyId = receiptVoucherDetail.ReceiptVoucher.CurrencyId,
                        CurrencyCode = receiptVoucherDetail.ReceiptVoucher.Currency.CurrencyCode,
                        ExchangeRate = receiptVoucherDetail.ReceiptVoucher.ExchangeRate,
                        PartyReferenceNo = receiptVoucherDetail.ReceiptVoucher.ChequeNo,
                    });
                }
            }

            return generalLedgerModelList; // returns.
        }

    }
}
