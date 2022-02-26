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
    public class LedgerAttachmentController : Controller
    {
        private readonly ILedgerAttachment _ledgerAttachment;
        private readonly IAttachmentCategory _ledgerAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public LedgerAttachmentController(ILedgerAttachment ledgerAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._ledgerAttachment = ledgerAttachment;
            this._ledgerAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// ledger detail.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int ledgerId)
        {
            ViewBag.LedgerId = ledgerId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get ledgerAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int ledgerId)
        {
            DataTableResultModel<LedgerAttachmentModel> resultModel = await _ledgerAttachment.GetAttachmentByLedgerId(ledgerId);

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
        /// add ledgerAttachment.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int ledgerId)
        {
            LedgerAttachmentModel ledgerAttachmentModel = new LedgerAttachmentModel();

            ledgerAttachmentModel.LedgerId = ledgerId;
            ledgerAttachmentModel.CategoryList = await _ledgerAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", ledgerAttachmentModel);
            });
        }

        /// <summary>
        /// edit ledgerAttachment.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            LedgerAttachmentModel ledgerAttachmentModel = await _ledgerAttachment.GetAttachmentById(associationId);

            ledgerAttachmentModel.CategoryList = await _ledgerAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", ledgerAttachmentModel);
            });
        }


        /// <summary>
        /// save ledgerAttachment.
        /// </summary>
        /// <param name="ledgerAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(LedgerAttachmentModel ledgerAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = ledgerAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = ledgerAttachmentModel.CategoryId;
                attachmentModel.Description = ledgerAttachmentModel.Description;
                attachmentModel.FileUpload = ledgerAttachmentModel.FileUpload;

                if (ledgerAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(ledgerAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(ledgerAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = ledgerAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = ledgerAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _ledgerAttachment.SaveLedgerAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    ledgerAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (ledgerAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _ledgerAttachment.UpdateAttachment(ledgerAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _ledgerAttachment.CreateAttachment(ledgerAttachmentModel) > 0)
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
        /// delete ledgerAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _ledgerAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
