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
    public class CreditNoteAttachmentController : Controller
    {
        private readonly ICreditNoteAttachment _creditNoteAttachment;
        private readonly IAttachmentCategory _creditNoteAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public CreditNoteAttachmentController(ICreditNoteAttachment creditNoteAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._creditNoteAttachment = creditNoteAttachment;
            this._creditNoteAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// creditNote detail.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int creditNoteId)
        {
            ViewBag.CreditNoteId = creditNoteId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get creditNoteAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int creditNoteId)
        {
            DataTableResultModel<CreditNoteAttachmentModel> resultModel = await _creditNoteAttachment.GetAttachmentByCreditNoteId(creditNoteId);

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
        /// add creditNoteAttachment.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int creditNoteId)
        {
            CreditNoteAttachmentModel creditNoteAttachmentModel = new CreditNoteAttachmentModel();

            creditNoteAttachmentModel.CreditNoteId = creditNoteId;
            creditNoteAttachmentModel.CategoryList = await _creditNoteAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", creditNoteAttachmentModel);
            });
        }

        /// <summary>
        /// edit creditNoteAttachment.
        /// </summary>
        /// <param name="creditNoteId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            CreditNoteAttachmentModel creditNoteAttachmentModel = await _creditNoteAttachment.GetAttachmentById(associationId);

            creditNoteAttachmentModel.CategoryList = await _creditNoteAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", creditNoteAttachmentModel);
            });
        }


        /// <summary>
        /// save creditNoteAttachment.
        /// </summary>
        /// <param name="creditNoteAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(CreditNoteAttachmentModel creditNoteAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = creditNoteAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = creditNoteAttachmentModel.CategoryId;
                attachmentModel.Description = creditNoteAttachmentModel.Description;
                attachmentModel.FileUpload = creditNoteAttachmentModel.FileUpload;

                if (creditNoteAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(creditNoteAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(creditNoteAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = creditNoteAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = creditNoteAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _creditNoteAttachment.SaveCreditNoteAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    creditNoteAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (creditNoteAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _creditNoteAttachment.UpdateAttachment(creditNoteAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _creditNoteAttachment.CreateAttachment(creditNoteAttachmentModel) > 0)
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
        /// delete creditNoteAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _creditNoteAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
