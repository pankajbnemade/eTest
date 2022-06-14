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
    public class CreditNoteController : Controller
    {
        private readonly ICreditNote _creditNote;
        private readonly ICreditNoteDetailTax _creditNoteDetailTax;
        private readonly ICreditNoteTax _creditNoteTax;
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
            ICurrencyConversion currencyConversion,
            ICreditNoteDetailTax creditNoteDetailTax,
            ICreditNoteTax creditNoteTax)
        {
            this._creditNote = creditNote;
            this._ledger = ledger;
            this._ledgerAddress = ledgerAddress;
            this._taxRegister = taxRegister;
            this._currency = currency;
            this._currencyConversion = currencyConversion;
            this._creditNoteDetailTax = creditNoteDetailTax;
            this._creditNoteTax = creditNoteTax;
        }

        public async Task<IActionResult> Index()
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.PartyList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, userSession.CompanyId, true);

            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, userSession.CompanyId, true);

            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get search purchase creditNote result list.
        /// </summary>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetCreditNoteList(DataTableAjaxPostModel dataTableAjaxPostModel, string searchFilter)
        {
            // deserilize string search filter.
            SearchFilterCreditNoteModel searchFilterModel = JsonConvert.DeserializeObject<SearchFilterCreditNoteModel>(searchFilter);

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            searchFilterModel.CompanyId=userSession.CompanyId;
            searchFilterModel.FinancialYearId=userSession.FinancialYearId;

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
        /// add new creditNote master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddCreditNoteMaster()
        {
            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            ViewBag.PartyList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, userSession.CompanyId, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, userSession.CompanyId, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();

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
        /// edit creditNote master.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditCreditNoteMaster(int creditNoteId)
        {
            CreditNoteModel creditNoteModel = await _creditNote.GetCreditNoteById(creditNoteId);

            ViewBag.PartyList = await _ledger.GetLedgerSelectList((int)LedgerName.SundryDebtor, creditNoteModel.CompanyId, true);
            ViewBag.AccountLedgerList = await _ledger.GetLedgerSelectList(0, creditNoteModel.CompanyId, true);
            ViewBag.TaxRegisterList = await _taxRegister.GetTaxRegisterSelectList();
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();
            ViewBag.TaxModelTypeList = EnumHelper.GetEnumListFor<TaxModelType>();
            ViewBag.DiscountTypeList = EnumHelper.GetEnumListFor<DiscountType>();


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
                    CreditNoteModel creditNoteModel_Old = await _creditNote.GetCreditNoteById(creditNoteModel.CreditNoteId);

                    // update record.
                    if (true == await _creditNote.UpdateCreditNote(creditNoteModel))
                    {
                        if (creditNoteModel_Old.TaxModelType != creditNoteModel.TaxModelType)
                        {
                            await _creditNoteDetailTax.DeleteCreditNoteDetailTaxByCreditNoteId(creditNoteModel.CreditNoteId);
                            await _creditNoteTax.DeleteCreditNoteTaxByCreditNoteId(creditNoteModel.CreditNoteId);

                            if (creditNoteModel.TaxModelType == TaxModelType.SubTotal.ToString())
                            {
                                await _creditNoteTax.AddCreditNoteTaxByCreditNoteId(creditNoteModel.CreditNoteId, (int)creditNoteModel.TaxRegisterId);
                            }
                            else if (creditNoteModel.TaxModelType == TaxModelType.LineWise.ToString())
                            {
                                await _creditNoteDetailTax.AddCreditNoteDetailTaxByCreditNoteId(creditNoteModel.CreditNoteId, (int)creditNoteModel.TaxRegisterId);
                            }
                        }
                        //else
                        //{
                        await _creditNoteTax.UpdateCreditNoteTaxAmountAll(creditNoteModel.CreditNoteId);
                        //}
                        data.Result.Status = true;
                        data.Result.Data = creditNoteModel.CreditNoteId;
                    }
                }
                else
                {
                    creditNoteModel.CreditNoteId = await _creditNote.CreateCreditNote(creditNoteModel);
                    // add new record.
                    if (creditNoteModel.CreditNoteId > 0)
                    {
                        if (creditNoteModel.TaxModelType == TaxModelType.SubTotal.ToString())
                        {
                            await _creditNoteTax.AddCreditNoteTaxByCreditNoteId(creditNoteModel.CreditNoteId, (int)creditNoteModel.TaxRegisterId);
                        }
                        data.Result.Status = true;
                        data.Result.Data = creditNoteModel.CreditNoteId;
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

            CreditNoteModel creditNoteModel = await _creditNote.GetCreditNoteById(creditNoteId);

            ViewBag.IsTaxMasterVisible = creditNoteModel.TaxModelType == TaxModelType.SubTotal.ToString() ? true : false;
            //ViewBag.IsApprovalRequestVisible = creditNoteModel.StatusId == 1 || creditNoteModel.StatusId == 3 ? true : false;
            //ViewBag.IsApproveVisible = creditNoteModel.StatusId == 2 ? true : false;

            ViewBag.IsApprovalRequestVisible = creditNoteModel.StatusId == (int)DocumentStatus.Inprocess || creditNoteModel.StatusId == (int)DocumentStatus.ApprovalRejected ? true : false;
            ViewBag.IsApproveVisible = creditNoteModel.StatusId == (int)DocumentStatus.ApprovalRequested ? true : false;
            ViewBag.IsCancelVisible = creditNoteModel.StatusId != (int)DocumentStatus.Cancelled ? true : false;

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

        [HttpPost]
        public async Task<JsonResult> UpdateStatusCreditNoteMaster(int creditNoteId, string action)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            int statusId = (int)EnumHelper.GetValueFromDescription<DocumentStatus>(action);

            if (creditNoteId > 0)
            {
                if (true == await _creditNote.UpdateStatusCreditNote(creditNoteId, statusId))
                {
                    data.Result.Status = true;
                    data.Result.Data = creditNoteId;
                }
            }

            return Json(data);
        }

        /// <summary>
        /// delete invoice master.
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
