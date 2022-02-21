using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class AttachmentCategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Charge Type Name is required.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
