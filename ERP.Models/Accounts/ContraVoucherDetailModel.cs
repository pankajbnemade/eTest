using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class ContraVoucherDetailModel
    {
        public int ContraVoucherDetId { get; set; }

        public int ContraVoucherId { get; set; }

        [Display(Name = "Particular")]
        [Required(ErrorMessage = "Particular is required.")]
        public int ParticularLedgerId { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Display(Name = "Debit Amount FC")]
        [Required(ErrorMessage = "Debit Amount FC is required.")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        public decimal DebitAmountFc { get; set; }

        [Display(Name = "Debit Amount")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Credit Amount FC")]
        [Required(ErrorMessage = "Credit Amount FC is required.")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        public decimal CreditAmountFc { get; set; }

        [Display(Name = "Credit Amount")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        public decimal CreditAmount { get; set; }

        //###

        public string ParticularLedgerName { get; set; }

    }
}