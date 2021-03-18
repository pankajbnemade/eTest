using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class ModuleModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public sbyte? IsActive { get; set; }

        //####
        //public string Name { get; set; }
    }
}
