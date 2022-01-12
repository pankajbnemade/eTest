using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SearchFilterDebitNoteModel
    {
        [Display(Name = "Debit Note No.")]
        public string DebitNoteNo { get; set; }

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

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Year")]
        public int FinancialYearId { get; set; }
    }
}
