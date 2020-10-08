using System;
using System.Collections.Generic;
using System.DTO.LanguageDTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.infrastructure;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Static;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : MyBaseController
    {
        IRepositroy<Langaues> _LangageRepositroy;
        public LanguagesController(IRepositroy<Langaues> repositroy, IMapper mapper) : base(mapper)
        {
            _LangageRepositroy = repositroy;
        }
        [Authorize(Roles = "AddLanguage")]
        [HttpPost]
        public IActionResult AddLanguage([FromBody]AddLanguageDTO addLanguageDTO)
        {
            try
            {
                addLanguageDTO.Name = addLanguageDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addLanguageDTO.Name))
                {
                    var message =Static.Messages.EmptyName;
                    message.ActionName = "Add Language ";
                    message.ControllerName = "Language";
                    return BadRequest(message);
                }
                if (_LangageRepositroy.Get(c => c.Name == addLanguageDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Language ";
                    message.ControllerName = "Language";
                    return Conflict(message);
                }
                var lan = _mapper.Map<Langaues>(addLanguageDTO);
                _LangageRepositroy.Add(lan, UserName());
                _LangageRepositroy.Save();
                return Ok(lan);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var x = _LangageRepositroy.Get(c => c.IsEnabled);
            return Ok(x);
        }
        [Authorize(Roles = "ShowLanguages")]
        [HttpGet]
        public IActionResult ShowLanguages()
        {
            try
            {
                var lan = _mapper.Map<LanguageResponseDTO[]>(_LangageRepositroy.Get());
                return Ok(lan);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateLanguage")]
        public IActionResult UpdateLanguage(int id, [FromBody]UpdateLanguageDTO updateLanguageDTO)
        {
            try
            {
                updateLanguageDTO.Name = updateLanguageDTO.Name.Trim();
                var simelar = _LangageRepositroy.Get(c => c.Name == updateLanguageDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Language ";
                    message.ControllerName = "Language";
                    return Conflict(message);
                }

                var orginalLanguage = _LangageRepositroy.Find(id);
                if (orginalLanguage == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Language ";
                    message.ControllerName = "Language";
                    message.Message = "اللغة غير موجودة";
                    return NotFound(message);
                }
                orginalLanguage = _mapper.Map(updateLanguageDTO, orginalLanguage);
                _LangageRepositroy.Update(orginalLanguage,UserName());
                _LangageRepositroy.Save();  
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveLanguage")]
        public IActionResult RempoveLanguage(int id)
        {
            try
            {
                var lan = _LangageRepositroy.Find(id);
                if (lan == null)
                {

                    var message = Messages.NotFound;
                    message.ActionName = "Remove Language ";
                    message.ControllerName = "Language";
                    message.Message = "اللغة غير موجودة";
                    return NotFound(message);
                }
                _LangageRepositroy.Remove(lan, UserName());
                _LangageRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}