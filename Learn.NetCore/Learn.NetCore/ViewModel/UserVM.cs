using Learn.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.NetCore.ViewModel
{
    public class UserVM
    {
        public UserVM(User manage)
        {
            Id = manage.Id;
            Email = manage.Email;
            UserName = manage.UserName;
            Password = manage.PasswordHash;
            Phone = manage.PhoneNumber;

        }

        public UserVM()
        {

        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Roles { get; set; }
    }
}
