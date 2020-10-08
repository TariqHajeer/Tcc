using System;
using System.Linq;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using DAL.infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : MyBaseController
    {
        AbstractUnitOfWork _abstractUnitOfWork;
        public DefaultController(AbstractUnitOfWork abstractUnitOfWork, IMapper mapper) : base(mapper)
        {
            _abstractUnitOfWork = abstractUnitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var x = _abstractUnitOfWork.Repository<Students>().Get(c => c.Ssn == "S15001", c => c.StudentSubject).FirstOrDefault();
            return Ok();
        }
        //[HttpGet]
        //public IActionResult Test()
        //{
        //    return OK();
        //}

        //[HttpGet("t/{ssn}")]
        //public IActionResult Tet(string ssn)
        //{
        //    try
        //    {
        //        var student = _abstractUnitOfWork.Repository<StudentSubject>().GetIQueryable(c => c.Ssn == ssn)
        //            .Include(c => c.Subject)
        //            .ThenInclude(c => c.SubjectType)
        //            .Include(c=>c.Subject)
        //            .ThenInclude(c=>c.StudySemester)
        //            .ToList();
        //        var firstSemeserSubject = student.Where(c => c.MainSemesterNumber == 1);
        //        var secoundSemeserSubject= student.Where(c => c.MainSemesterNumber == 1);
        //        var sucessFirstSemesterSuject = firstSemeserSubject.Where(c => c.IsSuccess());
        //        var sucessSeocunSemesteSubject = secoundSemeserSubject.Where(c => c.IsSuccess());
        //        var non = student.Where(c=>c.PracticalDegree==null||c.TheoreticlaDegree==null).Where(c=>! sucessFirstSemesterSuject.Select(fc=>fc.SubjectId).Union(sucessSeocunSemesteSubject.Select(ss=>ss.SubjectId)).Contains(c.SubjectId));
        //        var x = new
        //        {
        //            F = sucessFirstSemesterSuject.Select(c => new { c.Subject.Id, c.Subject.Name}),
        //            S = sucessSeocunSemesteSubject.Select(c => new { c.Subject.Id, c.Subject.Name }),
        //            none = non.Select(c => new { c.Subject.Id, c.Subject.Name })
        //        };
        //        //var sucess = student.Where(c => c.IsSuccess());
        //        //var x = new { count = sucess.Count(), subject = sucess.Select(c => new { c.Id, c.Subject.Name })};
        //        //return Ok(x);
        //        return Ok(x); ;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok( ex.Message);
        //    }
        //}
        //[HttpGet("unSecuess/{SSN}")]
        //public IActionResult Un(string SSN)
        //{
        //    var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == SSN)
        //        .Include(c => c.StudentSubject)
        //        .ThenInclude(c => c.Subject)
        //        .ThenInclude(c => c.SubjectType).FirstOrDefault();
        //    var s = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsSuccess());
        //    return Ok(s.Select(c =>new { c.Subject.Name,c.PracticalDegree,c.TheoreticlaDegree }));
        //}
    }
}