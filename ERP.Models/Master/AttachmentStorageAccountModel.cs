using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class AttachmentStorageAccountModel
    {
        public int StorageAccountId { get; set; }

        [Display(Name = "Storage Type")]
        public string StorageType { get; set; }

        [Required(ErrorMessage = "Account Name is required.")]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Account Key")]
        public string AccountKey { get; set; }

        [Display(Name = "Container")]
        public string ContainerName { get; set; }

        [Display(Name = "Allowed File Extension")]
        public string AllowedFileExtension { get; set; }

        [Display(Name = "Allowed Content Length")]
        public long AllowedContentLength { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
