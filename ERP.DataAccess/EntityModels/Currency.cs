﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("currency")]
    public partial class Currency
    {
        public Currency()
        {
            Companies = new HashSet<Company>();
            Creditnotes = new HashSet<Creditnote>();
            Currencyconversions = new HashSet<Currencyconversion>();
            Debitnotes = new HashSet<Debitnote>();
            Ledgerfinancialyearbalances = new HashSet<Ledgerfinancialyearbalance>();
            Purchaseinvoices = new HashSet<Purchaseinvoice>();
            Salesinvoices = new HashSet<Salesinvoice>();
        }

        [Key]
        public int CurrencyId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CurrencyCode { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public string CurrencyName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Denomination { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.CurrencyPreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.CurrencyUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [InverseProperty(nameof(Company.Currency))]
        public virtual ICollection<Company> Companies { get; set; }
        [InverseProperty(nameof(Creditnote.Currency))]
        public virtual ICollection<Creditnote> Creditnotes { get; set; }
        [InverseProperty(nameof(Currencyconversion.Currency))]
        public virtual ICollection<Currencyconversion> Currencyconversions { get; set; }
        [InverseProperty(nameof(Debitnote.Currency))]
        public virtual ICollection<Debitnote> Debitnotes { get; set; }
        [InverseProperty(nameof(Ledgerfinancialyearbalance.Currency))]
        public virtual ICollection<Ledgerfinancialyearbalance> Ledgerfinancialyearbalances { get; set; }
        [InverseProperty(nameof(Purchaseinvoice.Currency))]
        public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; }
        [InverseProperty(nameof(Salesinvoice.Currency))]
        public virtual ICollection<Salesinvoice> Salesinvoices { get; set; }
    }
}