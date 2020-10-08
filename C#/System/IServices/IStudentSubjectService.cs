using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Classes;
using DAL.Models;
namespace System.IServices
{
    public interface IStudentSubjectService
    {
        bool CheckStudentSubjectDegree(StudentSubject studentSubject);

        bool CheckStudentSubjectDegreeAllowNull(StudentSubject studentSubject);
        bool Update(StudentSubject studentSubject);
        

    }
}
