using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SearchFilterPurchaseRegisterModel
    {
        //[Required(ErrorMessage = "Supplier is required.")]
        [Display(Name = "Supplier")]
        public int? SupplierLedgerId { get; set; }

        //[Required(ErrorMessage = "Account is required.")]
        [Display(Name = "Account")]
        public int? AccountLedgerId { get; set; }

        [Required(ErrorMessage = "From Date is required.")]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required.")]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Year")]
        public int FinancialYearId { get; set; }
    }
}
