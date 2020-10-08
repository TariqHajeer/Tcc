using System;
using System.Collections.Generic;
using System.IServices;
using System.Linq;
using System.Threading.Tasks;
using DAL.Classes;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
namespace System.Services
{
    public class SubjectServices : AbstractService, ISubjectServices
    {
        public SubjectServices(AbstractUnitOfWork abstractUnitOfWork) : base(abstractUnitOfWork)
        {
        }

        //AbstractUnitOfWork _abstractUnitOfWork;
        //public SubjectServices(AbstractUnitOfWork abstractUnitOfWork)
        //=> _abstractUnitOfWork = abstractUnitOfWork;


        /// <summary>
        /// this use when add new subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="depandancySubject"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        public bool CheckSubjectAndDepandcySubject(/*int subjectsId*/ Subjects subject, Subjects depandancySubject, out bool found)
        {
            //var subject = _abstractUnitOfWork.Repository<Subjects>().GetIQueryable(c=>c.Id==subjectsId)
            //    .Include(c=>c.StudySemester)
            //    .FirstOrDefault();
            found = true;
            if (subject == null || depandancySubject == null)
            {
                found = false;
                return false;
            }

            if (subject.StudySemester.StudyplanId != depandancySubject.StudySemester.StudyplanId)
            {
                return false;
            }
            // check this subject is after dependancy
            //if (subject.StudySemester.StudyYearId < depandancySubject.StudySemester.StudyYearId)
            //    return false;
            //if (subject.StudySemester.StudyYearId == depandancySubject.StudySemester.StudyYearId)
            //{
            //    if (subject.StudySemester.Number <= depandancySubject.StudySemester.Number)
            //    {
            //        return false;
            //    }
            //}
            //return true;
            return CheckCanBeDepandacy(subject, depandancySubject);
        }
        /// <summary>
        /// this use when update old subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="depandancySubjectId"></param>
        /// <returns></returns>
        public bool CheckSubjectAndDepandcySubject(Subjects subject, Subjects depandancySubjectId)
        {
            bool found;
            return CheckSubjectAndDepandcySubject(subject, depandancySubjectId, out found);
        }
        /// <summary>
        /// this used for add study paln
        /// test the study year and semester 
        /// if the first subject can depnd on another subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="depandancySubject"></param>
        /// <returns></returns>
        public bool CheckCanBeDepandacy(Subjects subject, Subjects depandancySubject)
        {
            if (subject.StudySemester.StudyYearId < depandancySubject.StudySemester.StudyYearId)
                return false;
            if (subject.StudySemester.StudyYearId == depandancySubject.StudySemester.StudyYearId)
            {
                if (subject.StudySemester.Number <= depandancySubject.StudySemester.Number)
                {
                    return false;
                }
            }
            return true;

        }

        
    }

}
