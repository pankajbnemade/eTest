using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class AttachmentStorageAccountModel
    {
        public int StorageAccountId { get; set; }

        [Required(ErrorMessage = "Charge Type Name is required.")]
        [Display(Name = "Category Name")]
        public string AccountName { get; set; }

        [Display(Name = "Account Key")]
        public string AccountKey { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
