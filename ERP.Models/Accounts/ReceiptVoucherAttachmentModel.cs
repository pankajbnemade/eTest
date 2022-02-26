using ERP.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class ReceiptVoucherAttachmentModel
    {
        public int AssociationId { get; set; }

        [Required(ErrorMessage = "ReceiptVoucherId is required.")]
        public int ReceiptVoucherId { get; set; }

        [Required(ErrorMessage = "AttachmentId is required.")]
        public int AttachmentId { get; set; }

        [Display(Name = "Choose File..")]
        public IFormFile FileUpload { set; get; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public string ContainerName { get; set; }
        public string ServerFileName { get; set; }
        public string UserFileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public string Url { get; set; }
        public int? StorageAccountId { get; set; }
        public string StorageType { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string PreparedByName { get; set; }
        public IList<SelectListModel> CategoryList { get; set; }

    }
}
