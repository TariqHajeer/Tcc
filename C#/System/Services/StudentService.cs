using System.Linq;
using DAL.Classes;
using System.IServices;
using System.DTO.StudentDTO;
using DAL.Models;
using DAL.HelperEums;
using DAL.infrastructure;
using Microsoft.EntityFrameworkCore;
using DAL.Helper;
using Static;
using System.DTO.RegistrationDTOs;

namespace System.Services
{
    public class StudentService : AbstractService, IStudentService
    {
        public StudentService(AbstractUnitOfWork abstractUnitOfWork) : base(abstractUnitOfWork)
        {
        }
        #region ssn
        /// <summary>
        /// this method used when setudent is a new student or is transformed form another collage 
        /// but when the transformed student  is should be in the first year
        /// </summary>
        /// <param name="student"></param>
        private void SetSSNForFirstYearStudnet(AddStudetnDTO student)
        {
            #region
            //string ssn = student.SpecializationId;
            //var year = student.EnrollmentDate.ToString("yy");
            //ssn += year;
            //var students = _abstractUnitOfWork.Repository<Students>().Get(c => c.Ssn.StartsWith(ssn));
            //var lastOldStudent = students.LastOrDefault();
            //if (lastOldStudent != null)
            //{
            //    var oldSsn = lastOldStudent.Ssn;
            //    var ssnNumber = oldSsn.Remove(0, 3);
            //    if (!char.IsDigit(ssnNumber[ssnNumber.Length-1]))
            //    {
            //        ssnNumber.Remove(ssnNumber.Length - 1);
            //    }
            //    var oldPureSSN = Convert.ToInt16(oldSsn.Remove(0, 3));
            //    if (oldPureSSN < 10)
            //    {
            //        ssn += "00" + (oldPureSSN + 1).ToString();
            //    }
            //    else if (oldPureSSN < 99)
            //    {
            //        ssn += "0" + (oldPureSSN + 1).ToString();
            //    }
            //    else
            //    {
            //        ssn += (oldPureSSN + 1).ToString();
            //    }
            //}
            //else
            //{
            //    ssn += "001";
            //}
            //student.Ssn = ssn;
            #endregion old code 
            Years year = _abstractUnitOfWork.Repository<Years>().Find(student.AddRegistrationDTO.YearId);
            string ssn = student.SpecializationId;
            ssn += year.FirstYear.ToString().Substring(year.FirstYear.ToString().Length - 2);
            var students = _abstractUnitOfWork.Repository<Students>().Get(c => c.Ssn.StartsWith(ssn));
            var lastOldStudent = students.LastOrDefault();
            if (lastOldStudent != null)
            {
                var oldSsn = lastOldStudent.Ssn;
                var ssnNumber = oldSsn.Remove(0, 3);
                if (!char.IsDigit(ssnNumber[ssnNumber.Length - 1]))
                {
                    ssnNumber.Remove(ssnNumber.Length - 1);
                }
                var oldPureSSN = Convert.ToInt16(oldSsn.Remove(0, 3));
                if (oldPureSSN < 10)
                {
                    ssn += "00" + (oldPureSSN + 1).ToString();
                }
                else if (oldPureSSN < 99)
                {
                    ssn += "0" + (oldPureSSN + 1).ToString();
                }
                else
                {
                    ssn += (oldPureSSN + 1).ToString();
                }
            }
            else
            {
                ssn += "001";
            }
            student.Ssn = ssn;
        }
        private void SetTemporarySSN(AddStudetnDTO student)
        {
            var lastTempStudent = _abstractUnitOfWork.Repository<Students>().Get(c => c.Ssn.StartsWith('#')).LastOrDefault();
            if (lastTempStudent == null)
            {
                student.Ssn = "#-1";
                return;
            }
            int lastTempStudentNumber = Convert.ToInt32(lastTempStudent.Ssn.Split('-')[1]);
            student.Ssn = "#-" + ++lastTempStudentNumber;

        }
        private void SetSSNForTransformStudent(Students student)
        {
            if (student.Registrations == null || student.Registrations.Count == 0)
            {
                throw new Exception("Registrations null or empty");
            }
            string ssn = student.SpecializationId;
            var year = _abstractUnitOfWork.Repository<Years>().Find(student.Registrations.First().YearId).FirstYear;
            year = Convert.ToInt16(year.ToString().Substring(year.ToString().Length - 2));
            if (student.CurrentStudentStatusId == (int)StudentStateEnum.successful || student.CurrentStudentStatusId == (int)StudentStateEnum.transported)
            {
                ssn += (Convert.ToInt32(year) - 1).ToString();
            }
            else
                ssn += year;
            var students = _abstractUnitOfWork.Repository<Students>().Get(c => c.Ssn.StartsWith(ssn));
            var lastOldStudent = students.LastOrDefault();
            if (lastOldStudent != null)
            {
                var oldSsn = lastOldStudent.Ssn;
                var ssnNumber = oldSsn.Remove(0, 3);
                if (!char.IsDigit(ssnNumber[ssnNumber.Length - 1]))
                {
                    ssnNumber.Remove(ssnNumber.Length - 1);
                }
                var oldPureSSN = Convert.ToInt16(oldSsn.Remove(0, 3));
                if (oldPureSSN < 10)
                {
                    ssn += "00" + (oldPureSSN + 1).ToString();
                }
                else if (oldPureSSN < 99)
                {
                    ssn += "0" + (oldPureSSN + 1).ToString();
                }
                else
                {
                    ssn += (oldPureSSN + 1).ToString();
                }
            }
            else
            {
                ssn += "001";
            }
            student.Ssn = ssn;
        }
        public void SetSSN(ISSN Istudent)
        {
            if (Istudent is AddStudetnDTO)
            {
                var student = Istudent as AddStudetnDTO;
                if (student.TransformedFromId == null)
                {
                    SetSSNForFirstYearStudnet(student);
                    return;
                }
                SetTemporarySSN(student);
                return;
            }
            else
            {
                SetSSNForTransformStudent(Istudent as Students);
            }
        }
        #endregion

