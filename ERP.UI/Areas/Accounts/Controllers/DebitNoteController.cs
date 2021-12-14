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
    [Area("Accounts")]
    public class DebitNoteController : Controller
    {
        private readonly IDebitNote _debitNote;
        private readonly IDebitNoteDetailTax _debitNoteDetailTax;
        private readonly IDebitNoteTax _debitNoteTax;
        private readonly ILedger _ledger;
        private readonly ILedgerAddress _ledgerAddress;
        private readonly ITaxRegister _taxRegister;
        private readonly ICurrency _currency;
        private readonly ICurrencyConversion _currencyConversion;

        /// <summary>
        /// constractor.
        /// </summary>
        public DebitNoteController(
            IDebitNote debitNote,
            ILedger ledger,
            ILedgerAddress ledgerAddress,
            ITaxRegister taxRegister,
            ICurrency currency,
            ICurrencyConversion currencyConversion,
            IDebitNoteDetailTax debitNoteDetailTax,
            IDebitNoteTax debitNoteTax)
        {
            this._debitNote = debitNote;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
            this._debitNoteDetailTax = debitNoteDetailTax;
            this._debitNoteTax = debitNoteTax;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PartyList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get search purchase debitNote result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetDebitNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterDebitNoteModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterDebitNoteModel>(searchFilter);
            // get data.
            DataTableResultModel<DebitNoteModel> resultModel = await _debitNote.GetDebitNoteList(dataTableAjaxPostModel, searchFilterModel);

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
        /// add new debitNote master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddDebitNoteMaster()
        {
            ViewBag.PartyList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");
            DebitNoteModel debitNoteModel = new DebitNoteModel();
            debitNoteModel.CompanyId = userSession.CompanyId;
            debitNoteModel.FinancialYearId = userSession.FinancialYearId;

            // generate no.
            GenerateNoModel generateNoModel = await _debitNote.GenerateDebitNoteNo(userSession.CompanyId, userSession.FinancialYearId);
            debitNoteModel.DebitNoteNo = generateNoModel.VoucherNo;
            debitNoteModel.DebitNoteDate = DateTime.Now;

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteMaster", debitNoteModel);
            });
        }

        /// <summary>
        /// edit debitNote master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditDebitNoteMaster(int debitNoteId)
        {
            ViewBag.PartyList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryCreditor, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

            DebitNoteModel debitNoteModel = await _debitNote.GetDebitNoteById(debitNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_AddDebitNoteMaster", debitNoteModel);
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
        public async Task<JsonResult> GetExchangeRateByCurrencyId(int currencyId, DateTime debitNoteDate)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            CurrencyConversionModel currencyConversionModel = await
                _currencyConversion.GetExchangeRateByCurrencyId(currencyId, debitNoteDate);

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
        /// save sale debitNote master.
        /// </summary>
        /// <param name="debitNoteModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveDebitNoteMaster(DebitNoteModel debitNoteModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (debitNoteModel.DebitNoteId > 0)
                {
                    DebitNoteModel debitNoteModel_Old = await _debitNote.GetDebitNoteById(debitNoteModel.DebitNoteId);

                    // update record.
                    if (true == await _debitNote.UpdateDebitNote(debitNoteModel))
                    {
                        if (debitNoteModel_Old.TaxModelType != debitNoteModel.TaxModelType)
                        {
                            await _debitNoteDetailTax.DeleteDebitNoteDetailTaxByDebitNoteId(debitNoteModel.DebitNoteId);
                            await _debitNoteTax.DeleteDebitNoteTaxByDebitNoteId(debitNoteModel.DebitNoteId);

                            if (debitNoteModel.TaxModelType == TaxModelType.SubTotal.ToString())
                            {
                                await _debitNoteTax.AddDebitNoteTaxByDebitNoteId(debitNoteModel.DebitNoteId, (int)debitNoteModel.TaxRegisterId);
                            }
                            else if (debitNoteModel.TaxModelType == TaxModelType.LineWise.ToString())
                            {
                                await _debitNoteDetailTax.AddDebitNoteDetailTaxByDebitNoteId(debitNoteModel.DebitNoteId, (int)debitNoteModel.TaxRegisterId);
                            }
                        }
                        else
                        {
                            await _debitNoteTax.UpdateDebitNoteTaxAmountAll(debitNoteModel.DebitNoteId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = debitNoteModel.DebitNoteId;
                    }
                }
                else
                {
                    debitNoteModel.DebitNoteId = await _debitNote.CreateDebitNote(debitNoteModel);
                    // add new record.
                    if (debitNoteModel.DebitNoteId > 0)
                    {
                        if (debitNoteModel.TaxModelType == TaxModelType.SubTotal.ToString())
                        {
                            await _debitNoteTax.AddDebitNoteTaxByDebitNoteId(debitNoteModel.DebitNoteId, (int)debitNoteModel.TaxRegisterId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = debitNoteModel.DebitNoteId;
                    }
                }
            }

            return Json(data);
        }

        /// <summary>
        /// manage debitNote.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ManageDebitNote(int debitNoteId)
        {
            ViewBag.DebitNoteId = debitNoteId;

            DebitNoteModel debitNoteModel = await _debitNote.GetDebitNoteById(debitNoteId);

            ViewBag.IsTaxMasterVisible = debitNoteModel.TaxModelType == TaxModelType.SubTotal.ToString() ? true : false;
            //ViewBag.IsApprovalRequestVisible = debitNoteModel.StatusId == 1 || debitNoteModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = debitNoteModel.StatusId == 2 ? true : false;

            ViewBag.IsApprovalRequestVisible = debitNoteModel.StatusId == (int)DocumentStatus.Inprocess || debitNoteModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false;
            ViewBag.IsApproveVisible = debitNoteModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false;
            ViewBag.IsCancelVisible = debitNoteModel.StatusId != (int)DocumentStatus.Cancelled ? true : false;

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// view debitNote master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ViewDebitNoteMaster(int debitNoteId)
        {
            DebitNoteModel debitNoteModel = await _debitNote.GetDebitNoteById(debitNoteId);

            return await Task.Run(() =>
            {
                return PartialView("_ViewDebitNoteMaster", debitNoteModel);
            });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatusDebitNoteMaster(int debitNoteId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (debitNoteId > 0)
            {
                if (true == await _debitNote.UpdateStatusDebitNote(debitNoteId, statusId))
                {
                    data.Result.Status = true;
                    data.Result.Data = debitNoteId;
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice master.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteDebitNoteMaster(int debitNoteId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _debitNote.DeleteDebitNote(debitNoteId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
