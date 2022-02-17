﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class GeneralLedgerModel
    {
        public int SequenceNo { get; set; }
        
        public int SrNo { get; set; }

        public int DocumentId { get; set; }

        public string DocumentType { get; set; }

        public string DocumentNo { get; set; }

        public DateTime? DocumentDate { get; set; }

        public int? PurchaseInvoiceId { get; set; }

        public int? SalesInvoiceId { get; set; }

        public int? CreditNoteId { get; set; }

        public int? DebitNoteId { get; set; }

        public int? PaymentVoucherId { get; set; }

        public int? ReceiptVoucherId { get; set; }

        public int? ContraVoucherId { get; set; }

        public int? JournalVoucherId { get; set; }

        public int? CurrencyId { get; set; }

        public string CurrencyCode { get; set; }

        [Display(Name = "Party Ref No")]
        public string PartyReferenceNo { get; set; }

        [Display(Name = "Our Ref No")]
        public string OurReferenceNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal ExchangeRate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal CreditAmount_FC { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal DebitAmount_FC { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal CreditAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal DebitAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Amount_FC { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        public decimal ClosingAmount { get; set; }
    }
}