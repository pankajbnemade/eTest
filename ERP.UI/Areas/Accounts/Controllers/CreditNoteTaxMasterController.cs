using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CreditNoteTaxMasterController : Controller
    {
        private readonly ICreditNoteTax _creditNoteTax;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public CreditNoteTaxMasterController(ICreditNoteTax creditNoteTax, ILedger ledger)
        {
            this._creditNoteTax = creditNoteTax;
            this._ledger = ledger;
        }

        /// <summary>
        /// creditNote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreditNoteTaxMaster(int creditNoteId)
        {
            ViewBag.CreditNoteId = creditNoteId;

            return await Task.Run(() =>
            {
                return PartialView("_CreditNoteTaxMaster");
            });
        }

        /// <summary>
        /// get  creditNote tax master list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCreditNoteTaxMasterList(int creditNoteId)
        {
            DataTableResultModel<CreditNoteTaxModel> resultModel = await _creditNoteTax.GetCreditNoteTaxByCreditNoteId(creditNoteId);

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
        /// add creditNote tax master.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddCreditNoteTaxMaster(int creditNoteId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17, true);

            CreditNoteTaxModel creditNoteTaxModel = new CreditNoteTaxModel();
            creditNoteTaxModel.CreditNoteId = creditNoteId;
            creditNoteTaxModel.SrNo = await _creditNoteTax.GenerateSrNo(creditNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteTaxMaster", creditNoteTaxModel);
            });
        }

        /// <summary>
        /// edit creditNote tax master.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditCreditNoteTaxMaster(int creditNoteTaxId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList(17, true);

            CreditNoteTaxModel creditNoteTaxModel = await _creditNoteTax.GetCreditNoteTaxById(creditNoteTaxId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteTaxMaster", creditNoteTaxModel);
            });
        }

        /// <summary>
        /// save  creditNote tax master.
        /// </summary>
        /// <param name="creditNoteTaxModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCreditNoteTaxMaster(CreditNoteTaxModel creditNoteTaxModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (creditNoteTaxModel.CreditNoteTaxId > 0)
                {
                    // update record.
                    if (true == await _creditNoteTax.UpdateCreditNoteTax(creditNoteTaxModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _creditNoteTax.CreateCreditNoteTax(creditNoteTaxModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete creditNote tax master.
        /// </summary>
        /// <param name="creditNoteTaxId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCreditNoteTaxMaster(int creditNoteTaxId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _creditNoteTax.DeleteCreditNoteTax(creditNoteTaxId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
