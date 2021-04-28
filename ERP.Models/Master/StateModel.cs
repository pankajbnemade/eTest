using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Master
{
    public class StateModel
    {
        public int StateId { get; set; }

        [Required(ErrorMessage = "State Name is required.")]
        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        //####
        
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
