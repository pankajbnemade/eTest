﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class SearchFilterReceiptVoucherModel
    {
        [Display(Name = "Voucher No.")]
        public string VoucherNo { get; set; }

        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }

        [Display(Name = "Cash/Bank")]
        public string TypeCorB { get; set; }

        [Display(Name = "Account")]
        public int? LedgerId { get; set; }

        [Display(Name = "Cheque/Trans. No")]
        public string ChequeNo { get; set; }
    }
}
