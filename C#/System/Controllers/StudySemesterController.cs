using System;
using System.Collections.Generic;
using System.DTO.ExamSemesterDTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.DTO.SubjectDTO;
using DAL.HelperEums;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudySemesterController : MyBaseController
    {
        public StudySemesterController(IRepositroy<StudySemester> repositroy, IMapper mapper) : base( mapper)
        {
        }
    }
}