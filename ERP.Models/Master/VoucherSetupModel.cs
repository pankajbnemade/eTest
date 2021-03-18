using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class VoucherSetupModel
    {
        public int VoucherSetupId { get; set; }
        public string VoucherSetupName { get; set; }
        public int? ModuleId { get; set; }
        public sbyte? IsActive { get; set; }

        //####
        public string ModuleName { get; set; }
    }
}
