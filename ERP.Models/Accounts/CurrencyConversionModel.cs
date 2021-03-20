using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class CurrencyConversionModel
    {
        public int ConversionId { get; set; }
        public int? CompanyId { get; set; }
        public int? CurrencyId { get; set; }
        public DateTime? EffectiveDateTime { get; set; }
        public decimal? ExchangeRate { get; set; }

        //#####
        public string CompanyName { get; set; }
        public string CurrencyName { get; set; }
        public string PreparedByName { get; set; }

            }
}
