using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.DTO.Attachemnt;
using Static;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AttachmentController : MyBaseController
    {
        readonly IRepositroy<Attatchments> _attatchmentRepository;
        public AttachmentController(IRepositroy<Attatchments> repositroy, IMapper mapper) : base( mapper)
            => _attatchmentRepository = repositroy;

        [HttpGet("Enabled")]
        public IActionResult GetEnabled()
        {
            try
            {
                return Ok(_mapper.Map<AttachemetResponseDTO[]>(
                    _attatchmentRepository.Get(c => c.IsEnabled)));
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPost]
        [Authorize(Roles = "AddAttachment")]
        public IActionResult AddAttachment([FromBody] AddAttachmentDTO addAttachmentDTO)
        {
            try
            {
                addAttachmentDTO.Name = addAttachmentDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addAttachmentDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Attachment";
                    message.ControllerName = "Attachment";
                    return BadRequest(message);
                }
                if (_attatchmentRepository.Get(c => c.Name == addAttachmentDTO.Name).FirstOrDefault() != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Attachment";
                    message.ControllerName = "Attachment";
                    return Conflict(message);
                }
                var attachment = _mapper.Map<Attatchments>(addAttachmentDTO);
                _attatchmentRepository.Add(attachment,UserName());
                _attatchmentRepository.Save();
                return Ok(_mapper.Map<AttachemetResponseDTO>(attachment));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet]
        [Authorize(Roles = "ShowAttachment")]
        public IActionResult ShowAttachment()
        {
            try
            {

                var at = _mapper.Map<AttachemetResponseDTO[]>(_attatchmentRepository.Get());
                return Ok(at);
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateAttachment")]
        public IActionResult Update(int id, [FromBody] UpdateAttachmentDTO updateAttachmentDTO)
        {
            try
            {
                updateAttachmentDTO.Name = updateAttachmentDTO.Name.Trim();
                var simelar = _attatchmentRepository.Get(c => c.Name == updateAttachmentDTO.Name && c.Id != id).FirstOrDefault();
                if (simelar != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Attachment";
                    message.ControllerName = "Attachment";
                    return Conflict(message);
                }

                var orginalAttachment = _attatchmentRepository.Find(id);
                if (orginalAttachment == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Attachment";
                    message.ControllerName = "Attachment";
                    message.Message = "المرفق غير موجود";
                    return NotFound(message);
                }
                orginalAttachment = _mapper.Map(updateAttachmentDTO, orginalAttachment);
                _attatchmentRepository.Update(orginalAttachment,UserName());
                _attatchmentRepository.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveAttachment")]
        public IActionResult RemoveAttachment(int id)
        {
            try
            {
                var att =_attatchmentRepository.Find(id);
                if (att == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Attachment";
                    message.ControllerName = "Attachment";
                    message.Message = "المرفق غير موجود";
                    return NotFound(message);
                }
                _attatchmentRepository.Remove(att,UserName());
                _attatchmentRepository.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }

    }
}