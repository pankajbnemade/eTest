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
    public class PurchaseInvoiceAttachmentController : Controller
    {
        private readonly IPurchaseInvoiceAttachment _invoiceAttachment;
        private readonly IAttachmentCategory _invoiceAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public PurchaseInvoiceAttachmentController(IPurchaseInvoiceAttachment invoiceAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._invoiceAttachment = invoiceAttachment;
            this._invoiceAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int purchaseInvoiceId)
        {
            ViewBag.PurchaseInvoiceId = purchaseInvoiceId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get invoiceAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceAttachmentModel> resultModel = await _invoiceAttachment.GetAttachmentByPurchaseInvoiceId(purchaseInvoiceId);

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
        /// add invoiceAttachment.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int purchaseInvoiceId)
        {
            PurchaseInvoiceAttachmentModel invoiceAttachmentModel = new PurchaseInvoiceAttachmentModel();

            invoiceAttachmentModel.PurchaseInvoiceId = purchaseInvoiceId;
            invoiceAttachmentModel.CategoryList = await _invoiceAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", invoiceAttachmentModel);
            });
        }

        /// <summary>
        /// edit invoiceAttachment.
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            PurchaseInvoiceAttachmentModel invoiceAttachmentModel = await _invoiceAttachment.GetAttachmentById(associationId);

            invoiceAttachmentModel.CategoryList = await _invoiceAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", invoiceAttachmentModel);
            });
        }


        /// <summary>
        /// save invoiceAttachment.
        /// </summary>
        /// <param name="invoiceAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(PurchaseInvoiceAttachmentModel invoiceAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = invoiceAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = invoiceAttachmentModel.CategoryId;
                attachmentModel.Description = invoiceAttachmentModel.Description;
                attachmentModel.FileUpload = invoiceAttachmentModel.FileUpload;

                if (invoiceAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(invoiceAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(invoiceAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = invoiceAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = invoiceAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _invoiceAttachment.SaveInvoiceAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    invoiceAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (invoiceAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _invoiceAttachment.UpdateAttachment(invoiceAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _invoiceAttachment.CreateAttachment(invoiceAttachmentModel) > 0)
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
        /// delete invoiceAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _invoiceAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
