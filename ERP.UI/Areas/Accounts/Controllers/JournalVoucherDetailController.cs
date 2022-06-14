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
    [Area("Accounts")]
    public class JournalVoucherDetailController : Controller
    {
        private readonly IJournalVoucherDetail _journalVoucherDetail;
        private readonly ILedger _ledger;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IOutstandingInvoice _outstandingInvoice;
        private readonly IJournalVoucher _journalVoucher;

        public JournalVoucherDetailController(IJournalVoucherDetail journalVoucherDetail, ILedger ledger, ICurrencyConversion currencyConversion,
            IOutstandingInvoice outstandingInvoice,
            IJournalVoucher journalVoucher)
        {
            this._journalVoucherDetail = journalVoucherDetail;
            this._ledger = ledger;
            this._currencyConversion = currencyConversion;
            this._outstandingInvoice = outstandingInvoice;
            this._journalVoucher = journalVoucher;
        }

        public async Task<IActionResult> VoucherDetail(int journalVoucherId, int addRow_Blank)
        {
            ViewBag.JournalVoucherId = journalVoucherId;

            JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

            ViewBag.ParticularLedgerList = await _ledger.GetLedgerSelectList(0, journalVoucherModel.CompanyId, true);
            ViewBag.TransactionTypeList = EnumHelper.GetEnumListFor<TransactionType>();

            IList<JournalVoucherDetailModel> journalVoucherDetailModelList = await _journalVoucherDetail.GetJournalVoucherDetailByVoucherId(journalVoucherId, addRow_Blank);

            return await Task.Run(() =>
            {
                return PartialView("_VoucherDetail", journalVoucherDetailModelList);
            });
        }

        public async Task<IActionResult> MapOutstandingDetail(int particularLedgerId, int journalVoucherId)
        {
            JournalVoucherModel journalVoucherModel = await _journalVoucher.GetJournalVoucherById(journalVoucherId);

            Decimal exchangeRate = 0;
            Int32 currencyId = journalVoucherModel.CurrencyId;
            DateTime voucherDate = (DateTime)journalVoucherModel.VoucherDate;

            CurrencyConversionModel currencyConversionModel = await _currencyConversion.GetExchangeRateByCurrencyId(currencyId, voucherDate);

            exchangeRate = null != currencyConversionModel ? (decimal)currencyConversionModel.ExchangeRate : 0;

            exchangeRate = 0 != exchangeRate ? 1 / exchangeRate : 0;

            //################

            IList<OutstandingInvoiceModel> outstandingInvoiceModelList = await _outstandingInvoice.GetOutstandingInvoiceListByLedgerId(particularLedgerId, "Journal Voucher", journalVoucherId, voucherDate, exchangeRate);

            IList<JournalVoucherOutstandingInvoiceModel> journalVoucherOutstandingInvoiceModelList = new List<JournalVoucherOutstandingInvoiceModel>(); ;

            foreach (OutstandingInvoiceModel outstandingInvoiceModel in outstandingInvoiceModelList)
            {
                journalVoucherOutstandingInvoiceModelList.Add(new JournalVoucherOutstandingInvoiceModel
                {
                    JournalVoucherId = journalVoucherId,
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
                    CreditAmountFc = null,
                    DebitAmountFc = null,
                    Narration = "",
                });
            }

            return await Task.Run(() =>
            {
                return PartialView("_MapOutstandingDetail", journalVoucherOutstandingInvoiceModelList);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveVoucherDetail(List<JournalVoucherDetailModel> journalVoucherDetailModelList)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int journalVoucherId = 0;

            if (ModelState.IsValid)
            {
                foreach (JournalVoucherDetailModel journalVoucherDetailModel in journalVoucherDetailModelList)
                {
                    journalVoucherId = journalVoucherDetailModel.JournalVoucherId;

                    if (journalVoucherDetailModel.JournalVoucherDetId > 0)
                    {
                        // update record.
                        if (true == await _journalVoucherDetail.UpdateJournalVoucherDetail(journalVoucherDetailModel))
                        {
                            data.Result.Status = true;
                            data.Result.Data = journalVoucherDetailModel.JournalVoucherId;
                        }
                    }
                    else
                    {
                        // add new record.
                        if (await _journalVoucherDetail.CreateJournalVoucherDetail(journalVoucherDetailModel) > 0)
                        {
                            data.Result.Status = true;
                            data.Result.Data = journalVoucherDetailModel.JournalVoucherId;
                        }
                    }

                }

                await _journalVoucher.UpdateJournalVoucherMasterAmount(journalVoucherId);
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> SaveOutstandingDetail(List<JournalVoucherOutstandingInvoiceModel> journalVoucherOutstandingInvoiceModelList)
        {

            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int journalVoucherId = 0;

            if (ModelState.IsValid)
            {
                JournalVoucherDetailModel journalVoucherDetailModel = null;

                foreach (JournalVoucherOutstandingInvoiceModel journalVoucherOutstandingInvoiceModel in journalVoucherOutstandingInvoiceModelList)
                {
                    journalVoucherId = journalVoucherOutstandingInvoiceModel.JournalVoucherId;

                    journalVoucherDetailModel = new JournalVoucherDetailModel
                    {
                        JournalVoucherDetId = 0,
                        JournalVoucherId = journalVoucherOutstandingInvoiceModel.JournalVoucherId,
                        ParticularLedgerId = journalVoucherOutstandingInvoiceModel.ParticularLedgerId,
                        TransactionTypeId = journalVoucherOutstandingInvoiceModel.TransactionTypeId,
                        SalesInvoiceId = journalVoucherOutstandingInvoiceModel.SalesInvoiceId,
                        PurchaseInvoiceId = journalVoucherOutstandingInvoiceModel.PurchaseInvoiceId,
                        DebitNoteId = journalVoucherOutstandingInvoiceModel.DebitNoteId,
                        CreditNoteId = journalVoucherOutstandingInvoiceModel.CreditNoteId,
                        CreditAmountFc = journalVoucherOutstandingInvoiceModel.CreditAmountFc == null ? 0 : (decimal)journalVoucherOutstandingInvoiceModel.CreditAmountFc,
                        DebitAmountFc = journalVoucherOutstandingInvoiceModel.DebitAmountFc == null ? 0 : (decimal)journalVoucherOutstandingInvoiceModel.DebitAmountFc,
                        Narration = journalVoucherOutstandingInvoiceModel.Narration,
                    };

                    if (journalVoucherDetailModel.JournalVoucherId == 0
                        || journalVoucherDetailModel.ParticularLedgerId == 0
                        || journalVoucherDetailModel.TransactionTypeId == 0
                        || (journalVoucherDetailModel.PurchaseInvoiceId == 0 && journalVoucherDetailModel.DebitNoteId == 0 && journalVoucherDetailModel.PurchaseInvoiceId == 0 && journalVoucherDetailModel.CreditNoteId == 0)
                        || (journalVoucherDetailModel.CreditAmountFc == 0 && journalVoucherDetailModel.DebitAmountFc == 0)
                        || (journalVoucherDetailModel.CreditAmountFc != 0 && journalVoucherDetailModel.DebitAmountFc != 0)
                        )
                    {
                        // skip as all required fields are not entered
                        continue; // Skip the remainder of this iteration. go back to foreach
                    }

                    // add new record.
                    if (await _journalVoucherDetail.CreateJournalVoucherDetail(journalVoucherDetailModel) > 0)
                    {
                        data.Result.Status = true;
                        data.Result.Data = journalVoucherDetailModel.JournalVoucherId;
                    }

                }

                await _journalVoucher.UpdateJournalVoucherMasterAmount(journalVoucherId);

            }

            return Json(data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVoucherDetail(int journalVoucherDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _journalVoucherDetail.DeleteJournalVoucherDetail(journalVoucherDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }

    }
}
