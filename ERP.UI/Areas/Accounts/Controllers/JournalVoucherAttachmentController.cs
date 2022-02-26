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
    public class JournalVoucherAttachmentController : Controller
    {
        private readonly IJournalVoucherAttachment _journalVoucherAttachment;
        private readonly IAttachmentCategory _journalVoucherAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public JournalVoucherAttachmentController(IJournalVoucherAttachment journalVoucherAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._journalVoucherAttachment = journalVoucherAttachment;
            this._journalVoucherAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// journalVoucher detail.
        /// </summary>
        /// <param name="journalVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int journalVoucherId)
        {
            ViewBag.JournalVoucherId = journalVoucherId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get journalVoucherAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int journalVoucherId)
        {
            DataTableResultModel<JournalVoucherAttachmentModel> resultModel = await _journalVoucherAttachment.GetAttachmentByJournalVoucherId(journalVoucherId);

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
        /// add journalVoucherAttachment.
        /// </summary>
        /// <param name="journalVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int journalVoucherId)
        {
            JournalVoucherAttachmentModel journalVoucherAttachmentModel = new JournalVoucherAttachmentModel();

            journalVoucherAttachmentModel.JournalVoucherId = journalVoucherId;
            journalVoucherAttachmentModel.CategoryList = await _journalVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", journalVoucherAttachmentModel);
            });
        }

        /// <summary>
        /// edit journalVoucherAttachment.
        /// </summary>
        /// <param name="journalVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            JournalVoucherAttachmentModel journalVoucherAttachmentModel = await _journalVoucherAttachment.GetAttachmentById(associationId);

            journalVoucherAttachmentModel.CategoryList = await _journalVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", journalVoucherAttachmentModel);
            });
        }


        /// <summary>
        /// save journalVoucherAttachment.
        /// </summary>
        /// <param name="journalVoucherAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(JournalVoucherAttachmentModel journalVoucherAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = journalVoucherAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = journalVoucherAttachmentModel.CategoryId;
                attachmentModel.Description = journalVoucherAttachmentModel.Description;
                attachmentModel.FileUpload = journalVoucherAttachmentModel.FileUpload;

                if (journalVoucherAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(journalVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(journalVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = journalVoucherAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = journalVoucherAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _journalVoucherAttachment.SaveJournalVoucherAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    journalVoucherAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (journalVoucherAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _journalVoucherAttachment.UpdateAttachment(journalVoucherAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _journalVoucherAttachment.CreateAttachment(journalVoucherAttachmentModel) > 0)
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
        /// delete journalVoucherAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _journalVoucherAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
