using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterJournalRegisterModel
    {
        //[Required(ErrorMessage = "Account is required.")]
        //[Display(Name = "Account")]
        //public int LedgerId { get; set; }

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
