﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("ledgerfinancialyearbalance")]
    public partial class Ledgerfinancialyearbalance
    {
        [Key]
        public int LedgerBalanceId { get; set; }
        public int? LedgerId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? CompanyId { get; set; }
        public int? CurrencyId { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? ExchangeRate { get; set; }
        [Column("OpeningBalanceAmount_FC", TypeName = "decimal(18,4)")]
        public decimal? OpeningBalanceAmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? OpeningBalanceAmount { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Ledgerfinancialyearbalances")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        [InverseProperty("Ledgerfinancialyearbalances")]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(FinancialYearId))]
        [InverseProperty(nameof(Financialyear.Ledgerfinancialyearbalances))]
        public virtual Financialyear FinancialYear { get; set; }
        [ForeignKey(nameof(LedgerId))]
        [InverseProperty("Ledgerfinancialyearbalances")]
        public virtual Ledger Ledger { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.LedgerfinancialyearbalancePreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.LedgerfinancialyearbalanceUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
    }
}