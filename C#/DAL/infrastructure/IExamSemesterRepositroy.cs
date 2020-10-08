using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;
namespace DAL.infrastructure
{
   public interface IExamSemesterRepositroy : IRepositroy<ExamSemester>
    {
        /// <summary>
        /// check if the privous semester complitele fill in 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckCanSetDegreeById(int id);
        bool ChekcCanSetDegreeByNumber(int number);
    }
}
