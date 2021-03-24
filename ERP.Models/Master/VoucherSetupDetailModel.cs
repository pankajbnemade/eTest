using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class VoucherSetupDetailModel
    {
        public int VoucherSetupDetId { get; set; }
        public int? VoucherSetupId { get; set; }
        public string NoPad { get; set; }
        public string NoPreString { get; set; }
        public string NoPostString { get; set; }
        public string NoSeperator { get; set; }
        public string FormatText { get; set; }
        public int? VoucherStyleId { get; set; }
        public int? CompanyId { get; set; }
        public int? FinancialYearId { get; set; }

        //####
        public string VoucherStyleName { get; set; }

        public string PreparedByName { get; set; }
    }
}
