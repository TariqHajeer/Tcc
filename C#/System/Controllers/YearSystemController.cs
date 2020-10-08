using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.DTO.YearSystemDTO;
using Static;
using Microsoft.AspNetCore.Authorization;
using DAL.infrastructure;
namespace System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class YearSystemController : MyBaseController
    {
        IRepositroy<YearSystem> _yearSystemrepositroy;
        IRepositroy<Settings> _settingsRepositroy;
        IRepositroy<SettingYearSystem> _settingYearSystemRepositroy;
        AbstractUnitOfWork _abstractUnitOfWork;
        public YearSystemController(
            IRepositroy<YearSystem> repositroy, IMapper mapper,
            IRepositroy<Settings> settingsRepositroy,
            IRepositroy<SettingYearSystem> settingYearSystemRepositroy,
            AbstractUnitOfWork abstractUnitOfWork
            ) : base(mapper)
        {
            _yearSystemrepositroy = repositroy;
            _settingsRepositroy = settingsRepositroy;
            _settingYearSystemRepositroy = settingYearSystemRepositroy;
            _abstractUnitOfWork = abstractUnitOfWork;
        }
        [HttpGet]
        [Authorize(Roles = "ShowYearSystem")]
        public IActionResult Get()
        {
            try
            {
                var yearSystem = _yearSystemrepositroy.GetIQueryable()
                    .Include(c => c.SettingYearSystem)
                        .ThenInclude(k => k.Setting)
                        .Include(c => c.Years)
                    .ToList();
                return Ok(_mapper.Map<ResponseYearSystem[]>(yearSystem));
            }
            catch
            {
                return BadRequestAnonymousError();
            }

        }
        // [Authorize(Roles = "AddYearSystem")]
        [HttpPost]
        public IActionResult Add(AddYearSystemDTO addYearSystemDTO)
        {
            try
            {
                addYearSystemDTO.Name = addYearSystemDTO.Name.Trim();
                if (string.IsNullOrWhiteSpace(addYearSystemDTO.Name))
                {
                    var message = Messages.EmptyName;
                    message.ActionName = "Add";
                    message.ControllerName = "Year System";
                    return BadRequest(message);
                }
                var settingId = addYearSystemDTO.Settings.Select(c => c.Id).ToList();
                var mainSettinId = this._settingsRepositroy.Get().Select(c => c.Id).ToList();
                // this number of role shoud replace in correct way 
                if (settingId.Count < mainSettinId.Count())
                {
                    var message = new BadRequestErrors();
                    message.ActionName = "Add";
                    message.ControllerName = "Year System";
                    message.Message = "لم يتم إرسال كامل الإعدادات";
                    return Conflict(message);
                }
                settingId.Sort();
                if (settingId.Except(mainSettinId).Any() || mainSettinId.Except(settingId).Any())
                {
                    var message = new BadRequestErrors()
                    {
                        ActionName = "Add",
                        ControllerName = "Year System",
                        Message = "لام يتم إرسال جميع الإعدادات او تم إرسال قيمة خاطئة"
                    };
                    return Conflict(message);
                }
                if (addYearSystemDTO.IsMain)
                {
                    var oldMain = _abstractUnitOfWork.Repository<YearSystem>().Get(c => c.IsMain).FirstOrDefault();
                    if (oldMain != null)
                    {
                        oldMain.IsMain = false;
                        _abstractUnitOfWork.Repository<YearSystem>().Update(oldMain, UserName());
                    }
                }
                YearSystem yearSystem = _mapper.Map<YearSystem>(addYearSystemDTO);
                _abstractUnitOfWork.Add(yearSystem, UserName());
                foreach (var item in addYearSystemDTO.Settings)
                {

                    var setting = _settingsRepositroy.Find(item.Id);
                    var settingYearSystem = new SettingYearSystem
                    {
                        YearSystem = yearSystem.Id,
                        Setting = setting,
                        Count = item.Count,
                        Note = item.Note
                    };
                    _abstractUnitOfWork.Add(settingYearSystem, UserName());
                }
                _abstractUnitOfWork.Commit();
                return Ok(_mapper.Map<ResponseYearSystem>(yearSystem));
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveYearSystem")]
        public IActionResult Delete(int id)
        {
            try
            {
                var yearSystem = _yearSystemrepositroy.GetIQueryable(c => c.Id == id)
                    .Include(c => c.Years)
                    .FirstOrDefault();
                if (yearSystem == null)
                {
                    var message = Messages.NotFound;
                    message.Message = "نظام السنة غير موجود";
                    message.ActionName = "Delete";
                    message.ControllerName = "yearSyestem";
                    return NotFound(message);
                }
                if (yearSystem.Years.Count > 0)
                {

                    var message = Messages.CannotDelete;
                    message.ActionName = "Delete";
                    message.ControllerName = "yearSyestem";
                    return Conflict();
                }
                foreach (var item in yearSystem.SettingYearSystem)
                {
                    _settingYearSystemRepositroy.Remove(item, UserName());
                }
                _abstractUnitOfWork.Remove(yearSystem, UserName());
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpGet("settings")]
        public IActionResult Settings()
        {
            try
            {
                return Ok(_settingsRepositroy.Get());
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "UpdateYearSystem")]
        public IActionResult UpdateYearSystem(int Id, [FromBody]AddYearSystemDTO updateYearSystem)
        {
            try
            {
                var orginalYearSystem = _abstractUnitOfWork.Repository<YearSystem>().Get(c => c.Id == Id, c => c.SettingYearSystem, c => c.Years).SingleOrDefault();
                if (orginalYearSystem == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Update";
                    message.ControllerName = "YearSystem";
                    return NotFound(message);
                }
                if (orginalYearSystem.Years.Count > 0)
                {
                    var newSettingList = updateYearSystem.Settings;
                    var oldSettingList = orginalYearSystem.SettingYearSystem.ToList();
                    for (int i = 0; i < oldSettingList.Count; i++)
                    {
                        var settingId = oldSettingList[i].SettingId;
                        var newSetting = newSettingList.Where(c => c.Id == settingId).SingleOrDefault();
                        if (newSetting == null)
                        {
                            //لم يتم إرسال كامل الإعدادات
                            return Conflict();
                        }
                        if (newSetting.Count != oldSettingList[i].Count)
                        {
                            return Conflict();
                        }
                        oldSettingList[i].Note = newSetting.Note;
                        _abstractUnitOfWork.Update(oldSettingList[i], UserName());
                    }

                }
                else
                {
                    var newSettingList = updateYearSystem.Settings;
                    var oldSettingList = orginalYearSystem.SettingYearSystem.ToList();
                    for (int i = 0; i < oldSettingList.Count; i++)
                    {
                        var settingId = oldSettingList[i].SettingId;
                        var newSetting = newSettingList.Where(c => c.Id == settingId).SingleOrDefault();
                        if (newSetting == null)
                        {
                            //لم يتم إرسال كامل الإعدادات
                            return Conflict();
                        }
                        oldSettingList[i].Count = newSetting.Count;
                        oldSettingList[i].Note = newSetting.Note;
                        _abstractUnitOfWork.Update(oldSettingList[i], UserName());
                    }
                }
                orginalYearSystem.IsMain = updateYearSystem.IsMain;
                orginalYearSystem.Name = updateYearSystem.Name;
                orginalYearSystem.Note = updateYearSystem.Note;
                _abstractUnitOfWork.Update(orginalYearSystem, UserName());
                _abstractUnitOfWork.Commit();
                return Ok();
            }
            catch
            {
                return BadRequestAnonymousError();
            }
        }
    }
}