using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class CurrencyConversionModel
    {
        public int ConversionId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [Display(Name = "Company Name")]
        public int? CompanyId { get; set; }

        [Required(ErrorMessage = "Currency Name is required.")]
        [Display(Name = "Currency Name")]
        public int? CurrencyId { get; set; }

        [Required(ErrorMessage = "Effective Date Time is required.")]
        [Display(Name = "Effective Date Time")]
        public DateTime? EffectiveDateTime { get; set; }

        [Required(ErrorMessage = "Exchange Rate is required.")]
        [Display(Name = "Exchange Rate")]
        public decimal? ExchangeRate { get; set; }

        //#####
         [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

            }
}
