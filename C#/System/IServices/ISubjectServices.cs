using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IServices
{
    public interface ISubjectServices
    {
        /// <summary>
        /// this use when add new subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="depandancySubject"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        bool CheckSubjectAndDepandcySubject(/*int subjectsId*/ Subjects subject, Subjects depandancySubject, out bool found);
        /// <summary>
        /// this use when update old subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="depandancySubjectId"></param>
        /// <returns></returns>
        bool CheckSubjectAndDepandcySubject(Subjects subject, Subjects depandancySubjectId);
        /// <summary>
        /// this used for add study paln
        /// test the study year and semester 
        /// if the first subject can depnd on another subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="depandancySubject"></param>
        /// <returns></returns>
        bool CheckCanBeDepandacy(Subjects subject, Subjects depandancySubject);
    }
}
