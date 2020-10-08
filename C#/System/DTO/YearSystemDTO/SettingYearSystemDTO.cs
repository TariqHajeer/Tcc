using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.YearSystemDTO
{
    public class SettingYearSystemDTO
    {
        public int Id { set; get; }
        public int Count { get; set; }
        public string Name { set; get; }
        public string Note { set; get; }
        public static implicit operator SettingYearSystemDTO (SettingYearSystem settingYearStstem)
        {
            return new SettingYearSystemDTO()
            {
                Count = settingYearStstem.Count,
                Name = settingYearStstem.Setting.Name,
                Id = settingYearStstem.SettingId
            };
        }
    }
}
