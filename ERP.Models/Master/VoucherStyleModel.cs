using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class VoucherStyleModel
    {
        public int VoucherStyleId { get; set; }
        public string VoucherStyleName { get; set; }


        //####

        public string PreparedByName { get; set; }
    }
}
