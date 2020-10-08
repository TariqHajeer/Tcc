using DAL.infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DAL.HelperEums;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Helper;
using System.Reflection;
namespace DAL.Models
{

    public partial class Attatchments : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Subjects : IEntity
    {

        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
        public bool NeedPartialDegree()
        {
            return this.SubjectType.NominateDegree > 0;
        }
        [NotMapped]
        public int? MainSemesterNumber
        {
            get
            {
                if (this.StudySemester == null)
                    return null;
                return this.StudySemester.Number;
            }
        }

    }
    public partial class Students : IEntity, ISSN
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
        public Registrations NewRegistration()
        {

            var lr = this.Lastregistration;
            if (lr.FinalStateId == (int)StudentStateEnum.Graduated)
                return null;
            var newStateId = lr.FinalStateId == (int)StudentStateEnum.Drained ? (int)StudentStateEnum.Decree : (int)lr.FinalStateId;
            var r = new Registrations()
            {
                TypeOfRegistarId = lr.TypeOfRegistarId,
                Ssn = this.Ssn,
                StudyYearId = this.CurrentStudyYearId,
                StudentStateId = newStateId
            };
            return r;
        }

        public bool IsItTransferredToUs()
        {
            return this.TransformedFromId != null;
        }
        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + FatherName + " " + LastName;
            }
        }
        [NotMapped]
        public Years CurrentYear
        {
            get
            {
                return Lastregistration.Year;
            }
        }
        [NotMapped]
        public int CurrentYearId
        {
            get
            {
                return this.Lastregistration.YearId;
            }
        }
        [NotMapped]
        public int CurrentStudentStatusId
        {
            get
            {
                return this.Lastregistration.StudentStateId;
            }
        }
        [NotMapped]
        public int CurrentStudyYearId
        {
            get
            {
                return Lastregistration.ActuallyStudyYear;

            }
        }
        [NotMapped]
        public Registrations Lastregistration
        {
            get
            {

                // return new List<Registrations>(this.Registrations).OrderBy(c=>c.Id).ToList().Last();
                return this.Registrations.OrderBy(c => c.Id).Last();
            }
        }
        [NotMapped]
        public List<StudentSubject> CurrentStudentSubject
        {
            get
            {
                return this.StudentSubject.Where(c => c.ExamSemester != null && c.ExamSemester.YearId == this.CurrentYearId).
                    GroupBy(c => c.SubjectId).Select(g => g.Last()).ToList();
            }

        }
        [NotMapped]
        public List<StudentSubject> StudentSubjectWithoutDuplicate
        {
            get
            {
                return this.StudentSubject.OrderBy(c => c.Id).GroupBy(c => c.SubjectId).Select(c => c.Last()).ToList();
            }
        }

    }
    public partial class ClosesetPersons : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Colleges : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class Constraints : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Countries : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class CrossOf : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Deattach : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Degree : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class DependenceSubject : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class EquivalentSubject : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class ExamSystem : IEntity
    {
        //test
        // public int G {get{
        //     return 
        // }}
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Graduation : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class Group : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class GroupPrivilage : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Honesty : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Langaues : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class MoreInformation : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class Nationalies : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Partners : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class PersonPhone : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class PhoneType : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Privilage : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Registrations : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

        public int ActuallyStudyYear
        {
            get
            {
                if (this.StudyYearId == (int)StudyYearEnum.SecoundYear)
                {
                    return (int)StudyYearEnum.SecoundYear;
                }
                else
                {
                    switch (this.StudentStateId)
                    {
                        case (int)StudentStateEnum.successful:
                        case (int)StudentStateEnum.transported:
                            {
                                return (int)StudyYearEnum.SecoundYear;
                            }
                        default:
                            return (int)StudyYearEnum.FirstYear;
                    }

                }

            }
        }
        public void SetFaild()
        {
            if (this.ActuallyStudyYear == (int)StudyYearEnum.FirstYear)
            {
                if (StudentStateId == (int)StudentStateEnum.newStudent)
                {
                    FinalStateId = (int)StudentStateEnum.unSuccessful;
                }
                else if (StudentStateId == (int)StudentStateEnum.unSuccessful || StudentStateId == (int)StudentStateEnum.Decree)
                {
                    FinalStateId = (int)StudentStateEnum.Drained;
                }
            }
            else
            {
                FinalStateId = StudentStateId == (int)StudentStateEnum.successful || StudentStateId == (int)StudentStateEnum.transported ? (int)StudentStateEnum.unSuccessful : (int)StudentStateEnum.Drained;
            }
        }
    }
    public partial class Reparations : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class Sanctions : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Settings : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class SettingYearSystem : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class Siblings : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class SocialStates : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Specializations : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class StudentAttachment : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class StudentDegree : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class StudentPhone : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class StudentSubject : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
        public bool IsSuccess()
        {

            if (this.PracticalDegree == null || this.TheoreticlaDegree == null)
                return false;
            if (!this.IsNominate())
            {
                return false;
            }
            return this.OrignalTotal >= this.Subject.SubjectType.SuccessDegree;
        }
        public bool IsNominate()
        {
            try
            {
                return this.PracticalDegree >= this.Subject.SubjectType.NominateDegree;
            }
            catch (Exception ex)
            {
                var x = this;
                return false;
            }
        }
        public bool IsSucessWihtHelpDegree()
        {
            return (bool)this.HelpDegree;
        }
        public bool IsSuccessAnyWay()
        {
            return this.IsSuccess() || this.IsSucessWihtHelpDegree();
        }
        [NotMapped]
        public decimal SucessMark
        {
            get
            {
                return this.Subject.SubjectType.SuccessDegree;
            }
        }
        [NotMapped]
        public int? MainSemesterNumber
        {
            get
            {
                return this.Subject.MainSemesterNumber;
            }
        }
        [NotMapped]
        public decimal OrignalTotal
        {
            get
            {
                if (this.TheoreticlaDegree == null || this.PracticalDegree == null)
                {
                    if (this.TheoreticlaDegree == null)
                    {
                        if (this.PracticalDegree == null)
                        {
                            return 0;
                        }
                        else
                        {
                            return (decimal)this.PracticalDegree;
                        }
                    }
                    else
                    {
                        if (this.PracticalDegree == null)
                        {
                            return (decimal)this.TheoreticlaDegree;
                        }
                    }
                }
                return Math.Ceiling((decimal)(this.PracticalDegree + this.TheoreticlaDegree));
            }
        }
    }
    public partial class StudentState : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class StudyPlan : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class StudySemester : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class StudyYear : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class SubjectType : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Trasmentd : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class TypeOfRegistar : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class User : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }

    }
    public partial class UserGroup : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class ExamSemester : IEntity
    {
        //public List<Subjects> SubjectsTest
        //{
        //    get
        //    {
        //        return this.StudentSubject.GroupBy(c => c.Subject).Select(c => c.First().Subject).ToList();
        //    }
        //}
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
    public partial class Years : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
        [NotMapped]
        public string FullYear
        {
            get
            {
                return this.FirstYear + "/" + this.SecondYear;
            }
        }
        public void GetSubjectCountAndHelpDegreeCount(Students student, out int helpDgree, out int subjectCount)
        {
            helpDgree = 0;
            subjectCount = 0;

            if (student.CurrentStudentStatusId == (int)StudentStateEnum.newStudent || student.CurrentStudentStatusId == (int)StudentStateEnum.transported || student.CurrentStudentStatusId == (int)StudentStateEnum.successful || (student.CurrentStudentStatusId == (int)StudentStateEnum.unknown && student.CurrentStudyYearId == (int)StudyYearEnum.FirstYear))//unknow and first year when student is trasferToUs
            {
                helpDgree = this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumberOfHelpDegreeForStudentWhoWillBecomeUnsuccessful).First().Count;
                subjectCount = this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeUnsuccessful).First().Count;
            }

            if (student.CurrentStudentStatusId == (int)StudentStateEnum.OutOfInstitute || student.CurrentStudentStatusId == (int)StudentStateEnum.unSuccessful || student.CurrentStudentStatusId == (int)StudentStateEnum.Decree)
            {
                //عم عالج إذا كان آخد من قبل اكتر من علامتين مساعدة
                if (student.StudentSubjectWithoutDuplicate.Where(c => c.HelpDegree == true).Sum(c => c.SucessMark - c.OrignalTotal) >= BusinessLogicHelper.NumberOfHelpDegreeThatPriventGiveHelpDegreeForStudentWillBeDrained)
                {
                    helpDgree = -1;
                    subjectCount = -1;
                    return;
                }
                helpDgree = this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumberOfHelpDegreeForStudentWhoWillBecomeDrained).First().Count;
                subjectCount = this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeDrained).First().Count;
            }

        }
        //[NotMapped]
        //public int NumberOfSubjectForGrduatStudent
        //{
        //    get
        //    {
        //        return this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumberOfSubjectForGrudatStudent).First().Count;

        //    }
        //}
        [NotMapped]
        public bool HaveTheredSemester
        {
            get
            {
                return this.ExamSystemNavigation.HaveTheredSemester;
            }
        }

        public bool IsThisSemesterFinshed(int semeserNumber)
        {
            var examSemester = this.ExamSemester.Where(c => c.SemesterNumber == semeserNumber).First();
            if (examSemester.StudentSubject.Where(c => c.PracticalDegree == null || c.TheoreticlaDegree == null).Count() > 0)
                return false;
            return true;
        }
        public int? NonFinishedSemeserNumber()
        {
            if (!this.IsThisSemesterFinshed(1))
            {
                return 1;
            }
            if (!this.IsThisSemesterFinshed(2))
            {
                return 2;
            }
            return null;
        }
        [NotMapped]
        public bool IsDoubleExam

        {
            get
            {
                return this.ExamSystemNavigation.IsDoubleExam;
            }
        }
        public int GetNexSemeserIdCanTsetSubjectIn(StudentSubject studentSubject)
        {
            if (studentSubject.IsSuccess())
                return -1;
            var subjectSemeserNumber = studentSubject.ExamSemester.SemesterNumber;
            if (subjectSemeserNumber == 3)
                return -1;
            if (IsDoubleExam && studentSubject.ExamSemester.SemesterNumber == 1 || HaveTheredSemester)
            {
                var subjectMainSemeserNumber = studentSubject.MainSemesterNumber;
                if (subjectMainSemeserNumber == subjectSemeserNumber && !studentSubject.IsNominate())
                    return -1;

                if (IsDoubleExam && subjectSemeserNumber == 1)
                    return GetSemeserIdByNumber(2);
                if (HaveTheredSemester)
                    return GetSemeserIdByNumber(3);
            }
            return -1;
        }
        public int NumberofSubjectForGraduateStudentSemester()
        {
            if (this.ExamSystemNavigation == null)
                throw new Exception();
            // int? x = (int?)this.ExamSystemNavigation.GraduateStudentsSemester;
            return this.ExamSystemNavigation.GraduateStudentsSemester ?? 0;
        }
        public int GetSemeserIdByNumber(int semeserNumber)
        {
            return this.ExamSemester.Where(c => c.SemesterNumber == semeserNumber).First().Id;
        }
        [NotMapped]
        /// <summary>
        /// عدد علامات المساعدة للطالب الذي سوف يصبح راسب
        /// </summary>
        public int TheNumberOfHelpDegreeForStudentWhoWillBecomeUnsuccessful
        {
            get
            {
                return this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumberOfHelpDegreeForStudentWhoWillBecomeUnsuccessful).First().Count;
            }
        }
        [NotMapped]
        /// <summary>
        /// عدد المواد التي تقسم عليها علامات المساعدة للطالب الذي سوف يصبح راسب
        /// </summary>
        public int TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeUnsuccessful
        {
            get
            {
                return this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeUnsuccessful).First().Count;
            }
        }
        [NotMapped]
        /// <summary>
        /// عدد علامات السماعدة للطالب الذي سوف يصبح مستنفذ
        /// </summary>
        public int TheNumberOfHelpDegreeForStudentWhoWillBecomeDrained
        {
            get
            {
                return this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumberOfHelpDegreeForStudentWhoWillBecomeDrained).First().Count;
            }
        }
        [NotMapped]
        /// <summary>
        /// عدد المواد التي تقسم عليها علامات المساعدة للطالب الذي سوف يصبح مستنفذ
        /// </summary>
        public int TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeDrained
        {
            get
            {
                return this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeDrained).First().Count;
            }
        }
        public int NumberOfSubjectOfAdministrativeLift
        {
            get
            {
                return this.YearSystemNavigation.SettingYearSystem.Where(c => c.SettingId == (int)SettingEnum.NumberOfSubjectOfAdministrativeLift).First().Count;
            }
        }
    }
    public partial class YearSystem : IEntity
    {
        public string Log()
        {
            string log = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                try
                {
                    if (prop.GetValue(this, null) != null)
                        log += $"PropertyName:  {prop.Name}  Value {prop.GetValue(this, null)}\n";
                }
                catch
                {
                    log += "En\n";
                }
            }
            return log;
        }
    }
}
