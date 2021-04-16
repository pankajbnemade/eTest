using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class PurchaseInvoiceModel
    {
        public int PurchaseInvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? SupplierLedgerId { get; set; }
        public int? BillToAddressId { get; set; }
        public int? AccountLedgerId { get; set; }
        public string SupplierReferenceNo { get; set; }
        public DateTime? SupplierReferenceDate { get; set; }
        public int? CreditLimitDays { get; set; }
        public string PaymentTerm { get; set; }
        public string Remark { get; set; }
        public string TaxModelType { get; set; }
        public int? TaxRegisterId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal? TotalLineItemAmountFc { get; set; }
        public decimal? TotalLineItemAmount { get; set; }
        public decimal? GrossAmountFc { get; set; }
        public decimal? GrossAmount { get; set; }
        public string DiscountPercentageOrAmount { get; set; }
        public decimal? DiscountPerOrAmountFc { get; set; }
        public decimal? DiscountAmountFc { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxAmountFc { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? NetAmountFc { get; set; }
        public decimal? NetAmount { get; set; }
        public string NetAmountFcinWord { get; set; }
        public int? StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }

        public int? PreparedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        //####

        public string SupplierLedgerName { get; set; }
        public string BillToAddress { get; set; }
        public string AccountLedgerName { get; set; }
        public string TaxRegisterName { get; set; }
        public string CurrencyName { get; set; }

        public string StatusName { get; set; }
        public string PreparedByName { get; set; }

    }
}
