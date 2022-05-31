using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class TaxRegisterDetailController : Controller
    {
        private readonly ITaxRegisterDetail _taxRegisterDetail;
        private readonly ILedger _ledger;

        /// <summary>
        /// constractor.
        /// </summary>
        public TaxRegisterDetailController(ITaxRegisterDetail taxRegisterDetail, ILedger ledger)
        {
            this._taxRegisterDetail = taxRegisterDetail;
            this._ledger = ledger;
        }

        /// <summary>
        /// invoice tax detail.
        /// </summary>
        /// <param name="taxRegisterId"></param>
        /// <returns></returns>
        public async Task<IActionResult> TaxRegisterDetail(int taxRegisterId)
        {
            ViewBag.TaxRegisterId = taxRegisterId;

            return await Task.Run(() =>
            {
                return PartialView("_TaxRegisterDetail");
            });
        }


        /// <summary>
        /// get sales invoice tax detail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetTaxRegisterDetailList(int taxRegisterId)
        {
            DataTableResultModel<TaxRegisterDetailModel> resultModel = await _taxRegisterDetail.GetTaxRegisterDetailByTaxRegisterId(taxRegisterId);

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
        /// add invoice tax detail.
        /// </summary>
        /// <param name="taxRegisterId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddTaxRegisterDetail(int taxRegisterId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            TaxRegisterDetailModel taxRegisterDetailModel = new TaxRegisterDetailModel();

            taxRegisterDetailModel.TaxRegisterId = taxRegisterId;
            taxRegisterDetailModel.SrNo = await _taxRegisterDetail.GenerateSrNo(taxRegisterId);

            return await Task.Run(() =>
            {
                return PartialView("_AddTaxRegisterDetail", taxRegisterDetailModel);
            });
        }

        /// <summary>
        /// edit invoice tax detail.
        /// </summary>
        /// <param name="taxRegisterDetId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditTaxRegisterDetail(int taxRegisterDetId)
        {
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();
            ViewBag.TaxAddOrDeductList = EnumHelper.GetEnumListFor<TaxAddOrDeduct>();
            ViewBag.TaxLedgerList = await _ledger.GetLedgerSelectList((int)LedgerName.DutiesAndTaxes, true);

            TaxRegisterDetailModel taxRegisterDetailModel = await _taxRegisterDetail.GetTaxRegisterDetailById(taxRegisterDetId);

            return await Task.Run(() =>
            {
                return PartialView("_AddTaxRegisterDetail", taxRegisterDetailModel);
            });
        }

        /// <summary>
        /// save sales invoice tax detail.
        /// </summary>
        /// <param name="taxRegisterDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveTaxRegisterDetail(TaxRegisterDetailModel taxRegisterDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (taxRegisterDetailModel.TaxRegisterDetId > 0)
                {
                    // update record.
                    if (true == await _taxRegisterDetail.UpdateTaxRegisterDetail(taxRegisterDetailModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _taxRegisterDetail.CreateTaxRegisterDetail(taxRegisterDetailModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice tax detail.
        /// </summary>
        /// <param name="taxRegisterDetId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteTaxRegisterDetail(int taxRegisterDetId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _taxRegisterDetail.DeleteTaxRegisterDetail(taxRegisterDetId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
