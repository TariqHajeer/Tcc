using System;
using System.Collections.Generic;
using System.DTO.CountryDTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using Static;
using Microsoft.EntityFrameworkCore;
using System.DTO.HonestyDTO;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitesController : MyBaseController
    {
        readonly IRepositroy<Countries> _citesRepositroy;
        public CitesController(IRepositroy<Countries> repositroy, IMapper mapper) : base(mapper)
        => _citesRepositroy = repositroy;
        [Authorize(Roles = "ShowCities")]
        [HttpGet]
        public IActionResult ShowCities()
        {
            try
            {
                var cities = _citesRepositroy.GetIQueryable(o => o.MainCountry != null).Include(c => c.MainCountryNavigation);
                return Ok(_mapper.Map<CityResponseDTO[]>(cities));
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [Authorize(Roles = "AddCity")]
        [HttpPost()]
        public IActionResult AddCity([FromBody]AddCityDTO addCityDTO)
        {
            try
            {
                addCityDTO.Name = addCityDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addCityDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add City";
                    message.ControllerName = "Cities";
                    return BadRequest(message);
                }
                if (_citesRepositroy.Get(c => c.Name == addCityDTO.Name && c.MainCountry == addCityDTO.MainCountry      ).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add City";
                    message.ControllerName = "Cities";
                    return Conflict(message);
                }
                var mainCountry = _citesRepositroy.Find(addCityDTO.MainCountry);
                if (mainCountry == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add City";
                    message.ControllerName = "Cities";
                    message.Message = "اسم البلد غير موجود";

                    return Conflict(message);
                }
                if (mainCountry.MainCountry != null)
                {
                    var message = Messages.CityNotCountry;
                    message.ActionName = "Add City";
                    message.ControllerName = "Cites";
                    return Conflict(message);
                }

                var city = _mapper.Map<Countries>(addCityDTO);
                _citesRepositroy .Add(city,UserName());
                _citesRepositroy.Save();
                return Ok(city);
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateCity")]
        public IActionResult UpdateCity(int id, [FromBody]UpdateCityDTO updateCityDTO)
        {
            try
            {
                updateCityDTO.Name = updateCityDTO.Name.Trim();
                var Id = _citesRepositroy.Get(c => c.Id != id).FirstOrDefault();
                if (Id == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update City";
                    message.ControllerName = "Cites";
                    message.Message = "المدينة غير موجودة";
                    return NotFound(message);
                }
                var simelar = _citesRepositroy.Get(c => c.Name == updateCityDTO.Name).FirstOrDefault();
                if (simelar != null)
                {

                    var message = Messages.Exist;
                    message.ActionName = "Update City";
                    message.ControllerName = "Cites";
                    return Conflict(message);
                }

                var orginalCity = _citesRepositroy.Find(id);
                if (orginalCity == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update City";
                    message.ControllerName = "Cites";
                    message.Message = "المدينة غير موجودة";
                    return NotFound(message);
                }
                orginalCity = _mapper.Map(updateCityDTO, orginalCity);
                _citesRepositroy.Update(orginalCity, UserName());
                _citesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveCity")]
        public IActionResult RemoveCity(int id)
        {
            try
            {
                var city = _citesRepositroy.Find(id);
                if (city == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Remove City";
                    message.ControllerName = "Cites";
                    message.Message = "المدينة غير موجودة";
                    return NotFound(message);
                }
                _citesRepositroy.Remove(city, UserName());
                _citesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("Enabled")]
        public IActionResult GetEnabled()
        {
            try
            {
                return Ok(_mapper.Map<CityResponseDTO[]>(_citesRepositroy.Get(c => c.IsEnabled && c.MainCountry != null)));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}