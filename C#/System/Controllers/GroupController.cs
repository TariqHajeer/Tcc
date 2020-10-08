using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.DTO;
using Microsoft.AspNetCore.Authorization;
using System.DTO.GroupDTO;
using Static;
using System.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DAL.infrastructure;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : MyBaseController
    {
        AbstractUnitOfWork _abstractUnitOfWork;
        readonly IRepositroy<Group> _groupRepositroy;
        readonly IRepositroy<Privilage> _privilageRepositroy;
        readonly IRepositroy<GroupPrivilage> _groupPrivilageRepositroy;
        public GroupController(
            IRepositroy<Group> repositroy, IMapper mapper,
            IRepositroy<Privilage> privilageRepositroy,
            IRepositroy<GroupPrivilage> groupPrivilageRepositroy,
            AbstractUnitOfWork abstractUnitOfWork) : base(mapper)
        {
            _groupRepositroy = repositroy;
            _privilageRepositroy = privilageRepositroy;
            _groupPrivilageRepositroy = groupPrivilageRepositroy;
            _abstractUnitOfWork = abstractUnitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = "ShowGroup")]
        public IActionResult ShowGroup()
        {
            try
            {
                var groups = _groupRepositroy.GetIQueryable()
                    .Include(c => c.UserGroup)
                    .ThenInclude(ug => ug.User)
                    .Include(c => c.GroupPrivilage)
                    .ThenInclude(gp => gp.Privilage);
                return Ok(_mapper.Map<GroupResopnseDTO[]>(groups));
            }
            catch
            {
                return BadRequestAnonymousError();
            }

        }
        [HttpPost]
        //  [Authorize(Roles = "AddGroup")]
        public IActionResult Create([FromBody] AddGroupDTO addGroupDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addGroupDto.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add Group";
                    message.ControllerName = "Group";
                    return BadRequest(message);
                }
                // test if there is another group with the same name
                Group group = _groupRepositroy.Get(c => c.Name == addGroupDto.Name).FirstOrDefault();
                if (group != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Group";
                    message.ControllerName = "Group";
                    return Conflict(message);
                }
                var priveleges = _privilageRepositroy.Get().Select(c => c.Id);
                List<int> privelgesId = addGroupDto.Priveleges;

                if (privelgesId != null)
                    // test if theres an priveleg Id not in database
                    if (privelgesId.Except(priveleges).Any())
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add Group";
                        message.ControllerName = "Group";
                        message.Message = "الصلاحية غير موجودة";
                        return NotFound(message);
                    }

                group = new Group()
                {
                    Name = addGroupDto.Name
                };
                _abstractUnitOfWork.Add(group,UserName());
                // for add group without priveleges 
                if (privelgesId != null)
                    foreach (var item in addGroupDto.Priveleges)
                    {
                        _abstractUnitOfWork.Add(new GroupPrivilage() { GroupId = group.Id, PrivilageId = item },UserName());
                    }
                _abstractUnitOfWork.Commit();
                return Ok(_mapper.Map<GroupResopnseDTO>(group));
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "ShowGroup")]
        public IActionResult Details(int id)
        {
            try
            {
                Group group = _groupRepositroy.Find(id);
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Details";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                var groupResopnse = _mapper.Map<GroupResopnseDTO>(group);
                return Ok(groupResopnse);
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }

        }
        [HttpPatch("AddPriveleges/{id}")]
        [Authorize(Roles = "UpdateGroup")]
        public IActionResult AddPriveleges(int id, [FromForm] int privelegeId)
        {
            try
            {
                Group group = _groupRepositroy.Find(id);
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "AddPriveleges";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                Privilage privilage = _privilageRepositroy.Find(privelegeId);
                if (privilage == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "AddPriveleges";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                GroupPrivilage groupPrivilage = _groupPrivilageRepositroy.Get(c => c.GroupId == id && c.PrivilageId == privelegeId).FirstOrDefault();
                if (groupPrivilage != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "AddPriveleges";
                    message.ControllerName = "Group";
                    return Conflict(message);
                }
                groupPrivilage = new GroupPrivilage()
                {
                    GroupId = id,
                    PrivilageId = privelegeId
                };

                _groupPrivilageRepositroy.Add(groupPrivilage,UserName());
                _groupPrivilageRepositroy.Save();
                return Ok(_mapper.Map<GroupPrivilage>(groupPrivilage));
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("RemovePrivilage/{id}")]
        [Authorize(Roles = "UpdateGroup")]
        public IActionResult RemovePrivilage(int id, [FromForm] int privelegeId)
        {
            try
            {
                Group group = _groupRepositroy.Find(id);
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Privelage";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }

                Privilage privilage = _privilageRepositroy.Find(privelegeId);
                if (privilage == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Privelage";
                    message.ControllerName = "Group";
                    message.Message = "الصلاحية غير موجودة";
                    return NotFound(message);
                }
                GroupPrivilage groupPrivilage = _groupPrivilageRepositroy.Get(c => c.GroupId == id && c.PrivilageId == privelegeId).FirstOrDefault();
                if (groupPrivilage == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Privelage";
                    message.ControllerName = "Group";
                    message.Message = "مجموعة الصلاحيات غير موجودة";
                    return Conflict(message);
                }
                _groupPrivilageRepositroy.Remove(groupPrivilage, UserName());
                _groupPrivilageRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("{id}")]
        //  [Authorize(Roles = "UpdateGroup")]
        public IActionResult UpdateGroup(int id, [FromForm] string Name)
        {
            try
            {
                Name = Name.Trim();
                if (string.IsNullOrWhiteSpace(Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Update Group";
                    message.ControllerName = "Group"; ;
                    return BadRequest(message);
                }
                var group = _groupRepositroy.Find(id);
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update Group";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                var simelarGroup = _groupRepositroy.Get(c => c.Name == Name && c.Id != id).FirstOrDefault();
                if (simelarGroup != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Update Group";
                    message.ControllerName = "Group"; ;
                    return Conflict(message);
                }

                group.Name = Name;
                _groupRepositroy.Update(group,UserName());
                _groupRepositroy.Save();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }

        }
        [HttpDelete("{id}")]
        // [Authorize(Roles = "RemoveGroup")]
        public IActionResult RemoveGroup(int id)
        {
            if (id == 1)
            {
                return BadRequest(Messages.CannotDelete);
            }
            try
            {
                Group group = _abstractUnitOfWork.Repository<Group>().GetIQueryable(c=>c.Id==id)
                    .Include(c=>c.GroupPrivilage)
                    .Include(c=>c.UserGroup)
                    .FirstOrDefault();
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Group";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                group.GroupPrivilage.ToList().ForEach(item => _abstractUnitOfWork.Remove(item, UserName()));
                group.UserGroup.ToList().ForEach(item => _abstractUnitOfWork.Remove(item, UserName()));
                _abstractUnitOfWork.Remove(group,UserName());
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("ShowGroup")]
        [Authorize(Roles = "ShowGroup")]
        public IActionResult ShowGroupByName([FromBody] string name)
        {
            try
            {
                name = name.Trim();
                Group group = _groupRepositroy.Get(c => c.Name.StartsWith(name)).FirstOrDefault();
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Show Group By Name ";
                    message.ControllerName = "Group";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                return Ok(group);
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }


    }
}