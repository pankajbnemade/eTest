﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("vouchersetupdetails")]
    public partial class Vouchersetupdetail
    {
        [Key]
        public int VoucherSetupDetId { get; set; }
        public int? VoucherSetupId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string NoPad { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string NoPreString { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string NoPostString { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string NoSeperator { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string FormatText { get; set; }
        public int? VoucherStyleId { get; set; }
        public int? CompanyId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Vouchersetupdetails")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(FinancialYearId))]
        [InverseProperty(nameof(Financialyear.Vouchersetupdetails))]
        public virtual Financialyear FinancialYear { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.VouchersetupdetailPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.VouchersetupdetailUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey(nameof(VoucherSetupId))]
        [InverseProperty(nameof(Vouchersetup.Vouchersetupdetails))]
        public virtual Vouchersetup VoucherSetup { get; set; }
        [ForeignKey(nameof(VoucherStyleId))]
        [InverseProperty(nameof(Voucherstyle.Vouchersetupdetails))]
        public virtual Voucherstyle VoucherStyle { get; set; }
    }
}