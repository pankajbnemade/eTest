using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class ReceiptVoucherDetailController : Controller
    {
        private readonly IReceiptVoucherDetail _receiptVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IOutstandingInvoice _outstandingInvoice;
        private readonly IReceiptVoucher _receiptVoucher;

        public ReceiptVoucherDetailController(IReceiptVoucherDetail receiptVoucherDetail, ILedger ledger, ICurrencyConversion currencyConversion,
            IOutstandingInvoice outstandingInvoice,
            IReceiptVoucher receiptVoucher)
        {
            this._receiptVoucherDetail = receiptVoucherDetail;
            this._ledger = ledger;
            this._currencyConversion = currencyConversion;
            this._outstandingInvoice = outstandingInvoice;
            this._receiptVoucher = receiptVoucher;
        }

        public async Task<IActionResult> VoucherDetail(int receiptVoucherId, int addRow_Blank)
        {
            ViewBag.ReceiptVoucherId = receiptVoucherId;

            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TransactionTypeList = EnumHelper.GetEnumListFor<TransactionType>();

            IList<ReceiptVoucherDetailModel> receiptVoucherDetailModelList = await _receiptVoucherDetail.GetReceiptVoucherDetailByVoucherId(receiptVoucherId, addRow_Blank);

            return await Task.Run(() =>
            {
                return PartialView("_VoucherDetail", receiptVoucherDetailModelList);
            });
        }

        public async Task<IActionResult> MapOutstandingDetail(int particularLedgerId, int receiptVoucherId)
        {
            ReceiptVoucherModel receiptVoucherModel = await _receiptVoucher.GetReceiptVoucherById(receiptVoucherId);

            Decimal exchangeRate = 0;
            Int32 currencyId = receiptVoucherModel.CurrencyId;
            DateTime voucherDate = (DateTime)receiptVoucherModel.VoucherDate;

            CurrencyConversionModel currencyConversionModel = await _currencyConversion.GetExchangeRateByCurrencyId(currencyId, voucherDate);

            exchangeRate = null != currencyConversionModel ? (decimal)currencyConversionModel.ExchangeRate : 0;

            exchangeRate = 0 != exchangeRate ? 1 / exchangeRate : 0;

            //################

            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = await _outstandingInvoice.GetOutstandingInvoiceListByLedgerId(particularLedgerId, "Receipt Voucher", receiptVoucherId, voucherDate, exchangeRate);

            IList<ReceiptVoucherOutstandingInvoiceModel> receiptVoucherOutstandingInvoiceModelList = new List<ReceiptVoucherOutstandingInvoiceModel>(); ;

            foreach (OutstandingInvoiceModel outstandingInvoiceModel in outstandingInvoiceModelList)
            {
                receiptVoucherOutstandingInvoiceModelList.Add(new ReceiptVoucherOutstandingInvoiceModel
                {
                    ReceiptVoucherId = receiptVoucherId,
                    ParticularLedgerId = particularLedgerId,
                    TransactionTypeId = (int)TransactionType.Outstanding,
                    InvoiceId = outstandingInvoiceModel.InvoiceId,
                    InvoiceType = outstandingInvoiceModel.InvoiceType,
                    InvoiceNo = outstandingInvoiceModel.InvoiceNo,
                    InvoiceDate = outstandingInvoiceModel.InvoiceDate,
                    InvoiceAmount = outstandingInvoiceModel.InvoiceAmount,
                    OutstandingAmount = outstandingInvoiceModel.OutstandingAmount,
                    InvoiceAmount_FC = outstandingInvoiceModel.InvoiceAmount_FC,
                    OutstandingAmount_FC = outstandingInvoiceModel.OutstandingAmount_FC,
                    PurchaseInvoiceId = outstandingInvoiceModel.PurchaseInvoiceId,
                    SalesInvoiceId = outstandingInvoiceModel.SalesInvoiceId,
                    CreditNoteId = outstandingInvoiceModel.CreditNoteId,
                    DebitNoteId = outstandingInvoiceModel.DebitNoteId,
                    AmountFc = null,
                    Narration = "",
                });
            }

            return await Task.Run(() =>
            {
                return PartialView("_MapOutstandingDetail", receiptVoucherOutstandingInvoiceModelList);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveVoucherDetail(List<ReceiptVoucherDetailModel> receiptVoucherDetailModelList)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int receiptVoucherId = 0;

            if (ModelState.IsValid)
            {
                foreach (ReceiptVoucherDetailModel receiptVoucherDetailModel in receiptVoucherDetailModelList)
                {
                    receiptVoucherId = receiptVoucherDetailModel.ReceiptVoucherId;

                    if (receiptVoucherDetailModel.ReceiptVoucherDetId > 0)
                    {
                        // update record.
                        if (true == await _receiptVoucherDetail.UpdateReceiptVoucherDetail(receiptVoucherDetailModel))
                        {
                            data.Result.Status = true;
                            data.Result.Data = receiptVoucherDetailModel.ReceiptVoucherId;
                        }
                    }
                    else
                    {
                        // add new record.
                        if (await _receiptVoucherDetail.CreateReceiptVoucherDetail(receiptVoucherDetailModel) > 0)
                        {
                            data.Result.Status = true;
                            data.Result.Data = receiptVoucherDetailModel.ReceiptVoucherId;
                        }
                    }

                }

                await _receiptVoucher.UpdateReceiptVoucherMasterAmount(receiptVoucherId);

            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> SaveOutstandingDetail(List<ReceiptVoucherOutstandingInvoiceModel> receiptVoucherOutstandingInvoiceModelList)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int receiptVoucherId = 0;

            if (ModelState.IsValid)
            {
                ReceiptVoucherDetailModel receiptVoucherDetailModel = null;

                foreach (ReceiptVoucherOutstandingInvoiceModel receiptVoucherOutstandingInvoiceModel in receiptVoucherOutstandingInvoiceModelList)
                {
                    receiptVoucherId = receiptVoucherOutstandingInvoiceModel.ReceiptVoucherId;

                    receiptVoucherDetailModel = new ReceiptVoucherDetailModel
                    {
                        ReceiptVoucherDetId = 0,
                        ReceiptVoucherId = receiptVoucherOutstandingInvoiceModel.ReceiptVoucherId,
                        ParticularLedgerId = receiptVoucherOutstandingInvoiceModel.ParticularLedgerId,
                        TransactionTypeId = receiptVoucherOutstandingInvoiceModel.TransactionTypeId,
                        SalesInvoiceId = receiptVoucherOutstandingInvoiceModel.SalesInvoiceId,
                        CreditNoteId = receiptVoucherOutstandingInvoiceModel.CreditNoteId,
                        AmountFc = receiptVoucherOutstandingInvoiceModel.AmountFc == null ? 0 : (decimal)receiptVoucherOutstandingInvoiceModel.AmountFc,
                        Narration = receiptVoucherOutstandingInvoiceModel.Narration,
                    };

                    if (receiptVoucherDetailModel.ReceiptVoucherId == 0
                        || receiptVoucherDetailModel.ParticularLedgerId == 0
                        || receiptVoucherDetailModel.TransactionTypeId == 0
                        || (receiptVoucherDetailModel.SalesInvoiceId == 0 && receiptVoucherOutstandingInvoiceModel.CreditNoteId == 0)
                        || receiptVoucherDetailModel.AmountFc == 0
                        )
                    {
                        // skip as all required fields are not entered
                        continue; // Skip the remainder of this iteration. go back to foreach
                    }

                    // add new record.
                    if (await _receiptVoucherDetail.CreateReceiptVoucherDetail(receiptVoucherDetailModel) > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = receiptVoucherDetailModel.ReceiptVoucherId;
                    }

                }

                await _receiptVoucher.UpdateReceiptVoucherMasterAmount(receiptVoucherId);

            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherDetail(int receiptVoucherDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _receiptVoucherDetail.DeleteReceiptVoucherDetail(receiptVoucherDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}
