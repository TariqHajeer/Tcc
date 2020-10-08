using System;
using System.Collections.Generic;
using System.DTO.PrivilageDTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.infrastructure;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Static;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivilageController : MyBaseController
    {

        IRepositroy<Privilage> _privilageRepositroy;
        public PrivilageController(IRepositroy<Privilage> repositroy, IMapper mapper) : base( mapper)
        {
            _privilageRepositroy = repositroy;
        }

        [Authorize(Roles = "ShowPrivilage")]
        [HttpGet]
        public IActionResult ShowPrivilage()
        {
            try
            {
                var pri = _mapper.Map<Privilage[]>(_privilageRepositroy.Get());
                return Ok(pri);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

    }
}
