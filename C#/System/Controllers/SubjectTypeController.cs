using System;
using System.Collections.Generic;
using System.DTO.SubjectTypeDTO;
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
    public class SubjectTypeController : MyBaseController
    {
        IRepositroy<SubjectType> _subjectTypeRepositroy;
        public SubjectTypeController(IRepositroy<SubjectType> repositroy, IMapper mapper) : base(mapper)
        {
            this._subjectTypeRepositroy = repositroy;
        }
        [HttpPost]
        //[Authorize(Roles = "AddSubjectType")]
        public IActionResult Add([FromBody]AddSubjectTypeDTO addSubjectTypeDTO)
        {
            try
            {
                addSubjectTypeDTO.Name = addSubjectTypeDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addSubjectTypeDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add";
                    message.ControllerName = "Subject Type";
                    return BadRequest(message);
                }
                if (_subjectTypeRepositroy.Get(c => c.Name == addSubjectTypeDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add";
                    message.ControllerName = "Subject Type";
                    return Conflict(message);
                }
                var subjectType = _mapper.Map<SubjectType>(addSubjectTypeDTO);
                _subjectTypeRepositroy.Add(subjectType, UserName());
                _subjectTypeRepositroy.Save();

                return Ok(subjectType);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet]
        //  [Authorize(Roles = "ShowSubjectType")]
        public IActionResult Get()
        {
            try
            {
                var subject = _mapper.Map<SubjectTypeResponseDTO[]>(_subjectTypeRepositroy.Get());
                return Ok(subject);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateSubjectType")]
        public IActionResult UpdateSubjectType(int id, [FromBody] AddSubjectTypeDTO updateSubjectDTO)
        {
            try
            {
                updateSubjectDTO.Name = updateSubjectDTO.Name.Trim();
                var orginalSubjectType = _subjectTypeRepositroy.Find(id);
                if (orginalSubjectType == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Subject Type";
                    message.ControllerName = "Subject Type";
                    message.Message = "نوع المادة غير موجود";
                    return NotFound(message);
                }
                orginalSubjectType = _mapper.Map(updateSubjectDTO, orginalSubjectType);
                _subjectTypeRepositroy.Update(orginalSubjectType, UserName());
                _subjectTypeRepositroy.Save();
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
            return Ok(_mapper.Map<SubjectTypeResponseDTO>(_subjectTypeRepositroy.Get(c => c.IsEnabled)));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveSubjectType")]
        public IActionResult Remove(int id)
        {
            try
            {
                var sub = _subjectTypeRepositroy.Find(id);
                if (sub == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove";
                    message.ControllerName = "Subject Type";
                    message.Message = "المادة غير موجودة";
                    return NotFound(message);
                }
                _subjectTypeRepositroy.Remove(sub, UserName());
                _subjectTypeRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}