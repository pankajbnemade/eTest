using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class LedgerFinancialYearBalanceModel
    {
        public int LedgerBalanceId { get; set; }

        [Required(ErrorMessage = "Ledger Name is required.")]
        [Display(Name = "Ledger Name")]
        public int LedgerId { get; set; }

        [Required(ErrorMessage = "Financial Year is required.")]
        [Display(Name = "Financial Year")]
        public int FinancialYearId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [Display(Name = "Company Name")]
        public int CompanyId { get; set; }

        [Display(Name = "Opening Balance Credit")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        public decimal CreditAmount { get; set; }

        [Display(Name = "Opening Balance Debit")]
        [RegularExpression(RegexHelper.Decimal2Digit, ErrorMessage = RegexHelper.Decimal2DigitMessage)]
        public decimal DebitAmount { get; set; }

        //####
        [Display(Name = "Ledger Name")]
        public string LedgerName { get; set; }

        [Display(Name = "Financial Year Name")]
        public string FinancialYearName { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
