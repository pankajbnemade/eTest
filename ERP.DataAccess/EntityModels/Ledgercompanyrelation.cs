﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("ledgercompanyrelation")]
    public partial class Ledgercompanyrelation
    {
        [Key]
        public int RelationId { get; set; }
        public int? CompanyId { get; set; }
        public int? LedgerId { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Ledgercompanyrelations")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(LedgerId))]
        [InverseProperty("Ledgercompanyrelations")]
        public virtual Ledger Ledger { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.LedgercompanyrelationPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.LedgercompanyrelationUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
    }
}