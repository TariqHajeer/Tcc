    using System;
using System.Collections.Generic;
using System.DTO.PhoneTypeDTO;
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
    public class PhoneTypeController : MyBaseController
    {
        IRepositroy<PhoneType> _phoneTypeRepositroy;
        public PhoneTypeController(IRepositroy<PhoneType> repositroy, IMapper mapper) : base(mapper)
        {
            this._phoneTypeRepositroy = repositroy;
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var x = _phoneTypeRepositroy.Get(c => c.IsEnabled);
            return Ok(x);
        }
        [Authorize(Roles = "AddPhoneType")]
        [HttpPost]
        public IActionResult AddPhoneType([FromBody]AddPhoneTypeDTO addPhoneTypeDTO)
        {
            try
            {
                addPhoneTypeDTO.Name = addPhoneTypeDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addPhoneTypeDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Phone Type";
                    message.ControllerName = "Phone Type";
                    return BadRequest(message);
                }
                if (_phoneTypeRepositroy.Get(c => c.Name == addPhoneTypeDTO.Name).FirstOrDefault() != null)
                {

                    var message = Messages.Exist;
                    message.ActionName = "Add Phone Type";
                    message.ControllerName = "Phone Type";
                    return Conflict(message);
                }
                var pho = _mapper.Map<PhoneType>(addPhoneTypeDTO);
                _phoneTypeRepositroy.Add(pho, UserName());
                _phoneTypeRepositroy.Save();
                return Ok(pho);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [Authorize(Roles = "ShowPhoneType")]
        [HttpGet]
        public IActionResult ShowPhoneType()
        {
            try
            {
                var pho = _mapper.Map<PhoneType[]>(_phoneTypeRepositroy.Get());
                return Ok(pho);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdatePhoneType")]
        public IActionResult UpdatePhoneType(int id, [FromBody]UpdatePhoneTypeDTO updatePhoneTypeDTO)
        {
            try
            {
                updatePhoneTypeDTO.Name = updatePhoneTypeDTO.Name.Trim();
                var simelar = _phoneTypeRepositroy.Get(c => c.Name == updatePhoneTypeDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Phone Type";
                    message.ControllerName = "Phone Type";
                    return Conflict(message);
                }
                var orginaltypeOfPhone = _phoneTypeRepositroy.Find(id);
                if (orginaltypeOfPhone == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Update Phone Type";
                    message.ControllerName = "Phone Type";
                    message.Message = "نوع التيليفون غير موجود";
                    return NotFound(message);
                }
                orginaltypeOfPhone = _mapper.Map(updatePhoneTypeDTO, orginaltypeOfPhone);
                _phoneTypeRepositroy.Update(orginaltypeOfPhone, UserName());
                _phoneTypeRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemovePhoneType")]
        public IActionResult RemovePhoneType(int id)
        {
            try
            {
                var pho = _phoneTypeRepositroy.Find(id);
                if (pho == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Remove Phone Type";
                    message.ControllerName = "Phone Type";
                    message.Message = "نوع التيليفون غير موجود";
                    return NotFound(message);
                }
                _phoneTypeRepositroy.Remove(pho, UserName());
                _phoneTypeRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}