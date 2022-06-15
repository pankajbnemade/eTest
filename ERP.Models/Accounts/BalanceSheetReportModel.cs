﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public partial class BalanceSheetReportModel
    {
        public int SequenceNo { get; set; }

        public int SrNo { get; set; }

        [Display(Name = "Particular Liability")]
        public string ParticularLedgerName_Liability { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Amount_Liability { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ClosingAmount_Liability { get; set; }

        [Display(Name = "Particular Asset")]
        public string ParticularLedgerName_Asset { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Amount_Asset { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ClosingAmount_Asset { get; set; }

    }
}