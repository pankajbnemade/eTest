using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class DebitNoteTaxMasterController : Controller
    {
        private readonly IDebitNoteTax _debitNoteTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public DebitNoteTaxMasterController(IDebitNoteTax debitNoteTax, ILedger ledger)
        {
            this._debitNoteTax = debitNoteTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// debitnote detail.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DebitNoteTaxMaster(int debitNoteId)
        {
            ViewBag.DebitNoteId = debitNoteId;

            return await Task.Run(() =>
            {
                return PartialView("_DebitNoteTaxMaster");
            });
        }

        /// <summary>
        /// get debit note tax master list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetDebitNoteTaxMasterList(int debitNoteId)
        {
            DataTableResultModel<DebitNoteTaxModel> resultModel = await _debitNoteTax.GetDebitNoteTaxByDebitNoteId(debitNoteId);

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
        /// add debitnote tax master.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddDebitNoteTaxMaster(int debitNoteId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17, true);

            DebitNoteTaxModel debitNoteTaxModel = new DebitNoteTaxModel();
            debitNoteTaxModel.DebitNoteId = debitNoteId;
            debitNoteTaxModel.SrNo = await _debitNoteTax.GenerateSrNo(debitNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteTaxMaster", debitNoteTaxModel);
            });
        }

        /// <summary>
        /// edit debitnote tax master.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditDebitNoteTaxMaster(int debitNoteTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17, true);

            DebitNoteTaxModel debitNoteTaxModel = await _debitNoteTax.GetDebitNoteTaxById(debitNoteTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteTaxMaster", debitNoteTaxModel);
            });
        }

        /// <summary>
        /// save debit note tax master.
        /// </summary>
        /// <param name="debitNoteTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveDebitNoteTaxMaster(DebitNoteTaxModel debitNoteTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (debitNoteTaxModel.DebitNoteTaxId > 0)
                {
                    // update record.
                    if (true == await _debitNoteTax.UpdateDebitNoteTax(debitNoteTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _debitNoteTax.CreateDebitNoteTax(debitNoteTaxModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete debitnote tax master.
        /// </summary>
        /// <param name="debitNoteTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteDebitNoteTaxMaster(int debitNoteTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _debitNoteTax.DeleteDebitNoteTax(debitNoteTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
