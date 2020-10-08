using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.DTO.StudentDTO;
using Static;
using DAL.Classes;
using AutoMapper;
using DAL.Models;
using DAL.HelperEums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IServices;
using System.Services;
using System.IO;
using System.DTO.YearSystemDTO;
using DAL.Helper;
using System.DTO;
using System.HelperClasses;
using DAL.infrastructure;
using Newtonsoft.Json;
using DAL.IRepositories;
using System.DTO.RegistrationDTOs;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : MyBaseController
    {
        #region declration
        AbstractUnitOfWork _abstractUnitOfWork;
        readonly IStudentService _studentService;
        readonly IFileWriter _fileWriter;
        readonly IStudentSubjectService _studentSubjectService;
        readonly IStudentRepositroy _studentRepositroy;
        readonly IRepositroy<Years> _yearRepositroy;
        IRepositroy<Partners> _partnersRepositroy;
        IRepositroy<Sanctions> _sanctionsRepositroy;
        IRepositroy<Constraints> _constraintRepositroy;
        IRepositroy<Reparations> _reparationsRepositroy;
        readonly IRepositroy<Registrations> _registrationsRepositroy;
        IRepositroy<Nationalies> _nationaliesRepositroy;
        IRepositroy<ClosesetPersons> _closesetPersonsRepositroy;
        IRepositroy<Siblings> _siblingsRepositroy;
        IRepositroy<StudentPhone> _studentPhoneRepositroy;
        IRepositroy<StudentState> _studentStateRepositroy;
        IRepositroy<Attatchments> _attatchmentsRepositroy;
        IRepositroy<StudentAttachment> _studentAttachmentRepositroy;
        IRepositroy<Countries> _countriesRepositroy;
        IRepositroy<Langaues> _langauesRepositroy;
        IRepositroy<TypeOfRegistar> _typeOfRegistarRepositroy;
        IRepositroy<Specializations> _specializationsRepositroy;
        IRepositroy<Degree> _degreeRepositroy;
        IRepositroy<SocialStates> _socialStatesRepositroy;
        IRepositroy<PhoneType> _phoneTypeRepositroy;
        IRepositroy<StudyPlan> _studyPlanRepositroy;
        IRepositroy<StudentDegree> _StudentDegreeRepositroy;

        #endregion

        #region constrcutor
        public StudentController(AbstractUnitOfWork abstractUnitOfWork, IMapper mapper,
            IStudentService studentService, IFileWriter fileWriter, IStudentSubjectService studentSubjectService,
            IStudentRepositroy studentRepositroy,
            IRepositroy<Years> yearRepositroy,
            IRepositroy<Registrations> registrationsRepositroy,
            IRepositroy<Partners> partnersRepositroy,
            IRepositroy<Sanctions> sanctionsRepositroy,
            IRepositroy<Constraints> constraintRepositroy,
            IRepositroy<Reparations> reparationsRepositroy,
            IRepositroy<Nationalies> nationaliesRepositroy,
            IRepositroy<ClosesetPersons> closesetPersonsRepositroy,
            IRepositroy<Siblings> siblingsRepositroy,
            IRepositroy<StudentPhone> studentPhoneRepositroy,
            IRepositroy<StudentState> studentStateRepositroy,
            IRepositroy<Attatchments> attatchmentsRepositroy,
            IRepositroy<StudentAttachment> studentAttachmentRepositroy,
            IRepositroy<TypeOfRegistar> typeOfRegistarRepositroy,
            IRepositroy<Specializations> specializationsRepositroy,
            IRepositroy<Degree> degreeRepositroy,
            IRepositroy<Years> yearsRepositroy,
            IRepositroy<SocialStates> socialStatesRepositroy,
            IRepositroy<StudyPlan> studyPlanRepositroy,
            IRepositroy<PhoneType> phoneTypeRepositroy,
            IRepositroy<Countries> countriesRepositroy,
            IRepositroy<Langaues> langauesRepositroy,
            IRepositroy<StudentDegree> StudentDegreeRepositroy
            ) : base(mapper)
        {
            this._studentService = studentService;
            this._fileWriter = fileWriter;
            this._studentSubjectService = studentSubjectService;
            this._studentRepositroy = studentRepositroy;
            this._yearRepositroy = yearRepositroy;
            this._registrationsRepositroy = registrationsRepositroy;
            this._partnersRepositroy = partnersRepositroy;
            this._sanctionsRepositroy = sanctionsRepositroy;
            this._constraintRepositroy = constraintRepositroy;
            this._reparationsRepositroy = reparationsRepositroy;
            this._nationaliesRepositroy = nationaliesRepositroy;
            this._closesetPersonsRepositroy = closesetPersonsRepositroy;
            this._studentPhoneRepositroy = studentPhoneRepositroy;
            this._studentStateRepositroy = studentStateRepositroy;
            this._attatchmentsRepositroy = attatchmentsRepositroy;
            this._studentAttachmentRepositroy = studentAttachmentRepositroy;
            this._typeOfRegistarRepositroy = typeOfRegistarRepositroy;
            this._specializationsRepositroy = specializationsRepositroy;
            this._degreeRepositroy = degreeRepositroy;
            this._yearRepositroy = yearRepositroy;
            this._abstractUnitOfWork = abstractUnitOfWork;
            this._socialStatesRepositroy = socialStatesRepositroy;
            this._phoneTypeRepositroy = phoneTypeRepositroy;
            this._studyPlanRepositroy = studyPlanRepositroy;
            this._siblingsRepositroy = siblingsRepositroy;
            this._countriesRepositroy = countriesRepositroy;
            this._langauesRepositroy = langauesRepositroy;
            this._StudentDegreeRepositroy = StudentDegreeRepositroy;
        }
        #endregion


        [HttpGet]
        //[Authorize(Roles = "ShowStudent")]
        public IActionResult Show([FromQuery] string SSN, string specializationId, string firstName, string fatherName, string lastName, int? enrollmentDate, [FromQuery] PagingDTO pagingDTO)
        {
            try
            {
                var studentsQueryable = _studentRepositroy.GetIQueryable();
                if (!string.IsNullOrWhiteSpace(SSN))
                {
                    studentsQueryable = studentsQueryable.Where(c => c.Ssn.StartsWith(SSN));
                }
                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    studentsQueryable = studentsQueryable.Where(c => c.FirstName.StartsWith(firstName));
                }
                if (!string.IsNullOrWhiteSpace(fatherName))
                {
                    studentsQueryable = studentsQueryable.Where(c => c.FatherName.StartsWith(fatherName));
                }
                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    studentsQueryable = studentsQueryable.Where(c => c.LastName.StartsWith(lastName));
                }
                if (!string.IsNullOrWhiteSpace(specializationId))
                {
                    studentsQueryable = studentsQueryable.Where(c => c.SpecializationId.StartsWith(specializationId));
                }
                if (enrollmentDate != null)
                {
                    studentsQueryable = studentsQueryable.Where(c => c.EnrollmentDate.Year == (enrollmentDate));
                }

                var pD = new PagingDetalis
                {
                    TotalRows = studentsQueryable.Count()
                };
                pD.TotalPages = (int)Math.Ceiling((double)pD.TotalRows / pagingDTO.RowCount);
                pD.CurrentPage = pagingDTO.Page;
                pD.HasNexPage = pD.CurrentPage < pD.TotalPages;
                pD.HasPreviousPage = pD.CurrentPage > 1;

                Response.Headers.Add("x-paging", JsonConvert.SerializeObject(pD));

                studentsQueryable = studentsQueryable.Skip((pagingDTO.Page - 1) * pagingDTO.RowCount).Take(pagingDTO.RowCount);

                var students = studentsQueryable
                    .Include(c => c.Registrations)
                    .ThenInclude(c => c.StudentState)
                    .Include(c => c.Registrations)
                    .ThenInclude(c => c.StudyYear)
                    .Include(c => c.Registrations)
                    .ThenInclude(c => c.FinalState)
                    .Include(c => c.Registrations)
                    .ThenInclude(c => c.Year)
                    .Include(c => c.Specialization)

                    .ToList();
                return Ok(_mapper.Map<StudentsResponseDTO[]>(students));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("BySSN")]
        public IActionResult GetStudentBySSN([FromQuery] string ssn)
        {
            try
            {
                var student = this._studentRepositroy.GetBySSN(ssn);
                if (student == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Get Student By Ssn";
                    message.ControllerName = "Student";
                    return NotFound(new { message, ssn });
                }
                var response = _mapper.Map<ResponseStudentDTO>(student);
                return Ok(response);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpGet("GetStudentState")]
        // [Authorize(Roles = "ShowStudentState")]
        public IActionResult GetStudentState()
        {
            try
            {
                var state = _studentStateRepositroy.Get();
                return Ok(state);
            }

            catch
            {
                return BadRequestAnonymousError();
            }
        }


        [HttpGet("StudentNeedProcessingCount")]
        public IActionResult StudentNeedProcessingCount()
        {
            var count = _studentRepositroy.Get(c => c.Registrations.First().StudentStateId == (int)StudentStateEnum.unknown).Count();
            return Ok(count);
        }
        [HttpGet("StudentsNeedProcessing")]
        public IActionResult StudentsNeedProcessing()
        {
            var students = _studentRepositroy.Get(c => c.Registrations.First().StudentStateId == (int)StudentStateEnum.unknown);
            return Ok(_mapper.Map<TransmetedStudentResponseDTO[]>(students));
        }

        [HttpGet("StudentsNeedHelpDegreeCount")]
        public IActionResult StudentsNeedHelpDegree()
        {
            var studentCount = _registrationsRepositroy.Get(c => c.FinalStateId == (int)StudentStateEnum.unknown).Count();
            return Ok(studentCount);
        }

        [HttpGet("StudentsNeedHelpDegree")]
        public IActionResult StudentsNeedHelpDegree([FromQuery] string SSN, string specializationId, string firstName, string fatherName, string lastName, int? enrollmentDate, [FromQuery] PagingDTO pagingDTO)
        {
            var studentSSN = _registrationsRepositroy.Get(c => c.FinalStateId == (int)StudentStateEnum.unknown).Select(c => c.Ssn);

            var studentsQueryable = _studentRepositroy.GetIQueryable();
            if (!string.IsNullOrWhiteSpace(SSN))
            {
                studentsQueryable = studentsQueryable.Where(c => c.Ssn.StartsWith(SSN));
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                studentsQueryable = studentsQueryable.Where(c => c.FirstName.StartsWith(firstName));
            }
            if (!string.IsNullOrWhiteSpace(fatherName))
            {
                studentsQueryable = studentsQueryable.Where(c => c.FatherName.StartsWith(fatherName));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                studentsQueryable = studentsQueryable.Where(c => c.LastName.StartsWith(lastName));
            }
            if (!string.IsNullOrWhiteSpace(specializationId))
            {
                studentsQueryable = studentsQueryable.Where(c => c.SpecializationId.StartsWith(specializationId));
            }
            if (enrollmentDate != null)
            {
                studentsQueryable = studentsQueryable.Where(c => c.EnrollmentDate.Year == (enrollmentDate));
            }
            studentsQueryable = studentsQueryable.Where(c => studentSSN.Contains(c.Ssn));
            var pD = new PagingDetalis
            {
                TotalRows = studentsQueryable.Count()
            };
            pD.TotalPages = (int)Math.Ceiling((double)pD.TotalRows / pagingDTO.RowCount);
            pD.CurrentPage = pagingDTO.Page;
            pD.HasNexPage = pD.CurrentPage < pD.TotalPages;
            pD.HasPreviousPage = pD.CurrentPage > 1;
            Response.Headers.Add("x-paging", JsonConvert.SerializeObject(pD));
            studentsQueryable = studentsQueryable.Skip((pagingDTO.Page - 1) * pagingDTO.RowCount).Take(pagingDTO.RowCount);
            var students = studentsQueryable
                .Include(c => c.Registrations)
                .ThenInclude(c => c.StudentState)
                .Include(c => c.Registrations)
                .ThenInclude(c => c.StudyYear)
                .Include(c => c.Registrations)
                .ThenInclude(c => c.FinalState)
                .Include(c => c.Specialization)
                .ToList();
            return Ok(_mapper.Map<StudentsResponseDTO[]>(students));
        }

        [HttpGet("StudentNeedProcessing")]
        public IActionResult StudentNeedProcessing([FromQuery] string ssn)
        {
            var student = _studentRepositroy.GetIQueryable(c => c.Ssn == ssn &&
            c.Registrations.First().StudentStateId == (int)StudentStateEnum.unknown)
                        .Include(c => c.StudentSubject)
                        .ThenInclude(s => s.Subject)
                        .ThenInclude(st => st.SubjectType)
                        .FirstOrDefault();
            if (student == null)
            {
                var message = new BadRequestErrors
                {
                    Message = "لطالب غير موجود او الطالب غير منقول",
                    ControllerName = "Student",
                    ActionName = "Student Need Processing"
                };
                return BadRequest(message);
            }
            return Ok(_mapper.Map<TransmetedStudentResponseDTO>(student));
        }

        [HttpGet("StudentPreviousYearSetting")]
        public IActionResult StudentPreviousYearSetting([FromQuery] string ssn)
        {
            var student = _studentRepositroy.GetIQueryable(c => c.Ssn == ssn)
                .Include(c => c.Registrations)
                    .ThenInclude(c => c.Year)
                .LastOrDefault();
            if (student == null)
            {
                ///مافي طالب
                var message = Messages.NotFound;
                message.ActionName = "Student Previous Year Setting";
                message.ControllerName = "Student";
                message.Message = "لايوجد طالب";
                return NotFound(message);
            }
            var firstYear = student.Registrations.Last().Year.FirstYear;
            var previousYear = _yearRepositroy.GetIQueryable(c => c.FirstYear < firstYear)
                .Include(c => c.YearSystemNavigation)
                .ThenInclude(s => s.SettingYearSystem)
                .ThenInclude(ss => ss.Setting)
                .OrderBy(c => c.FirstYear)
                .LastOrDefault();
            if (previousYear == null)
            {
                //مافي سنة قبل
                var message = Messages.NotFound;
                message.ActionName = "Student Previous Year Setting";
                message.ControllerName = "Student";
                message.Message = "لايوجد سنة سابقة لتايخ تسجيل هذا الطالب ";
                return NotFound(message);
            }
            var yearSystem = previousYear.YearSystemNavigation;
            return Ok(_mapper.Map<ResponseYearSystem>(yearSystem));
        }

        #region delete student , add 
        [HttpDelete("{ssn}")]
        [Authorize(Roles = "RemoveStudent")]
        public IActionResult RemoveStudent(string ssn)
        {
            try
            {
                var student = _studentRepositroy.Find(ssn);
                if (student == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Student";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                _studentRepositroy.Remove(student, UserName());
                _studentRepositroy.Save();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch("updateStudentDgree")]
        public IActionResult UpdateStudentDegree([FromBody] AddStudentDegreeDTO AddStudentDegreeDTO)
        {
            var studentDegree = _StudentDegreeRepositroy.Get(c => c.Ssn == AddStudentDegreeDTO.Ssn).Single();
            if (studentDegree == null)
            {
                return NotFound();
            }
            studentDegree.Source = AddStudentDegreeDTO.Source;
            studentDegree.DegreeId = AddStudentDegreeDTO.DegreeId;
            studentDegree.Degree = AddStudentDegreeDTO.Degree;
            studentDegree.Date = AddStudentDegreeDTO.Date;
            _StudentDegreeRepositroy.Update(studentDegree, UserName());
            return Ok();
        }
        [HttpPatch("UpdateStudentInformation")]
        public IActionResult UpdateStudentInformation([FromBody] UpdateStudnetInformation studnetInformation)
        {
            var student = _studentRepositroy.Get(c => c.Ssn == studnetInformation.Ssn, c => c.MoreInformation).FirstOrDefault();
            _mapper.Map<UpdateStudnetInformation, Students>(studnetInformation, student);
            _studentRepositroy.Update(student, UserName());
            _studentRepositroy.Save();
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "AddStudent")]
        public IActionResult Add([FromBody] AddStudetnDTO addStudetnDTO)
        {
            try
            {
                #region main validation
                if (string.IsNullOrWhiteSpace(addStudetnDTO.FirstName) ||
                    string.IsNullOrWhiteSpace(addStudetnDTO.LastName) ||
                    string.IsNullOrWhiteSpace(addStudetnDTO.FatherName))
                    return BadRequest(Messages.EmptyName);
                addStudetnDTO.FirstName = addStudetnDTO.FirstName.Trim();
                addStudetnDTO.LastName = addStudetnDTO.LastName.Trim();
                addStudetnDTO.FatherName = addStudetnDTO.FatherName.Trim();
                if (addStudetnDTO.ConstraintId != null)
                {
                    var constraint = _constraintRepositroy.Find(addStudetnDTO.ConstraintId);
                    if (constraint == null)
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        message.Message = "القيد غير موجود";
                        return NotFound(message);
                    }
                    if (addStudetnDTO.ConstraintNumber == null)
                    {
                        var message = Messages.Empty;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        return Conflict(message);
                    }
                }
                else
                {
                    addStudetnDTO.ConstraintNumber = null;
                }
                var Nationality = _nationaliesRepositroy.Find(addStudetnDTO.NationalityId);
                if (Nationality == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "الجنسية غير موجودة";
                    return NotFound(message);
                }

                var Province = _countriesRepositroy.Find(addStudetnDTO.ProvinceId);
                if (Province == null)

                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "المحافظة غير موجودة";
                    return NotFound(message);
                }
                if (Province.MainCountry == null)
                {
                    var message = Messages.CityNotCountry;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    return Conflict(message);
                }
                var Language = _langauesRepositroy.Find(addStudetnDTO.LanguageId);
                if (Language == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "اللغة غير موجودة";
                    return NotFound(message);
                }

                var registar = _typeOfRegistarRepositroy.Find(addStudetnDTO.TypeOfRegistarId);
                if (registar == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "التسجيل غير موجود";
                    return NotFound(message);
                }

                if (!registar.IsEnabled)
                {
                    var message = Messages.Blocked;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "التسجيل غير متاح (isEnabled='flase')";
                    return Conflict(message);
                }
                var Specialization = _specializationsRepositroy.Find(addStudetnDTO.SpecializationId);
                if (Specialization == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "الاختصاص غير موجود";
                    return NotFound(message);
                }
                #endregion

                this._studentService.SetSSN(addStudetnDTO);
                var student = _mapper.Map<Students>(addStudetnDTO);
                _abstractUnitOfWork.Add(student, UserName());


                #region Degree
                var studentDegree = _mapper.Map<StudentDegree>(addStudetnDTO.StudentDegree);
                studentDegree.Ssn = student.Ssn;
                var degree = _degreeRepositroy.Find(studentDegree.DegreeId);
                if (degree == null)

                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "الشهادة غير موجودة";
                    return NotFound(message);
                }
                var degreeSource = _countriesRepositroy.Find(studentDegree.Source);
                if (degreeSource == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "مصدر الشهادة غير موجود";
                    return NotFound(message);
                }
                var degreeYear = _yearRepositroy.Find(studentDegree.Date);
                if (degreeYear == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "سنة الشهادة غير موجودة";
                    return NotFound(message);
                }
                _abstractUnitOfWork.Add(studentDegree, UserName());
                #endregion

                #region  Family with phone 
                foreach (var item in addStudetnDTO.Siblings)
                {
                    item.Ssn = student.Ssn;
                    item.Name = item.Name.Trim();
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        var message = Messages.EmptyName;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        return BadRequest(message);
                    }
                    if (item.SocialState != null)
                    {
                        var socialState = _socialStatesRepositroy.Find(item.SocialState);
                        if (socialState == null)
                        {

                            return NoContent();
                        }
                    }
                    var sbiling = _mapper.Map<Siblings>(item);
                    _abstractUnitOfWork.Add(sbiling, UserName());
                }


                foreach (var item in addStudetnDTO.Partners)
                {
                    item.Ssn = student.Ssn;
                    item.Name = item.Name.Trim();
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        var message = Messages.EmptyName;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        return BadRequest(message);
                    }
                    var Nationalit = _nationaliesRepositroy.Find(item.NationaliryId);
                    if (Nationalit == null)
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        message.Message = "الجنسية غير موجودة";
                        return NotFound(message);
                    }

                    var patner = _mapper.Map<Partners>(item);
                    _abstractUnitOfWork.Add(patner, UserName());
                }

                foreach (var item in addStudetnDTO.ClosesetPersons)
                {
                    item.Ssn = student.Ssn;
                    item.Name = item.Name.TrimEnd();
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        var message = Messages.EmptyName;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        return BadRequest(message);
                    }
                    var closestPerson = _mapper.Map<ClosesetPersons>(item);
                    _abstractUnitOfWork.Add(closestPerson, UserName());
                    foreach (var phone in item.PersonPhone)
                    {
                        phone.PersonId = closestPerson.Id;
                        var phoneType = _phoneTypeRepositroy.Find(phone.PhoneTypeId);
                        if (phoneType == null)
                        {
                            var message = Messages.NotFound;
                            message.ActionName = "Add Student";
                            message.ControllerName = "Student";
                            message.Message = "نوع التيليفون غير موجود";
                            return NotFound(message);
                        }

                        if (string.IsNullOrWhiteSpace(phone.Phone))
                        {
                            var message = Messages.Empty;
                            message.ActionName = "Add Student";
                            message.ControllerName = "Student";
                            message.Message = "رقم الطالب  فارغ";
                            return BadRequest(message);
                        }
                        var personPhone = _mapper.Map<PersonPhone>(phone);
                        _abstractUnitOfWork.Add(personPhone, UserName());
                    }
                }


                var information = _mapper.Map<MoreInformation>(addStudetnDTO.MoreInformation);
                information.Ssn = student.Ssn;
                _abstractUnitOfWork.Repository<MoreInformation>().Add(information, UserName());

                foreach (var item in addStudetnDTO.StudentPhone)
                {
                    item.Ssn = student.Ssn;
                    item.Phone = item.Phone.Trim();
                    if (string.IsNullOrWhiteSpace(item.Phone))
                    {
                        var message = Messages.EmptyName;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        return BadRequest(message);
                    }
                    var PhoneType = _phoneTypeRepositroy.Find(item.PhoneTypeId);
                    if (PhoneType == null)
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Student";
                        message.ControllerName = "Student";
                        message.Message = "نوع التيليفون غير موجود";
                        return NotFound(message);
                    }
                    var phone = _mapper.Map<StudentPhone>(item);
                    _abstractUnitOfWork.Add(phone, UserName());
                }
                #endregion

                var regisration = _mapper.Map<Registrations>(addStudetnDTO.AddRegistrationDTO);
                regisration.Ssn = student.Ssn;
                regisration.TypeOfRegistarId = addStudetnDTO.TypeOfRegistarId;
                regisration.StudyYearId = (int)StudyYearEnum.FirstYear;
                if (!student.IsItTransferredToUs())
                {
                    regisration.StudentStateId = (int)StudentStateEnum.newStudent;
                }
                else
                {
                    regisration.StudentStateId = (int)StudentStateEnum.unknown;
                }
                _abstractUnitOfWork.Add(regisration, UserName());
                ///get the year depend on his registratin
                var year = _yearRepositroy.GetIQueryable(c => c.Id == regisration.YearId)
                    .Include(y => y.ExamSemester)
                    .FirstOrDefault();
                if (year == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "السنة غير موجودة";
                    return NotFound(message);
                }
                ///get all study paln for this specialization 
                ///incloud with year and subjects
                var studyPaln = _studyPlanRepositroy.GetIQueryable(c => c.SpecializationId == Specialization.Id && c.Year.FirstYear <= year.FirstYear)
                    .Include(c => c.Year)
                    .ThenInclude(y => y.ExamSemester)
                    .Include(c => c.StudySemester)
                    .ThenInclude(ss => ss.Subjects)
                    .OrderByDescending(c => c.Year.FirstYear).First();
                if (studyPaln == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Student";
                    message.ControllerName = "Student";
                    message.Message = "لا يمكن إاضافة طالب ما لم يكن هناك خطة";
                    return Conflict(message);
                }
                #region add Subject for student
                ////
                ///get the last study plan for this year
                //var studyPaln = studyPalns.Where(c => c.Year.FirstYear <= year.FirstYear).First();

                var firstYearSemester = studyPaln.StudySemester.Where(c => c.StudyYearId == (int)StudyYearEnum.FirstYear);
                // first Semester
                {
                    var subjects = firstYearSemester.Where(c => c.Number == (int)StudySemesterNumberEunm.First).First().Subjects;
                    int? firstExamSemseterId = null;
                    if (!student.IsItTransferredToUs())
                        firstExamSemseterId = year.ExamSemester.Where(c => c.SemesterNumber == 1).First().Id;
                    foreach (var item in subjects)
                    {
                        var studentSubject = new StudentSubject()
                        {
                            SubjectId = item.Id,
                            Ssn = student.Ssn,
                            PracticalDegree = null,
                            TheoreticlaDegree = null,
                            ExamSemesterId = firstExamSemseterId,
                        };

                        _abstractUnitOfWork.Add(studentSubject, UserName());
                    }
                }
                //secound Semester
                {
                    var subjects = firstYearSemester.Where(c => c.Number == 2).First().Subjects;
                    int? secoundExamSemseterId = null;
                    if (!student.IsItTransferredToUs())
                        secoundExamSemseterId = year.ExamSemester.Where(c => c.SemesterNumber == 2).First().Id;

                    foreach (var item in subjects)
                    {
                        var studentSubject = new StudentSubject()
                        {
                            //Last update :By gawad
                            SubjectId = item.Id,
                            Ssn = student.Ssn,
                            PracticalDegree = null,
                            ///shoud replace with null
                            TheoreticlaDegree = null,
                            ExamSemesterId = secoundExamSemseterId,
                        };
                        _abstractUnitOfWork.Add(studentSubject, UserName());
                    }

                }

                #endregion


                _abstractUnitOfWork.Commit();
                return Ok(new { SSN = student.Ssn });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region weak api
        [HttpPost("AddClosestPerson/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult AddClosestPerson(string ssn, [FromBody] AddClosestPersonForStudent addClosestPersonForStudent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addClosestPersonForStudent.Name))
                {
                    var message = Messages.Null;
                    message.ActionName = "Add Closest Person";
                    message.ControllerName = "Student";
                    return BadRequest(message);
                }

                addClosestPersonForStudent.Name = addClosestPersonForStudent.Name.Trim();
                addClosestPersonForStudent.Address = addClosestPersonForStudent.Address.Trim();

                var Ssn = _studentRepositroy.Find(ssn);
                if (Ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Closest Person";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var Closest = _mapper.Map<ClosesetPersons>(addClosestPersonForStudent);
                _closesetPersonsRepositroy.Add(Closest, UserName());
                _closesetPersonsRepositroy.Save();
                return Ok(Closest);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost("AddPartners/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult AddPartners(string ssn, [FromBody] AddPartnersForStudent addPartnersForStudent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addPartnersForStudent.Name))
                {
                    var message = Messages.Null;
                    message.ActionName = "Add Partners";
                    message.ControllerName = "Student";
                    return BadRequest(message);
                }

                addPartnersForStudent.Name = addPartnersForStudent.Name.Trim();
                addPartnersForStudent.PartnerWork = addPartnersForStudent.PartnerWork.Trim();

                var Ssn = _studentRepositroy.Find(ssn);
                if (Ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Partners";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }

                var patners = _mapper.Map<Partners>(addPartnersForStudent);
                _partnersRepositroy.Add(patners, UserName());
                _partnersRepositroy.Save();
                return Ok(patners);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost("AddReperations/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult AddReperations(string ssn, [FromBody] AddReprationsForStudent addReprationsForStudent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addReprationsForStudent.Reparation))
                {
                    var message = Messages.Null;
                    message.ActionName = "Add Reperations";
                    message.ControllerName = "Student";
                    return BadRequest(message);
                }

                addReprationsForStudent.Reparation = addReprationsForStudent.Reparation.Trim();

                var Ssn = _studentPhoneRepositroy.Find(ssn);
                if (Ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Reperations";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var reparations = _mapper.Map<Reparations>(addReprationsForStudent);
                _reparationsRepositroy.Add(reparations, UserName());
                _reparationsRepositroy.Save();
                return Ok(reparations);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost("AddSanctions/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult AddSanctions(string ssn, [FromBody] AddSanctionsForStudent addSanctionsForStudent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addSanctionsForStudent.Sanction))
                {
                    var message = Messages.Null;
                    message.ActionName = "Add Sanctions";
                    message.ControllerName = "Student";
                    return BadRequest(message);
                }
                addSanctionsForStudent.Sanction = addSanctionsForStudent.Sanction.Trim();

                var Ssn = _studentPhoneRepositroy.Find(ssn);
                if (Ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Sanctions";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var sanctions = _mapper.Map<Sanctions>(addSanctionsForStudent);
                _sanctionsRepositroy.Add(sanctions, UserName());
                _sanctionsRepositroy.Save();
                return Ok(sanctions);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost("AddPhone/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult AddPhone(string ssn, [FromBody] AddPhoneForStudent addPhoneForStudent)
        {
            try
            {
                var Ssn = _studentPhoneRepositroy.Find(ssn);
                if (Ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Phone";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var phone = _mapper.Map<StudentPhone>(addPhoneForStudent);
                _studentPhoneRepositroy.Add(phone, UserName());
                _studentPhoneRepositroy.Save();
                return Ok(phone);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("RemoveSibilng/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult RemoveSibilng(string ssn, [FromBody] int SibilngId)
        {
            try
            {
                var SSn = _studentRepositroy.Find(ssn);
                if (ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Sbiling";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var Sibling = _siblingsRepositroy.Find(SibilngId);
                if (Sibling == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Sbiling";
                    message.ControllerName = "Student";
                    message.Message = "لايوجد اخوة للطالب بهذا المعرف";
                    return NotFound(message);
                }
                _siblingsRepositroy.Remove(Sibling, UserName());
                _siblingsRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("RemoveClosestPerson/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult RemoveClosestPerson(string ssn, [FromBody] int ClosestId)
        {
            try
            {
                var SSn = _studentRepositroy.Find(ssn);
                if (ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Closest Person";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var Closest = _closesetPersonsRepositroy.Find(ClosestId);
                if (Closest == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Closest Person";
                    message.ControllerName = "Student";
                    message.Message = "لايوجد اقارب للطالب بهذا المعرف";
                    return NotFound(message);
                }
                _closesetPersonsRepositroy.Remove(Closest, UserName());
                _closesetPersonsRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("RemoveReperations/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult RemoveReperations(string ssn, [FromBody] int RepId)
        {
            try
            {
                var SSn = _studentRepositroy.Find(ssn);
                if (ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Reperation";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var rep = _reparationsRepositroy.Find(RepId);
                if (rep == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Reperation";
                    message.ControllerName = "Student";
                    message.Message = "المثوبات غير موجودة";
                    return NotFound(message);
                }

                _reparationsRepositroy.Remove(rep, UserName());
                _reparationsRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpDelete("RemoveSanctions/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult RemoveSanctions(string ssn, [FromBody] int SanId)
        {
            try
            {
                var SSn = _studentRepositroy.Find(ssn);
                if (ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Sanction";
                    message.ControllerName = "Student";
                    return NotFound(message);
                }
                var san = _sanctionsRepositroy.Find(SanId);
                if (san == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Sanction";
                    message.ControllerName = "Student";
                    message.Message = "العقوبات غير موجودة";
                    return NotFound(message);
                }
                _sanctionsRepositroy.Remove(san, UserName());
                _sanctionsRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("RemovePartners/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult RemovePartners(string ssn, [FromBody] int parnterId)
        {
            try
            {
                var SSn = _studentRepositroy.Find(ssn);
                if (ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Partners";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var partner = _partnersRepositroy.Find(parnterId);
                if (partner == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Partners";
                    message.ControllerName = "Student";
                    message.Message = "معرف الزوجة غير موجود";
                    return NotFound(message);
                }
                _partnersRepositroy.Remove(partner, UserName());
                _partnersRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

        [HttpPost("AddSbiling/{ssn}")]
        [Authorize(Roles = "UpdateStudent")]
        public IActionResult AddSbiling(string ssn, [FromBody] AddSbiling addSbiling)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addSbiling.Name))
                {
                    var message = Messages.Null;
                    message.ActionName = "Add Sbiling";
                    message.ControllerName = "Student";
                    return BadRequest(message);
                }
                addSbiling.Name = addSbiling.Name.Trim();
                addSbiling.Address = addSbiling.Address.Trim();
                addSbiling.Work = addSbiling.Work.Trim();

                var Ssn = _studentRepositroy.Find(ssn);
                if (Ssn == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Sbiling";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }

                var sib = _mapper.Map<Siblings>(addSbiling);
                _siblingsRepositroy.Add(sib, UserName());
                _siblingsRepositroy.Save();
                return Ok(sib);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost("UploadImage")]
        [Authorize(Roles = "UploadImage")]
        public IActionResult UploadImage([FromBody] AddStudentAttachment addStudentAttachment)
        {
            string newPath = "";
            try
            {
                var student = _studentRepositroy.Find(addStudentAttachment.Ssn);
                if (student == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Upload Image";
                    message.ControllerName = "Student";
                    message.Message = "الطالب غير موجود";
                    return NotFound(message);
                }
                var attachmnet = _attatchmentsRepositroy.Find(addStudentAttachment.AttachmentId);
                if (attachmnet == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Upload Image";
                    message.ControllerName = "Student";
                    message.Message = "المرفق غير موجود";
                    return NotFound(message);
                }

                if (!_fileWriter.UploadFile(addStudentAttachment.Attachemnt, attachmnet.Name, student.Ssn + student.FullName, out newPath))
                {
                    return BadRequestAnonymousError();
                }
                if (System.IO.File.Exists(newPath))
                {
                    var studentAttachemnt = new StudentAttachment()
                    {
                        Ssn = addStudentAttachment.Ssn,
                        AttachmentId = addStudentAttachment.AttachmentId,
                        Attachemnt = newPath,
                        Note = addStudentAttachment.Note
                    };

                    _studentAttachmentRepositroy.Add(studentAttachemnt, UserName());
                    _studentAttachmentRepositroy.Save();
                }
                else
                {
                    return BadRequest();
                }
                return Ok();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                if (System.IO.File.Exists(newPath))
                {
                    System.IO.File.Delete(newPath);
                }
                return BadRequest();
            }
        }

        #endregion
        [HttpPost("Register/{SSN}")]
        public IActionResult Register(string ssn, [FromBody] CreateRegistrationDTO createRegistrationDTO)
        {
            try
            {

                var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == ssn)
                    .Include(c => c.Registrations)
                    .Include(c => c.StudentSubject)
                    .ThenInclude(ss => ss.Subject)
                    .ThenInclude(s => s.SubjectType)
                    .Include(c => c.StudentSubject)
                    .ThenInclude(c => c.Subject)
                    .ThenInclude(c => c.StudySemester)
                    .ThenInclude(c => c.Studyplan)
                    .FirstOrDefault();
                if (student == null)
                {
                    return NotFound();
                }
                var prviousYear = _abstractUnitOfWork.Repository<Years>().Get(c => c.Id == student.Lastregistration.YearId).FirstOrDefault();
                if (!prviousYear.Blocked)
                {
                    return Conflict();
                }
                var year = _abstractUnitOfWork.Repository<Years>().Get(c => c.FirstYear == prviousYear.SecondYear, c => c.YearSystemNavigation.SettingYearSystem, c => c.ExamSystemNavigation, c => c.ExamSemester).FirstOrDefault();
                if (year == null)
                {
                    return Conflict();
                }
                var newRegistration = student.NewRegistration();
                newRegistration.TypeOfRegistarId = createRegistrationDTO.TypeOfRegistarId;
                newRegistration.CardDate = createRegistrationDTO.CardDate;
                newRegistration.SoldierDate = createRegistrationDTO.SoldierDate;
                newRegistration.Note = createRegistrationDTO.Note;
                newRegistration.CardNumber = createRegistrationDTO.CardNumber;
                newRegistration.YearId = year.Id;
                if (newRegistration.ActuallyStudyYear == (int)StudyYearEnum.SecoundYear && newRegistration.StudentStateId == (int)StudentStateEnum.Drained)
                {
                    if (student.StudentSubjectWithoutDuplicate.Where(ss => !ss.IsSuccessAnyWay()).ToList().Count <= year.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumberOfSubjectsForStudentsOutsideTheInstitute).First().Count)
                    {
                        newRegistration.StudentStateId = (int)StudentStateEnum.OutOfInstitute;
                    }
                }
                _abstractUnitOfWork.Add(newRegistration, UserName());
                var faildSubject = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsSuccessAnyWay()).ToList();
                var justFaildSubject = false;
                if (newRegistration.ActuallyStudyYear == (int)StudyYearEnum.FirstYear)
                    justFaildSubject = true;
                if (newRegistration.ActuallyStudyYear == (int)StudyYearEnum.SecoundYear && newRegistration.StudentStateId != (int)StudentStateEnum.successful && newRegistration.StudentStateId != (int)StudentStateEnum.transported)
                    justFaildSubject = true;

                if (year.IsDoubleExam)
                {
                    var firstSemesterSubjectAndNormaiSubject = faildSubject.Where(c => c.IsNominate() || c.MainSemesterNumber == 1).ToList();
                    foreach (var item in firstSemesterSubjectAndNormaiSubject)
                    {
                        var studentSubject = new StudentSubject()
                        {
                            ExamSemesterId = year.GetSemeserIdByNumber(1),
                            Ssn = item.Ssn,
                            SubjectId = item.SubjectId,
                            PracticalDegree = item.IsNominate() ? item.PracticalDegree : null
                        };
                        _abstractUnitOfWork.Add(studentSubject, SentencesHelper.System);
                    }
                    var nonNormaitSubjectSecoundSemester = faildSubject.Where(c => !c.IsNominate() && c.MainSemesterNumber == 2).ToList();
                    foreach (var item in nonNormaitSubjectSecoundSemester)
                    {
                        var studentSubject = new StudentSubject()
                        {
                            ExamSemesterId = year.GetSemeserIdByNumber(2),
                            Ssn = item.Ssn,
                            SubjectId = item.SubjectId,
                            PracticalDegree = item.IsNominate() ? item.PracticalDegree : null
                        };
                        _abstractUnitOfWork.Add(studentSubject, SentencesHelper.System);
                    }
                }
                else
                {
                    faildSubject.ForEach(subject =>
                    {
                        var studentSubject = new StudentSubject()
                        {
                            ExamSemesterId = year.GetSemeserIdByNumber((int)subject.MainSemesterNumber),
                            Ssn = subject.Ssn,
                            SubjectId = subject.SubjectId,
                            PracticalDegree = subject.IsNominate() ? subject.PracticalDegree : null
                        };
                        _abstractUnitOfWork.Add(studentSubject, SentencesHelper.System);
                    });
                }
                if (!justFaildSubject)
                {
                    var studyPlanId = student.StudentSubject.First().Subject.StudySemester.Studyplan.Id;
                    var secoundYearSubjectByStudyPlan = _abstractUnitOfWork.Repository<Subjects>().Get(c => c.StudySemester.Studyplan.Id == studyPlanId && c.StudySemester.StudyYearId == (int)StudyYearEnum.SecoundYear, c => c.StudySemester).ToList();

                    foreach (var item in secoundYearSubjectByStudyPlan)
                    {
                        if (student.StudentSubject.Any(c => c.SubjectId == item.Id && c.IsSuccess()))
                        {
                            continue;
                        }
                        double? PracticalDegree = null;
                        if (student.StudentSubject.Any(c => c.SubjectId == item.Id))
                        {
                            PracticalDegree = student.StudentSubjectWithoutDuplicate.Where(c => c.SubjectId == item.Id).First().IsNominate() ? student.StudentSubjectWithoutDuplicate.Where(c => c.SubjectId == item.Id).First().PracticalDegree : null;
                        }
                        if (PracticalDegree != null)
                        {

                        }
                        var studentSubject = new StudentSubject()
                        {
                            PracticalDegree = PracticalDegree,
                            Ssn = student.Ssn,
                            SubjectId = item.Id,
                            ExamSemesterId = year.GetSemeserIdByNumber((int)item.MainSemesterNumber),
                        };
                        _abstractUnitOfWork.Add(studentSubject, SentencesHelper.System);

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
        [HttpGet("StudntCabableToDownload")]
        public IActionResult StudntCabableToDownload()
        {
            var p1 = (int)StudyYearEnum.FirstYear;
            var p2=  (int)StudentStateEnum.successful;
            var registrations= _abstractUnitOfWork.Repository<Registrations>().GetIQueryable(c=>c.StudyYearId.Equals(p1)&&c.StudentStateId.Equals(p2)).GroupBy(c=>c.Ssn).Select(c=>c.Last()).ToList();
            var SSNs= registrations.Select(c=>c.Ssn).ToList();
           var students= _abstractUnitOfWork.Repository<Students>().GetIQueryable(c=>SSNs.Contains(c.Ssn))
           .Include(c=>c.StudentSubject)
           .ThenInclude(c=>c.Subject)
           .ThenInclude(c=>c.SubjectType)
           .Include(c=>c.StudentSubject)
           .ThenInclude(c=>c.Subject)
           .ThenInclude(c=>c.StudySemester)
           .ToList();
           var studentCnaDownload = students.Where(c=>c.StudentSubjectWithoutDuplicate.Where(k=>k.PracticalDegree!=null&&k.TheoreticlaDegree!=null&& k.Subject.StudySemester.StudyYearId==(int)StudyYearEnum.FirstYear).ToList().Where(s=>!s.IsSuccess()).Count()<BusinessLogicHelper.NumberOfDownloadableSubject).ToList();
            return  Ok(studentCnaDownload.Select(c=>c.Ssn).Count());
        }
    }
}
