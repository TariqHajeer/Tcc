using System;
using System.Collections.Generic;
using System.DTO.Specialization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Static;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using DAL.infrastructure;
using DAL.IRepositories;


namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : MyBaseController
    {
        IRepositroy<Specializations> _specializationsRepositroy;
        IStudentRepositroy _studentRepositroy;
        public SpecializationController(IRepositroy<Specializations> repositroy,
            IStudentRepositroy studentRepositroy,
            IMapper mapper) : base(mapper)
        {
            _specializationsRepositroy = repositroy;
            _studentRepositroy = studentRepositroy;
        }
        [Authorize(Roles = "AddSpecialization")]
        [HttpPost]
        public IActionResult AddSpecialization([FromBody]AddSpecializationDTO addSpecializationDTO)
        {
            try
            {
                if (String.IsNullOrEmpty(addSpecializationDTO.Id))
                {
                    return Conflict();
                }
                addSpecializationDTO.Name = addSpecializationDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addSpecializationDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add specialization";
                    message.ControllerName = "specialization";
                    return Conflict(message);
                }
                if (addSpecializationDTO.Id.Length > 1)

                {
                    var message = Messages.OneCharacter;
                    message.ActionName = "Add specialization";
                    message.ControllerName = "specialization";
                    return Conflict(message);
                }
                if (_specializationsRepositroy.Get(c => c.Name == addSpecializationDTO.Name && c.Id == addSpecializationDTO.Id).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add specialization";
                    message.ControllerName = "specialization";
                    return Conflict(message);
                }
                var spec = _mapper.Map<Specializations>(addSpecializationDTO);
                _specializationsRepositroy.Add(spec, UserName());
                _specializationsRepositroy.Save();
                return Ok(_mapper.Map<SpecialziationResponseDTO>(spec));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var enabledSpecializations = _specializationsRepositroy.Get(c => c.IsEnabled);

            return Ok(_mapper.Map<SpecialziationResponseDTO[]>(enabledSpecializations));
        }
        [Authorize(Roles = "ShowSpecialization")]
        [HttpGet]
        public IActionResult ShowSpecialization()
        {
            try
            {
                var spec = _mapper.Map<SpecialziationResponseDTO[]>(_specializationsRepositroy.Get());
                return Ok(spec);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        /// [Authorize(Roles = "UpdateSpecialization")]
        public IActionResult UpdateSpecialization(string id, [FromBody]UpdateSpecializationDTO updateSpecializationDTO)
        {
            try
            {
                updateSpecializationDTO.Name = updateSpecializationDTO.Name.Trim();
                var simelar = _specializationsRepositroy.Get(c => c.Name == updateSpecializationDTO.Name && c.Id != id).FirstOrDefault();
                var orginalspec = _specializationsRepositroy.GetIQueryable(c => c.Id == id)
                    .Include(c => c.StudyPlan)
                    .FirstOrDefault();
                if (orginalspec == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update specialization";
                    message.ControllerName = "specialization";
                    message.Message = "الاختصاص غير موجود";
                    return NotFound(message);
                }
                if (orginalspec.Id != updateSpecializationDTO.Id)
                {
                    var studentHaveThisSpe = _studentRepositroy.GetIQueryable(c => c.SpecializationId == orginalspec.Id).Count();

                    if (orginalspec.StudyPlan.Count > 0 || studentHaveThisSpe > 0)
                    {
                        var message = Messages.CannotDelete;
                        message.ActionName = "Update specialization";
                        message.ControllerName = "specialization";
                        return Conflict(message);
                    }
                }
                orginalspec = _mapper.Map(updateSpecializationDTO, orginalspec);
                _specializationsRepositroy.Update(orginalspec, UserName());
                _specializationsRepositroy.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveSpecializations")]
        public IActionResult RemoveSpecializations(string id)
        {
            try
            {
                var spec = _specializationsRepositroy.Find(id);
                if (spec == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove specialization";
                    message.ControllerName = "specialization";
                    message.Message = "الاختصاص غير موجود";
                    return NotFound(message);
                }
                _specializationsRepositroy.Remove(spec, UserName());
                _specializationsRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

    }
}