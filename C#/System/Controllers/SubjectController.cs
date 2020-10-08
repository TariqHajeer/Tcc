using System.DTO.SubjectDTO;
using System.Linq;
using AutoMapper;
using DAL.Classes;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Static;
using System.Services;
using System.IServices;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : MyBaseController
    {
        ISubjectServices _subjectServices;
        IRepositroy<Subjects> _subjectRepositroy;
        IRepositroy<SubjectType> _subjectTypeRepositroy;
        IRepositroy<StudySemester> _studySemesterRepositroy;
        IRepositroy<StudentSubject> _studentSubjectRepositroy;
        IRepositroy<EquivalentSubject> _equivalentSubjectRepositroy;
        IRepositroy<DependenceSubject> _dependenceSubjectRepositroy;
        AbstractUnitOfWork _abstractUnitOfWork;
        public SubjectController(IRepositroy<Subjects> repositroy, IMapper mapper,
            IRepositroy<SubjectType> subjectTypeRepositroy,
            IRepositroy<StudySemester> studySemesterRepositroy,
            IRepositroy<StudentSubject> studentSubjectRepositroy,
            IRepositroy<EquivalentSubject> equivalentSubjectRepositroy,
            IRepositroy<DependenceSubject> dependenceSubjectRepositroy,

            AbstractUnitOfWork abstractUnitOfWork,
            ISubjectServices subjectServices) : base(mapper)
        {
            _subjectServices = subjectServices;
            _subjectRepositroy = repositroy;
            _subjectTypeRepositroy = subjectTypeRepositroy;
            _studySemesterRepositroy = studySemesterRepositroy;
            _equivalentSubjectRepositroy = equivalentSubjectRepositroy;
            _dependenceSubjectRepositroy = dependenceSubjectRepositroy;
            _abstractUnitOfWork = abstractUnitOfWork;
            _studentSubjectRepositroy = studentSubjectRepositroy;
        }
        [HttpPatch]
        public IActionResult Update([FromBody] UpdateSubjectDto UpdateSubjectDto)
        {
            var subject = _abstractUnitOfWork.Repository<Subjects>().Find(UpdateSubjectDto.Id);
            if (subject == null)
            {
                return NotFound();
            }
            if (subject.StudySemesterId != UpdateSubjectDto.StudySemesterId)
            {
                var dependancySubject = _abstractUnitOfWork.Repository<DependenceSubject>().Get(c => c.SubjectId == UpdateSubjectDto.Id || c.DependsOnSubjectId == UpdateSubjectDto.Id).ToList();
                dependancySubject.ForEach(ds =>
                {
                    _abstractUnitOfWork.Remove(ds, SentencesHelper.System);
                });
                var equivalentSubject = _abstractUnitOfWork.Repository<EquivalentSubject>().Get(c => c.FirstSubject == UpdateSubjectDto.Id || c.SecoundSubject == UpdateSubjectDto.Id).ToList();
                equivalentSubject.ForEach(es =>
                {
                    _abstractUnitOfWork.Remove(es, SentencesHelper.System);
                });
            }
            _mapper.Map<UpdateSubjectDto, Subjects>(UpdateSubjectDto, subject);
            _abstractUnitOfWork.Update(subject, UserName());
            return Ok();
        }
        
        [HttpGet("bycode")]
        [Authorize(Roles = "ShowSubject")]
        public IActionResult ByCode([FromQuery] string code)
        {
            try
            {
                var subject = _subjectRepositroy.GetIQueryable(c => c.SubjectCode == code)
                    .Include(c => c.SubjectType)
                    .FirstOrDefault();
                if (subject == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "By Code";
                    message.ControllerName = "Subject";
                    message.Message = "المادة غير موجودة";
                    return NotFound(message);
                }
                var responseSubject = _mapper.Map<ResponseSubjectDTO>(subject);
                return Ok(responseSubject);
            }
            catch (Exception)
            {

                return BadRequestAnonymousError();
            }
        }
        [HttpPost]
        //[Authorize(Roles = "AddSubject")]
        public IActionResult Add([FromBody] AddSubjectDTO addSubjectDTO)
        {
            try
            {
                addSubjectDTO.Name = addSubjectDTO.Name.Trim();
                var simelarCode = _subjectRepositroy.Get(c => c.SubjectCode == addSubjectDTO.SubjectCode).FirstOrDefault();
                if (simelarCode != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add";
                    message.ControllerName = "Subject";
                    return Conflict(message);
                }

                var subjectType = _subjectTypeRepositroy.Find(addSubjectDTO.SubjectTypeId);
                if (subjectType == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add";
                    message.ControllerName = "Subject";
                    message.Message = "نوع المادة غير موجود";
                    return Conflict(message);
                }
                var semester = _studySemesterRepositroy.GetIQueryable(c => c.Id == addSubjectDTO.StudySemesterId)
                .Include(c => c.Studyplan)
                .ThenInclude(c => c.Year)
                .FirstOrDefault();
                if (semester == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add";
                    message.ControllerName = "Subject";
                    message.Message = "الفصل التدريسي غير موجود";
                    return NotFound(message);
                }
                if (semester.Studyplan.Year.Blocked)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add";
                    message.ControllerName = "Subject";
                    message.Message = "لا يمكن تعديل الخطة ";
                    return Conflict(message);
                }
                var subject = _mapper.Map<Subjects>(addSubjectDTO);
                _abstractUnitOfWork.Add(subject, UserName());
                subject.StudySemester = semester;
                foreach (var item in addSubjectDTO.DependencySubjectsId)
                {
                    var depencaySubject = _subjectRepositroy.GetIQueryable(c => c.Id == item)
                        .Include(c => c.StudySemester).FirstOrDefault();
                    var checking = _subjectServices.CheckSubjectAndDepandcySubject(subject, depencaySubject);
                    if (!checking)
                    {
                        return Conflict();//؟؟؟؟
                    }
                    DependenceSubject dependenceSubject = new DependenceSubject
                    {
                        SubjectId = subject.Id,
                        DependsOnSubjectId = item
                    };
                    _abstractUnitOfWork.Add(dependenceSubject, UserName());
                }
                foreach (var item in addSubjectDTO.EquivalentSubjectsId)
                {
                    var equivalentSubject = _subjectRepositroy.Find(item);
                    if (equivalentSubject == null)
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add";
                        message.ControllerName = "Subject";
                        message.Message = "المادةالمكافئة غير موجودة";
                        return NotFound(message);
                    }
                    EquivalentSubject eq = new EquivalentSubject()
                    {
                        FirstSubject = subject.Id,
                        SecoundSubject = item
                    };
                    _abstractUnitOfWork.Add(eq, UserName());
                }
                _abstractUnitOfWork.Commit();
                return Ok(subject);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetSubjectCouldBeResset/{SSN}")]
        [Authorize(Roles = "ShowSubject")]
        public IActionResult GetSubjectCouldBeResset(string ssn)
        {
            var studentSubject = _studentSubjectRepositroy.Get(c => c.Ssn == ssn && c.ExamSemester.Year.Blocked == false).ToList();
            return Ok(studentSubject);
        }
        [HttpDelete]
        //[Authorize(Roles = "RemoveSubject")]
        public IActionResult Remove([FromForm] int id)
        {
            try
            {

                Subjects subject = _abstractUnitOfWork.Repository<Subjects>().Find(id);
                if (subject == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove";
                    message.ControllerName = "Subject";
                    message.Message = "المادة غير موجودة";
                    return NotFound(message);
                }
                var dep = _dependenceSubjectRepositroy.Get(c => c.SubjectId == id || c.DependsOnSubjectId == id);
                if (dep != null)
                {
                    foreach (var item in dep)
                    {
                        _abstractUnitOfWork.Remove(item, UserName());
                    }
                }
                var equ = _equivalentSubjectRepositroy.Get(c => c.FirstSubject == id
                || c.SecoundSubject == id).ToList();
                if (equ != null)
                {
                    foreach (var item in equ)
                    {
                        _abstractUnitOfWork.Remove(item, UserName());
                    }
                }
                _abstractUnitOfWork.Remove(subject, UserName());
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("RemoveDependencySubject/{SubjectId}")]
        [Authorize(Roles = "UpdateSubject")]
        public IActionResult RemoveDependencySubject(int SubjectId, [FromForm] int DependencyId)
        {
            try
            {
                DependenceSubject DependencySub = _dependenceSubjectRepositroy
                    .Get(c => (c.SubjectId == SubjectId && c.DependsOnSubjectId == DependencyId)).FirstOrDefault();
                if (DependencySub == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Dependency Subject";
                    message.ControllerName = "Subject";
                    message.Message = "المادة المعتمدة غير موجودة";
                    return NotFound(message);
                }
                _dependenceSubjectRepositroy.Remove(DependencySub, UserName());
                _dependenceSubjectRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("AddDependencySubject/{id}")]
        //[Authorize(Roles = "UpdateSubject")]
        public IActionResult AddDependencySubject(int id, [FromForm] int dependencyId)
        {
            try
            {
                var similar = _dependenceSubjectRepositroy.Get(c => c.SubjectId == id && c.DependsOnSubjectId == dependencyId).FirstOrDefault();
                if (similar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Dependency Subject";
                    message.ControllerName = "Subject";
                    return Conflict(message);
                }
                Subjects subject = _subjectRepositroy.GetIQueryable(s => s.Id == id)
                .Include(s => s.StudySemester)
                .FirstOrDefault();
                if (subject == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Dependency Subject";
                    message.ControllerName = "Subject";
                    message.Message = "المادة غير موجودة";
                    return NotFound(message);
                }

                bool found;
                Subjects dependancySubject = _subjectRepositroy.GetIQueryable(c => c.Id == dependencyId)
                    .Include(c => c.StudySemester)
                    .FirstOrDefault();
                var chicking = _subjectServices.CheckSubjectAndDepandcySubject(subject, dependancySubject, out found);
                if (!chicking)
                {
                    if (!found)
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Dependency Subject";
                        message.ControllerName = "Subject";
                        return NotFound(message);
                    }
                    return Conflict();
                }

                DependenceSubject dep = new DependenceSubject()
                {
                    SubjectId = id,
                    DependsOnSubjectId = dependencyId
                };
                _dependenceSubjectRepositroy.Add(dep, UserName());
                _dependenceSubjectRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("AddEquivlantSubject")]
        //[Authorize(Roles = "UpdateSubject")]
        public IActionResult AddEquivlantSubject([FromForm] int FirstSubject, [FromForm] int SecondSubject)
        {
            try
            {
                var similar = _equivalentSubjectRepositroy
                    .GetIQueryable(c => c.FirstSubject == FirstSubject && c.SecoundSubject == SecondSubject
                    || c.SecoundSubject == FirstSubject && c.FirstSubject == SecondSubject).FirstOrDefault();
                if (similar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Equivlant Subject";
                    message.ControllerName = "Subject";
                    return Conflict(message);
                }
                Subjects subject = _subjectRepositroy.Find(FirstSubject);
                if (subject == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Equivlant Subject";
                    message.ControllerName = "Subject";
                    message.Message = "المادة غير موجودة";
                    return NotFound(message);
                }

                Subjects EquivlantSub = _subjectRepositroy.Find(SecondSubject);
                if (EquivlantSub == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Equivlant Subject";
                    message.ControllerName = "Subject";
                    message.Message = "المادة غير موجودة";
                    return NotFound(message);
                }
                EquivalentSubject equ = new EquivalentSubject()
                {
                    FirstSubject = subject.Id,
                    SecoundSubject = EquivlantSub.Id
                };
                _equivalentSubjectRepositroy.Add(equ, UserName());
                _equivalentSubjectRepositroy.Save();
                return Ok(equ);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("RemoveEquivlantSubject")]
        //[Authorize(Roles = "UpdateSubject")]
        public IActionResult RemoveEquivlantSubject([FromForm] int FirstSubject, [FromForm] int SecondSubject)
        {
            try
            {
                var equalSubject = _equivalentSubjectRepositroy.Get(c => (c.FirstSubject == FirstSubject && c.SecoundSubject == SecondSubject) || (c.FirstSubject == SecondSubject && c.SecoundSubject == FirstSubject)).FirstOrDefault();
                if (equalSubject == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Equivlant Subject";
                    message.ControllerName = "Subject";
                    message.Message = "المادة المكافئة غير موجودة";
                    return NotFound(message);
                }
                _equivalentSubjectRepositroy.Remove(equalSubject, UserName());
                _equivalentSubjectRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }

        }

    }
}