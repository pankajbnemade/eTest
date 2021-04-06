using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Admin
{
    public class UserSessionModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }
    }
}
