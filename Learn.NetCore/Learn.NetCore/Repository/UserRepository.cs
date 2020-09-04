using Dapper;
using Learn.NetCore.Models;
using Learn.NetCore.Repository.Interface;
using Learn.NetCore.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.NetCore.Repository
{
    public class UserRepository : IUserRepository
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<UserVM>> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Learn.NetCore")))
            {
                var procName = "SP_GetAllUser";
                var getAllUser = await connection.QueryAsync<UserVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllUser;
            }
        }
        public UserVM GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Learn.NetCore")))
            {
                var procName = "SP_GetIdUser";
                parameters.Add("Id", Id);
                var getIdUser = connection.Query<UserVM>(procName, parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return getIdUser;
            }
        }
        public int Create(UserVM userVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Learn.NetCore")))
            {
                var procName = "SP_InsertUser";
                parameters.Add("Email");
                var InsertUser = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return InsertUser;
            }
            //var user = new User() { Email = userVM.Email, UserName = userVM.UserName, PasswordHash = userVM.Password, PhoneNumber = userVM.Phone };
            //user.
        }

        public int Delete(int Id)
        {
            throw new NotImplementedException();
        }


        public int Update(UserVM userVM, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
