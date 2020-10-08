using System;
using System.Collections.Generic;
using System.DTO.CountryDTO;
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
    public class CountriesController : MyBaseController
    {
        IRepositroy<Countries> _countriesRepositroy;
        public CountriesController(IRepositroy<Countries> repositroy, IMapper mapper) : base(mapper)
        => _countriesRepositroy = repositroy;

        [Authorize(Roles = "AddCountry")]
        [HttpPost]
        public IActionResult AddCountry([FromBody]AddCountryDTO addCountryDTO)
        {
            try
            {
                addCountryDTO.Name = addCountryDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addCountryDTO.Name))
                {

                    var message = Messages.EmptyName;
                    message.ActionName = "Add Countries";
                    message.ControllerName = "Countries";
                    return BadRequest(message);
                }
                if (_countriesRepositroy.Get(c => c.Name == addCountryDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Countries";
                    message.ControllerName = "Countries";
                    return Conflict(message);
                }
                var country = _mapper.Map<Countries>(addCountryDTO);
                _countriesRepositroy.Add(country,UserName());
                _countriesRepositroy.Save();
                return Ok(country);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [Authorize(Roles = "ShowCountries")]
        [HttpGet]
        public IActionResult ShowCountries()
        {
            try
            {
                var countries = _mapper.Map<CountryResponseDTO[]>(_countriesRepositroy.Get(c => c.MainCountry == null));
                return Ok(countries);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateCountry")]
        public IActionResult UpdateCountry(int id, [FromBody]UpdateCountryDTO updateCountryDTO)
        {
            try
            {
                updateCountryDTO.Name = updateCountryDTO.Name.Trim();
                var simelar = _countriesRepositroy.Get(c => c.Name == updateCountryDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {

                    var message = Messages.Exist;
                    message.ActionName = "Update Countries";
                    message.ControllerName = "Countries";
                    return Conflict(message);

                }

                var orginalCountry = _countriesRepositroy.Find(id);
                if (orginalCountry == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Countries";
                    message.ControllerName = "Countries";
                    message.Message = "البلد غير موجود";
                    return NotFound(message);
                }
                    orginalCountry = _mapper.Map(updateCountryDTO, orginalCountry);
                _countriesRepositroy.Update(orginalCountry,UserName());
                _countriesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveCountry")]
        public IActionResult RemoveCountry(int id)
        {
            try
            {

                var country = _countriesRepositroy.Find(id);
                if (country == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Countries";
                    message.ControllerName = "Countries";
                    message.Message = "البلد غير موجود";
                    return NotFound(message);
                }
                var cityes = _countriesRepositroy.Get(c => c.MainCountry == id).Count();
                if (cityes > 0)
                {

                    var message = Messages.CannotDelete;
                    message.ActionName = "Remove Countries";
                    message.ControllerName = "Countries";
                    return Conflict(message);
                }
                _countriesRepositroy.Remove(country, UserName());
                _countriesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            try
            {
                var countires = _mapper.Map<CountryResponseDTO[]>(_countriesRepositroy.Get(c => c.IsEnabled && c.MainCountry == null));
                return Ok(countires);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<Countries> countries = new List<Countries>();
                var mainCountries = _countriesRepositroy.Get(c => c.IsEnabled == true && c.MainCountry == null).OrderBy(c => c.Id);
                foreach (var item in mainCountries)
                {
                    countries.Add(item);
                    var cities = _countriesRepositroy.Get(c => c.MainCountry == item.Id && c.IsEnabled == true);
                    countries.AddRange(cities);
                }
                //var countriesWithCityes = _mapper.Map<CountryResponseDTO[]>(_countriesRepositroy.Get());
                //return Ok(countriesWithCityes);
                return Ok(_mapper.Map<CountryResponseDTO[]>(countries));
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
    }
}