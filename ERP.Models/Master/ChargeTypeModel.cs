using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class ChargeTypeModel
    {
        public int ChargeTypeId { get; set; }
        public string ChargeTypeName { get; set; }

        //####

        public string PreparedByName { get; set; }
    }
}
