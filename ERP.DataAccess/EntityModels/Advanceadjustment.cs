﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("advanceadjustment")]
    public partial class Advanceadjustment
    {
        public Advanceadjustment()
        {
            Advanceadjustmentdetails = new HashSet<Advanceadjustmentdetail>();
        }

        [Key]
        public int AdvanceAdjustmentId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string AdvanceAdjustmentNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AdvanceAdjustmentDate { get; set; }
        public int? AccountLedgerId { get; set; }
        public int? PaymentVoucherId { get; set; }
        public int? ReceiptVoucherId { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Narration { get; set; }
        [Column("Amount_FC", TypeName = "decimal(18,4)")]
        public decimal? AmountFc { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? Amount { get; set; }
        [Column("Amount_FCInWord", TypeName = "varchar(2000)")]
        public string AmountFcinWord { get; set; }
        public int? StatusId { get; set; }
        public int? CompanyId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? MaxNo { get; set; }
        public int? VoucherStyleId { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(AccountLedgerId))]
        [InverseProperty(nameof(Ledger.Advanceadjustments))]
        public virtual Ledger AccountLedger { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("Advanceadjustments")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(FinancialYearId))]
        [InverseProperty(nameof(Financialyear.Advanceadjustments))]
        public virtual Financialyear FinancialYear { get; set; }
        [ForeignKey(nameof(PaymentVoucherId))]
        [InverseProperty(nameof(Paymentvoucher.Advanceadjustments))]
        public virtual Paymentvoucher PaymentVoucher { get; set; }
        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.AdvanceadjustmentPreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(ReceiptVoucherId))]
        [InverseProperty(nameof(Receiptvoucher.Advanceadjustments))]
        public virtual Receiptvoucher ReceiptVoucher { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Advanceadjustments")]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.AdvanceadjustmentUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
        [ForeignKey(nameof(VoucherStyleId))]
        [InverseProperty(nameof(Voucherstyle.Advanceadjustments))]
        public virtual Voucherstyle VoucherStyle { get; set; }
        [InverseProperty(nameof(Advanceadjustmentdetail.AdvanceAdjustment))]
        public virtual ICollection<Advanceadjustmentdetail> Advanceadjustmentdetails { get; set; }
    }
}