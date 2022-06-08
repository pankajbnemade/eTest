using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SearchFilterOpeningBalanceTransferModel
    {

        [Required(ErrorMessage = "From Year is required.")]
        [Display(Name = "From Year")]
        public int FromYearId { get; set; }

        [Required(ErrorMessage = "To Year is required.")]
        [Display(Name = "To Year")]
        public int ToYearId { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Year")]
        public int FinancialYearId { get; set; }
    }
}
