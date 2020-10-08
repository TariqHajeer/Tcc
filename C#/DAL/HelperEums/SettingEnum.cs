using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.HelperEums
{
    public enum SettingEnum
    {

        /// <summary>
        /// عدد مواد الترفع الإداري
        /// </summary>
        NumberOfSubjectOfAdministrativeLift= 1,
        /// <summary>
        /// عدد مواد الطالب خارج المعهد
        /// </summary>
        TheNumberOfSubjectsForStudentsOutsideTheInstitute = 2,

        /// <summary>
        /// عدد علامات السماعدة للطالب الذي سوف يصبح مستنفذ
        /// </summary>
        TheNumberOfHelpDegreeForStudentWhoWillBecomeDrained = 6,
        /// <summary>
        /// عدد المواد التي تقسم عليها علامات المساعدة للطالب الذي سوف يصبح مستنفذ
        /// </summary>
        TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeDrained = 7,
        /// <summary>
        /// عدد علامات المساعدة للطالب الذي سوف يصبح راسب
        /// </summary>
        TheNumberOfHelpDegreeForStudentWhoWillBecomeUnsuccessful =8, 
        /// <summary>
        /// عدد المواد التي تقسم عليها علامات المساعدة للطالب الذي سوف يصبح راسب
        /// </summary>
        TheNumnerOfSubjectThatHelpDegeeDivideOnItForStudentWhoWillBecomeUnsuccessful=9,


    }
}
