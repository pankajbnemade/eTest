using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceChargeModel
    {
        public int InvoiceChargeId { get; set; }
        public int? InvoiceId { get; set; }
        public int? SrNo { get; set; }
        public int? ChargeTypeId { get; set; }
        public decimal? ChargeAmountFc { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string Remark { get; set; }

        //####
        public string ChargeTypeName { get; set; }
    }
}
