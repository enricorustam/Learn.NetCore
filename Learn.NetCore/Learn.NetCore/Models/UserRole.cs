using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.NetCore.Models
{
    [Table("TB_M_UserRole")]
    public class UserRole : IdentityUserRole<string>
    {
        public User Users { get; set; }
        public Role Roles { get; set; }
    }
}
