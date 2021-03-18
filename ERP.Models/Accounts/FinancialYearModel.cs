using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class FinancialYearModel
    {
        public int FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
