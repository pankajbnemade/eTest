using ERP.Models.Common;
using ERP.Models.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class SalesInvoiceModel
    {
        public int SalesInvoiceId { get; set; }

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
        [Required(ErrorMessage = "Bill To Address is required.")]
        public int? BillToAddressId { get; set; }

        [Display(Name = "Account")]
        [Required(ErrorMessage = "Account Ledger is required.")]
        public int? AccountLedgerId { get; set; }

        [Display(Name = "Bank Account")]
        public int? BankLedgerId { get; set; }

        [Display(Name = "Customer Ref No")]
        [StringLength(250, ErrorMessage = "250 chars only.")]
        [Required(ErrorMessage = "Customer Ref No is required.")]
        public string CustomerReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Customer Ref Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Customer Ref Date is required.")]
        public DateTime? CustomerReferenceDate { get; set; }

        [Display(Name = "Credit Limit Days")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        public int? CreditLimitDays { get; set; }

        [Display(Name = "Payment Term")]
        [StringLength(2000, ErrorMessage = "2000 chars only.")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Remark")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Remark { get; set; }

        [Display(Name = "Tax Model Type")]
        [Required(ErrorMessage = "Tax Model Type is required.")]
        public string TaxModelType { get; set; }

        [Display(Name = "Tax Register")]
        public int? TaxRegisterId { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        public decimal? ExchangeRate { get; set; }

        [Display(Name = "Line Total Amount FC")]
        public decimal? TotalLineItemAmountFc { get; set; }

        [Display(Name = "Line Total Amount")]
        public decimal? TotalLineItemAmount { get; set; }

        [Display(Name = "Gross Amount FC")]
        public decimal? GrossAmountFc { get; set; }

        [Display(Name = "Gross Amount")]
        public decimal? GrossAmount { get; set; }

        [Display(Name = "Net Amount FC")]
        public decimal? NetAmountFc { get; set; }

        [Display(Name = "Net Amount")]
        public decimal? NetAmount { get; set; }

        [Display(Name = "Net Amount FC In Word")]
        public string NetAmountFcinWord { get; set; }

        [Display(Name = "Tax Amount FC")]
        public decimal? TaxAmountFc { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Discount Type")]
        public string DiscountPercentageOrAmount { get; set; }

        [Display(Name = "Discount")]
        public decimal? DiscountPercentage { get; set; }

        [Display(Name = "Discount Amount FC")]
        public decimal? DiscountAmountFc { get; set; }

        [Display(Name = "Discount Amount FC")]
        public decimal? DiscountAmount { get; set; }

        public int? StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int MaxNo { get; set; }
        public int VoucherStyleId { get; set; }
        //public int? PreparedByUserId { get; set; }
        //public int? UpdatedByUserId { get; set; }
        //public DateTime? PreparedDateTime { get; set; }
        //public DateTime? UpdatedDateTime { get; set; }

        //#####

        public string CustomerLedgerName { get; set; }

        public string BillToAddress { get; set; }

        public string AccountLedgerName { get; set; }

        public string BankLedgerName { get; set; }

        public string TaxRegisterName { get; set; }

        public string CurrencyCode { get; set; }

        public string StatusName { get; set; }

        public string PreparedByName { get; set; }

    }
}
