﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.DataAccess.EntityModels
{
    public partial class CreditNoteModel
    {
        public int CreditNoteId { get; set; }
       
        [Display(Name = "Credit Note No")]
        [Required(ErrorMessage = "Credit Note No is required.")]
        public string CreditNoteNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Credit Note Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Credit Note Date is required.")]
        public DateTime? CreditNoteDate { get; set; }

         [Display(Name = "Party")]
        [Required(ErrorMessage = "Party is required.")]
        public int? PartyLedgerId { get; set; }

         [Display(Name = "Bill To Address")]
        [Required(ErrorMessage = "Bill To Address is required.")]
        public int? BillToAddressId { get; set; }

         [Display(Name = "Account Ledger")]
        [Required(ErrorMessage = "Account Ledger is required.")]
        public int? AccountLedgerId { get; set; }

        [Display(Name = "Our Ref No")]
        [StringLength(250, ErrorMessage = "250 chars only.")]
        [Required(ErrorMessage = "Our Ref No is required.")]
        public string PartyReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Party Ref Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Party Ref Date is required.")]
        public DateTime? PartyReferenceDate { get; set; }

        [Display(Name = "Our Ref No")]
        [StringLength(250, ErrorMessage = "250 chars only.")]
        [Required(ErrorMessage = "Our Ref No is required.")]
        public string OurReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Our Ref Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Our Ref Date is required.")]
        public DateTime? OurReferenceDate { get; set; }

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
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        public decimal ExchangeRate { get; set; }

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

        [Display(Name = "Tax amount FC")]
        public decimal? TaxAmountFc { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Discount Type")]
        public string DiscountPercentageOrAmount { get; set; }

        [Display(Name = "Discount")]
        public decimal? DiscountPerOrAmountFc { get; set; }

        [Display(Name = "Discount Amount FC")]
        public decimal? DiscountAmountFc { get; set; }

        [Display(Name = "Discount Amount FC")]
        public decimal? DiscountAmount { get; set; }

        public int? StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }

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