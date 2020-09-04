using Learn.NetCore.Models;
using Learn.NetCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.NetCore.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserVM>> GetAll();
        UserVM GetById(int Id);
        int Create(UserVM userVM);
        int Update(UserVM userVM, int Id);
        int Delete(int Id);


    }
}
