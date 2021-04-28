using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class ChargeTypeModel
    {
        public int ChargeTypeId { get; set; }

        [Required(ErrorMessage = "Charge Type Name is required.")]
        [Display(Name = "Charge Type Name")]
        public string ChargeTypeName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
