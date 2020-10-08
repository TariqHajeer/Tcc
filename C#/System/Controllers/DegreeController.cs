using System;
using System.Collections.Generic;
using System.DTO;
using System.DTO.DegreeDTO;
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
    public class DegreeController : MyBaseController
    {
        IRepositroy<Degree> _degreeRepositroy;
        public DegreeController(IRepositroy<Degree> repositroy, IMapper mapper) : base(mapper)
            =>_degreeRepositroy = repositroy;

        [HttpPost]
        [Authorize(Roles = "AddDegree")]
        public IActionResult AddDegree([FromBody] AddDegreeDTO degreeDTO)
        {
            try
            {
                degreeDTO.Name = degreeDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(degreeDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Degree";
                    message.ControllerName = "Degree";
                    return BadRequest(message);
                }
                if (_degreeRepositroy.Get(c => c.Name == degreeDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Degree";
                    message.ControllerName = "Degree";
                    return Conflict(message);
                }
                var degree = _mapper.Map<Degree>(degreeDTO);
                _degreeRepositroy.Add(degree, UserName());
                _degreeRepositroy.Save();

                return Ok(degree);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet]
        [Authorize(Roles = "ShowDegree")]
        public IActionResult Degree()
        {
            try
            {
                var de = _mapper.Map<DegreeResponseDTO[]>(_degreeRepositroy.Get());
                return Ok(de);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateDegree")]
        public IActionResult UpdateDegree(int id, [FromBody] UpdateDegreeDTO updateDegreeDTO)
        {
            try
            {
                updateDegreeDTO.Name = updateDegreeDTO.Name.Trim();
                if (string.IsNullOrEmpty(updateDegreeDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Update Degree";
                    message.ControllerName = "Degree";
                    return BadRequest(message);
                }
                var simelar = _degreeRepositroy.Get(c => c.Name == updateDegreeDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Degree";
                    message.ControllerName = "Degree";
                    return Conflict(message);
                }
                    

                var orginalDegree = _degreeRepositroy.Find(id);
                if (orginalDegree == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Degree";
                    message.ControllerName = "Degree";
                    message.Message = "الشهادة غير موجودة";
                    return NotFound(message);
                }
                orginalDegree = _mapper.Map(updateDegreeDTO, orginalDegree);
                _degreeRepositroy.Update(orginalDegree,UserName());
                _degreeRepositroy.Save();
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
                return Ok(_degreeRepositroy.Get(c => c.IsEnabled));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveDegree")]
        public IActionResult RemoveDegree(int id)
        {
            try
            {
                var deg = _degreeRepositroy.Find(id);
                if (deg == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Rwmove Degree";
                    message.ControllerName = "Degree";
                    message.Message = "الشهادة غير موجودة";
                    return NotFound(message);
                }
                _degreeRepositroy.Remove(deg, UserName());
                _degreeRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}