using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class PurchaseInvoiceDetailTaxModel
    {
        public int PurchaseInvoiceDetTaxId { get; set; }

        public int? PurchaseInvoiceDetId { get; set; }

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
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        public decimal? TaxPerOrAmountFc { get; set; }

        [Display(Name = "Tax Add or Deduct")]
        [Required(ErrorMessage = "Tax Add or Deduct is required.")]
        public string TaxAddOrDeduct { get; set; }
        public decimal? TaxAmountFc { get; set; }
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Remark")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Remark { get; set; }
        //####
        public string TaxLedgerName { get; set; }
    }
}
