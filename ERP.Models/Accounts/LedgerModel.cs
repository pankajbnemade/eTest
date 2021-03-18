using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Accounts
{
    public class LedgerModel
    {
        public int LedgerId { get; set; }
        public string LedgerCode { get; set; }
        public string LedgerName { get; set; }
        public sbyte IsGroup { get; set; }
        public sbyte IsMasterGroup { get; set; }
        public int ParentGroupId { get; set; }
        public sbyte IsDeActived { get; set; }
        public string TaxRegistredNo { get; set; }

        //######

        public int ParentGroupName { get; set; }


    }
}
