using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.HelperEums
{
    public enum StudentStateEnum
    {

        /// <summary>
        /// 
        /// </summary>
        Non = 0,
        /// <summary>
        /// مستجد
        /// </summary>
        newStudent = 1,
        /// <summary>
        /// ناجح
        /// </summary>
        successful = 2,
        /// <summary>
        /// منقول
        /// </summary>
        transported = 3,
        /// <summary>
        /// راسب متلنا
        /// </summary>
        unSuccessful = 4,

        /// <summary>
        /// خارج المعهد
        /// </summary>
        OutOfInstitute=5,

        /// <summary>
        /// مستنفذ
        /// </summary>
        Drained = 6,

        /// <summary>
        /// مرسوم
        /// </summary>
        Decree=7,
        /// <summary>
        /// هي للطلاب يلي لازم عالج وضعون
        /// </summary>
        unknown = 8,
        /// <summary>
        /// متخرج
        /// </summary>
        Graduated= 9

        
    }
}
