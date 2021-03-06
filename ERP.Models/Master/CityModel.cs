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
        [Display(Name = "City Name")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State Name")]
        public int StateId { get; set; }
        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country Name")]
        public int CountryId { get; set; }
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
