using API.Context;
using API.Models;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class DivisionRepo : GeneralRepo<Division, MyContext>
    {
        MyContext _context;
        public DivisionRepo(MyContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Division>> GetAll()
        {
            //var data = await _context.Set<Division>().Where(x => x.isDelete == false).ToListAsync();
            //return data;
            //var department = await _context.Departments.ToListAsync();
            var data = await _context.Divisions.Include("department").Where(x => x.isDelete == false).ToListAsync();
            return data;
        }

    }
}
