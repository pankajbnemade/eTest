using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceDetailTaxModel
    {
        public int SalesInvoiceDetTaxId { get; set; }

        public int SalesInvoiceDetId { get; set; }

        [Display(Name = "Sr No")]
        [Required(ErrorMessage = "Sr No is required.")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int SrNo { get; set; }

        [Display(Name = "Tax Ledger")]
        [Required(ErrorMessage = "Tax Ledger is required.")]
        public int TaxLedgerId { get; set; }

        [Display(Name = "Per or Amount")]
        [Required(ErrorMessage = "Per or Amount is required.")]
        public string TaxPercentageOrAmount { get; set; }

        [Display(Name = "Tax Per or Amount FC")]
        [Required(ErrorMessage = "Tax Per or Amount FC is required.")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxPerOrAmountFc { get; set; }

        [Display(Name = "Add or Deduct")]
        [Required(ErrorMessage = "Add or Deduct is required.")]
        public string TaxAddOrDeduct { get; set; }

        [Display(Name = "Tax Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxAmountFc { get; set; }

        [Display(Name = "Tax Amount")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Remark")]
        [StringLength(2000, ErrorMessage = "Remark cannot exceed 2000 characters.")]
        public string Remark { get; set; }

        //####
         [Display(Name = "Tax Ledger")]
        public string TaxLedgerName { get; set; }
    }
}
