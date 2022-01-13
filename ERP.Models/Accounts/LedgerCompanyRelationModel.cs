using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class LedgerCompanyRelationModel
    {
        public int RelationId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [Display(Name = "Company Name")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Ledger Name is required.")]
        [Display(Name = "Ledger Name")]
        public int LedgerId { get; set; }

        //####
        [Display(Name = "LedgerName")]
        public string LedgerName { get; set; }

        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
