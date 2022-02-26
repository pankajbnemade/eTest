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
    public class DebitNoteAttachmentController : Controller
    {
        private readonly IDebitNoteAttachment _debitNoteAttachment;
        private readonly IAttachmentCategory _debitNoteAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public DebitNoteAttachmentController(IDebitNoteAttachment debitNoteAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._debitNoteAttachment = debitNoteAttachment;
            this._debitNoteAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// debitNote detail.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int debitNoteId)
        {
            ViewBag.DebitNoteId = debitNoteId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get debitNoteAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int debitNoteId)
        {
            DataTableResultModel<DebitNoteAttachmentModel> resultModel = await _debitNoteAttachment.GetAttachmentByDebitNoteId(debitNoteId);

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
        /// add debitNoteAttachment.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int debitNoteId)
        {
            DebitNoteAttachmentModel debitNoteAttachmentModel = new DebitNoteAttachmentModel();

            debitNoteAttachmentModel.DebitNoteId = debitNoteId;
            debitNoteAttachmentModel.CategoryList = await _debitNoteAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", debitNoteAttachmentModel);
            });
        }

        /// <summary>
        /// edit debitNoteAttachment.
        /// </summary>
        /// <param name="debitNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            DebitNoteAttachmentModel debitNoteAttachmentModel = await _debitNoteAttachment.GetAttachmentById(associationId);

            debitNoteAttachmentModel.CategoryList = await _debitNoteAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", debitNoteAttachmentModel);
            });
        }


        /// <summary>
        /// save debitNoteAttachment.
        /// </summary>
        /// <param name="debitNoteAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(DebitNoteAttachmentModel debitNoteAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = debitNoteAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = debitNoteAttachmentModel.CategoryId;
                attachmentModel.Description = debitNoteAttachmentModel.Description;
                attachmentModel.FileUpload = debitNoteAttachmentModel.FileUpload;

                if (debitNoteAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(debitNoteAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(debitNoteAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = debitNoteAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = debitNoteAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _debitNoteAttachment.SaveDebitNoteAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    debitNoteAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (debitNoteAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _debitNoteAttachment.UpdateAttachment(debitNoteAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _debitNoteAttachment.CreateAttachment(debitNoteAttachmentModel) > 0)
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
        /// delete debitNoteAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _debitNoteAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
