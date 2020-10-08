using System;
using System.Collections.Generic;
using System.Data;
using System.DTO.ConstraintDTO;
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
    public class ConstraintsController : MyBaseController
    {
        private readonly IRepositroy<Constraints> _constraintsRepositroy;
        public ConstraintsController(IRepositroy<Constraints> repositroy, IMapper mapper) : base( mapper)
        => _constraintsRepositroy = repositroy;



        [HttpPost]
        [Authorize(Roles = "AddConstraint")]
        public IActionResult AddConstraint([FromBody] AddConstraintDTO addConstraintDTO)
        {
            try
            {
                addConstraintDTO.Name = addConstraintDTO.Name.Trim();
                var hone = _constraintsRepositroy.Find(addConstraintDTO.HonestyId);
                if (hone == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Constraint";
                    message.ControllerName = "Constraint";
                    message.Message = "القيد غير موجود";
                    return NotFound(message);
                }
                if (string.IsNullOrWhiteSpace(addConstraintDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Constraint";
                    message.ControllerName = "Constraint";
                    return BadRequest(message);
                }
                if (_constraintsRepositroy.Get(c => c.Name == addConstraintDTO.Name&&c.HonestyId==addConstraintDTO.HonestyId).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Constraint";
                    message.ControllerName = "Constraint";
                    return Conflict(message);
                }
                var constraint = _mapper.Map<Constraints>(addConstraintDTO);
                _constraintsRepositroy.Add(constraint,UserName());
                _constraintsRepositroy.Save();

                return Ok(constraint);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet]
        [Authorize(Roles = "ShowConstraint")]
        public IActionResult ShowConstraint()
        {
            try
            {
                var con = _constraintsRepositroy.GetIQueryable().Include(c => c.Honesty);
                return Ok(_mapper.Map<ConstraintResponseDTO[]>(con));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateConstraint")]
        public IActionResult UpdateConstraint(int id, [FromBody] UpdateConstraintDTO updateConstraintDTO)
        {
            try
            {
                updateConstraintDTO.Name = updateConstraintDTO.Name.Trim();
                var simelar = _constraintsRepositroy.Get(c => c.Name == updateConstraintDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {

                    var message = Messages.Exist;
                    message.ActionName = "Update Constraint";
                    message.ControllerName = "Constraint";
                    return Conflict(message);

                }

                var OrginalConstraint =_constraintsRepositroy.Find(id);
                if (OrginalConstraint == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Constraint";
                    message.ControllerName = "Constraint";
                    message.Message = "القيد غير موجود";
                    return NotFound(message);
                }
                OrginalConstraint = _mapper.Map(updateConstraintDTO, OrginalConstraint);
                _constraintsRepositroy.Update(OrginalConstraint,UserName());
                _constraintsRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveConstraint")]
        public IActionResult RemoveConstraint(int id)
        {
            try
            {

                var con = _constraintsRepositroy.Find(id);
                if (con == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Remove Constraint";
                    message.ControllerName = "Constraint";
                    message.Message = "القيد غير موجود";
                    return NotFound(message);
                }
                _constraintsRepositroy.Remove(con,UserName());
                _constraintsRepositroy.Save();
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
                return Ok(_constraintsRepositroy.Get(c => c.IsEnabled));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}