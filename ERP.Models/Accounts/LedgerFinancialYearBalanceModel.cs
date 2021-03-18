using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class LedgerFinancialYearBalanceModel
    {
        public int LedgerBalanceId { get; set; }
        public int LedgerId { get; set; }
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal OpeningBalanceAmountFc { get; set; }
        public decimal OpeningBalanceAmount { get; set; }

        //####

        public string LedgerName { get; set; }
        public string FinancialYearName { get; set; }
        public string CompanyName { get; set; }
        public string CurrencyName { get; set; }
    }
}
