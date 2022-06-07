using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SearchFilterBalanceSheetReportModel
    {
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }

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
