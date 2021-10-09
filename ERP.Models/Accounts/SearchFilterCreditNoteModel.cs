using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SearchFilterCreditNoteModel
    {
        [Display(Name = "Credit Note No")]
        public string CreditNoteNo { get; set; }

        [Display(Name = "Party Ledger")]
        public Nullable<int> PartyLedgerId { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }

        [Display(Name = "Party Ref No")]
        public string PartyReferenceNo { get; set; }

        [Display(Name = "Account")]
        public Nullable<int> AccountLedgerId { get; set; }
    }
}
