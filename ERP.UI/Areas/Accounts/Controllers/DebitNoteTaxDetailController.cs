using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class DebitNoteTaxDetailController : Controller
    {
        private readonly IDebitNoteDetailTax _debitNoteDetailTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public DebitNoteTaxDetailController(IDebitNoteDetailTax debitNoteDetailTax, ILedger ledger)
        {
            this._debitNoteDetailTax = debitNoteDetailTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// debitnote tax detail.
        /// </summary>
        /// <param name="debitNoteDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DebitNoteTaxDetail(int debitNoteDetId)
        {
            ViewBag.DebitNoteDetId = debitNoteDetId;

            return await Task.Run(() =>
            {
                return PartialView("_DebitNoteTaxDetail");
            });
        }

        ///// <summary>
        ///// view debitnote tax detail.
        ///// </summary>
        ///// <param name="debitNoteDetId"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> ViewDebitNoteTaxDetail(int debitNoteDetId)
        //{
        //    ViewBag.DebitNoteDetId = debitNoteDetId;

        //    return await Task.Run(() =>
        //    {
        //        return PartialView("_ViewDebitNoteTaxDetail");
        //    });
        //}

        /// <summary>
        /// get debit note tax detail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetDebitNoteTaxDetailList(int debitNoteDetId)
        {
            DataTableResultModel<DebitNoteDetailTaxModel> resultModel = await _debitNoteDetailTax.GetDebitNoteDetailTaxByDebitNoteDetailId(debitNoteDetId);

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
        /// add debitnote tax detail.
        /// </summary>
        /// <param name="debitNoteDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddDebitNoteTaxDetail(int debitNoteDetId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, userSession.CompanyId, true);

            DebitNoteDetailTaxModel debitNoteDetailTaxModel = new DebitNoteDetailTaxModel();
            debitNoteDetailTaxModel.DebitNoteDetId = debitNoteDetId;
            debitNoteDetailTaxModel.SrNo = await _debitNoteDetailTax.GenerateSrNo(debitNoteDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteTaxDetail", debitNoteDetailTaxModel);
            });
        }

        /// <summary>
        /// edit debitnote tax detail.
        /// </summary>
        /// <param name="debitNoteDetTaxId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditDebitNoteTaxDetail(int debitNoteDetTaxId)
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, userSession.CompanyId, true);

            DebitNoteDetailTaxModel debitNoteDetailTaxModel = await _debitNoteDetailTax.GetDebitNoteDetailTaxById(debitNoteDetTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteTaxDetail", debitNoteDetailTaxModel);
            });
        }

        /// <summary>
        /// save debit note tax detail.
        /// </summary>
        /// <param name="debitNoteDetailTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveDebitNoteTaxDetail(DebitNoteDetailTaxModel debitNoteDetailTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (debitNoteDetailTaxModel.DebitNoteDetTaxId > 0)
                {
                    // update record.
                    if (true == await _debitNoteDetailTax.UpdateDebitNoteDetailTax(debitNoteDetailTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _debitNoteDetailTax.CreateDebitNoteDetailTax(debitNoteDetailTaxModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete debitnote tax detail.
        /// </summary>
        /// <param name="debitNoteDetTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteDebitNoteTaxDetail(int debitNoteDetTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _debitNoteDetailTax.DeleteDebitNoteDetailTax(debitNoteDetTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
