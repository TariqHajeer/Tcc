using System;
using System.Collections.Generic;
using System.DTO.TypeOfRegisterDTO;
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
    public class TypeOfRegisterController : MyBaseController
    {
        readonly IRepositroy<TypeOfRegistar> _typeOfRegistarRepositroy;
        public TypeOfRegisterController(IRepositroy<TypeOfRegistar> repositroy, IMapper mapper) : base(mapper)
        {
            _typeOfRegistarRepositroy = repositroy;
        }
        [Authorize(Roles = "AddTypeOfRegistar")]
        [HttpPost]
        public IActionResult AddTypeOfRegistar([FromBody]AddTypeOfRegistarDTO addTypeOfRegistarDTO)
        {
            try
            {
                addTypeOfRegistarDTO.Name = addTypeOfRegistarDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addTypeOfRegistarDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Type Of Register";
                    message.ControllerName = "Typr Of Register";
                    return BadRequest(message);
                }
                if (_typeOfRegistarRepositroy.Get(c => c.Name == addTypeOfRegistarDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Type Of Register";
                    message.ControllerName = "Typr Of Register";
                    return Conflict(message);
                }
                var type = _mapper.Map<TypeOfRegistar>(addTypeOfRegistarDTO);
                _typeOfRegistarRepositroy.Add(type, UserName());
                _typeOfRegistarRepositroy.Save();
                return Ok(type);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var x = _typeOfRegistarRepositroy.Get(c => c.IsEnabled);
            return Ok(x);
        }
        [Authorize(Roles = "ShowTypeOfRegistar")]
        [HttpGet]
        public IActionResult ShowTypeOfRegistar()
        {
            try
            {
                var type = _mapper.Map<TypeOfRegistar[]>(_typeOfRegistarRepositroy.Get());
                return Ok(type);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateTypeOfRegistar")]
        public IActionResult UpdateTypeOfRegistar(int id, [FromBody]UpdateTypeOfRegistarDTO updateTypeOfRegistarDTO)
        {
            try
            {
                updateTypeOfRegistarDTO.Name = updateTypeOfRegistarDTO.Name.Trim();
                var simelar = _typeOfRegistarRepositroy.Get(c => c.Name == updateTypeOfRegistarDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Type Of Register";
                    message.ControllerName = "Typr Of Register";
                    return Conflict(message);
                }

                var orginaltype = _typeOfRegistarRepositroy.Find(id);
                if (orginaltype == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Type Of Register";
                    message.ControllerName = "Typr Of Register";
                    message.Message = "نوع التسجيل غير موجود";
                    return NotFound(message);
                }
                orginaltype = _mapper.Map(updateTypeOfRegistarDTO, orginaltype);
                _typeOfRegistarRepositroy.Update(orginaltype, UserName());
                _typeOfRegistarRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveTypeOfRegistar")]
        public IActionResult RemoveTypeOfRegistar(int id)
        {
            try
            {
                var type = _typeOfRegistarRepositroy.Find(id);
                if (type == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Type Of Register";
                    message.ControllerName = "Typr Of Register";
                    message.Message = "نوع التسجيل غير موجود";
                    return NotFound(message);
                }
                _typeOfRegistarRepositroy.Remove(type, UserName());
                _typeOfRegistarRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}