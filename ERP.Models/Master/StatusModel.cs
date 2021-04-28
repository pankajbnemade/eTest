using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class StatusModel
    {
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Status Name is required.")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
