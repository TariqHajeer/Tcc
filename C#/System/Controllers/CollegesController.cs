using System;
using System.Collections.Generic;
using System.DTO.College;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Static;
using Microsoft.EntityFrameworkCore;
using DAL.infrastructure;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegesController : MyBaseController
    {
        private readonly IRepositroy<Colleges> _collegesRepositroy;
        public CollegesController(IRepositroy<Colleges> repositroy, IMapper mapper) : base(mapper)
        => _collegesRepositroy = repositroy;
        [HttpPost]
        // [Authorize(Roles = "AddCollege")]
        public IActionResult AddCollege([FromBody] AddCollegeDTO addCollegeDTO)
        {
            try
            {
                addCollegeDTO.Name = addCollegeDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addCollegeDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add College";
                    message.ControllerName = "Colleges";
                    return BadRequest(message);
                }
                if (_collegesRepositroy.Get(c => c.Name == addCollegeDTO.Name&&c.ProvinceId==addCollegeDTO.ProvinceId).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add College";
                    message.ControllerName = "Colleges";

                    return Conflict(message);
                }
                var Colle = _mapper.Map<Colleges>(addCollegeDTO);
                _collegesRepositroy.Add(Colle, UserName());
                _collegesRepositroy.Save();
                return Ok(Colle);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

        [Authorize(Roles = "ShowColleges")]
        [HttpGet]
        public IActionResult ShowColleges()
        {
            try
            {
                var col = _collegesRepositroy.GetIQueryable().Include(c => c.Province).ToList();
                return Ok(_mapper.Map<CollegesResponseDTO[]>(col));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("Enabled")]
        public IActionResult GetEnabledCollage()
        {
            try
            {
                var col = _collegesRepositroy.
                    GetIQueryable(c => c.IsEnabled == true).Include(c => c.Province);

                return Ok(_mapper.Map<CollegesResponseDTO[]>(col));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [Authorize(Roles = "ShowColleges")]
        [HttpGet("{id}")]
        public IActionResult ShowCollegeById(int id)
        {
            try
            {
                Colleges col = _collegesRepositroy.Find(id);
                if (col == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Show College By Id";
                    message.ControllerName = "Colleges";
                    message.Message = "المعهد غير موجود";
                    return NotFound(message);
                }
                return Ok(col);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateCollege")]
        public IActionResult UpdateCollege(int id, [FromBody]UpdateCollegeDTO updateCollegeDTO)
        {
            try
            {
                updateCollegeDTO.Name = updateCollegeDTO.Name.Trim();
                var simelar = _collegesRepositroy.Get(c => c.Name == updateCollegeDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {

                    var message = Messages.Exist;
                    message.ActionName = "Update College";
                    message.ControllerName = "Colleges";
                    return Conflict(message);
                }
                var orginalCollege = _collegesRepositroy.Find(id);
                if (orginalCollege == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "update College";
                    message.ControllerName = "Colleges";
                    message.Message = "المعهد غير موجود";
                    return NotFound(message);
                }

                _mapper.Map(updateCollegeDTO, orginalCollege);
                _collegesRepositroy.Update(orginalCollege,UserName());
                _collegesRepositroy.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveCollege")]
        public IActionResult RemoveCollege(int id)
        {
            try
            {
                var col = _collegesRepositroy.Find(id);
                if (col == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Remove College";
                    message.ControllerName = "Colleges";
                    message.Message = "المعهد غير موجود";
                    return NotFound(message);
                }
                _collegesRepositroy.Remove(col, UserName());
                _collegesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

    }
}