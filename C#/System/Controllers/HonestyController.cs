using System;
using System.Collections.Generic;
using System.DTO.HonestyDTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.infrastructure;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Static;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HonestyController : MyBaseController
    {
        IRepositroy<Honesty> _honestyRepositroy;
        IRepositroy<Countries> _countryRepoistory;
        public HonestyController(
            IRepositroy<Honesty> repositroy,
            IRepositroy<Countries> countryRepoistory,
            IMapper mapper
            ) : base(mapper)
        {
            _countryRepoistory = countryRepoistory;
            _honestyRepositroy =repositroy;
        }
        [HttpPost]
        [Authorize(Roles = "AddHonesty")]
        public IActionResult AddHonesty([FromBody] AddHonestyDTO addHonestyDTO)
        {
            try
            {
                addHonestyDTO.Name = addHonestyDTO.Name.Trim();
                var city = _countryRepoistory.Find(addHonestyDTO.CountryId);
                if (city.MainCountry == null)
                {
                    var message = Messages.CityNotCountry;
                    message.ActionName = "Add Honesty ";
                    message.ControllerName = "Honesty";
                    return BadRequest(message);
                  
                }
                    
                if (string.IsNullOrWhiteSpace(addHonestyDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Honesty ";
                    message.ControllerName = "Honesty";
                    return BadRequest(message);
                }
                if (_honestyRepositroy.Get(c => c.Name == addHonestyDTO.Name&&c.CountryId==addHonestyDTO.CountryId).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Honesty ";
                    message.ControllerName = "Honesty";
                    return Conflict(message);
                }
                var honesty = _mapper.Map<Honesty>(addHonestyDTO);
                _honestyRepositroy.Add(honesty, UserName());
                _honestyRepositroy.Save();

                return Ok(honesty);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var hon = _honestyRepositroy.Get(c => c.IsEnabled);
            return Ok(hon);
        }
        [HttpGet]
        [Authorize(Roles = "ShowHonesty")]
        public IActionResult ShowHonesty()
        {
            try
            {
                var hon = _honestyRepositroy.GetIQueryable().Include(c => c.Country);
                return Ok(_mapper.Map<HonestyResponseDTO[]>(hon));
            }
            catch
            {
                return BadRequestAnonymousError();
            }

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateHonesty")]
        public IActionResult UpdateHonesty(int id, [FromBody] UpdateHonestyDTO updateHonestyDTO)
        {
            try
            {
                updateHonestyDTO.Name = updateHonestyDTO.Name.Trim();
                var simelar = _honestyRepositroy.Get(c => c.Name == updateHonestyDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Honesty ";
                    message.ControllerName = "Honesty";
                    return Conflict(message);
                }
                var orginalHonesty = _honestyRepositroy.Find(id);
                if (orginalHonesty == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Update Honesty ";
                    message.ControllerName = "Honesty";
                    message.Message = "الامانة غير موجودة";
                    return NotFound(message);
                }
                orginalHonesty = _mapper.Map(updateHonestyDTO, orginalHonesty);
                _honestyRepositroy.Update(orginalHonesty,UserName());
                _honestyRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveHonesty")]
        public IActionResult RemoveHonesty(int id)
        {
            try
            {
                var hon = _honestyRepositroy.Find(id);
                if (hon == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Remove Honesty ";
                    message.ControllerName = "Honesty";
                    message.Message = "الامانة غير موجودة";
                    return NotFound(message);
                }
                _honestyRepositroy.Remove(hon, UserName());
                _honestyRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}