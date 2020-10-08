using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DTO.StudentDTO;
using DAL.infrastructure;
using DAL.Models;
using DAL.Classes;
using System.DTO.RegistrationDTOs;

namespace System.IServices
{
    public interface IStudentService
    {
        /// <summary>
        /// set the ssn for student 
        /// </summary>
        /// <param name="student"></param>
        void SetSSN(ISSN student);
        //void StudentBalanceRegistration(Students student);
        void ProcessStudentState(Students student,bool testHelpDgree=true);
        void ProcessStudentState(string ssn);
        bool CanStudentGetHelpDegree(Students student, Years year=null);
        

    }
}
