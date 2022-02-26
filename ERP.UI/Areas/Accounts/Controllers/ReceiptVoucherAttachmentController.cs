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
    public class ReceiptVoucherAttachmentController : Controller
    {
        private readonly IReceiptVoucherAttachment _receiptVoucherAttachment;
        private readonly IAttachmentCategory _receiptVoucherAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public ReceiptVoucherAttachmentController(IReceiptVoucherAttachment receiptVoucherAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._receiptVoucherAttachment = receiptVoucherAttachment;
            this._receiptVoucherAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// receiptVoucher detail.
        /// </summary>
        /// <param name="receiptVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int receiptVoucherId)
        {
            ViewBag.ReceiptVoucherId = receiptVoucherId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get receiptVoucherAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int receiptVoucherId)
        {
            DataTableResultModel<ReceiptVoucherAttachmentModel> resultModel = await _receiptVoucherAttachment.GetAttachmentByReceiptVoucherId(receiptVoucherId);

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
        /// add receiptVoucherAttachment.
        /// </summary>
        /// <param name="receiptVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int receiptVoucherId)
        {
            ReceiptVoucherAttachmentModel receiptVoucherAttachmentModel = new ReceiptVoucherAttachmentModel();

            receiptVoucherAttachmentModel.ReceiptVoucherId = receiptVoucherId;
            receiptVoucherAttachmentModel.CategoryList = await _receiptVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", receiptVoucherAttachmentModel);
            });
        }

        /// <summary>
        /// edit receiptVoucherAttachment.
        /// </summary>
        /// <param name="receiptVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            ReceiptVoucherAttachmentModel receiptVoucherAttachmentModel = await _receiptVoucherAttachment.GetAttachmentById(associationId);

            receiptVoucherAttachmentModel.CategoryList = await _receiptVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", receiptVoucherAttachmentModel);
            });
        }


        /// <summary>
        /// save receiptVoucherAttachment.
        /// </summary>
        /// <param name="receiptVoucherAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(ReceiptVoucherAttachmentModel receiptVoucherAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = receiptVoucherAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = receiptVoucherAttachmentModel.CategoryId;
                attachmentModel.Description = receiptVoucherAttachmentModel.Description;
                attachmentModel.FileUpload = receiptVoucherAttachmentModel.FileUpload;

                if (receiptVoucherAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(receiptVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(receiptVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = receiptVoucherAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = receiptVoucherAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _receiptVoucherAttachment.SaveReceiptVoucherAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    receiptVoucherAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (receiptVoucherAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _receiptVoucherAttachment.UpdateAttachment(receiptVoucherAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _receiptVoucherAttachment.CreateAttachment(receiptVoucherAttachmentModel) > 0)
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
        /// delete receiptVoucherAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _receiptVoucherAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