        public void ProcessStudentState(string ssn)
        {
            var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == ssn)
                    .Include(c => c.StudentSubject)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(c => c.SubjectType)
                    .Include(c => c.StudentSubject)
                    .Include(c => c.StudentSubject)
                        .ThenInclude(c => c.ExamSemester)
                            .ThenInclude(c => c.Year)
                    .Include(c => c.StudentSubject)
                        .ThenInclude(c => c.Subject)
                            .ThenInclude(c => c.StudySemester)
                    .Include(c => c.Registrations)
                        .ThenInclude(c => c.Year)
                            .ThenInclude(c => c.ExamSystemNavigation)
                    .Include(c => c.Registrations)
                        .ThenInclude(c => c.Year)
                            .ThenInclude(c => c.YearSystemNavigation)
                                    .ThenInclude(c => c.SettingYearSystem)
                    .SingleOrDefault();
            // var student = _abstractUnitOfWork.Repository<Students>().GetIQueryable(c => c.Ssn == ssn)
            //                 .Include(c => c.StudentSubject)
            //                     .ThenInclude(c => c.ExamSemester)
            //                 .Include(c => c.StudentSubject)
            //                     .ThenInclude(c => c.Subject)
            //                         .ThenInclude(c => c.SubjectType)
            //                 .Include(c => c.StudentSubject)
            //                     .ThenInclude(c => c.Subject)
            //                     .ThenInclude(c => c.StudySemester)
            //                 .Include(c => c.Registrations)
            //                 .First();
            this.ProcessStudentState(student);
        }
        public void ProcessStudentState(Students student, bool testHelpDegree = true)
        {
            Years year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == student.CurrentYearId)
                       .Include(c => c.YearSystemNavigation)
                       .ThenInclude(c => c.SettingYearSystem)
                       .Include(c => c.ExamSemester)
                       .First();
            if (student.CurrentStudyYearId == (int)StudyYearEnum.SecoundYear)
            {
                var nonNominateSubject = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsNominate()).ToList();
                if (year.ExamSystemNavigation.GraduateStudentsSemester > 0 && nonNominateSubject.Count > 0)
                {
                    var theredSemesterId = year.GetSemeserIdByNumber(3);
                    student.StudentSubject.Where(c => c.ExamSemesterId == theredSemesterId).ToList().ForEach(subject =>
                      {
                          _abstractUnitOfWork.Remove(subject, SentencesHelper.System + "test");
                          student.StudentSubject.Remove(subject);
                          _abstractUnitOfWork.Commit();
                      });
                }
            }
            if (student.StudentSubject.Any(c => c.PracticalDegree == null || c.TheoreticlaDegree == null))
            {
                var nullAbleSubject = student.StudentSubject.Where(c => c.PracticalDegree == null || c.TheoreticlaDegree == null).ToList();
                student.Lastregistration.FinalStateId = null;
                student.Lastregistration.SystemNote = String.Empty;
                _abstractUnitOfWork.Update(student.Lastregistration, SentencesHelper.System);
                return;
            }
            var sucessCount = student.StudentSubjectWithoutDuplicate.Where(c => c.IsSuccess()).Count();
            var successWithHelpDegreeCount = student.StudentSubjectWithoutDuplicate.Where(c => c.HelpDegree == true).ToList();


            if (student.CurrentStudyYearId == (int)StudyYearEnum.FirstYear)
            {
                var studentState = StudentStateForFirstYear(student, out string Note);
                if (studentState != StudentStateEnum.unknown)
                {
                    student.Lastregistration.FinalStateId = (int)studentState;
                    student.Lastregistration.SystemNote = Note;
                    student.StudentSubject.Where(c => c.HelpDegree == true).ToList().ForEach(c => c.HelpDegree = false);

                }
                else
                {
                    if (testHelpDegree && this.CanStudentGetHelpDegree(student, year))
                    {
                        student.StudentSubject.Where(c => c.HelpDegree == true).ToList().ForEach(c => c.HelpDegree = false);
                        student.Lastregistration.FinalStateId = (int)StudentStateEnum.unknown;
                        student.Lastregistration.SystemNote = SentencesHelper.CanPutHelpDgree;
                    }
                    else if (!student.CurrentStudentSubject.Where(c => c.HelpDegree == true).Any())
                    {
                        if (student.CurrentStudentStatusId == (int)StudentStateEnum.newStudent)
                        {
                            student.Lastregistration.FinalStateId = (int)StudentStateEnum.unSuccessful;
                            student.Lastregistration.SystemNote = string.Empty;
                        }
                        else if (student.CurrentStudentStatusId == (int)StudentStateEnum.unSuccessful || student.CurrentStudentStatusId == (int)StudentStateEnum.Decree)
                        {
                            student.Lastregistration.FinalStateId = (int)StudentStateEnum.Drained;
                            student.Lastregistration.SystemNote = string.Empty;
                        }
                    }
                    else
                    {
                        //added while i'm sleping
                        student.CurrentStudentSubject.Where(c => c.HelpDegree == true).ToList()
                            .ForEach(c =>
                            {
                                c.HelpDegree = false;
                                _abstractUnitOfWork.Update(c, SentencesHelper.System);
                            });
                        if (testHelpDegree && this.CanStudentGetHelpDegree(student, year))
                        {
                            student.Lastregistration.FinalStateId = (int)StudentStateEnum.unknown;
                            student.Lastregistration.SystemNote = SentencesHelper.CanPutHelpDgree;
                        }
                        else
                        {

                            if (student.CurrentStudentStatusId == (int)StudentStateEnum.newStudent)
                            {
                                student.Lastregistration.FinalStateId = (int)StudentStateEnum.unSuccessful;
                                student.Lastregistration.SystemNote = string.Empty;
                            }
                            else if (student.CurrentStudentStatusId == (int)StudentStateEnum.unSuccessful || student.CurrentStudentStatusId == (int)StudentStateEnum.Decree)
                            {
                                student.Lastregistration.FinalStateId = (int)StudentStateEnum.Drained;
                                student.Lastregistration.SystemNote = string.Empty;
                            }
                        }
                    }
                }
                ////added while i'm sleping
                //_abstractUnitOfWork.Update(student.Lastregistration, SentencesHelper.System);
            }
            //for secound year 
            else
            {
                var faildStatue = student.CurrentStudentStatusId == (int)StudentStateEnum.successful || student.CurrentStudentStatusId == (int)StudentStateEnum.transported ? StudentStateEnum.unSuccessful : StudentStateEnum.Drained;
                if (student.StudentSubjectWithoutDuplicate.Any(c => !c.IsNominate()))
                {
                    student.Lastregistration.FinalStateId = (int)faildStatue;

                }
                else if (student.StudentSubjectWithoutDuplicate.All(c => c.IsSuccess() | c.IsSucessWihtHelpDegree()))
                {
                    student.Lastregistration.FinalStateId = (int)StudentStateEnum.Graduated;
                }
                else
                {
                    if (testHelpDegree && CanStudentGetHelpDegree(student, year))
                    {
                        student.Lastregistration.FinalStateId = (int)StudentStateEnum.unknown;
                        student.Lastregistration.SystemNote = SentencesHelper.CanPutHelpDgree;
                    }
                    //testing for grudate semester
                    else
                    {
                        var numberofSubjectForGraduateStudentSemester = year.NumberofSubjectForGraduateStudentSemester();
                        // if theres an thered semester for graduate student
                        if (numberofSubjectForGraduateStudentSemester > 0)
                        {
                            var semesterId = year.GetSemeserIdByNumber(3);


                            var faildSubject = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsSuccess()).ToList();
                            var subjectInGruateSemester = student.StudentSubjectWithoutDuplicate.Where(c => c.ExamSemesterId == semesterId).ToList();
                            if (!faildSubject.Except(subjectInGruateSemester).Any())
                            {
                                student.Lastregistration.FinalStateId = (int)faildStatue;
                                return;
                            }
                            var union = faildSubject.Union(subjectInGruateSemester).ToList();
                            if (numberofSubjectForGraduateStudentSemester >= union.Count() && faildSubject.All(c => c.IsNominate()))
                            {

                                faildSubject.ForEach(element =>
                                {
                                    StudentSubject studentSubject = new StudentSubject()
                                    {
                                        SubjectId = element.SubjectId,
                                        Ssn = element.Ssn,
                                        PracticalDegree = element.PracticalDegree,
                                        ExamSemesterId = semesterId
                                    };
                                    _abstractUnitOfWork.Add(studentSubject, SentencesHelper.System);
                                });
                                //student.Lastregistration.FinalStateId = (int)StudentStateEnum.unknown;
                                student.Lastregistration.SystemNote = SentencesHelper.canApplyForGraduateSemester;
                            }
                            else
                            {
                                subjectInGruateSemester.ForEach(s =>
                                {
                                    _abstractUnitOfWork.Remove(s, SentencesHelper.System);
                                });

                                student.Lastregistration.FinalStateId = (int)faildStatue;
                            }
                        }
                        else
                        {
                            student.Lastregistration.FinalStateId = (int)faildStatue;
                        }
                    }
                }
            }
            _abstractUnitOfWork.Update(student.Lastregistration, SentencesHelper.System);
        }
        public bool CanStudentGetHelpDegree(Students student, Years year)
        {
            if (year == null || year.YearSystemNavigation == null || year.YearSystemNavigation.SettingYearSystem == null)
            {
                year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == student.CurrentYearId)
                    .Include(c => c.YearSystemNavigation)
                    .ThenInclude(c => c.SettingYearSystem)
                    .First();
            }
            if (student.CurrentStudyYearId == (int)StudyYearEnum.FirstYear)
            {
                return CanStudentGetHelpDegreeForFirstYear(student, year);
            }
            else
            {
                return CanStudentGetHelpDegreeForSecoundYear(student, year);
            }
        }
        private bool CanStudentGetHelpDegreeForFirstYear(Students student, Years year)
        {
            #region ناجح 
            //ناجح نظامي
            var sucessCount = student.StudentSubjectWithoutDuplicate.Where(c => c.IsSuccess()).Count();
            if (sucessCount >= BusinessLogicHelper.SucessCount)
            {
                return false;
            }
            //نجاح إداري
            var numberOfSubjectOfAdministrativeLift = year.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.NumberOfSubjectOfAdministrativeLift).First().Count;
            if (numberOfSubjectOfAdministrativeLift != BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist && sucessCount >= numberOfSubjectOfAdministrativeLift)
            {
                return false;
            }
            #endregion

            var faildSubject = student.StudentSubjectWithoutDuplicate.Where(c => c.IsNominate() && !c.IsSuccess()).ToList();
            faildSubject.Sort((s1, s2) => s2.OrignalTotal.CompareTo(s1.OrignalTotal));

            var stillToSuccess = BusinessLogicHelper.SucessCount - sucessCount;
            if (numberOfSubjectOfAdministrativeLift != BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist)
            {
                stillToSuccess = numberOfSubjectOfAdministrativeLift - sucessCount;
            }
            year.GetSubjectCountAndHelpDegreeCount(student, out int helpDgree, out int helpDegreeSubjectCount);
            decimal totalHelpDegree = helpDgree;


            int oldGivenHelpDgree = TotlaHelpDgree(student);
            int oldCountSubjectHaveHelpDgree = SumOfSubjectThatHaveHelpDgree(student);


            if (oldCountSubjectHaveHelpDgree != 0)
            {
                if (oldCountSubjectHaveHelpDgree != stillToSuccess || oldGivenHelpDgree > totalHelpDegree)
                {
                    student.StudentSubject.ToList().ForEach(c =>
                    {
                        c.HelpDegree = false;
                        _abstractUnitOfWork.Repository<StudentSubject>().Update(c, SentencesHelper.System);
                    });
                }
                else
                {
                    return false;
                }
            }
            //إذا عدد المواد يلي بقيانلو لينجح بيقبلو يتقسمو على عدد المواد
            if (stillToSuccess <= helpDegreeSubjectCount)
            {
                foreach (var item in faildSubject)
                {
                    if (helpDegreeSubjectCount == 0 || totalHelpDegree == 0 || stillToSuccess == 0)
                    {
                        break;
                    }
                    var helpDegreeForSubject = item.SucessMark - item.OrignalTotal;
                    if (helpDegreeForSubject > totalHelpDegree)
                    {
                        break;
                    }
                    totalHelpDegree -= helpDegreeForSubject;
                    helpDegreeSubjectCount--;
                    stillToSuccess--;
                }
                if (stillToSuccess == 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool CanStudentGetHelpDegreeForSecoundYear(Students student, Years year)
        {

            var faildSubject = student.StudentSubjectWithoutDuplicate.Where(c => !c.IsSuccess()).ToList();
            if (faildSubject.Any(c => !c.IsNominate()))
                return false;

            year.GetSubjectCountAndHelpDegreeCount(student, out int helpDgree, out int subjectCount);
            if (faildSubject.Count() > subjectCount)
                return false;

            foreach (var item in faildSubject)
            {
                helpDgree -= (int)Math.Ceiling(item.SucessMark - item.OrignalTotal);
                if (helpDgree < 0)
                    return false;
            }
            return true;
        }
        private StudentStateEnum StudentStateForFirstYear(Students student, out string Note)
        {
            Note = "";
            var sucessCount = student.StudentSubjectWithoutDuplicate.Where(c => c.IsSuccess()
            //هاد الشرط منشان إذا كنت منزل
            && c.Subject.StudySemester.StudyYearId == (int)StudyYearEnum.FirstYear).Count();

            var year = _abstractUnitOfWork.Repository<Years>().GetIQueryable(c => c.Id == student.CurrentYearId)
                      .Include(c => c.YearSystemNavigation)
                      .ThenInclude(c => c.SettingYearSystem)
                      .Include(c => c.ExamSemester)
                      .First();
            if (sucessCount == BusinessLogicHelper.TotalSubjectFirstYear)
            {
                return StudentStateEnum.successful;
            }
            if (sucessCount >= BusinessLogicHelper.SucessCount)
            {
                return StudentStateEnum.transported;
            }
            var numberOfSubjectOfAdministrativeLift = year.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.NumberOfSubjectOfAdministrativeLift).First().Count;
            if (numberOfSubjectOfAdministrativeLift != BusinessLogicHelper.NumberOfSubjectOfAdministrativeLiftIfNotExist && sucessCount >= numberOfSubjectOfAdministrativeLift)
            {
                Note = "ناجح إدارياً";
                return StudentStateEnum.transported;
            }
            return StudentStateEnum.unknown;

        }
        private int TotlaHelpDgree(Students student)
        {
            var subjectHaveHelpDgree = student.StudentSubject.Where(c => c.HelpDegree == true).ToList();
            var total = (int)subjectHaveHelpDgree.Sum(c => (c.SucessMark - c.OrignalTotal));
            return total;
        }
        private int SumOfSubjectThatHaveHelpDgree(Students student)
        {

            return student.StudentSubject.Where(c => c.HelpDegree == true).Count();
        }
    }
}
