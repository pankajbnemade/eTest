using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CreditNoteTaxDetailController : Controller
    {
        private readonly ICreditNoteDetailTax _creditNoteDetailTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public CreditNoteTaxDetailController(ICreditNoteDetailTax creditNoteDetailTax, ILedger ledger)
        {
            this._creditNoteDetailTax = creditNoteDetailTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// creditnote tax detail.
        /// </summary>
        /// <param name="creditNoteDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreditNoteTaxDetail(int creditNoteDetId)
        {
            ViewBag.CreditNoteDetId = creditNoteDetId;

            return await Task.Run(() =>
            {
                return PartialView("_CreditNoteTaxDetail");
            });
        }

        ///// <summary>
        ///// view creditnote tax detail.
        ///// </summary>
        ///// <param name="creditNoteDetId"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> ViewCreditNoteTaxDetail(int creditNoteDetId)
        //{
        //    ViewBag.CreditNoteDetId = creditNoteDetId;

        //    return await Task.Run(() =>
        //    {
        //        return PartialView("_ViewCreditNoteTaxDetail");
        //    });
        //}

        /// <summary>
        /// get credit note tax detail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCreditNoteTaxDetailList(int creditNoteDetId)
        {
            DataTableResultModel<CreditNoteDetailTaxModel> resultModel = await _creditNoteDetailTax.GetCreditNoteDetailTaxByCreditNoteDetailId(creditNoteDetId);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    draw = "1",
                    recordsTotal = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }

        /// <summary>
        /// add creditnote tax detail.
        /// </summary>
        /// <param name="creditNoteDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddCreditNoteTaxDetail(int creditNoteDetId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            CreditNoteDetailTaxModel creditNoteDetailTaxModel = new CreditNoteDetailTaxModel();
            creditNoteDetailTaxModel.CreditNoteDetId = creditNoteDetId;
            creditNoteDetailTaxModel.SrNo = await _creditNoteDetailTax.GenerateSrNo(creditNoteDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteTaxDetail", creditNoteDetailTaxModel);
            });
        }

        /// <summary>
        /// edit creditnote tax detail.
        /// </summary>
        /// <param name="creditNoteDetTaxId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditCreditNoteTaxDetail(int creditNoteDetTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            CreditNoteDetailTaxModel creditNoteDetailTaxModel = await _creditNoteDetailTax.GetCreditNoteDetailTaxById(creditNoteDetTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteTaxDetail", creditNoteDetailTaxModel);
            });
        }

        /// <summary>
        /// save credit note tax detail.
        /// </summary>
        /// <param name="creditNoteDetailTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCreditNoteTaxDetail(CreditNoteDetailTaxModel creditNoteDetailTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (creditNoteDetailTaxModel.CreditNoteDetTaxId > 0)
                {
                    // update record.
                    if (true == await _creditNoteDetailTax.UpdateCreditNoteDetailTax(creditNoteDetailTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _creditNoteDetailTax.CreateCreditNoteDetailTax(creditNoteDetailTaxModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete creditnote tax detail.
        /// </summary>
        /// <param name="creditNoteDetTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCreditNoteTaxDetail(int creditNoteDetTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _creditNoteDetailTax.DeleteCreditNoteDetailTax(creditNoteDetTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
