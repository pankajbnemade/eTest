using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SearchFilterPDCReportModel
    {
        //[Required(ErrorMessage = "Supplier is required.")]
        [Display(Name = "Party")]
        public int? LedgerId { get; set; }

        [Display(Name = "PDC Type")]
        public string PDCType { get; set; }

        [Required(ErrorMessage = "Cheque From Date is required.")]
        [Display(Name = "Cheque From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Cheque To Date is required.")]
        [Display(Name = "Cheque To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Year")]
        public int FinancialYearId { get; set; }
    }
}
