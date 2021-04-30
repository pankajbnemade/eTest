using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class TaxRegisterModel
    {
        public int TaxRegisterId { get; set; }

        [Required(ErrorMessage = "Tax Register Name is required.")]
        [Display(Name = "Tax Register Name")]
        public string TaxRegisterName { get; set; }

        //####

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

    }
}
