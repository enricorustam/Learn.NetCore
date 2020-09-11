using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Employee")]
    public class Employee
    {
        public string EmpId { get; set; }
        public string Address { get; set; }
        public DateTimeOffset CreatedData { get; set; }
        public DateTimeOffset UpdatedData { get; set; }
        public DateTimeOffset DeletedData { get; set; }
        public bool isDelete { get; set; }

        public virtual User User { get; set; }
    }
}
