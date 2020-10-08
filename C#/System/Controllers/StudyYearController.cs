using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyYearController : MyBaseController
    {
        IRepositroy<StudyYear> _studyYearRepositroy;
        public StudyYearController(IRepositroy<StudyYear> repositroy, IMapper mapper) : base(mapper)
        {
            _studyYearRepositroy = repositroy;
        }
        [HttpGet]
        [Authorize(Roles ="ShowStudyYear")]
        public IActionResult Get()
        {
            var studyYears = _studyYearRepositroy.Get();
            return Ok(studyYears);
        }
    }
}