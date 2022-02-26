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
    public class PaymentVoucherAttachmentController : Controller
    {
        private readonly IPaymentVoucherAttachment _paymentVoucherAttachment;
        private readonly IAttachmentCategory _paymentVoucherAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public PaymentVoucherAttachmentController(IPaymentVoucherAttachment paymentVoucherAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._paymentVoucherAttachment = paymentVoucherAttachment;
            this._paymentVoucherAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// paymentVoucher detail.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int paymentVoucherId)
        {
            ViewBag.PaymentVoucherId = paymentVoucherId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get paymentVoucherAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int paymentVoucherId)
        {
            DataTableResultModel<PaymentVoucherAttachmentModel> resultModel = await _paymentVoucherAttachment.GetAttachmentByPaymentVoucherId(paymentVoucherId);

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
        /// add paymentVoucherAttachment.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int paymentVoucherId)
        {
            PaymentVoucherAttachmentModel paymentVoucherAttachmentModel = new PaymentVoucherAttachmentModel();

            paymentVoucherAttachmentModel.PaymentVoucherId = paymentVoucherId;
            paymentVoucherAttachmentModel.CategoryList = await _paymentVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", paymentVoucherAttachmentModel);
            });
        }

        /// <summary>
        /// edit paymentVoucherAttachment.
        /// </summary>
        /// <param name="paymentVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            PaymentVoucherAttachmentModel paymentVoucherAttachmentModel = await _paymentVoucherAttachment.GetAttachmentById(associationId);

            paymentVoucherAttachmentModel.CategoryList = await _paymentVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", paymentVoucherAttachmentModel);
            });
        }


        /// <summary>
        /// save paymentVoucherAttachment.
        /// </summary>
        /// <param name="paymentVoucherAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(PaymentVoucherAttachmentModel paymentVoucherAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = paymentVoucherAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = paymentVoucherAttachmentModel.CategoryId;
                attachmentModel.Description = paymentVoucherAttachmentModel.Description;
                attachmentModel.FileUpload = paymentVoucherAttachmentModel.FileUpload;

                if (paymentVoucherAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(paymentVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(paymentVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = paymentVoucherAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = paymentVoucherAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _paymentVoucherAttachment.SavePaymentVoucherAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    paymentVoucherAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (paymentVoucherAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _paymentVoucherAttachment.UpdateAttachment(paymentVoucherAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _paymentVoucherAttachment.CreateAttachment(paymentVoucherAttachmentModel) > 0)
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
        /// delete paymentVoucherAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _paymentVoucherAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
