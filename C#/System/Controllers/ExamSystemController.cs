using System;
using System.Collections.Generic;
using System.DTO.ExamSystemDTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.infrastructure;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Static;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamSystemController : MyBaseController
    {
        //add and update and delete and remove if theres no year with it 
        IRepositroy<ExamSystem> _examSystemRepositroy;
        public ExamSystemController(IRepositroy<ExamSystem> repositroy, IMapper mapper) : base(mapper)
        {
            _examSystemRepositroy = repositroy;
        }
        [HttpGet]
        public IActionResult GetExamSystem()
        {
            try
            {
                var examSystem = _mapper.Map<ResponseExamSystemDTO[]>(_examSystemRepositroy.Get(null, c => c.Years));
                return Ok(examSystem);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost]
        public IActionResult AddExamSystem([FromBody] AddExamSystemDTO addExamSystemDTO)
        {
            try
            {
                addExamSystemDTO.Name = addExamSystemDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addExamSystemDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Exam System";
                    message.ControllerName = "Exam System";
                    return BadRequest(message);
                }
                if (_examSystemRepositroy.Get(c => c.Name == addExamSystemDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Exam System";
                    message.ControllerName = "Exam System";
                    return Conflict(message);
                }
                if (addExamSystemDTO.HaveTheredSemester == true && addExamSystemDTO.GraduateStudentsSemester > 0)
                {
                    var message = new BadRequestErrors();
                    message.ActionName = "Add Exam System";
                    message.ControllerName = "Exam System";
                    message.Message = "لا يمكن وضع دورة خريجين و مع فصل ثالث";
                    return Conflict(message);
                }
                if (addExamSystemDTO.GraduateStudentsSemester != null && addExamSystemDTO.GraduateStudentsSemester < 0)
                {
                    var message = new BadRequestErrors();
                    message.ActionName = "Add Exam System";
                    message.ControllerName = "Exam System";
                    message.Message = "لا يمكن لدورة الخريجين ان تكون بالسابة";
                    return Conflict();
                }
                var examSystem = _mapper.Map<ExamSystem>(addExamSystemDTO);
                _examSystemRepositroy.Add(examSystem, UserName());
                _examSystemRepositroy.Save();

                return Ok(examSystem);
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        //[HttpPut("{id}")]
        //public IActionResult UpdateExamSystem(int id, [FromBody] UpdateExamSystemDTO updateExamSystemDTO)
        //{
        //    try
        //    {
        //        updateExamSystemDTO.Name = updateExamSystemDTO.Name.Trim();
        //        if (string.IsNullOrEmpty(updateExamSystemDTO.Name))
        //        {
        //            return BadRequest(Messages.EmptyName);
        //        }
        //        var simelar = _examSystemRepositroy.Get(c => c.Name == updateExamSystemDTO.Name && c.Id != id).FirstOrDefault();
        //        if (simelar != null)
        //            return Conflict(Messages.Exist);

        //        var examSystem = _examSystemRepositroy.Find(id);
        //        if (examSystem == null)
        //            return NotFound();
        //        examSystem = _mapper.Map(updateExamSystemDTO, examSystem);
        //        Update(examSystem);
        //        Commit();
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequestAnonymousError();
        //    }
        //}
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            try
            {
                return Ok(_examSystemRepositroy.Get(c => c.IsEnabled));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult RemoveExamSystem(int id)
        {
            try
            {
                var examSystem = _examSystemRepositroy.Get(c => c.Id == id, c => c.Years).FirstOrDefault();
                if (examSystem == null)
                    return NotFound();
                if (examSystem.Years.Count > 0)
                {
                    return Conflict();
                }
                _examSystemRepositroy.Remove(examSystem, UserName());
                _examSystemRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AddExamSystemDTO UpdateExamSystemDto)
        {
            try
            {
                var exam = _examSystemRepositroy.Get(c => c.Id == id, c => c.Years).SingleOrDefault();

                if (exam == null)
                {
                    return NotFound();
                }
                else
                {
                    if (exam.Years.Count > 0&&(exam.HaveTheredSemester!=UpdateExamSystemDto.HaveTheredSemester||exam.IsDoubleExam!=UpdateExamSystemDto.IsDoubleExam||(int)exam.GraduateStudentsSemester!=(int)UpdateExamSystemDto.GraduateStudentsSemester))
                    {
                        return Conflict();
                    }

                    exam = _mapper.Map(UpdateExamSystemDto, exam);
                    _examSystemRepositroy.Update(exam, UserName());
                    _examSystemRepositroy.Save();
                }
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}
