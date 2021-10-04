using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class PurchaseInvoiceTaxModel
    {
        public int PurchaseInvoiceTaxId { get; set; }

        public int? PurchaseInvoiceId { get; set; }

        [Display(Name = "Sr No.")]
        [Required(ErrorMessage = "Sr No. is required.")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int? SrNo { get; set; }

        [Display(Name = "Tax Ledger")]
        [Required(ErrorMessage = "Tax Ledger is required.")]
        public int? TaxLedgerId { get; set; }

        [Display(Name = "Tax Percentage or Amount")]
        [Required(ErrorMessage = "Tax Percentage or Amount is required.")]
        public string TaxPercentageOrAmount { get; set; }

        [Display(Name = "Tax Percentage or Amount FC")]
        [Required(ErrorMessage = "Tax Percentage or Amount FC is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? TaxPerOrAmountFc { get; set; }

        [Display(Name = "Tax Add or Deduct")]
        [Required(ErrorMessage = "Tax Add or Deduct is required.")]
        public string TaxAddOrDeduct { get; set; }

        [Display(Name = "Tax Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? TaxAmountFc { get; set; }

        [Display(Name = "Tax Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Remark")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Remark { get; set; }

        //####
        public string TaxLedgerName { get; set; }
    }
}
