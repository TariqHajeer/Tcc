import { MoreInformation } from './more-information.model';
import { StudentDegree } from './student-degree.model';
import { ClosesetPersons } from './closeset-persons.model';
import { Partners } from './partners.model';
import { Siblings } from './siblings.model';
import { StudentAttachment } from './student-attachment.model';
import { StudentPhone } from './student-phone.model';
import { Registration } from './registration.model';
import { Subject } from './subject.model';
import { Year } from './year.model';
import { Constraints } from './constraints.model';
import { Nationality } from './nationality.model';
import { Countries } from './countries.model';
import { Languages } from './languages.model';
import { Colleges } from './colleges.model';
import { Specialization } from './specialization.model';
import { Reperation, Sanctions } from '../Component/main/show-student/student-information/reperation-and-sanctions/reperation-and-sanctions.component';

// for add student
export class Student {

  Ssn: string;
  FirstName: string;
  FatherName: string;
  LastName: string;
  FullName:string=""
  BirthDate: Date;
  BirthBlace: string;
  ConstraintId: number;
  Constrain:Constraints
  ConstraintNumber: number;
  NationalityId: number;
  Nationality:Nationality
  ProvinceId: number;
  Country:Countries
  Sex: any;
  PermanentAddress: string;
  EnrollmentDate: Date;
  LanguageId: number;
  Language:Languages
  TransformedFromId: number;
  SpecializationId: string;
  Specialization:Specialization
  TypeOfRegistarId: number;
  Transformed:Colleges
  MoreInformation: MoreInformation;
  StudentDegree: StudentDegree;
  AddRegistrationDTO: Registration;
  ClosesetPersons: ClosesetPersons[];
  Partners: Partners[];
  Siblings: Siblings[];
   StudentAttachment:StudentAttachment[];
  StudentPhone: StudentPhone[];
  //response
  Registrations:Registration[];
  Reparations:Reperation[]
  Sanctions:Sanctions[]
}
export class StudentStateResponse {
  constructor() {
  }
  Id: number;
  Name: string;
}

export class StudentResponseDTO {
  /**
   *
   */
  constructor(obj?: any) {
    Object.assign(this, obj);
  }
  Ssn: string;
  FirstName: string;
  FatherName: string;
  LastName: string;
  BirthDate: Date
  BirthBlace: string
  ConstraintId: number
 
  ConstraintNumber: number
  NationalityId: number

  ProvinceId: number
 
  Sex: boolean
  PermanentAddress: string
  EnrollmentDate: Date
  LanguageId: number
 
  TransformedFromId: number

  SpecializationId: string
  Specialization:Specialization
  CeasedFromTheCollage: Date
  Created: Date
  CreatedBy: string
  Modified: Date
  ModifiedBy: string
  Subjects: StudentSubjectDTO[] = [];
  public FullName(): string {
    return this.FirstName + " " + this.FatherName + " " + this.LastName;
  }
}
export class StudentSubjectDTO {
  constructor(obj?: any) {
    if (obj != null) {
      Object.assign(this, obj);
    }
    this.message = "0"
    this.healpdegreeMessage = "0"
    this.disabledTheoreticlaDegree = true;
    this.disabledHelpDegree = true;
    this.HelperDegree = 0;
  }
  Id: number
  SSN:string
  StudentName:string
  PracticalDegree: number
  TheoreticlaDegree: number
 public Note: string
  SystemNote: string
  Created: Date
  CreatedBy: string
  Modified: Date
  ModifiedBy: string;
  ExamSemesterId: number;
  HelperDegree: number=0;
  HelpDegree: boolean;
  Subject: Subject;
  Total:number;
  //
  year:Year
  //validation 
  disabledTheoreticlaDegree: boolean = true;
  disabledPracticalDegree:boolean
  message: string;
  healpdegreeMessage: string
  TheoreticlaDegreeMessage: string;
  disabledHelpDegree: boolean = true;
  showEdit:boolean=false
  SuccessSubject:boolean;
  //response
  ExamSemesterNumber:number;
  FirstYear:number;
  SecondYaer:number;
  Updateable:any;

  
  // public IsSuccess(){
  //    return this.PracticalDegree + this.TheoreticlaDegree > this.Subject.SubjectType.SuccessDegree;
  // }
  // TotalOrigenalDegree(): number {
  //   return Number(this.PracticalDegree) + Number(this.TheoreticlaDegree);
  // }
  // HaveHelpDegree() {
  //   return this.HelperDegree != 0 || this.HelperDegree != null;
  // }
  // SucessMark(): number {
  //   return this.Subject.SubjectType.SuccessDegree;
  // }
}
