using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceDetailTaxModel
    {
        public int InvoiceDetTaxId { get; set; }
        public int? InvoiceDetId { get; set; }
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
