﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class ReceiptVoucherOutstandingInvoiceModel
    {
        public int ReceiptVoucherId { get; set; }

        public int ParticularLedgerId { get; set; }

        public int TransactionTypeId { get; set; }

        public int? InvoiceId { get; set; }

        public string InvoiceType { get; set; }

        public string InvoiceNo { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal InvoiceAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal OutstandingAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal InvoiceAmount_FC { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal OutstandingAmount_FC { get; set; }

        public int? PurchaseInvoiceId { get; set; }

        public int? SalesInvoiceId { get; set; }

        public int? CreditNoteId { get; set; }

        public int? DebitNoteId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal? AmountFc { get; set; }

        public string Narration { get; set; }

    }
}