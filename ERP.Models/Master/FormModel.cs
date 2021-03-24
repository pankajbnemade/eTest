using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class FormModel
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int? ModuleId { get; set; }

//#####
        public string ModuleName { get; set; }

        public string PreparedByName { get; set; }
    }
}
