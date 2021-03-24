namespace ERP.Models.Master
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public string EmailAddress { get; set; }

        //#####
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string PreparedByName { get; set; }

    }
}
