﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using ERP.Models.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class ReceiptVoucherDetailModel
    {
        public int ReceiptVoucherDetId { get; set; }

        public int? ReceiptVoucherId { get; set; }

        [Display(Name = "Particular")]
        [Required(ErrorMessage = "Particular is required.")]
        public int? ParticularLedgerId { get; set; }

        [Display(Name = "Transaction Type")]
        [Required(ErrorMessage = "Transaction Type is required.")]
        public int? TransactionTypeId { get; set; }

        [Display(Name = "Amount FC")]
        [Required(ErrorMessage = "Amount FC is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal? AmountFc { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount is required.")]
        //[RegularExpression(RegexHelper.DecimalOnly, ErrorMessage = "Numbers only.")]
        public decimal? Amount { get; set; }

        [Display(Name = "Narration")]
        [Required(ErrorMessage = "Narration is required.")]
        public string Narration { get; set; }

        [Display(Name = "Sales Invoice")]
        //[Required(ErrorMessage = " is required.")]
        public int? SalesInvoiceId { get; set; }

        [Display(Name = "Credit Note")]
        //[Required(ErrorMessage = " is required.")]
        public int? CreditNoteId { get; set; }

        [Display(Name = "Debit Note")]
        //[Required(ErrorMessage = " is required.")]
        public int? DebitNoteId { get; set; }

        //###

        public string TransactionTypeName { get; set; }

    }
}