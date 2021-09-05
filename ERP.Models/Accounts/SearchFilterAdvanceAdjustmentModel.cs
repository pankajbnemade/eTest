﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterAdvanceAdjustmentModel
    {
        [Display(Name = "Advance Adjustment No.")]
        public string AdvanceAdjustmentNo { get; set; }

        [Display(Name = "Account Ledger")]
        public Nullable<int> AccountLedgerId { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }
    }
}
