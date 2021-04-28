using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Models.Master
{
    public class CountryModel
    {
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        //####
        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
