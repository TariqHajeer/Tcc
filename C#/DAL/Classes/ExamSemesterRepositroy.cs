using System;
using System.Collections.Generic;
using System.Text;
using DAL.infrastructure;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Classes
{
    public class ExamSemesterRepositroy : Repository<ExamSemester>, IExamSemesterRepositroy
    {
        public ExamSemesterRepositroy(DbContext tccContext) : base(tccContext)
        {
        }

        public  bool CheckCanSetDegreeById(int id)
        {
            var examSemeter = this.GetIQueryable(c => c.Id == id)
                .Include(c => c.Year)
                .ThenInclude(c => c.ExamSemester)
                    .ThenInclude(c => c.StudentSubject)
                .FirstOrDefault();
            if (examSemeter.SemesterNumber == 1)
                return true;
            Years year = examSemeter.Year;
            return year.IsThisSemesterFinshed(examSemeter.SemesterNumber - 1);
             
        }

        public bool ChekcCanSetDegreeByNumber(int number)
        {
            throw new NotImplementedException();
        }
    }
}
