using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.NetCore.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Learn.NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        IConfiguration _configuration;
        MyContext _context;
        //UserManager<User> _userManager;
        
    }
}