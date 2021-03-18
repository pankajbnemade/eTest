using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class LedgerAddressModel
    {
        public int AddressId { get; set; }
        public int LedgerId { get; set; }
        public string AddressDescription { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }
        public string PostalCode { get; set; }
        public string FaxNo { get; set; }

//####
        public string LedgerName { get; set; }
        public string CountryName { get; set; }
        public string StateIName { get; set; }
        public string CityName { get; set; }
    }
}
