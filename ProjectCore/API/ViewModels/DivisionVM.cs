using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class DivisionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; } // ? = Nullable
        public DateTimeOffset DeleteData { get; set; }
        public bool isDelete { get; set; }
        public int DepartmentId { get; set; }
    }
}
