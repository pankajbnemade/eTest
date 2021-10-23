﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Financialyear
    {
        public Financialyear()
        {
            Advanceadjustments = new HashSet<Advanceadjustment>();
            Contravouchers = new HashSet<Contravoucher>();
            Creditnotes = new HashSet<Creditnote>();
            Debitnotes = new HashSet<Debitnote>();
            Financialyearcompanyrelations = new HashSet<Financialyearcompanyrelation>();
            Journalvouchers = new HashSet<Journalvoucher>();
            Ledgerfinancialyearbalances = new HashSet<Ledgerfinancialyearbalance>();
            Paymentvouchers = new HashSet<Paymentvoucher>();
            Purchaseinvoices = new HashSet<Purchaseinvoice>();
            Receiptvouchers = new HashSet<Receiptvoucher>();
            Salesinvoices = new HashSet<Salesinvoice>();
            Vouchersetupdetails = new HashSet<Vouchersetupdetail>();
        }

        public int FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
        public virtual ICollection<Advanceadjustment> Advanceadjustments { get; set; }
        public virtual ICollection<Contravoucher> Contravouchers { get; set; }
        public virtual ICollection<Creditnote> Creditnotes { get; set; }
        public virtual ICollection<Debitnote> Debitnotes { get; set; }
        public virtual ICollection<Financialyearcompanyrelation> Financialyearcompanyrelations { get; set; }
        public virtual ICollection<Journalvoucher> Journalvouchers { get; set; }
        public virtual ICollection<Ledgerfinancialyearbalance> Ledgerfinancialyearbalances { get; set; }
        public virtual ICollection<Paymentvoucher> Paymentvouchers { get; set; }
        public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; }
        public virtual ICollection<Receiptvoucher> Receiptvouchers { get; set; }
        public virtual ICollection<Salesinvoice> Salesinvoices { get; set; }
        public virtual ICollection<Vouchersetupdetail> Vouchersetupdetails { get; set; }
    }
}