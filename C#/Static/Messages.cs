using System;
using System.Collections.Generic;
using System.Text;

namespace Static
{
    public static class Messages
    {

        public static BadRequestErrors Null
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "Null"
                };
            }
        }
        public static BadRequestErrors Error
        {
            get
            {
                return new BadRequestErrors();
            }
        }
        public static BadRequestErrors NotFound
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "  غير موجود"
                };
            }
        }
        public static BadRequestErrors Blocked
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = " محظور"
                };
            }
        }
        //public static BadRequestErrors Toolong
        //{
        //    get
        //    {
        //        return new BadRequestErrors(2, "Too Long");
        //    }
        //}
        public static BadRequestErrors CannotDelete
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "لايمكن الحذف"
                };
            }
        }
        public static BadRequestErrors EmptyName
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "الاسم فارغ"
                };
            }
        }
        public static BadRequestErrors ReLoadPage
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "حدث خطأ ما الرجاء إعادة تحميل الصفحة"
                };
            }
        }
        public static BadRequestErrors Exist
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "يوجد اسم مشابه",
                };
            }
        }
        public static BadRequestErrors CannotAdd
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "لايمكن الاضافة  ",
                };
            }
        }
        public static BadRequestErrors Empty
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message="فارغ"
                };
            }
        }
        //public static BadRequestErrors CountryNotCity
        //{
        //    get
        //    {
        //        return new BadRequestErrors(8, "Shoul be Country Not City");
        //    }
        //}
        public static BadRequestErrors CityNotCountry
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "يجب ان تكون مدينة وليس بلد"
                };
            }
        }
        public static BadRequestErrors AnonymousError
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "خطأ غير معرف"
                };
            }
        }
        public static BadRequestErrors OneCharacter
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "يجب ان يكون المعرف من محرف واحد فقط"
                };
            }
        }
        public static BadRequestErrors ValidationError
        {
            get
            {
                return new BadRequestErrors()
                {
                    Message = "خطأ في الإدخال"
                };
            }
        }

    }

}
