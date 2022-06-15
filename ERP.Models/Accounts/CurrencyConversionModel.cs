using ERP.Models.Helpers;
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

        [Required(ErrorMessage = "Company is required.")]
        [Display(Name = "Company Name")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

         [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Effective Date Time is required.")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Effective Date Time")]
        public DateTime? EffectiveDateTime { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        [RegularExpression(RegexHelper.Decimal6Digit, ErrorMessage = RegexHelper.Decimal6DigitMessage)]
        [Range(0.000001, Double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal ExchangeRate { get; set; }

        //#####
        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedByName { get; set; }

    }
}
