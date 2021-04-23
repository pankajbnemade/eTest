namespace ERP.Models.Admin
{
    public class UserSessionModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }

    }
}
