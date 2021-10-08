using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterPurchaseInvoiceModel
    {
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Supplier")]
        public Nullable<int> SupplierLedgerId { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }

        [Display(Name = "Supplier Ref No")]
        public string SupplierReferenceNo { get; set; }

        [Display(Name = "Account")]
        public Nullable<int> AccountLedgerId { get; set; }
    }
}
