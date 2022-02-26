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
    public class ContraVoucherAttachmentController : Controller
    {
        private readonly IContraVoucherAttachment _contraVoucherAttachment;
        private readonly IAttachmentCategory _contraVoucherAttachmentCategory;

        /// <summary>
        /// constructor
        /// </summary>
        public ContraVoucherAttachmentController(IContraVoucherAttachment contraVoucherAttachment, IAttachmentCategory attachmentCategory
            )
        {
            this._contraVoucherAttachment = contraVoucherAttachment;
            this._contraVoucherAttachmentCategory = attachmentCategory;
        }

        /// <summary>
        /// contraVoucher detail.
        /// </summary>
        /// <param name="contraVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attachment(int contraVoucherId)
        {
            ViewBag.ContraVoucherId = contraVoucherId;

            return await Task.Run(() =>
            {
                return PartialView("_Attachment");
            });
        }

        /// <summary>
        /// get contraVoucherAttachment list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAttachmentList(int contraVoucherId)
        {
            DataTableResultModel<ContraVoucherAttachmentModel> resultModel = await _contraVoucherAttachment.GetAttachmentByContraVoucherId(contraVoucherId);

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
        /// add contraVoucherAttachment.
        /// </summary>
        /// <param name="contraVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAttachment(int contraVoucherId)
        {
            ContraVoucherAttachmentModel contraVoucherAttachmentModel = new ContraVoucherAttachmentModel();

            contraVoucherAttachmentModel.ContraVoucherId = contraVoucherId;
            contraVoucherAttachmentModel.CategoryList = await _contraVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", contraVoucherAttachmentModel);
            });
        }

        /// <summary>
        /// edit contraVoucherAttachment.
        /// </summary>
        /// <param name="contraVoucherId"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditAttachment(int associationId)
        {
            ContraVoucherAttachmentModel contraVoucherAttachmentModel = await _contraVoucherAttachment.GetAttachmentById(associationId);

            contraVoucherAttachmentModel.CategoryList = await _contraVoucherAttachmentCategory.GetCategorySelectList();

            return await Task.Run(() =>
            {
                return PartialView("_AddAttachment", contraVoucherAttachmentModel);
            });
        }


        /// <summary>
        /// save contraVoucherAttachment.
        /// </summary>
        /// <param name="contraVoucherAttachmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveAttachment(ContraVoucherAttachmentModel contraVoucherAttachmentModel)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (ModelState.IsValid)
            {
                AttachmentModel attachmentModel = new AttachmentModel();

                attachmentModel.AttachmentId = contraVoucherAttachmentModel.AttachmentId;
                attachmentModel.CategoryId = contraVoucherAttachmentModel.CategoryId;
                attachmentModel.Description = contraVoucherAttachmentModel.Description;
                attachmentModel.FileUpload = contraVoucherAttachmentModel.FileUpload;

                if (contraVoucherAttachmentModel.FileUpload != null)
                {
                    attachmentModel.ServerFileName = Guid.NewGuid().ToString().Substring(0, 15).ToLower();
                    attachmentModel.UserFileName = Path.GetFileNameWithoutExtension(contraVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.FileExtension = Path.GetExtension(contraVoucherAttachmentModel.FileUpload.FileName);
                    attachmentModel.ContentType = contraVoucherAttachmentModel.FileUpload.ContentType;
                    attachmentModel.ContentLength = contraVoucherAttachmentModel.FileUpload.Length;
                }

                attachmentModel = await _contraVoucherAttachment.SaveContraVoucherAttachment(attachmentModel);

                if (attachmentModel.AttachmentId == 0)
                {
                    data.Result.Status = false;
                    data.Result.Message = attachmentModel.ErrorMessage;
                    return Json(data);
                }
                else
                {
                    contraVoucherAttachmentModel.AttachmentId = attachmentModel.AttachmentId;
                }

                if (contraVoucherAttachmentModel.AssociationId > 0)
                {
                    // update record.
                    if (true == await _contraVoucherAttachment.UpdateAttachment(contraVoucherAttachmentModel))
                    {
                        data.Result.Status = true;
                    }
                }
                else
                {
                    // add new record.
                    if (await _contraVoucherAttachment.CreateAttachment(contraVoucherAttachmentModel) > 0)
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
        /// delete contraVoucherAttachment.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteAttachment(int associationId)
        {
            JsonData<JsonStatus> data = new JsonData<JsonStatus>(new JsonStatus());

            if (true == await _contraVoucherAttachment.DeleteAttachment(associationId))
            {
                data.Result.Status = true;
            }

            return Json(data); // returns.
        }
    }
}
