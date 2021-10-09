﻿using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class DebitNoteModel
    {
        public int DebitNoteId { get; set; }

        [Display(Name = "Debit Note No")]
        [Required(ErrorMessage = "DebitNote No is required.")]
        public string DebitNoteNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Debit Note Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "DebitNote Date is required.")]
        public DateTime? DebitNoteDate { get; set; }

        [Display(Name = "Party")]
        [Required(ErrorMessage = "Party is required.")]
        public int PartyLedgerId { get; set; }

        [Display(Name = "Bill To Address")]
        [Required(ErrorMessage = "Bill To Address is required.")]
        public int BillToAddressId { get; set; }

        [Display(Name = "Account")]
        [Required(ErrorMessage = "Account is required.")]
        public int AccountLedgerId { get; set; }
        
        [Display(Name = "Our Ref No")]
        [StringLength(250, ErrorMessage = "250 chars only.")]
        [Required(ErrorMessage = "Our Ref No is required.")]
        public string OurReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Our Ref Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Our Ref Date is required.")]
        public DateTime? OurReferenceDate { get; set; }

        [Display(Name = "Party Ref No")]
        [StringLength(250, ErrorMessage = "250 chars only.")]
        [Required(ErrorMessage = "Party Ref No is required.")]
        public string PartyReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Party Ref Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Party Ref Date is required.")]
        public DateTime? PartyReferenceDate { get; set; }

        [Display(Name = "Credit Limit Days")]
        [RegularExpression(RegexHelper.NumericOnly, ErrorMessage = "Numbers only.")]
        [Required(ErrorMessage = "Credit Limit Days is required.")]
        public int CreditLimitDays { get; set; }

        [Display(Name = "Payment Term")]
        [StringLength(2000, ErrorMessage = "2000 chars only.")]
        [Required(ErrorMessage = "Payment Term is required.")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Remark")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Remark { get; set; }

        [Display(Name = "Tax Model Type")]
        [Required(ErrorMessage = "Tax Model Type is required.")]
        public string TaxModelType { get; set; }

        [Display(Name = "Tax Register")]
        [Required(ErrorMessage = "Tax Register is required.")]
        public int TaxRegisterId { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Currency is required.")]
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        [RegularExpression(RegexHelper.DecimalOnly6Digit, ErrorMessage = "Up to 6 Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Line Total Amount FC")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal TotalLineItemAmountFc { get; set; }

        [Display(Name = "Line Total Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal TotalLineItemAmount { get; set; }

        [Display(Name = "Gross Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal GrossAmountFc { get; set; }

        [Display(Name = "Gross Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal GrossAmount { get; set; }

        [Display(Name = "Net Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal NetAmountFc { get; set; }

        [Display(Name = "Net Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal NetAmount { get; set; }

        [Display(Name = "Net Amount FC In Word")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public string NetAmountFcInWord { get; set; }

        [Display(Name = "Tax Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal TaxAmountFc { get; set; }

        [Display(Name = "Tax Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Discount Type")]
        [Required(ErrorMessage = "Discount Type is required.")]
        public string DiscountPercentageOrAmount { get; set; }

        [Display(Name = "Discount Per / Amount")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal DiscountPerOrAmountFc { get; set; }

        [Display(Name = "Discount Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal DiscountAmountFc { get; set; }

        [Display(Name = "Discount Amount")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int MaxNo { get; set; }
        public int VoucherStyleId { get; set; }

        //public int? PreparedByUserId { get; set; }
        //public int? UpdatedByUserId { get; set; }
        //public DateTime? PreparedDateTime { get; set; }
        //public DateTime? UpdatedDateTime { get; set; }

        //####

        public string PartyLedgerName { get; set; }
        public string BillToAddress { get; set; }
        public string AccountLedgerName { get; set; }
        public string TaxRegisterName { get; set; }
        public string CurrencyCode { get; set; }

        public string StatusName { get; set; }
        public string PreparedByName { get; set; }

    }
}
