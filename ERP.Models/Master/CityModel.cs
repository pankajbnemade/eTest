using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Models.Master
{
    public class CityModel
    {
        public int CityId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public string StateName { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
