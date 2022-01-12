using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterSalesInvoiceModel
    {
        [Display(Name = "Invoice No.")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Customer")]
        public Nullable<int> CustomerLedgerId { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }

        [Display(Name = "Customer Ref No")]
        public string CustomerReferenceNo { get; set; }

        [Display(Name = "Account")]
        public int? AccountLedgerId { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Year")]
        public int FinancialYearId { get; set; }
    }
}
