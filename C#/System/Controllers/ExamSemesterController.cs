using System;
using System.Collections.Generic;
using System.DTO.ExamSemesterDTO;
using System.DTO.SubjectDTO;
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
    public class ExamSemesterController : MyBaseController
    {
        IRepositroy<ExamSemester> _examSemesterRepositroy;
        IRepositroy<Years> _yearRepositroy;
        public ExamSemesterController(IRepositroy<ExamSemester> repositroy, IRepositroy<Years> yearrepositroy, IMapper mapper) : base(mapper)
        {
            _examSemesterRepositroy = repositroy;
            _yearRepositroy = yearrepositroy;

        }

        [HttpGet("GetAvilableExamSemester/{specializationId}/{yearId:int}/{studyYearId:int}")]
        public IActionResult GetAvilabelStudySemesterByYearAndStudyYear(string specializationId, int yearId, int studyYearId)
        {
            var year = _yearRepositroy.GetIQueryable(c => c.Id == yearId)
                 .Include(c => c.ExamSemester)
                 .ThenInclude(c => c.StudentSubject)
                 .ThenInclude(c => c.Subject)
                 .ThenInclude(c => c.StudySemester)
                 .ThenInclude(c => c.Studyplan)
                 .FirstOrDefault();
            if (year.Blocked)
            {
                var message = Messages.Blocked;
                message.Message = "السنة مقفولة";
                message.ActionName = "Get Avilabel Study Semester By Year And StudyYear";
                message.ControllerName = "Exam Semeter";
                return Conflict(message);
            }
            //var examSemesters = year.ExamSemester.Where(ss => ss.StudentSubject.Where(c => c.Subject.StudySemester.Studyplan.SpecializationId == specializationId && c.Subject.StudySemester.StudyYearId == studyYearId).Count() > 0).ToList();
            var examSemesters = year.ExamSemester.ToList();
            examSemesters.Sort((s1, s2) => s1.SemesterNumber.CompareTo(s2.SemesterNumber));

            foreach (var item in examSemesters)
            {
                if (item.SemesterNumber > 1)
                {
                    if (!year.IsThisSemesterFinshed(item.SemesterNumber - 1))
                    {
                        var message = new BadRequestErrors()
                        {
                            ActionName = "GetAvilabelStudySemesterByYearAndStudyYear",
                            ControllerName = "ExamSemester",
                            Message = "يجب انهاء الفصل السابق",
                        };
                        return Conflict(message);
                    }
                }
                if (item.StudentSubject.Any(c => c.PracticalDegree == null || c.TheoreticlaDegree == null))
                {
                    item.StudentSubject = item.StudentSubject.Where(c => c.TheoreticlaDegree == null).ToList();
                    item.StudentSubject = item.StudentSubject.Where(c => c.Subject.StudySemester.StudyYearId == studyYearId && c.Subject.StudySemester.Studyplan.SpecializationId == specializationId).ToList();
                    var subjects = item.StudentSubject.GroupBy(c => c.Subject).Select(c => c.First().Subject).ToList();
                    var repsonseExamSemester = _mapper.Map<ResponseExamSemesterDTO>(item);
                    repsonseExamSemester.Subjects = _mapper.Map<ResponseSubjectDTO[]>(subjects).ToList();
                    return Ok(repsonseExamSemester);
                }
            }
            return Ok(new ResponseExamSemesterDTO[0]);
        }
    }
}