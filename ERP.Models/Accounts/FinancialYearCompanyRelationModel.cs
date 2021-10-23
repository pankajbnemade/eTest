using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Accounts
{
    public class FinancialYearCompanyRelationModel
    {
        public int RelationId { get; set; }
        public int CompanyId { get; set; }
        public int FinancialYearId { get; set; }

        //####
        public string CompanyName { get; set; }
        public string FinancialYearName { get; set; }
        public string PreparedByName { get; set; }

    }
}
