using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    [Area("Master")]
    public class CompanyController : Controller
    {
        private readonly ICurrency _currency;
        private readonly ICompany _company;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="company"></param>
        public CompanyController(ICurrency currency, ICompany company)
        {
            this._company = company;
            this._currency = currency;
        }

        /// <summary>
        /// company list.
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
        /// get company list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCompanyList()
        {
            DataTableResultModel<CompanyModel> resultModel = await _company.GetCompanyList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new company.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddCompany()
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            return PartialView("_AddCompany", new CompanyModel());
        }

        /// <summary>
        /// edit company.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditCompany(int companyId)
        {
            ViewBag.CurrencyList = await _currency.GetCurrencySelectList();

            CompanyModel companyModel = await _company.GetCompanyById(companyId);

            return PartialView("_AddCompany", companyModel);
        }

        /// <summary>
        /// save company.
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCompany(CompanyModel companyModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (companyModel.CompanyId > 0)
                {
                    // update record.
                    if (true == await _company.UpdateCompany(companyModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _company.CreateCompany(companyModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }

        

        /// <summary>
        /// delete company by companyid.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCompany(int companyId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _company.DeleteCompany(companyId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
