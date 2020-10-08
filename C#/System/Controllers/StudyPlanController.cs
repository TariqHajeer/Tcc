using System;
using System.Collections.Generic;
using System.DTO;
using System.DTO.StudyPlanDTO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Static;
using System.Services;
using System.IServices;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyPlanController : MyBaseController
    {
        ISubjectServices _subjectServices;
        IRepositroy<StudyPlan> _StudyPlanRepositroy;
        IRepositroy<Years> _yearRepositroy;
        IRepositroy<Specializations> _specializationsRepositroy;
        IRepositroy<StudyYear> _studyYearRepositroy;
        IRepositroy<Subjects> _subjectRepositroy;
        IRepositroy<SubjectType> _subjectTypeRepositroy;
        AbstractUnitOfWork _abstractUnitOfWork;
        public StudyPlanController(
            IRepositroy<StudyPlan> repositroy,
            IRepositroy<Years> yearRepositroy,
            IRepositroy<Specializations> specializationsRepositroy,
            IRepositroy<StudyYear> studyYearRepositroy,
            IRepositroy<Subjects> subjectRepositroy,
            IRepositroy<SubjectType> subjectTypeRepositroy,
            AbstractUnitOfWork abstractUnitOfWork,
             IMapper mapper, ISubjectServices subjectServices) : base(mapper)
        {
            _subjectServices = subjectServices;
            _StudyPlanRepositroy = repositroy;
            _yearRepositroy = yearRepositroy;
            _specializationsRepositroy = specializationsRepositroy;
            _studyYearRepositroy = studyYearRepositroy;
            _subjectTypeRepositroy = subjectTypeRepositroy;
            _subjectRepositroy = subjectRepositroy;
            _abstractUnitOfWork = abstractUnitOfWork;
        }

        [HttpPost]
        //[Authorize(Roles = "AddStudyPlan")]
        public IActionResult Add([FromBody] AddStudyPalnDTO addStudyPalnDTO)
        {
            try
            {
                var year = _yearRepositroy.Find(addStudyPalnDTO.YearId);
                if (year == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Study Plan";
                    message.ControllerName = "Study Plan";
                    message.Message = "السنة غير موجودة";
                    return NotFound(message);

                }
                var specialization = _specializationsRepositroy.Find(addStudyPalnDTO.SpecializationId);
                if (specialization == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Study Plan";
                    message.ControllerName = "Study Plan";
                    message.Message = "الاختصاص غير موجود";
                    return NotFound(message);
                }

                #region check temporary Id 
                var tempId = addStudyPalnDTO.Subjects.Select(c => c.AddSubjectDTO.TempId).ToArray();
                if (tempId.Contains(0))
                {
                    var message = new BadRequestErrors();
                    message.ActionName = "Update";
                    message.ControllerName = "YearSystem";
                    message.Message = "الرجاء إعادة تحميل الصفحة";
                    return Conflict(message);
                }
                if (tempId.Count() != tempId.Distinct().Count())
                {
                    var message = new BadRequestErrors();
                    message.ActionName = "Update";
                    message.ControllerName = "YearSystem";
                    message.Message = "الرجاء إعادة تحميل الصفحة";
                    return Conflict(message);
                }
                #endregion


                #region check subject
                //but it should replace after asking the Eng
                //check subject 
                var studyYear = _studyYearRepositroy.Get().ToList();
                var subjectsTypesId = _subjectTypeRepositroy.Get().Select(c => c.Id);
                var subjectsId = _subjectRepositroy.Get().Select(c => c.Id);
                foreach (var item in addStudyPalnDTO.Subjects)
                {
                    //there's not thered study semester
                    if (item.StudySemesterNumber > 2)
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Study Plan";
                        message.ControllerName = "Study Plan";
                        message.Message = "ليست 3 فصول تدريسية";
                        return Conflict(message);
                    }
                    //chekc if all the year is correct 
                    //example
                    //this subject is for thered year and in our collage don't have any thered year yet 
                    //so it return an confliect
                    if (studyYear.Select(c => c.Id).ToList().IndexOf(item.StudyYearId) < 0)
                    {
                        var message = new BadRequestErrors();
                        message.ActionName = "Add Study Plan";
                        message.ControllerName = "Study Plan";
                        message.Message = "السنة الدراسية غير موجودة";
                        return Conflict(message);
                    }

                    //check Type if exist
                    if (!subjectsTypesId.Contains(item.AddSubjectDTO.SubjectTypeId))
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Study Plan";
                        message.ControllerName = "Study Plan";
                        message.Message = "نوع المادة غير موجود";

                        return NotFound(message);
                    }
                    var subject = item.AddSubjectDTO;
                    //befor tempId
                    //if (subject.DependencySubjectsId.Except(subjectsId).Any())
                    //{
                    //    return Conflict();
                    //}
                    subject.Name = subject.Name.Trim();
                    if (string.IsNullOrWhiteSpace(subject.Name))
                    {
                        var message = Messages.EmptyName;
                        message.ActionName = "Add Study Plan";
                        message.ControllerName = "Study Plan";
                        return Conflict(message);
                    }
                    if (subject.DependencySubjectsId.Except(tempId).Any())
                    {
                        var message = Messages.ReLoadPage;
                        message.ActionName = "Add Study Plan";
                        message.ControllerName = "Study Plan";
                        return Conflict(message);
                    }
                    if (subject.EquivalentSubjectsId.Except(subjectsId).Any())
                    {
                        var message = Messages.ReLoadPage;
                        message.ActionName = "Add Study Plan";
                        message.ControllerName = "Study Plan";
                        return Conflict(message);
                    }
                    subject.DependencySubjectsId = subject.DependencySubjectsId.Distinct().ToList();
                    subject.EquivalentSubjectsId = subject.EquivalentSubjectsId.Distinct().ToList();
                }
                #endregion

                var studyPaln = _mapper.Map<StudyPlan>(addStudyPalnDTO);
                _abstractUnitOfWork.Add(studyPaln, UserName());
                Dictionary<int, Subjects> tempIdAndSubject = new Dictionary<int, Subjects>();
                for (int i = 1; i <= studyYear.Count(); i++)
                {
                    //get the subject for this year mean first year and secound year ..etc 
                    var subjectsForYear = addStudyPalnDTO.Subjects.Where(c => c.StudyYearId == studyYear[i - 1].Id);
                    //for adding in semester
                    //semester number because we have 2 semester in the year
                    for (int semesterNumber = 1; semesterNumber <= 2; semesterNumber++)
                    {
                        var semester = new StudySemester()
                        {
                            Number = (short)semesterNumber,
                            StudyYearId = studyYear[i - 1].Id,
                            StudyplanId = studyPaln.Id,
                        };
                        _abstractUnitOfWork.Add(semester, UserName());
                        //get subject for the semester 
                        var subjectsForSemester = subjectsForYear.Where(c => c.StudySemesterNumber == semesterNumber);
                        #region explain
                        //the output is like subject for first yaer and semester 1 in the first looping .
                        //exmple of output for Specialization Softwere engineering.
                        // programing 1 , electorinec , math1 , operaing system 1 
                        //in the secound loop for the inner loop 
                        //out put example
                        //programing 2 , math2 ,e2 , 
                        //in the seocund loop for the outer loop and first loop for inner loop 
                        // advance programing 1 , oracle 1 ,operaing system 2
                        // this example depend on input 


                        //for git subject without any extande proraty
                        #endregion
                        var finalSubjectAfterFilering = subjectsForSemester.Select(c => c.AddSubjectDTO).ToList();
                        finalSubjectAfterFilering.ForEach(c => c.StudySemesterId = semester.Id);
                        foreach (var subjectDTO in finalSubjectAfterFilering)
                        {
                            var subject = _mapper.Map<Subjects>(subjectDTO);
                            subject.StudySemester = semester;
                            _abstractUnitOfWork.Add(subject, UserName());
                            //foreach (var DependencySubjectId in subjectDTO.DependencySubjectsId)
                            //{
                            //    _abstractUnitOfWork.Repository<DependenceSubject>()
                            //        .Add(new DependenceSubject()
                            //        {
                            //            SubjectId = subject.Id,
                            //            DependsOnSubjectId = DependencySubjectId
                            //        }, UserName());
                            //}
                            tempIdAndSubject[subjectDTO.TempId] = subject;
                            foreach (var EquivalentSubjectId in subjectDTO.EquivalentSubjectsId)
                            {
                                _abstractUnitOfWork.Add(new EquivalentSubject()
                                {
                                    FirstSubject = subject.Id,
                                    SecoundSubject = EquivalentSubjectId
                                }, UserName());
                            }
                        }
                    }
                }

                foreach (var subjectDTO in addStudyPalnDTO.Subjects.Select(c => c.AddSubjectDTO))
                {
                    var subject = tempIdAndSubject[subjectDTO.TempId];
                    foreach (var DependancySubjectId in subjectDTO.DependencySubjectsId)
                    {
                        var depenacySubjct = tempIdAndSubject[DependancySubjectId];

                        // if this subject can't depand on another subject
                        if (!_subjectServices.CheckCanBeDepandacy(subject, depenacySubjct))
                        {
                            var message = new BadRequestErrors();
                            message.ActionName = "Add Study Plan";
                            message.ControllerName = "Study Plan";
                            message.Message = "خطأ في تحديد اعتمادية المواد";
                            return Conflict();
                        }
                        _abstractUnitOfWork.Add(
                            new DependenceSubject()
                            {
                                SubjectId = subject.Id,
                                DependsOnSubjectId = depenacySubjct.Id
                            }, UserName());
                    }
                }
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("getbySpecialization")]
        //[Authorize(Roles = "ShowStudyPlan")]
        public IActionResult GetBySpecialization([FromQuery] string specializationId)
        {
            try
            {
                var specialization = _specializationsRepositroy.Find(specializationId);
                if (specialization == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Get By Specialization";
                    message.ControllerName = "Study Plan";
                    message.Message = "الاختصاص غير موجود";
                    return NotFound(message);
                }
                var studyPlans = _StudyPlanRepositroy.GetIQueryable()
                    .Where(sp => sp.SpecializationId == specializationId)
                    .Include(sp => sp.Year)
                    .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.StudyYear)
                    .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                         .ThenInclude(d => d.DependenceSubjectDependsOnSubject)
                          .ThenInclude(s => s.Subject)
                     .Include(sp => sp.StudySemester)
                       .ThenInclude(ss => ss.Subjects)
                         .ThenInclude(d => d.DependenceSubjectSubject)
                          .ThenInclude(s => s.DependsOnSubject)
                     .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                            .ThenInclude(s => s.SubjectType);

                var studyPantMapped = _mapper.Map<StudyPlanResponse[]>(studyPlans);
                return Ok(studyPantMapped);
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("getStudyPlan")]
        //[Authorize(Roles = "ShowStudyPlan")]
        public IActionResult GetBySpecialization([FromQuery] string specializationId, [FromQuery] int yearId)
        {
            try
            {
                var specialization = _specializationsRepositroy.Find(specializationId);
                if (specialization == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Get By Specialization";
                    message.ControllerName = "Study Plan";
                    message.Message = "الاختصاص غير موجود";
                    return NotFound(message);
                }
                var studyPlan = _StudyPlanRepositroy.GetIQueryable()
                    .Where(sp => sp.SpecializationId == specializationId && sp.YearId == yearId)
                    .Include(sp => sp.Year)
                    .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.StudyYear)
                    .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                         .ThenInclude(d => d.DependenceSubjectDependsOnSubject)
                          .ThenInclude(s => s.Subject)
                     .Include(sp => sp.StudySemester)
                       .ThenInclude(ss => ss.Subjects)
                         .ThenInclude(d => d.DependenceSubjectSubject)
                          .ThenInclude(s => s.DependsOnSubject)
                     .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                            .ThenInclude(s => s.SubjectType)
                #region المواد المكافئة
                #region اول مادة مكافئة
                     .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                        .ThenInclude(e => e.EquivalentSubjectFirstSubjectNavigation)
                         .ThenInclude(es => es.SecoundSubjectNavigation)
                            .ThenInclude(es => es.SubjectType)
                      .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                        .ThenInclude(e => e.EquivalentSubjectFirstSubjectNavigation)
                         .ThenInclude(es => es.SecoundSubjectNavigation)
                            .ThenInclude(es => es.StudySemester)
                            .ThenInclude(esss => esss.StudyYear)
                     .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                            .ThenInclude(e => e.EquivalentSubjectFirstSubjectNavigation)
                                .ThenInclude(es => es.SecoundSubjectNavigation)
                                     .ThenInclude(es => es.StudySemester)
                                        .ThenInclude(essp => essp.Studyplan)
                                            .ThenInclude(esspsp => esspsp.Specialization)
                #endregion
                #region تاني مادة مكافئة
                .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                        .ThenInclude(e => e.EquivalentSubjectSecoundSubjectNavigation)
                         .ThenInclude(es => es.FirstSubjectNavigation)
                            .ThenInclude(es => es.SubjectType)
                      .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                        .ThenInclude(e => e.EquivalentSubjectSecoundSubjectNavigation)
                         .ThenInclude(es => es.FirstSubjectNavigation)
                            .ThenInclude(es => es.StudySemester)
                            .ThenInclude(esss => esss.StudyYear)
                     .Include(sp => sp.StudySemester)
                        .ThenInclude(ss => ss.Subjects)
                            .ThenInclude(e => e.EquivalentSubjectSecoundSubjectNavigation)
                                .ThenInclude(es => es.FirstSubjectNavigation)
                                     .ThenInclude(es => es.StudySemester)
                                        .ThenInclude(essp => essp.Studyplan)
                                            .ThenInclude(esspsp => esspsp.Specialization)
                #endregion
                #endregion
                        .FirstOrDefault();
                var studyPantMapped = _mapper.Map<StudyPlanResponse>(studyPlan);
                return Ok(studyPantMapped);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return BadRequestAnonymousError();
            }
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] int studyPlanId)
        {
            try
            {
                var studyPlan = _studyYearRepositroy.GetIQueryable(c => c.Id == studyPlanId)
                .Include(c => c.StudySemester)
                .ThenInclude(c => c.Subjects)
                .ThenInclude(c => c.StudentSubject)
                .SingleOrDefault();
                if (studyPlan == null)
                {
                    return NotFound();
                }
                if (studyPlan.StudySemester.SelectMany(c => c.Subjects).SelectMany(c => c.StudentSubject).Any())
                {
                    return Conflict();
                }
                _studyYearRepositroy.Remove(studyPlan, UserName());

                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        
    }
}

