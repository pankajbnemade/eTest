﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Contravoucher
    {
        public Contravoucher()
        {
            Contravoucherattachments = new HashSet<Contravoucherattachment>();
            Contravoucherdetails = new HashSet<Contravoucherdetail>();
        }

        public int ContraVoucherId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Narration { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public decimal AmountFc { get; set; }
        public string AmountFcinWord { get; set; }
        public decimal Amount { get; set; }
        public decimal DebitAmountFc { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmountFc { get; set; }
        public decimal CreditAmount { get; set; }
        public int StatusId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
        public int MaxNo { get; set; }
        public int VoucherStyleId { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Company Company { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Financialyear FinancialYear { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Status Status { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
        public virtual Voucherstyle VoucherStyle { get; set; }
        public virtual ICollection<Contravoucherattachment> Contravoucherattachments { get; set; }
        public virtual ICollection<Contravoucherdetail> Contravoucherdetails { get; set; }
    }
}