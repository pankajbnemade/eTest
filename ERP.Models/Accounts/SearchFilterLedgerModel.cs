using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterLedgerModel
    {
        [Display(Name = "Ledger Name")]
        public string LedgerName { get; set; }

        [Display(Name = "Parent Group")]
        public Nullable<int> ParentGroupId { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }

        [Display(Name = "Show Group")]
        public bool IsGroup { get; set; }
    }
}
