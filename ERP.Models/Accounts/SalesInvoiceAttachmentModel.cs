using ERP.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceAttachmentModel
    {
        public int AssociationId { get; set; }
        public int SalesInvoiceId { get; set; }
        public int AttachmentId { get; set; }

        [Display(Name = "Choose File..")]
        public IFormFile FileUpload { set; get; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
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
