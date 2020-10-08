using DAL.Classes;
using DAL.infrastructure;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IRepositories
{
    public interface IStudentRepositroy : IRepositroy<Students>
    {
        List<Students> GetStudentWithFilter(string SSN, string specializationId, string firstName, string fatherName, string lastName, DateTime? enrollmentDate, int RowCount, int Page, out int totalRow);
        Students GetBySSN(string ssn);      
    }
}
