using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class LedgerAddressModel
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Ledger Name is required.")]
        [Display(Name = "Ledger Name")]
        public int LedgerId { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string AddressDescription { get; set; }

        [Required(ErrorMessage = "Country Name is required.")]
        [Display(Name = "Country Name")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State Name is required.")]
        [Display(Name = "State Name")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City Name is required.")]
        [Display(Name = "City Name")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "EmailAddress is required.")]
        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "PhoneNo is required.")]
        [Display(Name = "PhoneNo")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "PostalCode is required.")]
        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "FaxNo is required.")]
        [Display(Name = "FaxNo")]
        public string FaxNo { get; set; }

        //####
        [Display(Name = "LedgerName")]
        public string LedgerName { get; set; }

        [Display(Name = "CountryName")]
        public string CountryName { get; set; }

        [Display(Name = "StateIName")]
        public string StateName { get; set; }

        [Display(Name = "CityName")]
        public string CityName { get; set; }

        [Display(Name = "Prepared By Name")]
        public string PreparedByName { get; set; }
    }
}
