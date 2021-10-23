﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace ERP.DataAccess.EntityModels
{
    public partial class Ledgerfinancialyearbalance
    {
        public int LedgerBalanceId { get; set; }
        public int LedgerId { get; set; }
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal OpeningBalanceAmountFc { get; set; }
        public decimal OpeningBalanceAmount { get; set; }
        public int PreparedByUserId { get; set; }
        public DateTime? PreparedDateTime { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual Company Company { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Financialyear FinancialYear { get; set; }
        public virtual Ledger Ledger { get; set; }
        public virtual Aspnetuser PreparedByUser { get; set; }
        public virtual Aspnetuser UpdatedByUser { get; set; }
    }
}