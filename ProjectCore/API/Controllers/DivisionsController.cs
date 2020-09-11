using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : BaseController<Division, DivisionRepo>
    {
        readonly DivisionRepo _divRepo;

        public DivisionsController(DivisionRepo divisionRepo) : base(divisionRepo)
        {
            _divRepo = divisionRepo;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, Division entity)
        {
            var getId = await _divRepo.GetID(id);
            getId.Name = entity.Name;
            var data = await _divRepo.Update(getId);
            if (data.Equals(null))
            {
                return BadRequest("Something Wrong! Please check again");
            }
            return data;
        }
    }
}