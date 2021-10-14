﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Paymentvoucherdetail
    {
        public int PaymentVoucherDetId { get; set; }
        public int PaymentVoucherId { get; set; }
        public int ParticularLedgerId { get; set; }
        public int TransactionTypeId { get; set; }
        public decimal AmountFc { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        public int? DebitNoteId { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Debitnote DebitNote { get; set; }
        public virtual Ledger ParticularLedger { get; set; }
        public virtual Paymentvoucher PaymentVoucher { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Purchaseinvoice PurchaseInvoice { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
    }
}