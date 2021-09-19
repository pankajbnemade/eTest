﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterPaymentVoucherModel
    {
        [Display(Name = "Voucher No.")]
        public string VoucherNo { get; set; }

        [Display(Name = "Ledger")]
        public Nullable<int> LedgerId { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }
    }
}