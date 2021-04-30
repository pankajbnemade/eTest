using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class FinancialYearModel
    {
        public int FinancialYearId { get; set; }

        [Required(ErrorMessage = "Financial Year Name is required.")]
        [Display(Name = "Financial Year Name")]
        public string FinancialYearName { get; set; }

        [Required(ErrorMessage = "From Date is required.")]
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required.")]
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }

        //#####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

    }
}
