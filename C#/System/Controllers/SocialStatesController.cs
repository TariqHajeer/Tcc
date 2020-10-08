using System;
using System.Collections.Generic;
using System.DTO.SocialStatesDTO;
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
    public class SocialStatesController : MyBaseController
    {
        IRepositroy<SocialStates> _socialStatesRepositroy;
        public SocialStatesController(IRepositroy<SocialStates> repositroy, IMapper mapper) : base(mapper)
        {
            _socialStatesRepositroy = repositroy;
        }
        [Authorize(Roles = "AddSocialStates")]
        [HttpPost]
        public IActionResult AddSocialStates([FromBody]AddSocialStatesDTO addSocialStatesDTO)
        {
            try
            {
                addSocialStatesDTO.Name = addSocialStatesDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addSocialStatesDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Social State";
                    message.ControllerName = "Social State";
                    return BadRequest(message);
                }
                if (_socialStatesRepositroy.Get(c => c.Name == addSocialStatesDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Social State";
                    message.ControllerName = "Social State";
                    return Conflict(message);
                }
                var soc = _mapper.Map<SocialStates>(addSocialStatesDTO);
                _socialStatesRepositroy.Add(soc, UserName());
                _socialStatesRepositroy.Save();
                return Ok(soc);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var x = _socialStatesRepositroy.Get(c => c.IsEnabled);
            return Ok(x);
        }
        [Authorize(Roles = "ShowSocialStates")]
        [HttpGet]
        public IActionResult ShowSocialStates()
        {
            try
            {
                var soc = _mapper.Map<SocialStates[]>(_socialStatesRepositroy.Get());
                return Ok(soc);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        //[Authorize(Roles = "UpdateSocialStates")]
        public IActionResult UpdateSocialStates(int id, [FromBody]UpdateSocialStatesDTO updateSocialStatesDTO)
        {

            try
            {
                updateSocialStatesDTO.Name = updateSocialStatesDTO.Name.Trim();
                var simelar = _socialStatesRepositroy.Get(c => c.Name == updateSocialStatesDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Social State";
                    message.ControllerName = "Social State";
                    return Conflict(message);
                }
                var orginalSocial = _socialStatesRepositroy.Find(id);
                if (orginalSocial == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Social State";
                    message.ControllerName = "Social State";
                    message.Message = "الحالة الاجتماعية غير موجودة";
                    return NotFound(message);
                }
                orginalSocial = _mapper.Map(updateSocialStatesDTO, orginalSocial);
                _socialStatesRepositroy.Update(orginalSocial, UserName());
                _socialStatesRepositroy.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveSocialStates")]
        public IActionResult RemoveSocialStates(int id)
        {
            try
            {
                var soc = _socialStatesRepositroy.Find(id);
                if (soc == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Social State";
                    message.ControllerName = "Social State";
                    message.Message = "الحالة الاجتماعية غير موجودة";
                    return NotFound(message);
                }
                _socialStatesRepositroy.Remove(soc, UserName());
                _socialStatesRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}