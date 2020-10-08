import { Component, OnInit } from '@angular/core';
import { PersonPhone } from 'src/app/Model/person-phone.model';
import { ClosesetPersons } from 'src/app/Model/closeset-persons.model';
import { Partners } from 'src/app/Model/partners.model';
import { Siblings } from 'src/app/Model/siblings.model';
import { StudentAttachment } from 'src/app/Model/student-attachment.model';
import { StudentPhone } from 'src/app/Model/student-phone.model';
import { PhoneTypeService } from 'src/app/Service/phone-type.service';
import { SocialStatesService } from 'src/app/Service/social-states.service';
import { NationalityService } from 'src/app/Service/nationality.service';
import { AttachmentService } from 'src/app/Service/attachment.service';
import { Student, StudentStateResponse } from 'src/app/Model/student.model';
import { MoreInformation } from 'src/app/Model/more-information.model';
import { StudentDegree } from 'src/app/Model/student-degree.model';
import { Registration } from 'src/app/Model/registration.model';
import { StudentService } from 'src/app/Service/student.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HelpService } from 'src/app/Help/help.service';
import { ConstraintsService } from 'src/app/Service/constraints.service';
import { CountriesService } from 'src/app/Service/countries.service';
import { LanguagesService } from 'src/app/Service/languages.service';
import { CollegesService } from 'src/app/Service/colleges.service';
import { TypeOfRegisterService } from 'src/app/Service/type-of-register.service';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { MaincountryService } from 'src/app/Service/maincountry.service';
import { DegreeService } from 'src/app/Service/degree.service';
import { YearService } from 'src/app/Service/year.service';
import { StudentSubjectService } from 'src/app/Service/student-subject.service';
import { ActivatedRoute } from '@angular/router';
import { StudentSubjectDTOService } from 'src/app/Service/student-subject-dto.service';
import { HttpClient } from '@angular/common/http';
import { Reperation, Sanctions } from './reperation-and-sanctions/reperation-and-sanctions.component';
import { MainComponent } from '../../main.component';


@Component({
  selector: 'app-student-information',
  templateUrl: './student-information.component.html',
  styleUrls: ['./student-information.component.css']
})
export class StudentInformationComponent extends AppComponent implements OnInit {

  constructor(public PhoneTypeService: PhoneTypeService
    , public SocialStateServic: SocialStatesService,
    public NationaltyService: NationalityService,
    public AttachmentService: AttachmentService,
    public studentService: StudentService,
    public spinner: NgxSpinnerService,
    public ConstraintService: ConstraintsService,
    public CountryServic: MaincountryService,
    public cityService: CountriesService,
    public LanguageService: LanguagesService,
    public CollegeService: CollegesService,
    public SpecializationService: SpecializationService,
    public TypeOfRegesterService: TypeOfRegisterService,
    public DegreeService: DegreeService,
    public YearService: YearService,
    public HelperService: HelpService,
    public studentSubjectService: StudentSubjectService,
    public getrouter: ActivatedRoute
    , public studentSubjectDTOService: StudentSubjectDTOService,
    public http:HttpClient,
    public MainComponent:MainComponent
  ) { super(spinner, HelperService) }
  show: any[]
  selectedIndex
  date

  GetClosesetPersons: ClosesetPersons[] = [];
  ShowPersonPhone: PersonPhone[] = [];
  GetPartners: Partners[] = [];

  GetSiblings: Siblings[] = [];

  GetStudentAttachment: StudentAttachment[] = [];

  GetStudentPhone: StudentPhone[] = [];
  Student: Student;
  MoreInformation: MoreInformation;
  StudentDegree: StudentDegree;
  Regsteration: Registration[];
  SSN
  GetReperations:Reperation[] = []
  GetSanctions:Sanctions[] = []
  showMessage: boolean = false;
  errormessage: string;
  ngOnInit(): void {
  this.Student=new Student
  this.Student.FullName=" "
    var show = localStorage.getItem("helpdgree")
    if (show == "helpdgree") {
      this.show = [false, false, false, false, false, true, false, false, false, false, false, false];
      this.selectedIndex = 5;
      localStorage.removeItem("helpdgree")
    }
    else{
      this.show = [true, false, false, false, false, false,false, false, false, false, false, false, false];
      this.selectedIndex = 0;
    }
  



    this.MoreInformation = new MoreInformation
    this.StudentDegree = new StudentDegree
    this.Student = new Student
    //this.SSN=localStorage.getItem("studentSSN");
    this.getrouter.params.subscribe(par => {
      this.SSN = par['Ssn'] as string
    });

    this.getStudent();
    // this.SubjectNeedHelpDegree()
    // this.SubjectCanReset()

  }
 TabIndex
  tabClick(tab) {
    //console.log(tab)
    this.TabIndex=tab.index
    for (let index = 0; index < this.show.length; index++) {
      // if (index == 5 || index == 6 || tab.index == 6 || tab.index == 5) {
      //   if (this.GetSubjectCanReset == [] || this.GetSubjectCanReset == null || this.GetSubjectCanReset == undefined
      //     || this.GetSubjectNeedHelpDegree == [] || this.GetSubjectNeedHelpDegree == null || this.GetSubjectNeedHelpDegree == undefined) {
      //     this.show[index] = false;
        
      //   }
      // }
       if (index == tab.index)
      {  this.show[index] = true;
      }
      else
       { this.show[index] = false;
      }
    }
  }

