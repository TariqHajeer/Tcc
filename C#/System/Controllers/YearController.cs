using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.DTO.YearDTO;
using Static;
using Microsoft.AspNetCore.Authorization;
using DAL.HelperEums;
using DAL.infrastructure;
using System.DTO.YearSystemDTO;
using System.IServices;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YearController : MyBaseController
    {
        IRepositroy<Years> _yearRepositroy;
        IRepositroy<StudyPlan> _studyPlanRepositroy;
        IRepositroy<YearSystem> _yearSystemRepositroy;
        IRepositroy<ExamSystem> _examSystemRepositroy;
        IStudentService _studentServic;
        AbstractUnitOfWork _abstractUnitOfWork;
        public YearController(IRepositroy<Years> repositroy,
            IRepositroy<StudyPlan> studyPlanRepositroy,
            IRepositroy<YearSystem> yearSystemRepositroy,
            IRepositroy<ExamSystem> examSystemRepositroy,
            IStudentService studentService,
            AbstractUnitOfWork abstractUnitOfWork,

            IMapper mapper) : base(mapper)
        {
            _studentServic = studentService;
            _yearRepositroy = repositroy;
            _studyPlanRepositroy = studyPlanRepositroy;
            _yearSystemRepositroy = yearSystemRepositroy;
            _examSystemRepositroy = examSystemRepositroy;
            _abstractUnitOfWork = abstractUnitOfWork;
        }
        [HttpGet("GetYearBySpecialization")]
        //[Authorize(Roles = "ShowYears")]
        public IActionResult GetYearBySpecialization([FromQuery] string SpecializationId)
        {
            var years = _studyPlanRepositroy.GetIQueryable(c => c.SpecializationId == SpecializationId)
              .Include(c => c.Year).Select(c => c.Year).ToList();
            var responseYear = _mapper.Map<YearResponseDTO[]>(years);
            return Ok(responseYear);
        }
        [HttpGet("GetNonBlockedYear")]
        //[Authorize(Roles = "ShowYears")]
        public IActionResult GetNonBlockedYear()
        {
            var years = _yearRepositroy.Get(c => c.Blocked == false).ToList();
            var responseYear = _mapper.Map<YearResponseDTO[]>(years);
            return Ok(responseYear);
        }

        [HttpGet]
        //[Authorize(Roles = "ShowYears")]
        public IActionResult Show()
        {
            try
            {

                var year = _yearRepositroy.GetIQueryable()
                    .Include(c => c.ExamSystemNavigation)
                    .Include(c => c.YearSystemNavigation).OrderBy(c => c.FirstYear).ToList();

                return Ok(_mapper.Map<YearResponseDTO[]>(year));
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return BadRequestAnonymousError();
            }
        }

        [HttpPost]
        //[Authorize(Roles = "AddYear")]
        public IActionResult Add(AddYearDTO addYearDTO)
        {
            try
            {
                if (addYearDTO.FirstYear + 1 != addYearDTO.SecondYear)
                {
                    var message = Messages.CannotAdd;
                    message.ActionName = "Add Year";
                    message.ControllerName = "Year";
                    message.Message = "العام غير صالح";
                    return Conflict(message);
                }
                if (addYearDTO.FirstYear < 1998)
                {
                    var message = Messages.CannotAdd;
                    message.ActionName = "Add Year";
                    message.ControllerName = "Year";
                    message.Message = "لا يجب ان يكون العام قبل تاريخ 1998 ";
                    return Conflict(message);
                }
                {
                    var prvoiusYear = _yearRepositroy.Get(c => c.FirstYear < addYearDTO.FirstYear).OrderByDescending(c => c.FirstYear).FirstOrDefault();

                    if (prvoiusYear != null && !prvoiusYear.Blocked)
                    {
                        var message = Messages.CannotAdd;
                        message.ActionName = "Add Year";
                        message.ControllerName = "Year";
                        message.Message = "يجب قفل السنة السابقة";
                        return Conflict(message);
                    }

                    if (prvoiusYear == null)
                    {
                        if (_yearRepositroy.Get().Any())
                        {
                            var message = Messages.CannotAdd;
                            message.ActionName = "Add Year";
                            message.ControllerName = "Year";
                            message.Message = "لا يمكن إضافة سنة قديمة";
                            return Conflict(message);
                        }
                    }
                    else
                    {
                        if (prvoiusYear.SecondYear != addYearDTO.FirstYear)
                        {
                            var message = Messages.CannotAdd;
                            message.ActionName = "Add Year";
                            message.ControllerName = "Year";
                            message.Message = "خطأ بإدخال السنة ";
                            return Conflict(message);
                        }
                    }
                }
                var yearSystem = _yearSystemRepositroy.GetIQueryable(c => c.Id == addYearDTO.YearSystem)
                    .Include(c => c.SettingYearSystem)
                    .FirstOrDefault();
                if (yearSystem == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Year";
                    message.ControllerName = "Year";
                    message.Message = "نظام السنة غير موجود";
                    return NotFound(message);
                }
                var examSystem = _examSystemRepositroy.Find(addYearDTO.ExamSystem);
                if (examSystem == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Year";
                    message.ControllerName = "Year";
                    message.Message = "النظام الامتحاني غير موجود";
                    return NotFound(message);
                }
                var years = _yearRepositroy.Get(c => c.FirstYear == addYearDTO.FirstYear || c.SecondYear == addYearDTO.SecondYear).ToList();
                if (years == null || years.Count() > 0)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Year";
                    message.ControllerName = "Year";
                    message.Message = "يوجد عام مشابه";
                    return Conflict(message);
                }
                var newYear = _mapper.Map<Years>(addYearDTO);
                _abstractUnitOfWork.Add(newYear, UserName());
                int examNumber = 2;

                if (examSystem.HaveTheredSemester || (examSystem.GraduateStudentsSemester.HasValue && (int)examSystem.GraduateStudentsSemester > 0))
                {
                    examNumber++;
                }
                for (int i = 1; i <= examNumber; i++)
                {
                    _abstractUnitOfWork.Add(new ExamSemester
                    {
                        YearId = newYear.Id,
                        SemesterNumber = i,

                    }, UserName());
                }
                _abstractUnitOfWork.Commit();
                return Ok(_mapper.Map<YearResponseDTO>(newYear));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetYearCanAddStudyPaln")]
        public IActionResult GetYearCanAddStudyPaln()
        {
            var year = _yearRepositroy.GetIQueryable(c => c.ExamSemester.SelectMany(k => k.StudentSubject).Count() == 0).FirstOrDefault();
            return Ok(_mapper.Map<YearResponseDTO>(year));

        }

        //[Authorize(Roles = "ShowYears")]
        [HttpGet("yearWithoutStudyPlan")]
        public IActionResult YearWtihoutStudyPlan([FromQuery] string specializationId)
        {
            try
            {
                var years = _yearRepositroy.GetIQueryable(c => !c.StudyPlan.Where(sp => sp.SpecializationId == specializationId).Any() && c.Blocked == false)
                    .Include(c => c.ExamSystemNavigation)
                    .Include(c => c.YearSystemNavigation);
                return Ok(_mapper.Map<YearResponseDTO[]>(years));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetSettingsByFirstYear/{firstYear}")]
        public IActionResult GetByFirstYear(int firstYear)
        {
            try
            {
                var year = _yearRepositroy.GetIQueryable(c => c.FirstYear == firstYear)
                .Include(c => c.YearSystemNavigation)
                .ThenInclude(s => s.SettingYearSystem)
                .ThenInclude(ss => ss.Setting)
                .OrderBy(c => c.FirstYear)
                .LastOrDefault();
                if (year == null)
                {
                    //مافي سنة قبل
                    var message = Messages.NotFound;
                    message.ActionName = "Student Previous Year Setting";
                    message.ControllerName = "Student";
                    message.Message = "لايوجد سنة سابقة لتايخ تسجيل هذا الطالب ";
                    return NotFound(message);
                }
                var yearSystem = year.YearSystemNavigation;
                return Ok(_mapper.Map<ResponseYearSystem>(yearSystem));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("StudentDontRegister/{id}")]
        public IActionResult StudnetDontRegister(int id)
        {
            var cureentYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == id)
                   .Include(c => c.Registrations)
                   .FirstOrDefault();
            var privousYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.SecondYear == cureentYear.FirstYear)
                .Include(c => c.Registrations).FirstOrDefault();
            if (privousYear != null)
            {
                var studentRegistarInprivousYear = privousYear.Registrations.Where(c => c.FinalStateId != (int)StudentStateEnum.Graduated || c.FinalStateId == (int)StudentStateEnum.Drained).Select(c => c.Ssn);
                var studentRegistarInThisYear = cureentYear.Registrations.Select(c => c.Ssn).ToList();
                var studentDontRegister = studentRegistarInprivousYear.Except(studentRegistarInThisYear).ToList();
                return Ok(new { count = studentDontRegister.Count });
            }
            return Ok(new { count = 0 });

        }
        [HttpGet("StudentDontTakeExam/{id}")]
        public IActionResult StudentDontTakeExam(int id)
        {
            var cureentYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == id)
                   .Include(c => c.ExamSemester)
                   .ThenInclude(c => c.StudentSubject)
                   .FirstOrDefault();
            var studentSubjects = cureentYear.ExamSemester.SelectMany(c => c.StudentSubject.Where(ss => ss.PracticalDegree == null || ss.TheoreticlaDegree == null)).ToList();
            var ssns = studentSubjects.Select(c => c.Ssn).Distinct();
            return Ok(new { count = ssns });
        }
        [HttpPut("{id}")]
        public IActionResult BlockYear(int id)
        {
            var cureentYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == id)
                .Include(c => c.Registrations)
                .Include(c => c.ExamSemester)
                .ThenInclude(c => c.StudentSubject)
                .FirstOrDefault();
            if (cureentYear == null)
            {
                return NotFound();
            }
            var nullStudentSubjects = cureentYear.ExamSemester.SelectMany(c => c.StudentSubject.Where(ss => ss.PracticalDegree == null || ss.TheoreticlaDegree == null)).ToList();
            if (nullStudentSubjects.Count > 0)
            {
                return Conflict();
            }
            //processAllStudent
            var studentsSSn = nullStudentSubjects.Select(c => c.Ssn).Distinct().ToList();
            studentsSSn.ForEach(s =>
            _studentServic.ProcessStudentState(s));
            var privousYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.SecondYear == cureentYear.FirstYear)
                .Include(c => c.Registrations).FirstOrDefault();
            if (privousYear != null)
            {
                var studentRegistarInprivousYear = privousYear.Registrations.Where(c => c.FinalStateId != (int)StudentStateEnum.Graduated || c.FinalStateId == (int)StudentStateEnum.Drained).Select(c => c.Ssn).ToList();
                var studentRegistarInThisYear = cureentYear.Registrations.Select(c => c.Ssn).ToList();
                var studentDontRegister = studentRegistarInprivousYear.Except(studentRegistarInThisYear).ToList();
                foreach (var item in studentDontRegister)
                {
                    var student = _abstractUnitOfWork.Repository<Students>().Get(c => c.Ssn == item, c => c.Registrations).Single();
                    var newRegistration = student.NewRegistration();
                    newRegistration.SetFaild();
                    newRegistration.YearId = id;
                    _abstractUnitOfWork.Add(newRegistration, SentencesHelper.System);
                }
            }
            cureentYear.Blocked = true;
            _abstractUnitOfWork.Update(cureentYear, UserName());
            _abstractUnitOfWork.Commit();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == id)
            .Include(c => c.Registrations)
            .ThenInclude(c => c.SsnNavigation)
            .FirstOrDefault();
            if (year == null)
                return NotFound();
            if (_abstractUnitOfWork.Repository<Years>().Get(c => c.FirstYear > year.FirstYear).Any())
            {
                var message = Messages.NotFound;
                message.Message = "لا يمكن حذف سنة و يوجد سنة بعدها ";
                message.ActionName = "Delete";
                message.ControllerName = "Year";
                return Conflict(message);
            }
            return Ok();
        }
        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateYearDTO UpdateYear)
        {
            if (UpdateYear == null)
            {
                return Conflict();
            }
            var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == id)
            .Include(c => c.ExamSystemNavigation)
            .Include(c => c.YearSystemNavigation)
            .ThenInclude(c => c.SettingYearSystem)
            .Include(c => c.ExamSemester)
            .ThenInclude(c => c.StudentSubject)
            .ThenInclude(c => c.Subject)
            .ThenInclude(c => c.SubjectType)
            .Include(c => c.Registrations)
            .SingleOrDefault();
            if (year == null)
            {
                return NotFound();
            }

            if (year.Blocked)
            {
                return Conflict();
            }
            var yearSystem = _yearSystemRepositroy.Get(c => c.Id == UpdateYear.YearSystem, c => c.SettingYearSystem).SingleOrDefault();
            if (yearSystem == null)
                return NotFound();
            var examSystem = _examSystemRepositroy.Find(UpdateYear.ExamSystem);
            if (examSystem == null)
                return NotFound();
            if (year.ExamSystem == UpdateYear.ExamSystem && year.YearSystem == UpdateYear.YearSystem)
                return Ok();
            //reset all help Degree
            var studentSubjects = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => year.ExamSemester.Select(es => es.Id).Contains((int)c.ExamSemesterId) && c.HelpDegree == true).ToList();
            studentSubjects.ForEach(subject =>
            {
                subject.HelpDegree = false;
                _abstractUnitOfWork.Update(subject, SentencesHelper.System);
            });
            if (year.ExamSystem != UpdateYear.ExamSystem)
            {
                //delete thered semester
                var theredSemester = _abstractUnitOfWork.Repository<ExamSemester>().GetIQueryable(c => c.YearId == year.Id && c.SemesterNumber == 3).SingleOrDefault();
                if (theredSemester != null)
                {
                    _abstractUnitOfWork.Remove(theredSemester, SentencesHelper.System);
                }
                if (year.IsDoubleExam && !examSystem.IsDoubleExam)
                {
                    var studentSubjectInThisYear = _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => c.ExamSemesterId != null && year.ExamSemester.Select(es => es.Id).Contains((int)c.ExamSemesterId))
                    .Include(s => s.Subject)
                    .ThenInclude(s => s.StudySemester)
                    .ToList();
                    var subjectInSeocunSemeseter = studentSubjectInThisYear.Where(su => su.ExamSemester.SemesterNumber == (int)StudySemesterNumberEunm.Second && su.MainSemesterNumber == (int)StudySemesterNumberEunm.First).ToList();
                    subjectInSeocunSemeseter.ForEach(subject =>
                    {
                        _abstractUnitOfWork.Remove(subject, SentencesHelper.System);
                    });
                    var subjectInFirstSemestetr = studentSubjectInThisYear.Where(su => su.ExamSemester.SemesterNumber == (int)StudySemesterNumberEunm.First && su.MainSemesterNumber == (int)StudySemesterNumberEunm.Second).ToList();
                    var secoundSemester = year.ExamSemester.Where(c => c.SemesterNumber == (int)StudySemesterNumberEunm.Second).Single();
                    subjectInFirstSemestetr.ForEach(subject =>
                    {
                        if (secoundSemester.StudentSubject.Where(c => c.Ssn == subject.Ssn && c.SubjectId == subject.Id).FirstOrDefault() == null)
                        {
                            var newStudnetSubject = new StudentSubject()
                            {
                                Ssn = subject.Ssn,
                                SubjectId = subject.Id,
                                PracticalDegree = subject.PracticalDegree,
                                ExamSemesterId = secoundSemester.Id,
                            };
                            _abstractUnitOfWork.Add(newStudnetSubject, SentencesHelper.System);
                        }
                        _abstractUnitOfWork.Remove(subject, SentencesHelper.System);
                    });
                }
                else if (!year.IsDoubleExam && examSystem.IsDoubleExam)
                {
                    //first to sec 
                    {
                        var subjectInFirstSemseter = year.ExamSemester.Where(c => c.SemesterNumber == (int)StudySemesterNumberEunm.First).Single().StudentSubject.Where(c => c.TheoreticlaDegree != null && c.IsNominate() && !c.IsSuccess()).ToList();
                        var secExamSemesterId = year.ExamSemester.Where(c => c.SemesterNumber == (int)StudySemesterNumberEunm.Second).Single().Id;
                        subjectInFirstSemseter.ForEach(subject =>
                        {
                            var newSubject = new StudentSubject()
                            {
                                PracticalDegree = subject.PracticalDegree,
                                Ssn = subject.Ssn,
                                SubjectId = subject.Id,
                                ExamSemesterId = secExamSemesterId,
                            };
                            _abstractUnitOfWork.Add(newSubject, SentencesHelper.System);
                        });
                    }

                    {
                        var StudnetsNotNewStudnetSSn = year.Registrations.Where(c => c.StudentStateId != (int)StudentStateEnum.newStudent).Select(c => c.Ssn).ToList();
                        var studnets = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => year.Registrations.Select(r => r.Ssn).ToList().Contains(c.Ssn))
                        .Include(s => s.StudentSubject)
                        .ThenInclude(ss => ss.ExamSemester)
                        .ToList();
                        var firstSemesertId = year.ExamSemester.Where(c => c.SemesterNumber == (int)StudySemesterNumberEunm.First).Single().Id;
                        var secSemesterId = year.ExamSemester.Where(c => c.SemesterNumber == (int)StudySemesterNumberEunm.Second).Single().Id;
                        studnets.ForEach(student =>
                        {
                            var subjectNotIThinsYear = student.StudentSubject.Where(c => c.ExamSemester.YearId != year.Id).ToList();
                            subjectNotIThinsYear = subjectNotIThinsYear.OrderBy(c => c.Id).GroupBy(c => c.SubjectId).Select(c => c.Last()).ToList();
                            subjectNotIThinsYear = subjectNotIThinsYear.Where(c => !c.IsSuccess()).ToList();
                            subjectNotIThinsYear = subjectNotIThinsYear.Where(c => c.IsNominate()).ToList();
                            subjectNotIThinsYear = subjectNotIThinsYear.Where(c => c.MainSemesterNumber != (int)StudySemesterNumberEunm.First).ToList();
                            //insert for this year
                            subjectNotIThinsYear.ForEach(item =>
                            {
                                var subjectInSeocunSemeseter = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => c.ExamSemesterId == secSemesterId && c.Ssn == item.Ssn && c.SubjectId == item.Id).FirstOrDefault();
                                if (subjectInSeocunSemeseter != null)
                                {
                                    _abstractUnitOfWork.Remove(subjectInSeocunSemeseter, SentencesHelper.System);
                                }
                                var newStudnetSubject = new StudentSubject()
                                {
                                    SubjectId = item.Id,
                                    Ssn = item.Ssn,
                                    PracticalDegree = item.PracticalDegree,
                                    ExamSemesterId = firstSemesertId
                                };
                                _abstractUnitOfWork.Add(newStudnetSubject, SentencesHelper.System);
                            });
                        });
                    }
                }
            }

            Func<SettingYearSystem, bool> myFilter = x => x.SettingId == (int)SettingEnum.TheNumberOfSubjectsForStudentsOutsideTheInstitute;
            //إذا عدل المواد للطالب خارج المعهد
            if (year.YearSystemNavigation.SettingYearSystem.Where(myFilter).First().Count != yearSystem.SettingYearSystem.Where(myFilter).First().Count)
            {
                //let him after test studnet out of institute
            }
            if (examSystem.HaveTheredSemester)
            {
                var theredSemeseter = new ExamSemester()
                {
                    YearId = year.Id,
                    SemesterNumber = 3
                };
                _abstractUnitOfWork.Add(theredSemeseter, UserName());
                var subjectsInThisYear = year.ExamSemester.SelectMany(c => c.StudentSubject).ToList();
                List<StudentSubject> faildSubjectInThisYear =  new  List<StudentSubject>();
                var subjectByStudnet = subjectsInThisYear.GroupBy(c=>c.Ssn).ToList();
                
                // faildSubjectInThisYear = faildSubjectInThisYear.OrderBy(c => c.Id).GroupBy(c => c.SubjectId).Select(c => c.Last()).ToList();
                // faildSubjectInThisYear = faildSubjectInThisYear.Where(c => !c.IsSuccess() && !c.IsNominate()).ToList();
                // List<StudentSubject> test = new List<StudentSubject>();
                // foreach (var item in faildSubjectInThisYear)
                // {
                //     if (!item.IsSuccess() && !item.IsNominate())
                //     {
                //         test.Add(item);
                //     }
                // }
                faildSubjectInThisYear.ForEach(subject =>
                {
                    var subjectInThredSemester = new StudentSubject()
                    {
                        SubjectId = subject.Id,
                        Ssn = subject.Ssn,
                        PracticalDegree = subject.PracticalDegree,
                        ExamSemesterId = theredSemeseter.Id,
                    };
                    _abstractUnitOfWork.Add(subjectInThredSemester, SentencesHelper.System);
                });
            }
            //process all student that regiseter in this year
            var studentRegistarInThisYear = year.Registrations.Select(c => c.Ssn).ToList();
            studentRegistarInThisYear.ForEach(ssn =>
            {
                _studentServic.ProcessStudentState(ssn);
            });
            _abstractUnitOfWork.Commit();
            return Ok();
        }


    }
}