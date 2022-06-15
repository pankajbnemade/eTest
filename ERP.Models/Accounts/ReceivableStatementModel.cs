﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class ReceivableStatementModel
    {
        public int SequenceNo { get; set; }
        public int SrNo { get; set; }

        [Display(Name = "Invoice Type")]
        public string InvoiceType { get; set; }

        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Customer Ref No")]
        public string CustomerReferenceNo { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Customer Ref Date")]
        public DateTime? CustomerReferenceDate { get; set; }

        [Display(Name = "Credit Limit Days")]
        public int? CreditLimitDays { get; set; }

        [Display(Name = "Payment Term")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Exchange Rate")]
        [Required(ErrorMessage = "Exchange Rate is required.")]
        public decimal ExchangeRate { get; set; }
        public string CurrencyCode { get; set; }

        [Display(Name = "Net Amount FC")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal NetAmountFc { get; set; }

        [Display(Name = "Net Amount")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal NetAmount { get; set; }

        [Display(Name = "Received Amount")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ReceivedAmount { get; set; }

        [Display(Name = "Outstanding Amount")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal OutstandingAmount { get; set; }

        [Display(Name = "Outstanding Days")]
        public int? OutstandingDays { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        public int? PurchaseInvoiceId { get; set; }

        public int? SalesInvoiceId { get; set; }

        public int? CreditNoteId { get; set; }

        public int? DebitNoteId { get; set; }

    }
}