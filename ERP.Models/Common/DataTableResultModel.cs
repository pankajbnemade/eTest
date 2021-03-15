using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Common
{
    public class DataTableResultModel<T>
    {
        public IList<T> ResultList { get; set; }
        public int FilterResultCount { get; set; }
        public int TotalResultCount { get; set; }
    }
}
