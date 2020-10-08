using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using System.DTO.UserDTO;
using Static;
using Microsoft.EntityFrameworkCore;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : MyBaseController
    {
        readonly IRepositroy<User> _userRepositroy;
        readonly IRepositroy<Group> _groupRepository;
        readonly IRepositroy<UserGroup> _userGroupRepository;
        AbstractUnitOfWork _abstractUnitOfWork;
        public UserController(IRepositroy<User> repositroy,
            IRepositroy<Group> groupRepositroy,
            IRepositroy<UserGroup> userGroupRepository,
            AbstractUnitOfWork abstractUnitOfWork,
        IMapper mapper) : base( mapper)
        {
            _userRepositroy = repositroy;
            _groupRepository = groupRepositroy;
            _userGroupRepository = userGroupRepository;
              _abstractUnitOfWork= abstractUnitOfWork;
            
        }
        [HttpGet]
        [Authorize(Roles = "ShowUser")]
        public IActionResult Users()
        {
            try
            {
                var user = _userRepositroy.GetIQueryable()
                      .Include(v => v.UserGroup)
                      .Include(c => c.UserGroup)
                    .ThenInclude(gp => gp.Group);



                return Ok(_mapper.Map<UserResponDTO[]>(user));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[Authorize(Roles = "AddUser")]
        public IActionResult AddUser([FromBody]AddUserDTO addUserDTO)
        {
            try
            {
                addUserDTO.Name = addUserDTO.Name.Trim();
                User simelarUserName = _userRepositroy.Get(c => c.Username == addUserDTO.Username).FirstOrDefault();
                if (simelarUserName != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add User";
                    message.ControllerName = "User";
                    return Conflict(message);
                }

                User user = _mapper.Map<User>(addUserDTO);
                user.Password = MD5Hash.GetMd5Hash(user.Password);

                //check  for groups Id 
                var groupsId = _groupRepository.Get().Select(c => c.Id);
                if (addUserDTO.GroupIds != null && addUserDTO.GroupIds.Count > 0)
                {
                    if (addUserDTO.GroupIds.Except(groupsId).Any())
                    {
                        var message = Messages.NotFound;
                        message.ActionName = "Add User";
                        message.ControllerName = "User";
                        message.Message = "يوجد مجموعة على الاقل غير موجودة";
                        return NotFound(message);
                    }
                }
                //end checking
                _abstractUnitOfWork.Add(user, UserName());
                if (addUserDTO.GroupIds != null && addUserDTO.GroupIds.Count > 0)
                {
                    foreach (var item in addUserDTO.GroupIds)
                    {
                        UserGroup userGroup = new UserGroup()
                        {
                            UserId = user.Id,
                            GroupId = item,
                        };
                        _abstractUnitOfWork.Add(userGroup, UserName());
                    }
                }
                _abstractUnitOfWork.Commit();
                return Ok(_mapper.Map<UserResponDTO>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(Messages.AnonymousError);
            }
        }
        [HttpGet("GetEnabled")]
        public IActionResult GetEnabled()
        {
            var x = _userRepositroy.Get(c => c.IsEnabled);
            return Ok(x);
        }
        [HttpPatch("AddGroup/{id}")]
        [Authorize(Roles = "UpdateUser")]
        public IActionResult AddGroup(int id, [FromForm]int groupId)
        {
            try
            {
                var user = _userRepositroy.Find(id);
                if (user == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Group";
                    message.ControllerName = "User";
                    message.Message = "المستخدم غير موجود";
                    return NotFound(message);
                }
                var group = _groupRepository.Find(groupId);
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Add Group";
                    message.ControllerName = "User";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                var UserGroup = _userGroupRepository.Get(c => c.UserId == id && c.GroupId == groupId).FirstOrDefault();
                if (UserGroup != null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Add Group";
                    message.ControllerName = "User";
                    message.Message = "مجموعة المستخدمبن غير موجودة";
                    return Conflict(message);
                }
                 _userGroupRepository.Add(new UserGroup() { UserId = id, GroupId = groupId },UserName());
                _userGroupRepository.Save();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPatch("RemoveGroup/{id}")]
        [Authorize(Roles = "UpdateUser")]
        public IActionResult RemoveGroup(int id, [FromForm] int groupId)
        {
            try
            {
                var user = _userRepositroy.Find(id);
                if (user == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Group";
                    message.ControllerName = "User";
                    message.Message = "المستخدم غير موجود";
                    return NotFound(message);
                }

                
               var group = _groupRepository.Find(groupId);
                if (group == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove Group";
                    message.ControllerName = "User";
                    message.Message = "المجموعة غير موجودة";
                    return NotFound(message);
                }
                var UserGroup = _userGroupRepository.Get(c => c.UserId == id && c.GroupId == groupId).FirstOrDefault();
                if (UserGroup == null)
                {
                    var message = Messages.Exist;
                    message.ActionName = "Remove Group";
                    message.ControllerName = "User";
                    message.Message = "مجموعة المستخدمين غير موجودة";
                    return Conflict(message);
                }
                _userGroupRepository.Remove(UserGroup, UserName());
                _userRepositroy.Save();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("ShowUser/{id}")]
        [Authorize(Roles = "ShowUser")]
        public IActionResult ShowUserById(int id)
        {
            try
            {
                var user = _userRepositroy.Find(id);
                if (user == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Show User By Id";
                    message.ControllerName = "User";
                    message.Message = "المستخدم غير موجود";
                    return NotFound(message);
                }
                UserResponDTO userResponDTO = _mapper.Map<UserResponDTO>(user);
                return Ok(userResponDTO);

            }
            catch (Exception)
            {
                return BadRequest(Messages.AnonymousError);
            }
        }
        [HttpPatch("UpdateUser/{id}")]
        //[Authorize(Roles = "UpdateUser")]
        public IActionResult UpdateUser(int id, [FromBody]UpdateUserDTO updateUserDTO)
        {
            updateUserDTO.Name = updateUserDTO.Name.Trim();
            if (string.IsNullOrWhiteSpace(updateUserDTO.Name))
            {
                var message = Messages.EmptyName;
                message.ActionName = "Update User";
                message.ControllerName = "User";
                return BadRequest(message);
            }
            var simelar = _userRepositroy.Get(c => c.Name == updateUserDTO.Name && c.Id != id).FirstOrDefault();
            if (simelar != null)
            {
                var message = Messages.Exist;
                message.ActionName = "Update User";
                message.ControllerName = "User";
                return Conflict(message);
            }
            var user = _userRepositroy.Find(id);
            if (user == null)
            {
                var message = Messages.NotFound;
                message.ActionName = "Update User";
                message.ControllerName = "User";
                message.Message = "المستخدم غير موجود";
                return NotFound(message);
            }
            user = _mapper.Map(updateUserDTO,user);
            _userRepositroy.Update(user,UserName());
            _userRepositroy.Save();
            return Ok();
        }
        [HttpDelete("{id}")]
        //[Authorize(Roles = "RemoveUser")]
        public IActionResult RemoveUser(int id)
        {
            try
            {
                var user = _userRepositroy.Find(id);
                if (user == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Remove User";
                    message.ControllerName = "User";
                    message.Message = "المستخدم غير موجود";
                    return NotFound(message);
                }
                _userRepositroy.Remove(user, UserName());
                _userRepositroy.Save();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}