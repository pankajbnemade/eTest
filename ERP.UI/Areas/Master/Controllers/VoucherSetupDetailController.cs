using ERP.Models.Admin;
using ERP.Models.Common;
using ERP.Models.Extension;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Master.Controllers
{
    public class VoucherSetupDetailController : Controller
    {
        private readonly IVoucherStyle _voucherStyle;
        private readonly IVoucherSetupDetail _voucherSetupDetail;
        private readonly IVoucherSetup _voucherSetup;

        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="voucherStyle"></param>
        /// <param name="voucherSetupDetail"></param>
        public VoucherSetupDetailController(IVoucherStyle voucherStyle, IVoucherSetupDetail voucherSetupDetail, IVoucherSetup voucherSetup)
        {
            this._voucherSetupDetail = voucherSetupDetail;
            this._voucherSetup = voucherSetup;
            this._voucherStyle = voucherStyle;
        }

        /// <summary>
        /// voucherSetupDetail list.
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
        /// get voucherSetupDetail list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetVoucherSetupDetailList()
        {
            DataTableResultModel<VoucherSetupDetailModel> resultModel = await _voucherSetupDetail.GetVoucherSetupDetailList();

            return await Task.Run(() =>
            {
                return Json(new { draw = 1, data = resultModel.ResultList });
            });
        }

        /// <summary>
        /// add new voucherSetupDetail.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> AddVoucherSetupDetail()
        {
            VoucherSetupDetailModel voucherSetupDetailModel = new VoucherSetupDetailModel();

            ViewBag.VoucherSetupList = await _voucherSetup.GetVoucherSetupSelectList();
            ViewBag.VoucherStyleList = await _voucherStyle.GetVoucherStyleSelectList();

            UserSessionModel userSession = SessionExtension.GetComplexData<UserSessionModel>(HttpContext.Session, "UserSession");

            voucherSetupDetailModel.CompanyId = userSession.CompanyId;
            voucherSetupDetailModel.FinancialYearId = userSession.FinancialYearId;

            return PartialView("_AddVoucherSetupDetail", voucherSetupDetailModel);
        }

        /// <summary>
        /// edit voucherSetupDetail.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> EditVoucherSetupDetail(int voucherSetupDetailId)
        {
            ViewBag.VoucherSetupList = await _voucherSetup.GetVoucherSetupSelectList();
            ViewBag.VoucherStyleList = await _voucherStyle.GetVoucherStyleSelectList();

            VoucherSetupDetailModel voucherSetupDetailModel = await _voucherSetupDetail.GetVoucherSetupDetailById(voucherSetupDetailId);

            return PartialView("_AddVoucherSetupDetail", voucherSetupDetailModel);
        }

        /// <summary>
        /// save voucherSetupDetail.
        /// </summary>
        /// <param name="voucherSetupDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveVoucherSetupDetail(VoucherSetupDetailModel voucherSetupDetailModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                if (voucherSetupDetailModel.VoucherSetupDetId > 0)
                {
                    // update record.
                    if (true == await _voucherSetupDetail.UpdateVoucherSetupDetail(voucherSetupDetailModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _voucherSetupDetail.CreateVoucherSetupDetail(voucherSetupDetailModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }

            return Json(data);
        }



        /// <summary>
        /// delete voucherSetupDetail by voucherSetupDetailid.
        /// </summary>
        /// <param name="voucherSetupDetailId"></param>
        /// <returns>
        /// return json.
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> DeleteVoucherSetupDetail(int voucherSetupDetailId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());
            if (true == await _voucherSetupDetail.DeleteVoucherSetupDetail(voucherSetupDetailId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
