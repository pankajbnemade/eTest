using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ERP.Services.Master.Interface;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class FinancialYearCompanyRelationController : Controller
    {
        private readonly IFinancialYear _financialYear;
        private readonly ICompany _company;
        private readonly IFinancialYearCompanyRelation _financialYearCompanyRelation;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="financialYear"></param>
        /// <param name="financialYearCompanyRelation"></param>
        public FinancialYearCompanyRelationController(IFinancialYear financialYear, IFinancialYearCompanyRelation financialYearCompanyRelation,ICompany company)
        {
            this._financialYearCompanyRelation = financialYearCompanyRelation;
            this._financialYear = financialYear;
            this._company = company;
        }

        /// <summary>
        /// financialYearCompanyRelation list.
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
        /// get financialYearCompanyRelation list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFinancialYearCompanyRelationList()
        {
            DataTableResultModel<FinancialYearCompanyRelationModel> resultModel = await _financialYearCompanyRelation.GetFinancialYearCompanyRelationList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new financialYearCompanyRelation.
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
        /// edit financialYearCompanyRelation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditFinancialYearCompanyRelation(int financialYearCompanyRelationId)
        {
            ViewBag.FinancialYearList = await _financialYear.GetFinancialYearSelectList();
            ViewBag.CompanyList = await _company.GetCompanySelectList();

            FinancialYearCompanyRelationModel financialYearCompanyRelationModel = await _financialYearCompanyRelation.GetFinancialYearCompanyRelationById(financialYearCompanyRelationId);

            return PartialView("_AddFinancialYearCompanyRelation", financialYearCompanyRelationModel);
        }

        /// <summary>
        /// save financialYearCompanyRelation.
        /// </summary>
        /// <param name="financialYearCompanyRelationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveFinancialYearCompanyRelation(FinancialYearCompanyRelationModel financialYearCompanyRelationModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                ////if (financialYearCompanyRelationModel.RelationId > 0)
                ////{
                ////    // update record.
                ////    if (true == await _financialYearCompanyRelation.UpdateFinancialYearCompanyRelation(financialYearCompanyRelationModel))
                ////    {
                ////        data.Result.Status = true;
                ////    }
                ////}
                ////else
                ////{
                //// add new record.
                if (await _financialYearCompanyRelation.CreateFinancialYearCompanyRelation(financialYearCompanyRelationModel) > 0)
                {
                    data.Result.Status = true;
                }
                ////}
            }

            return Json(data);
        }



        /// <summary>
        /// delete financialYearCompanyRelation by financialYearCompanyRelationid.
        /// </summary>
        /// <param name="financialYearCompanyRelationId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteFinancialYearCompanyRelation(int financialYearCompanyRelationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _financialYearCompanyRelation.DeleteFinancialYearCompanyRelation(financialYearCompanyRelationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
