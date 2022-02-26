using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
//using System.IO;
using System.Threading.Tasks;

namespace ERP.UI.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AdvanceAdjustmentAttachmentController : Controller
    {
        private readonly IAdvanceAdjustmentAttachment _advanceAdjustmentAttachment;
        private readonly IAttachmentCategory _advanceAdjustmentAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public AdvanceAdjustmentAttachmentController(IAdvanceAdjustmentAttachment advanceAdjustmentAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._advanceAdjustmentAttachment = advanceAdjustmentAttachment;
            this._advanceAdjustmentAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// advanceAdjustment detail.
        /// </summary>
        /// <param name="advanceAdjustmentId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int advanceAdjustmentId)
        {
            ViewBag.AdvanceAdjustmentId = advanceAdjustmentId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get advanceAdjustmentAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int advanceAdjustmentId)
        {
            DataTableResultModel<AdvanceAdjustmentAttachmentModel> resultModel = await _advanceAdjustmentAttachment.GetAttachmentByAdvanceAdjustmentId(advanceAdjustmentId);

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
        /// add advanceAdjustmentAttachment.
        /// </summary>
        /// <param name="advanceAdjustmentId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int advanceAdjustmentId)
        {
            AdvanceAdjustmentAttachmentModel advanceAdjustmentAttachmentModel = new AdvanceAdjustmentAttachmentModel();

            advanceAdjustmentAttachmentModel.AdvanceAdjustmentId = advanceAdjustmentId;
            advanceAdjustmentAttachmentModel.CategoryList = await _advanceAdjustmentAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", advanceAdjustmentAttachmentModel);
            });
        }

        /// <summary>
        /// edit advanceAdjustmentAttachment.
        /// </summary>
        /// <param name="advanceAdjustmentId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            AdvanceAdjustmentAttachmentModel advanceAdjustmentAttachmentModel = await _advanceAdjustmentAttachment.GetAttachmentById(associationId);

            advanceAdjustmentAttachmentModel.CategoryList = await _advanceAdjustmentAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", advanceAdjustmentAttachmentModel);
            });
        }


        /// <summary>
        /// save advanceAdjustmentAttachment.
        /// </summary>
        /// <param name="advanceAdjustmentAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(AdvanceAdjustmentAttachmentModel advanceAdjustmentAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = advanceAdjustmentAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = advanceAdjustmentAttachmentModel.CategoryId;
                attachmentModel.Description = advanceAdjustmentAttachmentModel.Description;
                attachmentModel.FileUpload = advanceAdjustmentAttachmentModel.FileUpload;

                if (advanceAdjustmentAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(advanceAdjustmentAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(advanceAdjustmentAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = advanceAdjustmentAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = advanceAdjustmentAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _advanceAdjustmentAttachment.SaveAdvanceAdjustmentAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    advanceAdjustmentAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (advanceAdjustmentAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _advanceAdjustmentAttachment.UpdateAttachment(advanceAdjustmentAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _advanceAdjustmentAttachment.CreateAttachment(advanceAdjustmentAttachmentModel) > 0)
                    {
                        data.Result.Status = true;
                    }
                }
            }
            else
            {
                data.Result.Status = false;
                data.Result.Message = "Model state is not valid, please enter all required value.";
            }

            return Json(data);
        }

        /// <summary>
        /// delete advanceAdjustmentAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _advanceAdjustmentAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
