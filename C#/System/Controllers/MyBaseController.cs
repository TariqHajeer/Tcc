using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Classes;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using Static;
using Microsoft.AspNetCore.Cors;
using DAL.infrastructure;

namespace System.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController] // last one that comment
    [EnableCors("EnableCORS")]
    //[Authorize]
    public abstract class MyBaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        public MyBaseController(IMapper mapper)
        {
            _mapper = mapper;
        }


        protected string UserName()
        {
            //return User.Claims.Where(c => c.Type == ClaimTypes.Email).First().Value;  
            return "Admin";
        }
        public IActionResult BadRequestAnonymousError()
        {
            return BadRequest(Messages.AnonymousError);
        }

        ///// <summary>
        ///// used when system update somthing 
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="entity"></param>
        ///// <param name="updateBy"></param>
        //protected void Update<TEntity>(TEntity entity, string updateBy) where TEntity : class, IEntity
        //{
        //    _abstractUnitOfWork.Repository<TEntity>().Update(entity, updateBy);
        //}
        //protected void Commit()
        //{

        //}
    }
}