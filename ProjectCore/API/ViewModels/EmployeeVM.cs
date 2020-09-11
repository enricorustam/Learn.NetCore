using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class EmployeeVM
    {
        public string EmpId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTimeOffset CreatedData { get; set; }
        public DateTimeOffset UpdatedData { get; set; }
        public DateTimeOffset DeletedData { get; set; }
        public bool isDelete { get; set; }
    }
}
