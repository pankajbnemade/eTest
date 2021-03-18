using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class PurchaseInvoiceDetailModel
    {

        public int InvoiceDetId { get; set; }
        public int? InvoiceId { get; set; }
        public int? SrNo { get; set; }
        public string Description { get; set; }
        public int? UnitOfMeasurementId { get; set; }
        public decimal? Quantity { get; set; }
        public int? PerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? GrossAmountFc { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? TaxAmountFc { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? NetAmountFc { get; set; }
        public decimal? NetAmount { get; set; }
        //####
        public string UnitOfMeasurementName { get; set; }
    }
}
