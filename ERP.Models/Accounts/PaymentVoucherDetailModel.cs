﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class PaymentVoucherDetailModel
    {
        public int PaymentVoucherDetId { get; set; }

        public int PaymentVoucherId { get; set; }

        [Display(Name = "Particular")]
        [Required(ErrorMessage = "Particular is required.")]
        public int ParticularLedgerId { get; set; }

        [Display(Name = "Transaction Type")]
        [Required(ErrorMessage = "Transaction Type is required.")]
        public int TransactionTypeId { get; set; }

        [Display(Name = "Amount FC")]
        [Required(ErrorMessage = "Amount FC is required.")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal AmountFc { get; set; }

        [Display(Name = "Amount")]
        [RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Decimal only.")]
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Display(Name = "Purchase Invoice")]
        public int? PurchaseInvoiceId { get; set; }

        [Display(Name = "Debit Note")]
        public int? DebitNoteId { get; set; }

        //###

        public string TransactionTypeName { get; set; }
        public string ParticularLedgerName { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }

    }
}