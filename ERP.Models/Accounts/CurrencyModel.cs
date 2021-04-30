using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class CurrencyModel
    {
        
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Currency Code is required.")]
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Currency Name is required.")]
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Required(ErrorMessage = "Denomination is required.")]
        [Display(Name = "Denomination")]
        public string Denomination { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

    }
}
