﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("ledgeraddress")]
    public partial class Ledgeraddress
    {
        public Ledgeraddress()
        {
            Purchaseinvoices = new HashSet<Purchaseinvoice>();
            Salesinvoices = new HashSet<Salesinvoice>();
        }

        [Key]
        public int AddressId { get; set; }
        public int? LedgerId { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string AddressDescription { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string EmailAddress { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PhoneNo { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PostalCode { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string FaxNo { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Ledgeraddresses")]
        public virtual City City { get; set; }
        [ForeignKey(nameof(CountryId))]
        [InverseProperty("Ledgeraddresses")]
        public virtual Country Country { get; set; }
        [ForeignKey(nameof(LedgerId))]
        [InverseProperty("Ledgeraddresses")]
        public virtual Ledger Ledger { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.LedgeraddressPreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(StateId))]
        [InverseProperty("Ledgeraddresses")]
        public virtual State State { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.LedgeraddressUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [InverseProperty(nameof(Purchaseinvoice.BillToAddress))]
        public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; }
        [InverseProperty(nameof(Salesinvoice.BillToAddress))]
        public virtual ICollection<Salesinvoice> Salesinvoices { get; set; }
    }
}