﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("company")]
    public partial class Company
    {
        public Company()
        {
            Currencyconversions = new HashSet<Currencyconversion>();
            Financialyearcompanyrelations = new HashSet<Financialyearcompanyrelation>();
            Ledgercompanyrelations = new HashSet<Ledgercompanyrelation>();
            Ledgerfinancialyearbalances = new HashSet<Ledgerfinancialyearbalance>();
            Purchaseinvoices = new HashSet<Purchaseinvoice>();
            Salesinvoices = new HashSet<Salesinvoice>();
            Vouchersetupdetails = new HashSet<Vouchersetupdetail>();
        }

        [Key]
        public int CompanyId { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public string CompanyName { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Address { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string EmailAddress { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Website { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PhoneNo { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string AlternatePhoneNo { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string FaxNo { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PostalCode { get; set; }
        public int? CurrencyId { get; set; }
        public int? NoOfDecimals { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        [InverseProperty("Companies")]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.CompanyPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.CompanyUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [InverseProperty(nameof(Currencyconversion.Company))]
        public virtual ICollection<Currencyconversion> Currencyconversions { get; set; }
        [InverseProperty(nameof(Financialyearcompanyrelation.Company))]
        public virtual ICollection<Financialyearcompanyrelation> Financialyearcompanyrelations { get; set; }
        [InverseProperty(nameof(Ledgercompanyrelation.Company))]
        public virtual ICollection<Ledgercompanyrelation> Ledgercompanyrelations { get; set; }
        [InverseProperty(nameof(Ledgerfinancialyearbalance.Company))]
        public virtual ICollection<Ledgerfinancialyearbalance> Ledgerfinancialyearbalances { get; set; }
        [InverseProperty(nameof(Purchaseinvoice.Company))]
        public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; }
        [InverseProperty(nameof(Salesinvoice.Company))]
        public virtual ICollection<Salesinvoice> Salesinvoices { get; set; }
        [InverseProperty(nameof(Vouchersetupdetail.Company))]
        public virtual ICollection<Vouchersetupdetail> Vouchersetupdetails { get; set; }
    }
}