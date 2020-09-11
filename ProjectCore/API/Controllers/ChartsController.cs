using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Context;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly MyContext _context;

        public ChartsController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("pie")]
        public async Task<List<PieChartVM>> GetPie()
        {
            //List<PieChartVM> list = new List<PieChartVM>();

            var data = await _context.Divisions.Include("department")
                .Where(Q => Q.isDelete == false)
                .GroupBy(q => q.department.Name)
                .Select(q => new PieChartVM
            {
                DepartmentName = q.Key,
                count = q.Count()
            }).ToListAsync();

            return data;
        }
    }
}