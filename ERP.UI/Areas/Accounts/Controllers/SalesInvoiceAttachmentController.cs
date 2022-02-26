using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Models.Utility;
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
    public class SalesInvoiceAttachmentController : Controller
    {
        private readonly ISalesInvoiceAttachment _invoiceAttachment;
        private readonly IAttachmentCategory _invoiceAttachmentCategory;
        private readonly IAttachment _attachment;

        /// <summary>
        /// constructor
        /// </summary>
        //AttachmentController attachmentController,
        public SalesInvoiceAttachmentController(ISalesInvoiceAttachment invoiceAttachment, IAttachmentCategory attachmentCategory,
                                                 IAttachment attachment)
        {
            this._invoiceAttachment = invoiceAttachment;
            this._invoiceAttachmentCategory = attachmentCategory;
            this._attachment = attachment;
        }

        /// <summary>
        /// invoice detail.
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int salesInvoiceId)
        {
            ViewBag.SalesInvoiceId = salesInvoiceId;

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
        public async Task<JsonResult> GetAttachmentList(int salesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceAttachmentModel> resultModel = await _invoiceAttachment.GetAttachmentBySalesInvoiceId(salesInvoiceId);

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
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int salesInvoiceId)
        {
            SalesInvoiceAttachmentModel invoiceAttachmentModel = new SalesInvoiceAttachmentModel();

            invoiceAttachmentModel.SalesInvoiceId = salesInvoiceId;
            invoiceAttachmentModel.CategoryList = await _invoiceAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", invoiceAttachmentModel);
            });
        }

        /// <summary>
        /// edit invoiceAttachment.
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {

            SalesInvoiceAttachmentModel invoiceAttachmentModel = await _invoiceAttachment.GetAttachmentById(associationId);

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
        public async Task<JsonResult> SaveAttachment(SalesInvoiceAttachmentModel invoiceAttachmentModel)
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

                //int attachmentId = 0;
                attachmentModel = await _attachment.SaveAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = "Attachment not saved to file storage, please try again.";
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
