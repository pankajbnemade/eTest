using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class LedgerModel
    {
        public int LedgerId { get; set; }

        [Required(ErrorMessage = "Ledger Code is required.")]
        [Display(Name = "Ledger Code")]
        public string LedgerCode { get; set; }

        [Required(ErrorMessage = "Ledger Name is required.")]
        [Display(Name = "Ledger Name")]
        public string LedgerName { get; set; }

        [Display(Name = "Is Group")]
        public bool IsGroup { get; set; }

        [Display(Name = "Is Master Group")]
        public bool IsMasterGroup { get; set; }

        [Required(ErrorMessage = "Parent Group is required.")]
        [Display(Name = "Parent Group")]
        public int? ParentGroupId { get; set; }

        [Display(Name = "Is DeActivated")]
        public sbyte IsDeActive { get; set; }

        [Display(Name = "Tax Registered No")]
        public string TaxRegisteredNo { get; set; }

        [Display(Name = "Description")]
        [StringLength(2000, ErrorMessage = "Narration cannot exceed 2000 characters.")]
        public string Description { get; set; }

        public int MaxNo { get; set; }
        //######
        [Display(Name = "Parent Group Name")]
        public string ParentGroupName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

        //######
        [Display(Name = "Closing Balance")]
        public decimal ClosingBalance { get; set; }

        [Display(Name = "Opening Balance Credit")]
        public decimal CreditAmountOpBal { get; set; }

        [Display(Name = "Opening Balance Debit")]
        public decimal DebitAmountOpBal { get; set; }

    }
}
