using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Phone No is required.")]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Display(Name = "Alternate Phone No")]
        public string AlternatePhoneNo { get; set; }

        [Display(Name = "Fax No")]
        public string FaxNo { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Currency Name is required.")]
        [Display(Name = "Currency Name")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "No Of Decimals is required.")]
        [Display(Name = "No Of Decimals")]
        public int NoOfDecimals { get; set; }

        //####
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }

    }
}
