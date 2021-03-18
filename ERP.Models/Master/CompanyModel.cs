using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string PhoneNo { get; set; }
        public string AlternatePhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        public int CurrencyId { get; set; }

        public int NoOfDecimals { get; set; }

        public string CurrencyName { get; set; }


        //public int? PreparedByUserId { get; set; }
        //public DateTime? PreparedDateTime { get; set; }
        //public int? UpdatedByUserId { get; set; }
        //public DateTime? UpdatedDateTime { get; set; }

    }
}
