using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class PurchaseInvoiceDetailTaxModel
    {
        public int PurchaseInvoiceDetTaxId { get; set; }
        public int? PurchaseInvoiceDetId { get; set; }
        public int? SrNo { get; set; }
        public int? TaxLedgerId { get; set; }
        public string TaxPercentageOrAmount { get; set; }
        public decimal? TaxPerOrAmountFc { get; set; }
        public string TaxAddOrDeduct { get; set; }
        public decimal? TaxAmountFc { get; set; }
        public decimal? TaxAmount { get; set; }
        public string Remark { get; set; }
        //####
        public string TaxLedgerName { get; set; }
    }
}
