﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("ledger")]
    public partial class Ledger
    {
        public Ledger()
        {
            InverseParentGroup = new HashSet<Ledger>();
            Ledgeraddresses = new HashSet<Ledgeraddress>();
            Ledgercompanyrelations = new HashSet<Ledgercompanyrelation>();
            Ledgerfinancialyearbalances = new HashSet<Ledgerfinancialyearbalance>();
            PurchaseinvoiceAccountLedgers = new HashSet<Purchaseinvoice>();
            PurchaseinvoiceBillToAddresses = new HashSet<Purchaseinvoice>();
            PurchaseinvoiceSupplierLedgers = new HashSet<Purchaseinvoice>();
            Purchaseinvoicedetailtaxes = new HashSet<Purchaseinvoicedetailtax>();
            Purchaseinvoicetaxes = new HashSet<Purchaseinvoicetax>();
            SalesinvoiceAccountLedgers = new HashSet<Salesinvoice>();
            SalesinvoiceBankLedgers = new HashSet<Salesinvoice>();
            SalesinvoiceBillToAddresses = new HashSet<Salesinvoice>();
            SalesinvoiceCustomerLedgers = new HashSet<Salesinvoice>();
            Salesinvoicedetailtaxes = new HashSet<Salesinvoicedetailtax>();
            Salesinvoicetaxes = new HashSet<Salesinvoicetax>();
            Taxregisterdetails = new HashSet<Taxregisterdetail>();
        }

        [Key]
        public int LedgerId { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string LedgerCode { get; set; }
        [Required]
        [Column(TypeName = "varchar(500)")]
        public string LedgerName { get; set; }
        public sbyte? IsGroup { get; set; }
        public sbyte? IsMasterGroup { get; set; }
        public int? ParentGroupId { get; set; }
        public sbyte? IsDeActived { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string TaxRegistredNo { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(ParentGroupId))]
        [InverseProperty(nameof(Ledger.InverseParentGroup))]
        public virtual Ledger ParentGroup { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.LedgerPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.LedgerUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [InverseProperty(nameof(Ledger.ParentGroup))]
        public virtual ICollection<Ledger> InverseParentGroup { get; set; }
        [InverseProperty(nameof(Ledgeraddress.Ledger))]
        public virtual ICollection<Ledgeraddress> Ledgeraddresses { get; set; }
        [InverseProperty(nameof(Ledgercompanyrelation.Ledger))]
        public virtual ICollection<Ledgercompanyrelation> Ledgercompanyrelations { get; set; }
        [InverseProperty(nameof(Ledgerfinancialyearbalance.Ledger))]
        public virtual ICollection<Ledgerfinancialyearbalance> Ledgerfinancialyearbalances { get; set; }
        [InverseProperty(nameof(Purchaseinvoice.AccountLedger))]
        public virtual ICollection<Purchaseinvoice> PurchaseinvoiceAccountLedgers { get; set; }
        [InverseProperty(nameof(Purchaseinvoice.BillToAddress))]
        public virtual ICollection<Purchaseinvoice> PurchaseinvoiceBillToAddresses { get; set; }
        [InverseProperty(nameof(Purchaseinvoice.SupplierLedger))]
        public virtual ICollection<Purchaseinvoice> PurchaseinvoiceSupplierLedgers { get; set; }
        [InverseProperty(nameof(Purchaseinvoicedetailtax.TaxLedger))]
        public virtual ICollection<Purchaseinvoicedetailtax> Purchaseinvoicedetailtaxes { get; set; }
        [InverseProperty(nameof(Purchaseinvoicetax.TaxLedger))]
        public virtual ICollection<Purchaseinvoicetax> Purchaseinvoicetaxes { get; set; }
        [InverseProperty(nameof(Salesinvoice.AccountLedger))]
        public virtual ICollection<Salesinvoice> SalesinvoiceAccountLedgers { get; set; }
        [InverseProperty(nameof(Salesinvoice.BankLedger))]
        public virtual ICollection<Salesinvoice> SalesinvoiceBankLedgers { get; set; }
        [InverseProperty(nameof(Salesinvoice.BillToAddress))]
        public virtual ICollection<Salesinvoice> SalesinvoiceBillToAddresses { get; set; }
        [InverseProperty(nameof(Salesinvoice.CustomerLedger))]
        public virtual ICollection<Salesinvoice> SalesinvoiceCustomerLedgers { get; set; }
        [InverseProperty(nameof(Salesinvoicedetailtax.TaxLedger))]
        public virtual ICollection<Salesinvoicedetailtax> Salesinvoicedetailtaxes { get; set; }
        [InverseProperty(nameof(Salesinvoicetax.TaxLedger))]
        public virtual ICollection<Salesinvoicetax> Salesinvoicetaxes { get; set; }
        [InverseProperty(nameof(Taxregisterdetail.TaxLedger))]
        public virtual ICollection<Taxregisterdetail> Taxregisterdetails { get; set; }
    }
}