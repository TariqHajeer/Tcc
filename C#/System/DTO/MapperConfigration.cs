using AutoMapper;
using System.Linq;
using DAL.Models;
using System.DTO.GroupDTO;
using System.DTO.UserDTO;
using System.DTO.Attachemnt;
using System.DTO.DegreeDTO;
using System.DTO.College;
using System.DTO.LanguageDTO;
using System.DTO.NationalityDTO;
using System.DTO.CountryDTO;
using System.DTO.Specialization;
using System.DTO.SocialStatesDTO;
using System.DTO.PrivilageDTO;
using System.DTO.PhoneTypeDTO;
using System.DTO.TypeOfRegisterDTO;
using System.DTO.HonestyDTO;
using System.DTO.ConstraintDTO;
using System.DTO.YearSystemDTO;
using System.DTO.YearDTO;
using System.DTO.SubjectTypeDTO;
using System.DTO.StudyPlanDTO;
using System.DTO.SubjectDTO;
using System.DTO.StudentDTO;
using System.DTO.ExamSemesterDTO;
using System.DTO.ExamSystemDTO;
using System.DTO.StudentSubjectDTOs;
using System.DTO.CommonDTO;
using System.DTO.RegistrationDTOs;

namespace System.DTO
{
    public class MapperConfigration : Profile
    {
        readonly IMapper _mapper;
        public MapperConfigration(IMapper mapper)
            => _mapper = mapper;
        public MapperConfigration()
        {

            #region group
            CreateMap<Privilage, PrivelegGroupDTO>();
            CreateMap<Group, GroupResopnseDTO>()
                .ForMember(r => r.PrivilagesCount, opt => opt.MapFrom(g => g.GroupPrivilage.Count))
                .ForMember(r => r.UserCount, opt => opt.MapFrom(g => g.UserGroup.Count))
                .ForMember(r => r.Privilages, opt => opt.MapFrom(g => g.GroupPrivilage.Select(c => c.Privilage)))
                .ForMember(r => r.User, opt => opt.MapFrom(g => g.UserGroup.Select(u => u.User)));
            #endregion
            #region User
            //user 
            CreateMap<User, UserLoginDTO>();
            CreateMap<UserLoginDTO, User>();
            // ask any one have an knowledge in autoMapper or learn it
            CreateMap<User, UserResponDTO>()
                .ForMember(ur => ur.GroupCount, opt => opt.MapFrom(ug => ug.UserGroup.Count))
                .ForMember(u => u.Group, opt => opt.MapFrom(ug => ug.UserGroup.Select(g => g.Group)));
            CreateMap<AddUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();


            #endregion
            #region Attachment
            CreateMap<AddAttachmentDTO, Attatchments>();
            CreateMap<Attatchments, AttachemetResponseDTO>();
            CreateMap<UpdateAttachmentDTO, Attatchments>().
                ForMember(r => r.StudentAttachment, opt => opt.Ignore());

            #endregion
            #region Degree
            CreateMap<AddDegreeDTO, Degree>();
            CreateMap<Degree, DegreeResponseDTO>();
            CreateMap<UpdateDegreeDTO, Degree>();
            #endregion
            #region College
            CreateMap<AddCollegeDTO, Colleges>();
            CreateMap<Colleges, CollegesResponseDTO>()
                .ForMember(c => c.City, opt => opt.MapFrom(g => g.Province));
            CreateMap<UpdateCollegeDTO, Colleges>();
            #endregion
            #region Language
            CreateMap<AddLanguageDTO, Langaues>();
            CreateMap<Langaues, LanguageResponseDTO>();
            CreateMap<UpdateLanguageDTO, Langaues>();
            #endregion
            #region Natinality
            CreateMap<AddNationalityDTO, Nationalies>();
            CreateMap<Nationalies, NationalityResponseDTO>();
            CreateMap<UpdateNAtionalityDTO, Nationalies>();
            #endregion
            #region Specializations
            CreateMap<AddSpecializationDTO, Specializations>();
            CreateMap<Specializations, SpecialziationResponseDTO>();
            CreateMap<UpdateSpecializationDTO, Specializations>();
            #endregion
            #region SocialStates
            CreateMap<AddSocialStatesDTO, SocialStates>();
            CreateMap<SocialStates, SocialStatesResponseDTo>();
            CreateMap<UpdateSocialStatesDTO, SocialStates>();
            #endregion
            #region Privilage
            CreateMap<Privilage, PrivilageResponseDTO>()
             .ForMember(r => r.groupCount, opt => opt.MapFrom(o => o.GroupPrivilage.Count))
             .ForMember(r => r.groups, opt => opt.MapFrom(o => o.GroupPrivilage.Select(u => u.Group)));
            #endregion
            #region PhoneType
            CreateMap<AddPhoneTypeDTO, PhoneType>();
            CreateMap<PhoneType, PhoneTypeResponseDTO>();
            CreateMap<UpdatePhoneTypeDTO, PhoneType>();
            CreateMap<AddPersonPhone, PersonPhone>();
            CreateMap<AddStudentPhoneDTO, StudentPhone>();
            #endregion
            #region TypeOfRegistar
            CreateMap<AddTypeOfRegistarDTO, TypeOfRegistar>();
            CreateMap<TypeOfRegistar, TypeOfRegistarResponseDTO>();
            CreateMap<UpdateTypeOfRegistarDTO, TypeOfRegistar>();
            #endregion
            #region Honesty
            CreateMap<AddHonestyDTO, Honesty>();
            CreateMap<Honesty, HonestyResponseDTO>().
                ForMember(c => c.City, opt => opt.MapFrom(o => o.Country));
            CreateMap<UpdateHonestyDTO, Honesty>();
            #endregion
            #region Constraints
            CreateMap<AddConstraintDTO, Constraints>();
            CreateMap<Constraints, ConstraintResponseDTO>()
                .ForMember(c => c.Honesty, opt => opt.MapFrom(o => o.Honesty));
            CreateMap<UpdateConstraintDTO, Constraints>();
            #endregion
            #region Countries
            CreateMap<AddCountryDTO, Countries>();
            CreateMap<Countries, CountryResponseDTO>();
            CreateMap<UpdateCountryDTO, Countries>();
            #endregion
            #region City
            CreateMap<AddCityDTO, Countries>();
            CreateMap<Countries, CityResponseDTO>()
                .ForMember(c => c.Country, opt => opt.MapFrom(o => o.MainCountryNavigation));
            CreateMap<UpdateCityDTO, Countries>();

            #endregion
            #region year system
            CreateMap<YearSystem, ResponseYearSystem>().
                ForMember(Ry => Ry.Settings, opt => opt.MapFrom(y => y.SettingYearSystem))
                .ForMember(c => c.Updateable, opt => opt.MapFrom(src => src.Years.Count == 0));
            CreateMap<AddYearSystemDTO, YearSystem>();
            CreateMap<UpdateYearSystem, YearSystem>();
            #endregion
            #region year 
            CreateMap<Years, YearResponseDTO>()
                .ForMember(c => c.ExamSystem, opt => opt.MapFrom(y => y.ExamSystemNavigation))
                .ForMember(c => c.Yearystem, opt => opt.MapFrom(y => y.YearSystemNavigation));
            CreateMap<AddYearDTO, Years>();
            CreateMap<UpdateYearDTO, Years>();

            #endregion
            #region ExamSystem
            CreateMap<AddExamSystemDTO, ExamSystem>();
            CreateMap<ExamSystem, ResponseExamSystemDTO>()
                .ForMember(c => c.Updateable, opt => opt.MapFrom(src => src.Years.Count == 0));
            #endregion
            #region SubjectType
            CreateMap<AddSubjectTypeDTO, SubjectType>();
            CreateMap<SubjectType, SubjectTypeResponseDTO>();
            #endregion  
            #region Subject
            CreateMap<AddSubjectDTO, Subjects>();
            CreateMap<Subjects, ResponseSubjectDTO>()
                .ForMember(rs => rs.EqvuvalentSubject, src => src.MapFrom((subject, responseSubject, i, context) =>
                 {
                     var firstEquivalentSubject = subject.EquivalentSubjectFirstSubjectNavigation.Select(c => c.SecoundSubjectNavigation);
                     var secoundEquivalentSubject = subject.EquivalentSubjectSecoundSubjectNavigation.Select(c => c.FirstSubjectNavigation);
                     var allEquvilaSubject = firstEquivalentSubject.Union(secoundEquivalentSubject);
                     return context.Mapper.Map<EquivalentSubjectDTO[]>(allEquvilaSubject);
                 }))
                .ForMember(rs => rs.DependOnSubjects, src => src.MapFrom((subject, responseSubject, i, context) =>
                {
                    return context.Mapper.Map<ResponseSubjectDTO[]>(subject.DependenceSubjectSubject.Select(c => c.DependsOnSubject));
                })).PreserveReferences()
                .ForMember(rs => rs.SubjectsDependOnMe, src => src.MapFrom((subject, responSubject, i, context) =>
                {
                    return context.Mapper.Map<ResponseSubjectDTO[]>(subject.DependenceSubjectDependsOnSubject.Select(c => c.Subject));
                })).PreserveReferences()
                .ForMember(rs => rs.SubjectType, src => src.MapFrom((subject, SubjectTypeDTO, i, context) =>
                {
                    return context.Mapper.Map<SubjectType, SubjectTypeResponseDTO>(subject.SubjectType);
                }));
            CreateMap<Subjects, EquivalentSubjectDTO>()
                .ForMember(rs => rs.SemesterNumber, src => src.MapFrom(s => s.StudySemester.Number))
                .ForMember(rs => rs.StudyYearName, src => src.MapFrom(s => s.StudySemester.StudyYear.Name))
                .ForMember(rs => rs.SpecializationName, src => src.MapFrom(s => s.StudySemester.Studyplan.Specialization.Name))
                .ForMember(rs => rs.FirstYear, src => src.MapFrom(s => s.StudySemester.Studyplan.Year.FirstYear))
                .ForMember(rs => rs.SecondYear, src => src.MapFrom(s => s.StudySemester.Studyplan.Year.SecondYear))
                .ForMember(rs => rs.SubjectType, src => src.MapFrom((subject, SubjectTypeDTO, i, context) =>
                {
                    return context.Mapper.Map<SubjectType, SubjectTypeResponseDTO>(subject.SubjectType);
                }));

            #endregion
            #region StudyPlan
            CreateMap<AddStudyPalnDTO, StudyPlan>();
            CreateMap<StudyPlan, StudyPlanResponse>()
            .ForMember(rs=>rs.Updateable,opt=>opt.MapFrom(src=>!src.Year.Blocked));
            CreateMap<StudyYear, StudyYearDTO>();
            CreateMap<DAL.Models.StudySemester, StudySemesterDTO>();
            CreateMap<UPdateStudyPlan, StudyPlan>();
            #endregion
            #region student
            CreateMap<AddMoreInformationDTO, MoreInformation>();
            CreateMap<MoreInformation, AddMoreInformationDTO>();
            CreateMap<AddStudentDegreeDTO, StudentDegree>();
            CreateMap<AddStudetnDTO, Students>();
            
            CreateMap<UpdateMoreInformationDto,MoreInformation>();
            CreateMap<UpdateStudnetInformation,Students>()
            .ForMember(src=>src.MoreInformation,opt=>opt.MapFrom((source,destination,i,contet)=>{
                return contet.Mapper.Map<UpdateMoreInformationDto,MoreInformation>(source.MoreInformation,destination.MoreInformation);
            }));
            CreateMap<AddClosestPerson, ClosesetPersons>();

            CreateMap<AddPartnersDTO, Partners>();

            CreateMap<AddSbilingsDTO, Siblings>();

            CreateMap<AddRegistrationDTO, Registrations>();
            CreateMap<Registrations, RegistrationDTO>();

            CreateMap<RegistrationDTO, CreateRegistrationDTO>();
            CreateMap<CreateRegistrationDTO, RegistrationDTO>();
            //CreateMap<Attatchments,>
            //.ForMember(c => c.MoreInformation, opt => opt.Ignore())
            //.ForMember(c => c.Siblings, opt => opt.Ignore())
            //.ForMember(c => c.StudentDegree, opt => opt.Ignore());

            //.ForMember(s => s.StudentDegree, src => src.MapFrom((addSubjectDTO,subject,i,context) =>
            //{
            //    return context.Mapper.Map<AddStudentDegreeDTO, StudentDegree>(addSubjectDTO.StudentDegree);
            //}));
            CreateMap<AddSbiling, Siblings>();
            CreateMap<AddClosestPersonForStudent, ClosesetPersons>();
            CreateMap<AddPartnersForStudent, Partners>();
            CreateMap<AddReprationsForStudent, Reparations>();
            CreateMap<AddSanctionsForStudent, Sanctions>();
            CreateMap<AddPhoneForStudent, StudentPhone>();
            CreateMap<Students, TransmetedStudentResponseDTO>()
            .ForMember(rs => rs.Subjects, src => src.MapFrom((studentSubject, responseSubject, i, context) =>
            {
                return context.Mapper.Map<StudentSubjectDTO[]>(studentSubject.StudentSubject);
            }));
            CreateMap<Students, StudentsResponseDTO>()
                .ForMember(rs => rs.CurrentState, src => src.MapFrom(s => s.Lastregistration.StudentState.Name))
                .ForMember(rs => rs.CurrentYear, src => src.MapFrom(s => s.Lastregistration.Year.FullYear))
                .ForMember(rs => rs.FinalState, src => src.MapFrom(s => s.Lastregistration.FinalState.Name))
                .ForMember(rs => rs.SpecializationName, src => src.MapFrom(s => s.Specialization.Name))
                .ForMember(rs => rs.ActuallyStudyYear, opt => opt.MapFrom(src => src.Registrations.Last().ActuallyStudyYear));
            CreateMap<Students, ResponseStudentDTO>()
                .ForMember(response => response.MoreInformation, src => src.MapFrom((student, responseStudent, i, context) =>
                     {
                         return context.Mapper.Map<AddMoreInformationDTO>(student.MoreInformation);
                     }))
                .ForMember(response => response.StudentDegree, src => src.MapFrom((studentDegree, student, i, context) =>
                    {
                        return context.Mapper.Map<StudentDegreeDTO>(studentDegree.StudentDegree);
                    }))
                .ForMember(response => response.ClosesetPersons, src => src.MapFrom((student, rstudent, i, context) =>
                    {
                        return context.Mapper.Map<ClosesetPersons[]>(student.ClosesetPersons);
                    }))
                .ForMember(response => response.Partners, src => src.MapFrom((student, rstudent, i, context) =>
                    {
                        return context.Mapper.Map<PartnerDTO[]>(student.Partners);
                    }))
                .ForMember(response => response.Reparations, src => src.MapFrom((student, rstudent, i, context) =>
                    {
                        return context.Mapper.Map<ReparationDTO[]>(student.Reparations);
                    }))
                .ForMember(response => response.Sanctions, src => src.MapFrom((student, rstudent, i, context) =>
                    {
                        return context.Mapper.Map<SanctionDTO[]>(student.Sanctions);
                    }))
                .ForMember(response => response.StudentAttachment, src => src.MapFrom((student, rstudent, i, context) =>
                    {
                        return context.Mapper.Map<StudentAttachmentDTO[]>(student.StudentAttachment);
                    }))
                .ForMember(response => response.StudentPhone, src => src.MapFrom((student, rstudent, i, context) =>
                    {
                        return context.Mapper.Map<StudentPhoneDTO[]>(student.StudentPhone);
                    }));
            CreateMap<StudentDegree, StudentDegreeDTO>();
            CreateMap<PersonPhone, PersonPhoneDTO>();
            CreateMap<ClosesetPersons, ResponseClosesetPersons>()
                .ForMember(res => res.PersonPhone, src => src.MapFrom((cp, rcp, i, context) =>
                {
                    return context.Mapper.Map<PersonPhoneDTO[]>(cp.PersonPhone);
                }));

            CreateMap<Partners, PartnerDTO>();
            CreateMap<Reparations, ReparationDTO>();
            CreateMap<Sanctions, SanctionDTO>();
            CreateMap<Siblings, SiblingDTO>();
            CreateMap<StudentAttachment, StudentAttachmentDTO>();
            CreateMap<StudentPhone, StudentPhoneDTO>();
            
            #endregion

            #region studentSubject
            CreateMap<StudentSubject, StudentSubjectDTO>()
               .ForMember(rs => rs.SSN, src => src.MapFrom(s => s.Ssn))
               .ForMember(rs => rs.StudentName, src => src.MapFrom(s => s.SsnNavigation == null ? " " : s.SsnNavigation.FullName))
               .ForMember(rs => rs.Subject, src => src.MapFrom((studentSubject, responseSubject, i, context) =>
               {
                   return context.Mapper.Map<ResponseSubjectDTO>(studentSubject.Subject);
               }))
               .ForMember(rs => rs.ExamSemesterNumber, src => src.MapFrom(opt => opt.ExamSemester.SemesterNumber))
               .ForMember(rs => rs.FirstYear, src => src.MapFrom(opt => opt.ExamSemester.Year.FirstYear))
               .ForMember(rs => rs.Updateable, src => src.MapFrom(opt => !opt.ExamSemester.Year.Blocked))
               .ForMember(rs => rs.ExamSemesterNumber, src => src.MapFrom(s => s.ExamSemester.SemesterNumber))
               .ForMember(rs => rs.FirstYear, src => src.MapFrom(s => s.ExamSemester.Year.FirstYear))
               .ForMember(rs => rs.Updateable, src => src.MapFrom(s => !s.ExamSemester.Year.Blocked));


            CreateMap<StudentSubjectDTO, StudentSubject>()
                .ForMember(sss => sss.Subject, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.Created, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.CreatedBy, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.SystemNote, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.Modified, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.ModifiedBy, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.Ssn, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.SubjectId, opt => { opt.UseDestinationValue(); opt.Ignore(); })
                .ForMember(ss => ss.ExamSemesterId, opt => { opt.UseDestinationValue(); opt.Ignore(); });

            CreateMap<UpdateStudentSubjectDto, StudentSubject>();
            #endregion
            CreateMap<ExamSemester, ResponseExamSemesterDTO>();
            CreateMap<AddExamSemesterDTO, ExamSemester>();

        }

    }
}
