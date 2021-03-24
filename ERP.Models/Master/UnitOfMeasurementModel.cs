using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class UnitOfMeasurementModel
    {
        public int UnitOfMeasurementId { get; set; }
        public string UnitOfMeasurementName { get; set; }

        //####

        public string PreparedByName { get; set; }
    }
}
