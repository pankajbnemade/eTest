using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class UnitOfMeasurementModel
    {
        public int UnitOfMeasurementId { get; set; }

        [Required(ErrorMessage = "Unit Of Measurement Name is required.")]
        [Display(Name = "Unit Of Measurement Name")]
        public string UnitOfMeasurementName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