  getStudent() {
    this.showSpinner();
    this.PhoneTypeService.getEnabled();
    this.NationaltyService.getEnabled()
    this.SocialStateServic.getEnabled();
    this.AttachmentService.getEnabled();
    //get Constraint
    this.ConstraintService.getEnabled();
    //get city
    this.cityService.getEnabled();
    //get Language
    this.LanguageService.getEnabled();
    //get College
    this.CollegeService.getEnabled();
    //get Specialization
    this.SpecializationService.getEnabled();
    //get TypeOfRegester
    this.TypeOfRegesterService.getEnabled();
    //get Degree
    this.DegreeService.getEnabled();
    //get Attachment
    this.AttachmentService.getEnabled();
    //get PhoneType
    this.PhoneTypeService.getEnabled();

    this.SocialStateServic.getEnabled();

    this.CountryServic.getEnabled();

    this.YearService.getYearAll();
    this.NationaltyService.getEnabled();
    this.studentService.GetStudentState();
    this.studentService.GetStudentBySSN(this.SSN).subscribe(
      res => {
        this.hideSpinner()
        this.Student = res as Student
        console.log(this.Student)
        if (this.Student.Sex == true) this.Student.Sex = 1
        else if (this.Student.Sex == false) this.Student.Sex = 0
        this.Student.FullName = this.Student.FirstName + " " + this.Student.FatherName + " " + this.Student.LastName
        this.MoreInformation = this.Student.MoreInformation
        this.Regsteration = this.Student.Registrations
        this.Regsteration.forEach(item=>{
          if(item.FinalStateId==undefined||item.FinalStateId==null)
          item.FinalState=new StudentStateResponse()
          else
          item.FinalState=this.studentService.StudentState.find(s=>s.Id==item.FinalStateId)
          item.StudentState=this.studentService.StudentState.find(s=>s.Id==item.StudentStateId)
          item.TypeOfRegistar=this.TypeOfRegesterService.typeofregisterall.find(s=>s.Id==item.TypeOfRegistarId)
          item.Year=this.YearService.years.find(s=>s.Id==item.YearId)
         
        })
        this.StudentDegree = this.Student.StudentDegree
        this.GetClosesetPersons = this.Student.ClosesetPersons
        this.GetClosesetPersons.forEach(item => {
          this.ShowPersonPhone = item.PersonPhone
          this.ShowPersonPhone.forEach(ph => {
            ph.PhoneType = this.PhoneTypeService.phonetypeall.find(p => p.Id == ph.PhoneTypeId)
          })
        })
        this.GetPartners = this.Student.Partners
        this.GetPartners.forEach(item => {
          item.Nationaliry = this.NationaltyService.nationaltyall.find(n => n.Id == item.NationaliryId)
        })
        this.GetSiblings = this.Student.Siblings
        this.GetSiblings.forEach(item => {
          item.getSocialState = this.SocialStateServic.socialstateall.find(n => n.Id == item.SocialState)
        })
        this.GetStudentPhone = this.Student.StudentPhone
        this.GetStudentPhone.forEach(item => {
          item.PhoneType = this.PhoneTypeService.phonetypeall.find(p => p.Id == item.PhoneTypeId)
        })

        this.GetStudentAttachment = this.Student.StudentAttachment
        this.GetStudentAttachment.forEach(item => {
          item.getAttachment = this.AttachmentService.attachmentall.find(p => p.Id == item.AttachmentId)
        
        })
        this.GetReperations=this.Student.Reparations
        this.GetSanctions=this.Student.Sanctions

      }, err => {

        this.hideSpinner()
      }
    )
  }
 
  // GetSubjectCanReset = []
  // SubjectCanReset() {
  //   this.studentSubjectService.CanReset(this.SSN).subscribe(res => {
  //     this.GetSubjectCanReset = res as any[]
  //   }, err => {
  //     this.GetSubjectCanReset = null
  //   })
  // }

  // GetSubjectNeedHelpDegree
  // SubjectNeedHelpDegree() {
  //   this.studentSubjectService.SubjectNeedHelpDegree(this.SSN).subscribe(res => {
  //     this.GetSubjectNeedHelpDegree = res

  //   }, err => {
  //     this.GetSubjectNeedHelpDegree = null
  //   })
  // }
// HiddenTab(index):boolean{
// if(index==4||index==5)
// if (this.GetSubjectCanReset == [] || this.GetSubjectCanReset == null || this.GetSubjectCanReset == undefined
//   || this.GetSubjectNeedHelpDegree == [] || this.GetSubjectNeedHelpDegree == null || this.GetSubjectNeedHelpDegree == undefined)
// return false
// else return true
// }
ReloadPage(){
  this.getStudent();
  this.MainComponent.StudentService.StudentsNeedHelpDegreeCounts();
}
}

