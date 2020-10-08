using System;
using System.Collections.Generic;
using System.Data;
using System.DTO.NationalityDTO;
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
    public class NationalityController : MyBaseController
    {
        IRepositroy<Nationalies> _nationaliesRepositroy;
        public NationalityController(IRepositroy<Nationalies> repositroy, IMapper mapper) : base(mapper)
        {
            _nationaliesRepositroy = repositroy;
        }
        [Authorize(Roles = "AddNationality")]
        [HttpPost]
        public IActionResult AddNationality([FromBody]AddNationalityDTO addNationalityDTO)
        {
            try
            {
                addNationalityDTO.Name = addNationalityDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addNationalityDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Nationalities";
                    message.ControllerName = "Nationalities";
                    return BadRequest(message);
                }
                if (_nationaliesRepositroy.Get(c => c.Name == addNationalityDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Nationalities";
                    message.ControllerName = "Nationalities";
                    return Conflict(message);
                }
                var nat = _mapper.Map<Nationalies>(addNationalityDTO);
                _nationaliesRepositroy.Add(nat, UserName());
                _nationaliesRepositroy.Save();
                return Ok(nat);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [Authorize(Roles = "ShowNationality")]
        [HttpGet]
        public IActionResult ShowNationality()
        {
            try
            {
                var nat = _mapper.Map<NationalityResponseDTO[]>(_nationaliesRepositroy.Get());
                return Ok(nat);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateNationality")]
        public IActionResult UpdateNationality(int id, [FromBody]UpdateNAtionalityDTO updateNAtionalityDTO)
        {
            try
            {
                updateNAtionalityDTO.Name = updateNAtionalityDTO.Name.Trim();
                var simelar = _nationaliesRepositroy.Get(c => c.Name == updateNAtionalityDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {

                    var message = Messages.Exist;
                    message.ActionName = "Update Nationalities";
                    message.ControllerName = "Nationalities";
                    return Conflict(message);
                }
                var orginalNationalites = _nationaliesRepositroy.Find(id);
                if (orginalNationalites == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Nationalities";
                    message.ControllerName = "Nationalities";
                    message.Message = "الجنسية غير موجودة";
                    return NotFound(message);
                }
                orginalNationalites = _mapper.Map(updateNAtionalityDTO, orginalNationalites);
                //Update(orginalNationalites);
                //Commit();
                _nationaliesRepositroy.Update(orginalNationalites, UserName());
                _nationaliesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveNationality")]
        public IActionResult RemoveNationality(int id)
        {
            try
            {
                var nat = _nationaliesRepositroy.Find(id);
                if (nat == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Nationalities";
                    message.ControllerName = "Nationalities";
                    message.Message = "الجنسية غير موجودة";
                    return NotFound(message);
                }
                _nationaliesRepositroy.Remove(nat, UserName());
                _nationaliesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        //Removed Use Get Instade 
        //[HttpGet("GetEnabled")]
        //public IActionResult GetEnabled()
        //{
        //    var x = _nationaliesRepositroy.Get(c => c.IsEnabled);
        //    return Ok(x);
        //}
    }
}