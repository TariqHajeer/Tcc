using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IServices;
using DAL.Models;
using DAL.Classes;
using Microsoft.EntityFrameworkCore;
using Static;
using DAL.HelperEums;

namespace System.Services
{
    public class StudentSubjectService : AbstractService, IStudentSubjectService
    {
        public StudentSubjectService(AbstractUnitOfWork abstractUnitOfWork) : base(abstractUnitOfWork) { }
        /// <summary>
        /// checking for parctical and theoretical defree
        /// 
        /// </summary>
        /// <param name="studentSubject"></param>
        /// <returns></returns>
        public bool CheckStudentSubjectDegree(StudentSubject studentSubject)
        {
            var subjectType = studentSubject.Subject.SubjectType;
            if (!this.CheckStudentSubjectPracticalDegree(studentSubject))
                return false;
            if (!CheckStudentSubjectTheoreticalDegree(studentSubject))
                return false;
            if (studentSubject.TheoreticlaDegree != 0 && !studentSubject.IsNominate())
                return false;
            if (studentSubject.OrignalTotal >= subjectType.SuccessDegree && studentSubject.HelpDegree == true)
                return false;
            if (studentSubject.ExamSemester != null)
                if (studentSubject.ExamSemester.SemesterNumber != studentSubject.MainSemesterNumber)
                {
                    var oldStudnetSubject = _abstractUnitOfWork.Repository<StudentSubject>().Get(
                        (c => c.SubjectId == studentSubject.SubjectId && c.Ssn == studentSubject.Ssn && c.Id < studentSubject.Id && c.Subject.StudySemester.Number == studentSubject.MainSemesterNumber)
                        , c => c.Subject.SubjectType
                    ).LastOrDefault();
                    if (oldStudnetSubject != null)
                    {

                        if (oldStudnetSubject.PracticalDegree != studentSubject.PracticalDegree || !oldStudnetSubject.IsNominate())
                        {
                            return false;
                        }
                    }
                }
            return true;
        }
        public bool CheckStudentSubjectDegreeAllowNull(StudentSubject studentSubject)
        {

            if (studentSubject.PracticalDegree == null && studentSubject.TheoreticlaDegree == null)
                return true;
            if (studentSubject.PracticalDegree == null && studentSubject.TheoreticlaDegree != null)
                return false;
            if (studentSubject.PracticalDegree != null && studentSubject.TheoreticlaDegree == null)
                return CheckStudentSubjectPracticalDegree(studentSubject);
            return CheckStudentSubjectDegree(studentSubject);
        }
        private bool CheckStudentSubjectPracticalDegree(StudentSubject studentSubject)
        {
            var subjectType = studentSubject.Subject.SubjectType;

            if (studentSubject.PracticalDegree < 0 || studentSubject.PracticalDegree > subjectType.PracticalDegree)
            {
                return false;
            }
            return true;
        }
        private bool CheckStudentSubjectTheoreticalDegree(StudentSubject studentSubject)
        {
            var subjectType = studentSubject.Subject.SubjectType;
            if (studentSubject.TheoreticlaDegree < 0 || studentSubject.TheoreticlaDegree > subjectType.TheoreticalDegree)
            {
                return false;
            }
            return true;
        }
        public bool Update(StudentSubject studentSubject)
        {

            if (!CheckStudentSubjectDegree(studentSubject))
            {
                return false;
            }
            var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == studentSubject.ExamSemester.YearId)
                .Include(c => c.ExamSystemNavigation)
                .Include(c => c.ExamSemester)
                .ThenInclude(c => c.StudentSubject)
                .Single();
            var semesertNumber = studentSubject.ExamSemester.SemesterNumber;
            if (studentSubject.IsSuccess())
            {
                var examSemseters = year.ExamSemester.Where(c => c.SemesterNumber > semesertNumber).ToList();
                foreach (var item in examSemseters)
                {
                    var nexStudentSubject = item.StudentSubject.Where(c => c.SubjectId == studentSubject.SubjectId && c.Ssn == studentSubject.Ssn).FirstOrDefault();
                    if (nexStudentSubject != null)
                    {
                        _abstractUnitOfWork.Repository<StudentSubject>().Remove(nexStudentSubject, SentencesHelper.System);
                    }
                }
                return true;
            }
            else
            {
                if (studentSubject.IsNominate())
                {
                    var nextExamSemeserId = year.GetNexSemeserIdCanTsetSubjectIn(studentSubject);
                    if (nextExamSemeserId != -1)
                    {
                        var existstudnetSubjectInNexExamSemester = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => c.SubjectId == studentSubject.SubjectId && c.Ssn == studentSubject.Ssn && c.ExamSemesterId == nextExamSemeserId).FirstOrDefault();
                        if (existstudnetSubjectInNexExamSemester == null)
                        {
                            var newStudentSubject = new StudentSubject()
                            {
                                Ssn = studentSubject.Ssn,
                                PracticalDegree = studentSubject.PracticalDegree,
                                SubjectId = studentSubject.SubjectId,
                                ExamSemesterId = nextExamSemeserId,
                                SystemNote = "اضيفت بعد التعديل",
                            };
                            _abstractUnitOfWork.Add(newStudentSubject, "System");
                        }
                        else
                        {
                            existstudnetSubjectInNexExamSemester.PracticalDegree = studentSubject.PracticalDegree;
                            _abstractUnitOfWork.Update(existstudnetSubjectInNexExamSemester, SentencesHelper.System);
                        }

                    }
                    /*
                     *هي الحالة لما يكون في بس دورة خريجين بس و هي مو من ضمن الائحة الداخلية
                     * هي   منشان لماضفتها يكون الطالب سنة تانية و في دورة خريجين و انا عم عدل على مادة من الفصل يلي سبقو 
                     * ف هون رح اضطر اني امسح تقديمو للفصل يلي بعدو منشان ارجع افحصو إذا بحسن ياخد علامات مساعدة او بيطلع راسب 
                     * و ممكن تتعالج غير هيك اي بس كتييير صعبة
                     */
                    else
                    {
                        //منشان إذا عم عدل على مادة بالفصل التالت ما ينمسحو
                        if (studentSubject.ExamSemester.SemesterNumber < 3)
                        {
                            var studentSubjectInTheredSemester = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => c.Ssn == studentSubject.Ssn && c.ExamSemester.YearId == year.Id && c.ExamSemester.SemesterNumber == 3).ToList();
                            studentSubjectInTheredSemester.ForEach(element =>
                            {
                                _abstractUnitOfWork.Remove(element, SentencesHelper.System);
                            });
                        }
                    }
                }
                else
                {
                    var semstersId = year.ExamSemester.Select(c => c.Id).ToList();
                    var subjectsInNextSemester = _abstractUnitOfWork.Repository<StudentSubject>().Get(c => c.SubjectId == studentSubject.SubjectId && c.Ssn == studentSubject.Ssn && c.ExamSemesterId > studentSubject.ExamSemesterId && semstersId.Contains((int)c.ExamSemesterId)).ToList();
                    foreach (var item in subjectsInNextSemester)
                    {
                        _abstractUnitOfWork.Remove(item, SentencesHelper.System);
                    }
                }
            }
            return true;
        }
    }
}
