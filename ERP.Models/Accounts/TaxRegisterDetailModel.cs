using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class TaxRegisterDetailModel
    {
        public int TaxRegisterDetId { get; set; }
        public int? TaxRegisterId { get; set; }
        public int? SrNo { get; set; }
        public int? TaxLedgerId { get; set; }
        public string TaxPercentageOrAmount { get; set; }
        public decimal? Rate { get; set; }
        public string TaxAddOrDeduct { get; set; }

        //####
        public string TaxLedgerName { get; set; }
    }
}
