using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceModel
    {
        public int InvoiceId { get; set; }

        [Display(Name = "Invoice No.")]
        [Required(ErrorMessage = "Invoice No. is required.")]
        public string InvoiceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Invoice Date is required.")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Customer is required.")]
        public int? CustomerLedgerId { get; set; }

        [Display(Name = "Bill To Address")]
        public int? BillToAddressId { get; set; }

        public int? AccountLedgerId { get; set; }

        [Display(Name = "Bank Ledger")]
        public int? BankLedgerId { get; set; }

        [Display(Name = "Customer Ref No")]
        [StringLength(250, ErrorMessage = "250 chars only.")]
        public string CustomerReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Customer Ref Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CustomerReferenceDate { get; set; }

        [Display(Name = "Credit Limit Days")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int? CreditLimitDays { get; set; }

        [Display(Name = "Payment Term")]
        [StringLength(2000, ErrorMessage = "2000 chars only.")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Remark")]
        [StringLength(2000, ErrorMessage = "2000 chars only.")]
        public string Remark { get; set; }

        [Display(Name = "Tax Model Type")]
        public string TaxModelType { get; set; }

        [Display(Name = "Tax Register")]
        public int? TaxRegisterId { get; set; }

        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalLineItemAmountFc { get; set; }
        public decimal? TotalLineItemAmount { get; set; }
        public decimal? GrossAmountFc { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmountFc { get; set; }
        public decimal? NetAmount { get; set; }
        public string NetAmountFcinWord { get; set; }
        public decimal? TaxAmountFc { get; set; }
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Discount Percentage/Amount")]
        public string DiscountPercentageOrAmount { get; set; }

        [Display(Name = "Discount Type")]
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmountFc { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? StatusId { get; set; }
        public int? CompanyId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }
        public int? PreparedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        //#####

        public string CustomerLedgerName { get; set; }

        public string BillToAddress { get; set; }

        public string AccountLedgerName { get; set; }

        public string BankLedgerName { get; set; }

        public string TaxRegisterName { get; set; }

        public string CurrencyName { get; set; }

        public string StatusName { get; set; }

        public string PreparedByName { get; set; }

    }
}
