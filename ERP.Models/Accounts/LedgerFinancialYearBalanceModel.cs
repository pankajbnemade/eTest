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

        [Required(ErrorMessage = "Currency Name is required.")]
        [Display(Name = "Currency Name")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Exchange Rate is required.")]
        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Opening Balance Amount Fc")]
        public decimal OpeningBalanceAmountFc { get; set; }

        [Display(Name = "Opening Balance Amount")]
        public decimal OpeningBalanceAmount { get; set; }

        //####
        [Display(Name = "Ledger Name")]
        public string LedgerName { get; set; }

        [Display(Name = "Financial Year Name")]
        public string FinancialYearName { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
