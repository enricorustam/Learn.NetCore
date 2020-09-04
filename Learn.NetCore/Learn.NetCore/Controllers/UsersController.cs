using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Learn.NetCore.Context;
using Learn.NetCore.Models;
using Learn.NetCore.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Learn.NetCore.Controllers
{
    //IConfiguration _configuration;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IConfiguration _configuration;
        MyContext _context;
        UserManager<User> _userManager;
        public UsersController(MyContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        [HttpGet]
        public List<UserVM> GetAll()
        {
            //List<UserVM> list = new List<UserVM>();
            //return await context.Users.ToList();
            //using (context)
            //{
            //    return await context.Users.ToListAsync();
            //}
           

            List<UserVM> list = new List<UserVM>();

            foreach (var users in _context.Users)
            {
                var getrole = _context.UserRoles.SingleOrDefault(Q => Q.UserId == users.Id);
                var role = _context.Roles.SingleOrDefault(Q => Q.Id == getrole.RoleId);
                UserVM user = new UserVM()
                {
                    Id = users.Id,
                    Email = users.Email,
                    Phone = users.PhoneNumber,
                    UserName = users.UserName,
                    Password = users.PasswordHash,
                    Roles = role.Name

                };
                list.Add(user);

            }

            //return await UserVM(list);
            return list;
        }

        [HttpGet("{id}")]
        public UserVM GetID(string id)
        {
            var getId = _context.Users.Find(id);
            UserVM user = new UserVM()
            {
                Id = getId.Id,
                UserName = getId.UserName,
                Email = getId.Email,
                Password = getId.PasswordHash,
                Phone = getId.PhoneNumber
            };
            return user;
        }

        [HttpPost]
        public IActionResult Create(UserVM userVM)
        {
            
            var user = new User
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userVM.Password),
                PhoneNumber = userVM.Phone
            };
            _context.Users.Add(user);
            var role = new UserRole
            {
                UserId = user.Id,
                RoleId = "2",
            };
            _context.UserRoles.Add(role);
            _context.SaveChanges();
            return Ok("Successfully Created");
            //return data;
            //var user = new User { Email = userVM.Email, UserName = userVM.UserName, PasswordHash = userVM.Password, PhoneNumber = userVM.Phone };
            //var result = await _userManager.CreateAsync(user, userVM.Password);
            //return Ok("Successfully Create");

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserVM userVM)
        {

            var getId = _context.Users.Find(id);
            getId.Id = userVM.Id;
            getId.UserName = userVM.UserName;
            getId.Email = userVM.Email;
            getId.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userVM.Password);
            getId.PhoneNumber = userVM.Phone;
            _context.SaveChanges();
            return Ok("Successfully Update");
            //var user = await _userManager.FindByIdAsync(userVM.Id);
            //user.UserName = userVM.UserName;
            //user.Email = userVM.Email;
            //user.PhoneNumber = userVM.Phone;
            //var pass = await _userManager.PasswordHasher.HashPassword(user, userVM.Password);
            ////var pass = await _userManager.ChangePasswordAsync(user, user.PasswordHash, userVM.Password);
            //user.PasswordHash = pass;

            //await _userManager.UpdateAsync(user);
            //return Ok("Successfully Update");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {

            var getId = _context.Users.Find(id);
            _context.Users.Remove(getId);
            _context.SaveChanges();
            return Ok("Successfully Delete");

            //var user = await _userManager.FindByIdAsync(id);
            //await _userManager.DeleteAsync(user);
            //return Ok("Successfully Delete");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserVM userVM)
        {
            var users = _context.Users.Where(Q => Q.Email == userVM.Email).FirstOrDefault();
            if (users == null)
            {
                return BadRequest("Email Does not Exist");
            }

            var pass = BCrypt.Net.BCrypt.Verify(userVM.Password, users.PasswordHash);
            if (!pass)
            {
                return BadRequest("Password Does not Match");
            }
            //var x = _context.Users.Include("Role").Include("UserRole").Where(Q => Q.Email == userVM.Email).SingleOrDefault();
            //var role = users.UserRoles.Where(Q => Q.RoleId == "2").Any();
            //if (!role)
            //{
            //    return BadRequest("Role not Sales");
            //}
            return Ok("Successfully Login");
            //return data;
            //var user = new User { Email = userVM.Email, UserName = userVM.UserName, PasswordHash = userVM.Password, PhoneNumber = userVM.Phone };
            //var result = await _userManager.CreateAsync(user, userVM.Password);
            //return Ok("Successfully Create");

        }
    }
}