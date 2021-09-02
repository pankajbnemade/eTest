﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("debitnotedetail")]
    public partial class Debitnotedetail
    {
        public Debitnotedetail()
        {
            Debitnotedetailtaxes = new HashSet<Debitnotedetailtax>();
        }

        [Key]
        public int DebitNoteDetId { get; set; }
        public int? DebitNoteId { get; set; }
        public int? SrNo { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Description { get; set; }
        public int? UnitOfMeasurementId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }
        public int? PerUnit { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? UnitPrice { get; set; }
        [Column("GrossAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? GrossAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossAmount { get; set; }
        [Column("TaxAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? TaxAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TaxAmount { get; set; }
        [Column("NetAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? NetAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetAmount { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(DebitNoteId))]
        [InverseProperty(nameof(Debitnote.Debitnotedetails))]
        public virtual Debitnote DebitNote { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.DebitnotedetailPreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(UnitOfMeasurementId))]
        [InverseProperty(nameof(Unitofmeasurement.Debitnotedetails))]
        public virtual Unitofmeasurement UnitOfMeasurement { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.DebitnotedetailUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [InverseProperty(nameof(Debitnotedetailtax.DebitNoteDet))]
        public virtual ICollection<Debitnotedetailtax> Debitnotedetailtaxes { get; set; }
    }
}