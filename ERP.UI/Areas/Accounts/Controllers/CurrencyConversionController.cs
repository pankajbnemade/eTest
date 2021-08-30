using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ERP.Services.Master.Interface;

namespace ERP.UI.Areas.Accounts.Controllers
{
    public class CurrencyConversionController : Controller
    {
        private readonly IFinancialYear _financialYear;
        private readonly ICompany _company;
        private readonly ICurrencyConversion _currencyConversion;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="financialYear"></param>
        /// <param name="currencyConversion"></param>
        public CurrencyConversionController(IFinancialYear financialYear, ICurrencyConversion currencyConversion,ICompany company)
        {
            this._currencyConversion = currencyConversion;
            this._financialYear = financialYear;
            this._company = company;
        }

        /// <summary>
        /// currencyConversion list.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        /// <summary>
        /// get currencyConversion list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFinancialYearCompanyRelationList()
        {
            DataTableResultModel<FinancialYearCompanyRelationModel> resultModel = await _currencyConversion.GetFinancialYearCompanyRelationList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new currencyConversion.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddFinancialYearCompanyRelation()
        {
            ViewBag.FinancialYearList = await _financialYear.GetFinancialYearSelectList();
            ViewBag.CompanyList = await _company.GetCompanySelectList();

            return PartialView("_AddFinancialYearCompanyRelation", new FinancialYearCompanyRelationModel());
        }

        /// <summary>
        /// edit currencyConversion.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditFinancialYearCompanyRelation(int currencyConversionId)
        {
            ViewBag.FinancialYearList = await _financialYear.GetFinancialYearSelectList();
            ViewBag.CompanyList = await _company.GetCompanySelectList();

            FinancialYearCompanyRelationModel currencyConversionModel = await _currencyConversion.GetFinancialYearCompanyRelationById(currencyConversionId);

            return PartialView("_AddFinancialYearCompanyRelation", currencyConversionModel);
        }

        /// <summary>
        /// save currencyConversion.
        /// </summary>
        /// <param name="currencyConversionModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveFinancialYearCompanyRelation(FinancialYearCompanyRelationModel currencyConversionModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                ////if (currencyConversionModel.RelationId > 0)
                ////{
                ////    // update record.
                ////    if (true == await _currencyConversion.UpdateFinancialYearCompanyRelation(currencyConversionModel))
                ////    {
                ////        data.Result.Status = true;
                ////    }
                ////}
                ////else
                ////{
                //// add new record.
                if (await _currencyConversion.CreateFinancialYearCompanyRelation(currencyConversionModel) > 0)
                {
                    data.Result.Status = true;
                }
                ////}
            }

            return Json(data);
        }



        /// <summary>
        /// delete currencyConversion by currencyConversionid.
        /// </summary>
        /// <param name="currencyConversionId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteFinancialYearCompanyRelation(int currencyConversionId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _currencyConversion.DeleteFinancialYearCompanyRelation(currencyConversionId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
