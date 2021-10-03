using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CreditNoteController : Controller
    {
        private readonly ICreditNote _creditNote;
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ITaxRegister _taxRegister;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        /// <summary>
        /// constractor.
        /// </summary>
        public CreditNoteController(
            ICreditNote creditNote,
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ITaxRegister taxRegister,
            ICurrency currency,
            ICurrencyConversion currencyConversion)
        {
            this._creditNote = creditNote;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PartyList = await _ledger.GetLedgerSelectList(0, true);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get search credit note result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetCreditNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterCreditNoteModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterCreditNoteModel>(searchFilter);
            // get data.
            DataTableResultModel<CreditNoteModel> resultModel = await _creditNote.GetCreditNoteList(dataTableAjaxPostModel, searchFilterModel);

            return await Task.Run(() =>
            {
                return Json(new
                {
                    dataTableAjaxPostModel.draw,
                    recordsTotal = resultModel.TotalResultCount,
                    recordsFiltered = resultModel.TotalResultCount,
                    data = resultModel.ResultList
                });
            });
        }

        /// <summary>
        /// add new credit note master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddCreditNoteMaster()
        {
            ViewBag.PartyList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            CreditNoteModel creditNoteModel = new CreditNoteModel();
            creditNoteModel.CompanyId = userSession.CompanyId;
            creditNoteModel.FinancialYearId = userSession.FinancialYearId;

            // generate no.
            GenerateNoModel generateNoModel = await _creditNote.GenerateCreditNoteNo(userSession.CompanyId, userSession.FinancialYearId);
            creditNoteModel.CreditNoteNo = generateNoModel.VoucherNo;
            creditNoteModel.CreditNoteDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteMaster", creditNoteModel);
            });
        }

        /// <summary>
        /// edit credit note master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditCreditNoteMaster(int creditNoteId)
        {
            ViewBag.PartyList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            CreditNoteModel creditNoteModel = await _creditNote.GetCreditNoteById(creditNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_AddCreditNoteMaster", creditNoteModel);
            });
        }

        /// <summary>
        /// get bill to address based on ledgerId
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> GetBillToAddressByLedgerId(int ledgerId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            IList<SelectListModel> selectList = await _ledgerAddress.GetLedgerAddressSelectList(ledgerId);
            if (null != selectList && selectList.Any())
            {
                data.Result.Status = true;
                data.Result.Data = selectList;
            }
            else
            {
                data.Result.Message = "NoItems";
            }

            return Json(data); // returns.
        }

        [HttpPost]
        public async Task<JsonResult> GetExchangeRateByCurrencyId(int currencyId, DateTime creditNoteDate)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            CurrencyConversionModel currencyConversionModel = await
                _currencyConversion.GetExchangeRateByCurrencyId(currencyId, creditNoteDate);

            if (null != currencyConversionModel)
            {
                data.Result.Status = true;
                data.Result.Data = currencyConversionModel.ExchangeRate;
            }
            else
            {
                data.Result.Data = "0";
            }

            return Json(data); // returns.
        }

        /// <summary>
        /// save sale creditNote master.
        /// </summary>
        /// <param name="creditNoteModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCreditNoteMaster(CreditNoteModel creditNoteModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (creditNoteModel.CreditNoteId > 0)
                {
                    // update record.
                    if (true == await _creditNote.UpdateCreditNote(creditNoteModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _creditNote.CreateCreditNote(creditNoteModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// manage creditNote.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ManageCreditNote(int creditNoteId)
        {
            ViewBag.CreditNoteId = creditNoteId;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view creditNote master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewCreditNoteMaster(int creditNoteId)
        {
            CreditNoteModel creditNoteModel = await _creditNote.GetCreditNoteById(creditNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewCreditNoteMaster", creditNoteModel);
            });
        }

        /// <summary>
        /// view creditNote summary.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewCreditNoteSummary(int creditNoteId)
        {
            CreditNoteModel creditNoteModel = await _creditNote.GetCreditNoteById(creditNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewCreditNoteSummary", creditNoteModel);
            });
        }

        /// <summary>
        /// delete creditNote master.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCreditNoteMaster(int creditNoteId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _creditNote.DeleteCreditNote(creditNoteId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
