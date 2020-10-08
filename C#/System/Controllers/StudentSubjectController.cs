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
using System.DTO.StudentDTO;
using System.Services;
using System.IServices;
using System.Threading;
using DAL.infrastructure;
using Static;
using Microsoft.AspNetCore.Authorization;
using System.DTO.StudentSubjectDTOs;
using DAL.HelperEums;
using DAL.Helper;
using DAL.IRepositories;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubjectController : MyBaseController
    {
        readonly IStudentSubjectService _studentSubjectService;
        readonly IStudentService _studentService;
        readonly IExamSemesterRepositroy _examSemesterRepositroy;
        IRepositroy<StudentSubject> _studentSubjectRepositroy;
        IRepositroy<Years> _yearsRepositroy;
        IStudentRepositroy _studentRepositroy;
        AbstractUnitOfWork _abstractUnitOfWork;
        public StudentSubjectController(AbstractUnitOfWork abstractUnitOfWork, IMapper mapper, IStudentSubjectService studentSubjectService,
            IStudentService studentService,
            IRepositroy<StudentSubject> studentSubjectRepositroy,
      IExamSemesterRepositroy examSemesterRepositroy,
      IRepositroy<Years> yearsRepositroy,
        IStudentRepositroy studentRepositroy
            ) : base(mapper)
        {
            this._studentSubjectService = studentSubjectService;
            this._studentService = studentService;
            this._examSemesterRepositroy = examSemesterRepositroy;
            this._studentSubjectRepositroy = studentSubjectRepositroy;
            this._abstractUnitOfWork = abstractUnitOfWork;
            this._studentRepositroy = studentRepositroy;
            this._yearsRepositroy = yearsRepositroy;
        }
        #region Get
        [HttpGet]
        [Route("GetStudentPracticalDegree/{subjectId:int}/{yearId:int}/{examSemeserId:int}")]
        [Authorize(Roles = "GetStudentPracticalDegree")]
        public async Task<IActionResult> GetStudentPracticalDegree(int subjectId, int yearId, int examSemeserId)
        {
            try
            {
                Task<bool> task = new Task<bool>(() => this._examSemesterRepositroy.CheckCanSetDegreeById(examSemeserId));
                task.Start();
                var studentsSubject = _studentSubjectRepositroy.GetIQueryable(c => c.SubjectId == subjectId
                && c.ExamSemesterId == examSemeserId
               && c.PracticalDegree == null | c.TheoreticlaDegree == null)
               .Include(c => c.SsnNavigation)
               .ThenInclude(c => c.Registrations)
               .Include(c => c.Subject)
               .ThenInclude(s => s.SubjectType)
               .Include(c => c.Subject)
               .ThenInclude(s => s.StudySemester)
               .Include(c => c.ExamSemester)
               .ToList();
                if (!await task)
                {
                    var message = new BadRequestErrors()
                    {
                        ActionName = "GetStudentPracticalDegree",
                        ControllerName = "StudentSubject",
                        Message = "لا يمكن وضع علامات لهذا الفصل"
                    };
                    return Conflict(message);
                }
                var studentsSubjectDTO = _mapper.Map<StudentSubjectDTO[]>(studentsSubject);
                return Ok(studentsSubjectDTO);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpGet("CanReset")]
        //[Authorize(Roles = "CanReset")]
        public IActionResult SubjectCanReset([FromQuery] string ssn)
        {
            var student = _studentRepositroy.GetIQueryable(c => c.Ssn == ssn)
                .Include(c => c.Registrations)
                .Include(c => c.StudentSubject)
                .ThenInclude(c => c.Subject)
                .ThenInclude(s => s.SubjectType)
                .Include(s => s.StudentSubject)
                .ThenInclude(s => s.Subject)
                .ThenInclude(s => s.StudySemester)
                .Include(c => c.StudentSubject)
                .ThenInclude(c => c.ExamSemester)
                .FirstOrDefault();
            if (student == null)
                return NotFound();
            var year = _yearsRepositroy.GetIQueryable(c => c.Id == student.CurrentYearId)
                .Include(c => c.ExamSemester)
                .ThenInclude(c => c.StudentSubject)
                .FirstOrDefault();
            if (year == null)
            {
                var message = new BadRequestErrors
                {
                    ActionName = "Subject Can Reset",
                    ControllerName = "Student Subject",
                    Message = "السنة غير موجودة"
                };
                return NotFound(message);
            }
            if (year.Blocked || year.NonFinishedSemeserNumber() == null)
            {
                return Ok(new StudentSubjectDTO[0]);
            }
            var studentSubject = student.CurrentStudentSubject.Where(c => c.PracticalDegree != null && c.TheoreticlaDegree == null && c.Subject.NeedPartialDegree() == true)
                .ToList();
            studentSubject = studentSubject.Where(c => c.MainSemesterNumber == (int)year.NonFinishedSemeserNumber()).ToList();
            return Ok(_mapper.Map<StudentSubjectDTO[]>(studentSubject));
        }



        [HttpGet("GetSubjectsBySSN")]
        public IActionResult GetSubjectBySSN([FromQuery] string ssn)
        {
            var studentSubjects = _studentSubjectRepositroy.Get(c => c.Ssn == ssn, c => c.Subject.SubjectType, c => c.ExamSemester.Year, c => c.Subject.StudySemester);

            return Ok(_mapper.Map<StudentSubjectDTO[]>(studentSubjects));
        }


        [HttpGet("SubjectNeedHelpDegree/{SSN}")]
        public IActionResult StudentNeedProcessBySSN(string SSN)
        {
            var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == SSN)
            .Include(c => c.StudentSubject)
            .ThenInclude(c => c.Subject)
            .ThenInclude(c => c.SubjectType)
            .Include(c => c.Registrations)
            .ThenInclude(c => c.StudentState)
            .Include(c => c.Registrations)
            .ThenInclude(c => c.StudyYear)
            .Include(c => c.Specialization)
            .FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            if (student.Lastregistration.FinalStateId != (int)StudentStateEnum.unknown)
            {
                return Conflict();
            }
            var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == student.CurrentYearId)
                .Include(c => c.YearSystemNavigation)
                .ThenInclude(c => c.SettingYearSystem)
                .First();
            if (student == null)
            {
                return NotFound();
            }
            if (_studentService.CanStudentGetHelpDegree(student, year))
            {
                int helpDegreeCount = 0;
                int helpDegreeSubjectCount = 0;
                int stillToSuccess = BusinessLogicHelper.SucessCount - student.StudentSubjectWithoutDuplicate.Where(c => c.IsSuccess()).Count();
                var numberOfSubjectOfAdministrativeLift = year.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.NumberOfSubjectOfAdministrativeLift).First().Count;
                if (numberOfSubjectOfAdministrativeLift != 0)
                {
                    stillToSuccess = numberOfSubjectOfAdministrativeLift - student.StudentSubjectWithoutDuplicate.Where(c => c.IsSuccess()).Count();
                }

                if (student.CurrentStudentStatusId == (int)StudentStateEnum.newStudent || student.CurrentStudentStatusId == (int)StudentStateEnum.transported || student.CurrentStudentStatusId == (int)StudentStateEnum.successful)
                {
                    helpDegreeCount = year.TheNumberOfHelpDegreeForStudentWhoWillBecomeUnsuccessful;
                    helpDegreeSubjectCount = year.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeUnsuccessful;
                }
                else
                {
                    helpDegreeCount = year.TheNumberOfHelpDegreeForStudentWhoWillBecomeDrained;
                    helpDegreeSubjectCount = year.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeDrained;
                }
                var subject = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsSuccess() && c.IsNominate()).ToList();
                subject = subject.Where(c => c.OrignalTotal + helpDegreeCount >= c.SucessMark).ToList();
                var responsse = new ResponseStudentHelpDegreeDto()
                {
                    StillToSucess = stillToSuccess,
                    HelpDegreeDivideOn = helpDegreeSubjectCount,
                    HelpDgreeCount = helpDegreeCount,
                    StudentSubjects = _mapper.Map<StudentSubjectDTO[]>(subject)
                };
                return Ok(responsse);
            }
            return Ok();
        }
        #endregion

        [HttpPost("SetDegrees")]
        [Authorize(Roles = "SetStudentsDegreeBySubject")]
        public IActionResult SetStudentsDegreeBySubject(List<StudentSubjectDTO> studentSubjectDTOs)
        {
            try
            {
                Years year;
                List<StudentSubject> studentSubjects;
                var subjectId = studentSubjectDTOs.First().Subject.Id;
                //هاد المتحول 
                int examSemeserNumber;
                #region validation
                {
                    var examSemesterId = studentSubjectDTOs.First().ExamSemesterId;
                    var yearId = _abstractUnitOfWork.Repository<ExamSemester>().Get(c => c.Id == examSemesterId).First().YearId;
                    //Task<List<string>> ssnTask = new Task<List<string>>(() =>
                    //{
                    //    return _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => c.SubjectId == subjectId
                    //             && c.ExamSemesterId == examSemesterId &&
                    //            (c.PracticalDegree == null || c.TheoreticlaDegree == null))
                    //            .Select(c => c.Ssn).ToList();
                    //});
                    //ssnTask.Start();
                    var studentsSSN = _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => c.SubjectId == subjectId
                                 && c.ExamSemesterId == examSemesterId &&
                                (c.PracticalDegree == null || c.TheoreticlaDegree == null))
                                .Select(c => c.Ssn).ToList();
                    year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == yearId)
                            .Include(c => c.ExamSystemNavigation)
                            .Include(c => c.ExamSemester)
                            .Include(c => c.YearSystemNavigation)
                            .ThenInclude(c => c.SettingYearSystem)
                            .First();
                    //var ssnTask = _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => c.SubjectId == subjectId
                    //            && c.ExamSemesterId == examSemesterId &&
                    //           (c.PracticalDegree == null || c.TheoreticlaDegree == null))
                    //            .Select(c => c.Ssn).ToListAsync();
                    //Task<Years> yearsTask = new Task<Years>(() =>
                    //{
                    //    //var yearId = _abstractUnitOfWork.Repository<ExamSemester>().Get(c => c.Id == examSemesterId).First().YearId;
                    //    return _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == yearId)
                    //        .Include(c => c.ExamSystemNavigation)
                    //        .Include(c => c.ExamSemester)
                    //        .Include(c => c.YearSystemNavigation)
                    //        .ThenInclude(c => c.SettingYearSystem)
                    //        .First();
                    //});
                    //yearsTask.Start();
                    //var yearsTask = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == yearId)
                    //        .Include(c => c.ExamSystemNavigation)
                    //        .Include(c => c.ExamSemester)
                    //        .Include(c => c.YearSystemNavigation)
                    //        .ThenInclude(c => c.SettingYearSystem)
                    //        .FirstAsync();
                    //yearsTask.Start();
                    //Task<List<StudentSubject>> studentSubjectsTask = new Task<List<StudentSubject>>(() =>
                    //{
                    //    return _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => studentSubjectDTOs.Select(s => s.Id).Contains(c.Id) && (c.TheoreticlaDegree == null || c.PracticalDegree == null))
                    //    .Include(c => c.Subject)
                    //    .ThenInclude(c => c.SubjectType)
                    //    .Include(c => c.Subject)
                    //    .ThenInclude(c => c.StudySemester)
                    //    .Include(c => c.ExamSemester)
                    //    .ToList();
                    //});
                    //studentSubjectsTask.Start();
                    if (studentSubjectDTOs.Any(c => c.ExamSemesterId != examSemesterId))
                    {
                        //مو كلون عم يقدمو بنفس الفصل
                        var message = Messages.Error;
                        message.ActionName = "Set Students Degree By Subject";
                        message.ControllerName = "Student subject";
                        message.Message = "لم يتقدم جميعهم بنفس الفصل";
                        return Conflict(message);
                    }

                    if (studentSubjectDTOs.Select(c => c.Subject.Id).Distinct().Count() != 1)
                    {
                        var message = Messages.Error;
                        message.ActionName = "Set Students Degree By Subject";
                        message.ControllerName = "Student Subject";
                        message.Message = "جميعهم لنفس المادة";
                        return Conflict(message);
                    }


                    if (year.Blocked)
                    {
                        var message = new BadRequestErrors
                        {
                            Message = "السنة مغلقة",
                            ActionName = "Set Students Degree By Subject",
                            ControllerName = "Student subject"
                        };
                        return Conflict(message);
                    }
                    examSemeserNumber = year.ExamSemester.Where(c => c.Id == examSemesterId).First().SemesterNumber;
                    if (examSemeserNumber > 1 && (!year.IsThisSemesterFinshed(examSemeserNumber - 1)))
                    {
                        var message = new BadRequestErrors()
                        {
                            ActionName = "SetStudentsDegreeBySubject",
                            ControllerName = "StudentSubject",
                            Message = "يجب إنهاء الفصل السابق",
                        };
                        return Conflict(message);
                    }
                    //var studentsSSN = await ssnTask;
                    var requestSSN = studentSubjectDTOs.Select(c => c.SSN).ToList();
                    var union = studentsSSN.Union(requestSSN);
                    if (union.Except(studentsSSN).Count() > 0 || union.Except(requestSSN).Count() > 0)
                    {
                        var message = new BadRequestErrors()
                        {
                            ActionName = "SetStudentsDegreeBySubject",
                            ControllerName = "StudentSubject",
                            Message = "لم يتم إرسال جميع الطلاب او تم إرسال طلاب زيادة",
                        };
                        return Conflict(message);
                    }
                    studentSubjects = _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => studentSubjectDTOs.Select(s => s.Id).Contains(c.Id) && (c.TheoreticlaDegree == null || c.PracticalDegree == null))
                    .Include(c => c.Subject)
                    .ThenInclude(c => c.SubjectType)
                    .Include(c => c.Subject)
                    .ThenInclude(c => c.StudySemester)
                    .Include(c => c.ExamSemester)
                    .ToList();
                    if (studentSubjects.Count() != studentSubjectDTOs.Count())
                    {
                        var message = Messages.Error;
                        message.ActionName = "Set Students Degree By Subject";
                        message.ControllerName = "Student Subject";
                        message.Message = "عدد موا د الطالب غير متساوية";
                        return Conflict(message);
                    }

                }
                #endregion validation

                foreach (var item in studentSubjectDTOs)
                {
                    var allSubjectExam = _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => c.Ssn == item.SSN && c.SubjectId == subjectId && c.ExamSemester.YearId == year.Id)
                        .Include(c => c.Subject)
                        .ThenInclude(c => c.StudySemester)
                        .ToList();
                    var studentSubject = studentSubjects.Where(c => c.Id == item.Id).First();
                    if (allSubjectExam.Count > 1)
                    {
                        var privousSubjectExam = allSubjectExam[allSubjectExam.Count - 2];
                        if (privousSubjectExam.PracticalDegree != item.PracticalDegree)
                        {
                            if (privousSubjectExam.MainSemesterNumber != studentSubject.ExamSemester.SemesterNumber)
                            {
                                var message = new BadRequestErrors
                                {
                                    ActionName = "SetStudentsDegreeBySubject",
                                    ControllerName = "StudentSubject",
                                    Message = "لا يمكن تغيير علامة العملي"
                                };
                                return Conflict(message);
                            }
                            studentSubject.SystemNote += "تم تغيير علامة العملي";
                        }
                    }
                    studentSubject.PracticalDegree = item.PracticalDegree;
                    studentSubject.TheoreticlaDegree = item.TheoreticlaDegree;
                    studentSubject.Note = item.Note;
                    if (!this._studentSubjectService.CheckStudentSubjectDegreeAllowNull(studentSubject))
                    {
                        var message = new BadRequestErrors()
                        {
                            Message = "خطأ في وضع علامات المواد",
                            ActionName = "SetStudentsDegreeBySubject",
                            ControllerName = "StudentSubject"
                        };
                        return Conflict(message);
                    }
                    _abstractUnitOfWork.Update(studentSubject, UserName());
                    bool shouldTryToProcessStudent = true;
                    if (studentSubject.TheoreticlaDegree == null)
                    {
                        shouldTryToProcessStudent = false;
                        goto breakIf;
                    }
                    var newExamsemesterId = year.GetNexSemeserIdCanTsetSubjectIn(studentSubject);
                    if (newExamsemesterId != -1)
                    {
                        var newStudentSubject = new StudentSubject()
                        {
                            SubjectId = studentSubject.SubjectId,
                            Ssn = studentSubject.Ssn,
                            ExamSemesterId = newExamsemesterId,
                            PracticalDegree = studentSubject.PracticalDegree,
                            TheoreticlaDegree = null,
                        };
                        _abstractUnitOfWork.Add(newStudentSubject, UserName());
                        shouldTryToProcessStudent = false;
                    }
                breakIf:;
                    if (shouldTryToProcessStudent)
                    {
                        var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == studentSubject.Ssn)
                            .Include(c => c.StudentSubject)
                                .ThenInclude(c => c.ExamSemester)
                            .Include(c => c.StudentSubject)
                                .ThenInclude(c => c.Subject)
                                    .ThenInclude(c => c.SubjectType)
                            .Include(c => c.StudentSubject)
                                .ThenInclude(c => c.Subject)
                                .ThenInclude(c => c.StudySemester)
                            .Include(c => c.Registrations)
                            .First();
                        if (!student.StudentSubject.Where(c => c.ExamSemester != null && c.ExamSemester.YearId == year.Id).Any(c => c.PracticalDegree == null || c.TheoreticlaDegree == null))
                        {
                            this._studentService.ProcessStudentState(student.Ssn);
                            // _abstractUnitOfWork.Update(student.Lastregistration, UserName());
                        }
                    }

                }
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpPost("Reset")]
        //[Authorize(Roles = "Reset")]
        public IActionResult Reset([FromBody] ResetDTo ressetDTO)
        {
            var studentSubject = _studentSubjectRepositroy.GetIQueryable(c => c.Id == ressetDTO.Id)
                .Include(c => c.Subject)
                .ThenInclude(c => c.StudySemester)
                .FirstOrDefault();
            if (studentSubject == null)
            {
                return NotFound();
            }
            var allStudentSubject = _studentSubjectRepositroy.GetIQueryable(c => c.Ssn == studentSubject.Ssn && c.SubjectId == studentSubject.SubjectId)
                .Include(c => c.Subject)
                .ThenInclude(c => c.StudySemester)
                .ToList();
            if (allStudentSubject.Count > 1)
            {
                var privouseSubject = allStudentSubject[allStudentSubject.Count - 1];
                if (privouseSubject.MainSemesterNumber == studentSubject.MainSemesterNumber)
                {
                    if (studentSubject.MainSemesterNumber == 2)
                    {
                        var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.ExamSemester.Any(s => s.Id == studentSubject.ExamSemesterId))
                            .Include(c => c.ExamSemester)
                            .ThenInclude(c => c.StudentSubject)
                            .First();
                        if (!year.IsThisSemesterFinshed(1))
                        {
                            var m = new BadRequestErrors
                            {
                                Message = "لا يمكن تصفير حالياً",
                                ControllerName = "StudentSubject",
                                ActionName = "Reset"
                            };
                            return Conflict(m);
                        }
                    }
                    studentSubject.PracticalDegree = null;
                    studentSubject.TheoreticlaDegree = null;
                    studentSubject.Note += ressetDTO.Note + "\n";
                    _studentSubjectRepositroy.Update(studentSubject, UserName());
                    _studentRepositroy.Save();
                    return Ok();
                }
            }
            var message = new BadRequestErrors
            {
                Message = "لا يمكن تصفير المادة",
                ControllerName = "StudentSubject",
                ActionName = "Reset"
            };
            return Conflict();
        }

        [HttpPost("SetDegreeForTransformStudent")]
        [Authorize(Roles = "SetDegreeForTransformStudent")]
        public IActionResult SetDegreeForTransformStudent(List<StudentSubjectDTO> studentSubjectDTO)
        {
            try
            {
                //_abstractUnitOfWork.BegeinTransaction();
                #region validate

                var StudentSubject = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => studentSubjectDTO.Select(k => k.Id).Contains(c.Id)).ToList();
                var ssn = StudentSubject.First().Ssn;

                var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == ssn)
                    .Include(c => c.Registrations)
                    .ThenInclude(c => c.Year)
                    .Include(c => c.StudentSubject)
                    .ThenInclude(c => c.ExamSemester)
                    .Include(c => c.StudentSubject)
                    .ThenInclude(c => c.Subject)
                    .ThenInclude(c => c.SubjectType)
                    .Include(c => c.StudentSubject)
                    .ThenInclude(c => c.Subject)
                    .ThenInclude(c => c.StudySemester)
                    .First();


                bool IsStudentTransforToUs = student.CurrentStudentStatusId == (int)StudentStateEnum.unknown;

                //المواد لطالب غير منقول
                //should have error messsges
                if (!IsStudentTransforToUs)
                {
                    var message = Messages.Error;
                    message.ActionName = "Set Degree For Transform Student";
                    message.ControllerName = "Student";
                    message.Message = "المواد لطالب غير منقول";
                    return Conflict(message);
                }

                //should have error messsges
                //المواد مو لنفس الطالب

                if (StudentSubject.Any(c => c.Ssn != ssn))
                {
                    var message = Messages.Error;
                    message.ActionName = "Set Degree For Transform Student";
                    message.ControllerName = "Student";
                    message.Message = "المواد ليست لنفس الطالب";
                    return Conflict(message);
                }

                var orginalStudentSubject = student.StudentSubject.ToList();

                var studentSubjectId = orginalStudentSubject.Select(c => c.Id).ToList();
                var union = studentSubjectId.Union(studentSubjectDTO.Select(c => c.Id)).ToList();
                //ما بعتنا كل المواد لنفس الطالب
                if (union.Except(studentSubjectId).Count() > 0 || union.Except(studentSubjectDTO.Select(c => c.Id)).Count() > 0)

                {
                    var message = Messages.Error;
                    message.ActionName = "Set Degree For Transform Student";
                    message.ControllerName = "Student";
                    message.Message = "لم يتم إرسال كامل المواد للطالب";
                    return Conflict(message);
                }

                var firstYear = student.CurrentYear.FirstYear;

                var beforYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.SecondYear == firstYear)
                    .Include(c => c.YearSystemNavigation)
                    .ThenInclude(c => c.SettingYearSystem)
                    .FirstOrDefault();

                if (beforYear == null)
                {
                    var message = Messages.Error;
                    message.ActionName = "Set Degree For Transform Student";
                    message.ControllerName = "Student";
                    message.Message = " يجب إضافة نظام السنة السابقة لتسجيل الطالب";
                    return Conflict(message);
                }
                #endregion
                #region should Replace with auto mapper 
                int successCount = 0;
                int successWithHelpDegree = 0;
                int totalHelpDgree = 0;
                foreach (var item in studentSubjectDTO)
                {
                    //should repalce with auto mapper
                    var studentSubject = orginalStudentSubject.Find(c => c.Id == item.Id);
                    studentSubject.PracticalDegree = item.PracticalDegree;
                    studentSubject.TheoreticlaDegree = item.TheoreticlaDegree;
                    studentSubject.Note = item.Note;
                    studentSubject.HelpDegree = item.HelpDegree;
                    if (!_studentSubjectService.CheckStudentSubjectDegree(studentSubject))
                    {
                        var message = new BadRequestErrors()
                        {
                            Message = "خطأ في وضع علامات المواد",
                            ActionName = "SetDegreeForTransformStudent",
                            ControllerName = "Student"
                        };
                        return Conflict(message);
                    }
                    if (studentSubject.IsSuccess())
                    {
                        successCount++;
                    }
                    if (studentSubject.HelpDegree == true)
                    {
                        successWithHelpDegree++;
                        totalHelpDgree += (int)Math.Ceiling(studentSubject.SucessMark - studentSubject.OrignalTotal);
                    }
                    _abstractUnitOfWork.Update(studentSubject, UserName());
                }
                var registration = student.Lastregistration;
                registration.StudentStateId = (int)StudentStateEnum.unSuccessful;
                //registration.StudyYearId = (int)StudyYearEnum.FirstYear;
                //عدد مواد الترفع الإداري
                var numberOfSubjectOfAdministrativeLift = beforYear.NumberOfSubjectOfAdministrativeLift;

                #endregion 

                if (successWithHelpDegree > 0 && successCount >= BusinessLogicHelper.SucessCount)
                {
                    //خطأ ما لازم يكون ناجح و فيه علامات مساعدة 
                    var message = Messages.Error;
                    message.ActionName = "Set Degree For Transform Student";
                    message.ControllerName = "Student";
                    message.Message = "علامات المساعدة لاتعطى للناجح";
                    return Conflict(message);
                }
                //إذا ناجح  او منقول
                if (successCount >= BusinessLogicHelper.SucessCount)
                {

                    if (successCount == 14)
                    {
                        registration.StudentStateId = (int)StudentStateEnum.successful;
                    }
                    else
                    {
                        registration.StudentStateId = (int)StudentStateEnum.transported;
                    }
                }
                ///هون إذا كان ناجح بشيك اول شي على الترفع الإداري
                ///إذا نجح اوكيه بس ما لازم يكون اخد علامات مساعدة لأنو ناجح إدارياً
                ///إذا راسب بحطلو علامات مساعدة و بشوف إذا ناجح عادي او إدارياً

                else
                {
                    if (numberOfSubjectOfAdministrativeLift > BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist && successCount >= numberOfSubjectOfAdministrativeLift)
                    {
                        if (successWithHelpDegree != 0)
                        {
                            //الزلمة ناجح إدارياً ف ما بصير ياخد علامات مساعدة
                            var message = Messages.Error;
                            message.ActionName = "Set Degree For Transform Student";
                            message.ControllerName = "Student";
                            message.Message = "الناجح الاداري لاياخذ علمات مساعدة";
                            return Conflict(message);
                        }
                        registration.StudentStateId = (int)StudentStateEnum.transported;
                        registration.SystemNote = "تسجيل بنجاح إداري";
                    }
                    else
                    {
                        if (successWithHelpDegree != 0)
                        {
                            //عدد علامات المساعدة المستحقة للطالب

                            beforYear.GetSubjectCountAndHelpDegreeCount(student, out int helpDegrees, out int subjectCount);


                            if (helpDegrees < totalHelpDgree || subjectCount < successWithHelpDegree)
                            {
                                //خطأ بإعطاء علامات المساعدة 
                                var message = Messages.Error;
                                message.ActionName = "Set Degree For Transform Student";
                                message.ControllerName = "Student";
                                message.Message = "خطا باعطاء علامات المساعدة";
                                return Conflict(message);
                            }
                            successCount += successWithHelpDegree;

                            if (numberOfSubjectOfAdministrativeLift > BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist)
                            {
                                if (successCount < numberOfSubjectOfAdministrativeLift)
                                {
                                    var message = Messages.Error;
                                    message.ActionName = "Set Degree For Transform Student";
                                    message.ControllerName = "Student";
                                    message.Message = "غير مسموح اعطاء علامات المساعدة في حال سيبقى الطالب راسب";
                                    return Conflict(message);
                                }
                                else if (successCount > numberOfSubjectOfAdministrativeLift)
                                {
                                    var message = Messages.Error;
                                    message.ActionName = "Set Degree For Transform Student";
                                    message.ControllerName = "Student";
                                    message.Message = "خطأ لا يمكن اخذ علامات مساعدة لينجح بمواد اكثر من عدد مواد النجاح";
                                    return Conflict(message);
                                }
                                else
                                {
                                    registration.StudentStateId = (int)StudentStateEnum.transported;
                                    registration.SystemNote = "تسجيل بنجاح إداري و علامات مساعدة";
                                }
                            }
                            else if (successCount < BusinessLogicHelper.SucessCount)
                            {
                                var message = Messages.Error;
                                message.ActionName = "Set Degree For Transform Student";
                                message.ControllerName = "Student";
                                message.Message = "خطأ لا يمكن اخذ علامات مساعدة لينجح بمواد اكثر من عدد مواد النجاح";
                                return Conflict(message);
                            }
                            else if (successCount > BusinessLogicHelper.SucessCount)
                            {
                                var message = Messages.Error;
                                message.ActionName = "Set Degree For Transform Student";
                                message.ControllerName = "Student";
                                message.Message = "خطأ لا يمكن اخذ علامات مساعدة لينجح بمواد اكثر من عدد مواد النجاح";
                                return Conflict(message);
                            }
                            else
                            {
                                registration.StudentStateId = (int)StudentStateEnum.transported;
                                registration.SystemNote = "تسجيل و نجاح بعلامات مساعدة";
                            }
                        }
                    }
                }
                Years thisYear = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == registration.YearId)
                    .Include(c => c.ExamSemester)
                    .Include(c => c.ExamSystemNavigation)
                    .First();
                if (thisYear.Blocked)
                {

                    //السنة مقفولة
                    var message = Messages.Error;
                    message.ActionName = "Set Degree For Transform Student";
                    message.ControllerName = "Student";
                    message.Message = "السنة منقولة";
                    return Conflict(message);
                }
                int firstExamSemesterId = thisYear.ExamSemester.Where(c => c.SemesterNumber == 1).First().Id;
                int secoundExamSemesteId = thisYear.ExamSemester.Where(c => c.SemesterNumber == 2).First().Id;
                ///إذا كان الطالب ناجح
                ///منضفلو مواد السنة التانية
                ///و منرجع منحطلو مواد السنة الأولى إذا كان منقول
                if (registration.StudentStateId == (int)StudentStateEnum.successful || registration.StudentStateId == (int)StudentStateEnum.transported)
                {

                    var studyPlan = _abstractUnitOfWork.Repository<StudyPlan>().GetIQueryable(sp => sp.StudySemester.Any(ss => ss.Subjects.Any(s => s.Id == StudentSubject.First().SubjectId)))
                            .Include(c => c.StudySemester)
                            .ThenInclude(c => c.Subjects)
                            .First();
                    var secoundYearSemester = studyPlan.StudySemester.Where(c => c.StudyYearId == (int)StudyYearEnum.SecoundYear).ToList();
                    var secoundYearFirstSemester = secoundYearSemester.Where(c => c.Number == 1).First().Subjects;
                    var secoundYearSecoundSemester = secoundYearSemester.Where(c => c.Number == 2).First().Subjects;

                    foreach (var item in secoundYearFirstSemester)
                    {
                        var studentSubject = new StudentSubject()
                        {
                            ExamSemesterId = firstExamSemesterId,
                            Ssn = ssn,
                            SubjectId = item.Id
                        };
                        _abstractUnitOfWork.Add(studentSubject, UserName());
                    }
                    foreach (var item in secoundYearSecoundSemester)
                    {
                        var studentSubject = new StudentSubject()
                        {
                            ExamSemesterId = secoundExamSemesteId,
                            Ssn = ssn,
                            SubjectId = item.Id
                        };
                        _abstractUnitOfWork.Add(studentSubject, UserName());
                    }

                    if (registration.StudentStateId == (int)StudentStateEnum.transported)
                    {
                        var subjectHaveHelpDgree = studentSubjectDTO.Where(c => c.HelpDegree == true).Select(c => c.Id);
                        var faildOrdinalSubject = orginalStudentSubject.Where(c => !c.IsSuccess() && !subjectHaveHelpDgree.Contains(c.Id));
                        foreach (var item in faildOrdinalSubject)
                        {
                            int mainExamSemester = 0;
                            if (item.MainSemesterNumber == 1)
                            {
                                mainExamSemester = firstExamSemesterId;
                            }
                            else if (item.MainSemesterNumber == 2)
                            {
                                mainExamSemester = secoundExamSemesteId;
                            }
                            else
                            {
                                return BadRequestAnonymousError();
                            }
                            double? PracticalDegree = null;
                            if (item.IsNominate())
                            {
                                //add new
                                if (thisYear.IsDoubleExam)
                                {
                                    mainExamSemester = firstExamSemesterId;
                                }
                                PracticalDegree = (double)item.PracticalDegree;
                            }

                            var studentSubject = new StudentSubject()
                            {
                                PracticalDegree = PracticalDegree,
                                Ssn = ssn,
                                SubjectId = item.SubjectId,
                                ExamSemesterId = mainExamSemester
                            };
                            _abstractUnitOfWork.Add(studentSubject, UserName());
                        }
                    }
                }
                ///إذا كان راسب بس منرجع منضفلو مواد السنة الأولى يلي لازم يقدمها
                else
                {
                    var faildOrdinalSubject = orginalStudentSubject.Where(c => c.IsSuccess() == false);
                    foreach (var item in faildOrdinalSubject)
                    {
                        int mainExamSemester = 0;
                        if (item.MainSemesterNumber == 1)
                        {
                            mainExamSemester = firstExamSemesterId;
                        }
                        else if (item.MainSemesterNumber == 2)
                        {
                            mainExamSemester = secoundExamSemesteId;
                        }
                        else
                        {
                            return BadRequestAnonymousError();
                        }
                        double? PracticalDegree = null;
                        if (item.IsNominate())
                        {
                            if (thisYear.IsDoubleExam)
                            {
                                mainExamSemester = firstExamSemesterId;
                            }
                            PracticalDegree = (double)item.PracticalDegree;
                        }
                        var studentSubject = new StudentSubject()
                        {
                            PracticalDegree = PracticalDegree,
                            Ssn = ssn,
                            SubjectId = item.SubjectId,
                            ExamSemesterId = mainExamSemester
                        };
                        _abstractUnitOfWork.Add(studentSubject, UserName());
                    }
                }
                var oldStudentSSN = student.Ssn;
                var studentBeFroUpdate = student.ToString();
                _abstractUnitOfWork.Commit();
                _studentService.SetSSN(student);
                var studentAfterUpdate = student.ToString();
                var newSSN = student.Ssn;
                _abstractUnitOfWork.Context.Database.ExecuteSqlCommand($"update Students set SSN = {newSSN} where SSN={oldStudentSSN}");
                return Ok();

            }
            catch (Exception ex)
            {
                //_abstractUnitOfWork.RoleBack();
                //_abstractUnitOfWork.Dispose();
                return BadRequestAnonymousError();
            }
        }
        [HttpPost("SetHelpDgree/{SSN}")]
        public IActionResult SetNelpDgeree(string SSN, [FromBody] IList<HelpDgreeDto> helpDgreeDto)
        {
            try
            {

                #region validation

                var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == SSN)
                    .Include(c => c.StudentSubject)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(c => c.SubjectType)
                    .Include(c => c.StudentSubject)
                    .Include(c => c.StudentSubject)
                        .ThenInclude(c => c.ExamSemester)
                            .ThenInclude(c => c.Year)
                    .Include(c => c.StudentSubject)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(c => c.StudySemester)
                    .Include(c => c.Registrations)
                        .ThenInclude(c => c.Year)
                            .ThenInclude(c => c.ExamSystemNavigation)
                    .Include(c => c.Registrations)
                        .ThenInclude(c => c.Year)
                            .ThenInclude(c => c.YearSystemNavigation)
                                    .ThenInclude(c => c.SettingYearSystem)
                    .FirstOrDefault();
                if (student == null)
                {
                    return NotFound();
                }
                if (helpDgreeDto.Count == 0)
                {
                    this._studentService.ProcessStudentState(student, false);
                }
                helpDgreeDto = helpDgreeDto.GroupBy(c => c.Id).Select(c => c.First()).ToList();
                var currentStudentSubject = student.CurrentStudentSubject.Where(c => c.IsNominate() && !c.IsSuccess()).ToList();
                if (helpDgreeDto.Select(c => c.Id).Except(currentStudentSubject.Select(c => c.Id)).Any())
                {
                    //المواد مالها للطالب
                    return Conflict();
                }
                var studentSubjects = currentStudentSubject.Where(c => helpDgreeDto.Select(h => h.Id).Contains(c.Id)).ToList();

                if (studentSubjects.Any(c => c.HelpDegree == true))
                {
                    //الطالب اخد علامة مساعدة من قبل لشي مادة من هل مواد
                    return Conflict();
                }
                var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == student.CurrentYearId)
                    .Include(c => c.YearSystemNavigation)
                    .ThenInclude(c => c.SettingYearSystem)
                    .First();

                year.GetSubjectCountAndHelpDegreeCount(student, out int helpDgreeCount, out int subjectCount);

                if (subjectCount < helpDgreeDto.Count)
                {
                    return Conflict();
                }
                var subjectHaveHelpDgree = student.StudentSubjectWithoutDuplicate.Where(c => helpDgreeDto.Select(k => k.Id).Contains(c.Id)).ToList();
                var allRequiredToSucess = subjectHaveHelpDgree.Select(c => c.SucessMark - c.OrignalTotal).Sum();
                if (allRequiredToSucess > helpDgreeCount)
                {
                    //مجموع العلامات المطعى اكبر من المسموح
                    return Conflict();
                }
                #endregion
                if (student.CurrentStudyYearId == (int)StudyYearEnum.FirstYear)
                {
                    var sucessCount = student.StudentSubjectWithoutDuplicate.Where(c => c.IsSuccess()).Count();
                    if (sucessCount >= BusinessLogicHelper.SucessCount || (sucessCount >= year.NumberOfSubjectOfAdministrativeLift && year.NumberOfSubjectOfAdministrativeLift > BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist))
                    {
                        return Conflict();
                    }
                    sucessCount += helpDgreeDto.Count;
                    if (year.NumberOfSubjectOfAdministrativeLift > BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist)
                    {
                        if (sucessCount == year.NumberOfSubjectOfAdministrativeLift)
                        {
                            student.Lastregistration.FinalStateId = (int)StudentStateEnum.transported;
                            student.Lastregistration.SystemNote = "ناجح ترفع إداري و علامات مساعدة";
                            _abstractUnitOfWork.Update(student.Lastregistration, UserName());
                        }
                        else
                        {
                            //خطأ بإعطاء علامات المساعدة
                            return Conflict();
                        }
                    }
                    else
                    {
                        if (sucessCount == BusinessLogicHelper.SucessCount)
                        {
                            student.Lastregistration.FinalStateId = (int)StudentStateEnum.transported;
                            student.Lastregistration.SystemNote = "ناجح بعلامات المساعدة";
                            _abstractUnitOfWork.Update(student.Lastregistration, UserName());
                        }
                        else
                        {
                            //خطأ بإعطاء علامات المساعدة
                            return Conflict();
                        }
                    }
                }
                //إذا سنة تانية
                else
                {
                    var falidSubject = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsSuccess()).ToList();
                    if (falidSubject.Any(c => !helpDgreeDto.Select(k => k.Id).Contains(c.Id)))
                    {
                        //ما مرقت كل المواد للطالب او مو كل اللطالب اخد المواد
                        return Conflict();
                    }
                    student.Lastregistration.FinalStateId = (int)StudentStateEnum.Graduated;
                    student.Lastregistration.SystemNote = String.Empty;
                }

                studentSubjects.ForEach(c =>
                {
                    c.HelpDegree = true;
                    c.Note = helpDgreeDto.Where(h => h.Id == c.Id).First().Note;
                    _abstractUnitOfWork.Update(c, UserName());
                });
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpPatch]
        public IActionResult Update([FromBody] UpdateStudentSubjectDto studentSubjectDTO)
        {
            try
            {
                _abstractUnitOfWork.BegeinTransaction();
                var studentSubject = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => c.Id == studentSubjectDTO.Id, c => c.Subject, c => c.Subject.SubjectType, c => c.ExamSemester, c => c.Subject.StudySemester).Single();
                if (studentSubject == null)
                {
                    return NotFound();
                }
                studentSubject = _mapper.Map(studentSubjectDTO, studentSubject);
                if (!this._studentSubjectService.Update(studentSubject))
                {
                    return Conflict();
                }
                _abstractUnitOfWork.Commit();
                // var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == studentSubject.Ssn)
                //     .Include(c => c.StudentSubject)
                //     .ThenInclude(c => c.Subject)
                //     .ThenInclude(c => c.SubjectType)
                //     .Include(c=>c.StudentSubject)
                //     .Include(c => c.StudentSubject)
                //     .ThenInclude(c => c.ExamSemester)
                //     .ThenInclude(c => c.Year)
                //     .Include(c => c.Registrations)
                //     .ThenInclude(c => c.Year)
                //     .ThenInclude(c => c.ExamSystemNavigation)
                //     .Include(c => c.Registrations)
                //     .ThenInclude(c => c.Year)
                //     .ThenInclude(c => c.YearSystemNavigation)
                //     .ThenInclude(c => c.SettingYearSystem)
                //     .SingleOrDefault();
                this._studentService.ProcessStudentState(studentSubject.Ssn);
                _abstractUnitOfWork.Commit();
                _abstractUnitOfWork.CommitTransaction();
                return Ok();
            }
            catch (Exception ex)
            {
                _abstractUnitOfWork.RoleBack();
                return BadRequestAnonymousError();
            }
        }


    }
}