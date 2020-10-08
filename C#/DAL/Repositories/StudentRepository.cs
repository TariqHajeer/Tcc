using DAL.Classes;
using DAL.infrastructure;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.IRepositories;

namespace DAL.Repositories
{
    public class StudentRepository : Repository<Students>, IStudentRepositroy
    {
        public StudentRepository(DbContext tccContext) : base(tccContext)
        {
        }

        public Students GetBySSN(string ssn)
        {
            return this.GetIQueryable(s => s.Ssn == ssn)
                .Include(s => s.CrossOf)
                .Include(s => s.Deattach)
                .Include(s => s.Graduation)
                .Include(s => s.MoreInformation)
                .Include(s => s.StudentDegree)
                .Include(s => s.Trasmentd)
                .Include(s => s.ClosesetPersons)
                    .ThenInclude(cp => cp.PersonPhone)
                .Include(s => s.Partners)
                .Include(s => s.Reparations)
                .Include(s => s.Sanctions)
                .Include(s => s.Siblings)
                .Include(s => s.StudentAttachment)
                .Include(s => s.StudentPhone)
                .Include(s => s.Registrations)
                    .ThenInclude(R => R.FinalState)
                .Include(s => s.Registrations)
                    .ThenInclude(R => R.StudentState)
                .Include(s => s.Registrations)
                    .ThenInclude(R => R.StudyYear)
                .Include(s => s.Registrations)
                    .ThenInclude(R => R.TypeOfRegistar)
                .Include(s => s.Registrations)
                    .ThenInclude(R => R.Year)
                .FirstOrDefault();
        }

        public List<Students> GetStudentWithFilter(string SSN, string specializationId, string firstName, string fatherName, string lastName, DateTime? enrollmentDate, int RowCount, int Page, out int totalRow)
        {
            var studentsQueryable = this.GetIQueryable();
            if (!string.IsNullOrWhiteSpace(SSN))
            {
                studentsQueryable.Where(c => c.Ssn.StartsWith(SSN));
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                studentsQueryable.Where(c => c.FatherName.StartsWith(firstName));
            }
            if (!string.IsNullOrWhiteSpace(fatherName))
            {
                studentsQueryable.Where(c => c.FatherName.StartsWith(fatherName));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                studentsQueryable.Where(c => c.FatherName.StartsWith(lastName));
            }
            if (!string.IsNullOrWhiteSpace(specializationId))
            {
                studentsQueryable.Where(c => c.FatherName.StartsWith(specializationId));
            }
            if (enrollmentDate != null)
            {
                studentsQueryable.Where(c => c.EnrollmentDate.Year == ((DateTime)enrollmentDate).Year);
            }
            totalRow = studentsQueryable.Count();
            studentsQueryable = studentsQueryable.Skip((Page - 1) * RowCount).Take(RowCount);
            return studentsQueryable.ToList();
        }
        

    }
}
