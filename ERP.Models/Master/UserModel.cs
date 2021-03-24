using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Master
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? EmployeeId { get; set; }

        //####
        public string EmployeeName { get; set; }

        public string PreparedByName { get; set; }
    }
}
