using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class TaxRegisterDetailModel
    {
        public int TaxRegisterDetId { get; set; }

        [Required(ErrorMessage = "Tax Register Name is required.")]
        [Display(Name = "Tax Register Name")]
        public int TaxRegisterId { get; set; }

        [Required(ErrorMessage = "Sr No is required.")]
        [Display(Name = "Sr No")]
        public int SrNo { get; set; }

        [Required(ErrorMessage = "Tax Ledger Name is required.")]
        [Display(Name = "Tax Ledger Name")]
        public int TaxLedgerId { get; set; }

        [Required(ErrorMessage = "Tax Percentage Or Amount is required.")]
        [Display(Name = "Tax Percentage Or Amount")]
        public string TaxPercentageOrAmount { get; set; }

        [Required(ErrorMessage = "Rate is required.")]
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Tax Add Or Deduct is required.")]
        [Display(Name = "Tax Add Or Deduct")]
        public string TaxAddOrDeduct { get; set; }

        //####
        [Display(Name = "Tax Ledger Name")]
        public string TaxLedgerName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
