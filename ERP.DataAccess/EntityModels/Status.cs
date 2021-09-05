﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Status
    {
        public Status()
        {
            Advanceadjustments = new HashSet<Advanceadjustment>();
            Contravouchers = new HashSet<Contravoucher>();
            Creditnotes = new HashSet<Creditnote>();
            Debitnotes = new HashSet<Debitnote>();
            Journalvouchers = new HashSet<Journalvoucher>();
            Paymentvouchers = new HashSet<Paymentvoucher>();
            Purchaseinvoices = new HashSet<Purchaseinvoice>();
            Receiptvouchers = new HashSet<Receiptvoucher>();
            Salesinvoices = new HashSet<Salesinvoice>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
        public virtual ICollection<Advanceadjustment> Advanceadjustments { get; set; }
        public virtual ICollection<Contravoucher> Contravouchers { get; set; }
        public virtual ICollection<Creditnote> Creditnotes { get; set; }
        public virtual ICollection<Debitnote> Debitnotes { get; set; }
        public virtual ICollection<Journalvoucher> Journalvouchers { get; set; }
        public virtual ICollection<Paymentvoucher> Paymentvouchers { get; set; }
        public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; }
        public virtual ICollection<Receiptvoucher> Receiptvouchers { get; set; }
        public virtual ICollection<Salesinvoice> Salesinvoices { get; set; }
    }
}