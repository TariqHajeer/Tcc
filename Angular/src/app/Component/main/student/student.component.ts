import { Component, OnInit, ViewChild } from '@angular/core';
import { MatFormFieldModule,MatFormFieldControl } from '@angular/material/form-field';
import { Student } from 'src/app/Model/student.model';
import { MoreInformation } from 'src/app/Model/more-information.model';
import { StudentDegree } from 'src/app/Model/student-degree.model';
import { ClosesetPersons } from 'src/app/Model/closeset-persons.model';
import { Partners } from 'src/app/Model/partners.model';
import { Siblings } from 'src/app/Model/siblings.model';
import { StudentAttachment } from 'src/app/Model/student-attachment.model';
import { StudentPhone } from 'src/app/Model/student-phone.model';
import { NationalityService } from 'src/app/Service/nationality.service';
import { ConstraintsService } from 'src/app/Service/constraints.service';
import { CountriesService } from 'src/app/Service/countries.service';
import { LanguagesService } from 'src/app/Service/languages.service';
import { CollegesService } from 'src/app/Service/colleges.service';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { TypeOfRegisterService } from 'src/app/Service/type-of-register.service';
import { DegreeService } from 'src/app/Service/degree.service';
import { AttachmentService } from 'src/app/Service/attachment.service';
import { PhoneTypeService } from 'src/app/Service/phone-type.service';
import { FormControl, FormGroup, NgForm } from '@angular/forms';
import { YearService } from 'src/app/Service/year.service';
import { SocialStatesService } from 'src/app/Service/social-states.service';
import { Registration } from 'src/app/Model/registration.model';
import { MaincountryService } from 'src/app/Service/maincountry.service';
import { StudentService } from 'src/app/Service/student.service';
import { PersonPhone } from 'src/app/Model/person-phone.model';
import { ThemePalette } from '@angular/material/core';
import {ProgressSpinnerMode} from '@angular/material/progress-spinner';
import { StudentAttachmentService } from 'src/app/Service/student-attachment.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { StudyPlanService } from 'src/app/Service/study-plan.service';
import { ResponseStudyPlan } from 'src/app/Model/study-plan.model';
import { Subject } from 'src/app/Model/subject.model';
import { HelpService } from 'src/app/Help/help.service';
import { Router } from '@angular/router';
import { Year } from 'src/app/Model/year.model';
import { MainComponent } from '../main.component';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent extends AppComponent implements  OnInit {
  panelOpenState = false;
 
  
  AddStudent:Student;
  AddMoreInformation:MoreInformation;
  AddStudentDegree:StudentDegree;
  AddRegsteration:Registration;

  
AddPersonPhone:PersonPhone;
GetPersonPhone:PersonPhone[]=[];
  AddClosesetPersons:ClosesetPersons;
  GetClosesetPersons:ClosesetPersons[]=[];
  AddPartners:Partners;
 GetPartners:Partners[]=[];
  AddSiblings:Siblings;
  GetSiblings:Siblings[]=[];
  AddStudentAttachment:StudentAttachment;
  GetStudentAttachment:StudentAttachment[]=[];
  AddStudentPhone:StudentPhone;
  GetStudentPhone:StudentPhone[]=[];

  AddandShowStudent:boolean=true
BirthOfDate(age,name){
var date=new Date()
var year=date.getFullYear()-age
if(name=="MotherAge"){
  this.AddMoreInformation.MotherBirthDay=new Date(year,1,1)
}
if(name=="FatherAge"){
  this.AddMoreInformation.FatherBirthDay=new Date(year,1,1)
}
}
  Age(birthdate,name){
    if(name=="MotherAge"){
      let timeDiff = Math.abs(Date.now() - birthdate.getTime());
      this.AddMoreInformation.MotherAge = Math.floor((timeDiff / (1000 * 3600 * 24))/365.25);
    }
    if(name=="FatherAge"){
      let timeDiff = Math.abs(Date.now() - birthdate.getTime());
      this.AddMoreInformation.FatherAge = Math.floor((timeDiff / (1000 * 3600 * 24))/365.25);
    }
  

  }
  AddStudentAttachments(){
   this.AddStudentAttachment.AttachmentId= this.AddStudentAttachment.getAttachment.Id;   
    this.GetStudentAttachment.push(this.AddStudentAttachment as StudentAttachment ); 
    this.AddStudentAttachment=new StudentAttachment;
  }
  DeleteStudentAttachments(item){
    this.GetStudentAttachment = this.GetStudentAttachment.filter(c => c != item);
  }
  AddClosesetPerson(){
    
    this.AddClosesetPersons.PersonPhone=this.GetPersonPhone;
     this.GetClosesetPersons.push(this.AddClosesetPersons as ClosesetPersons ); 
     this.AddClosesetPersons=new ClosesetPersons;
     this.GetPersonPhone=[];
   }
   DeleteClosesetPerson(item){
     this.GetClosesetPersons = this.GetClosesetPersons.filter(c => c != item);
   }
   AddPersonPhones(){
   this.AddPersonPhone.PhoneTypeId=this.AddPersonPhone.PhoneType.Id;
    this.GetPersonPhone.push(this.AddPersonPhone as PersonPhone ); 
    this.AddPersonPhone=new PersonPhone;
  }
  DeletePersonPhones(item){
    this.GetPersonPhone = this.GetPersonPhone.filter(c => c != item);
  }
   AddPartner(){
    this.AddPartners.NationaliryId= this.AddPartners.Nationaliry.Id;   
     this.GetPartners.push(this.AddPartners as Partners ); 
     this.AddPartners=new Partners;
   }
   DeletePartner(item){
     this.GetPartners = this.GetPartners.filter(c => c != item);
   }
   AddSibling(){
    this.AddSiblings.SocialState= this.AddSiblings.getSocialState.Id;   
     this.GetSiblings.push(this.AddSiblings as Siblings ); 
     this.AddSiblings=new Siblings;
   }
   DeleteSibling(item){
     this.GetSiblings = this.GetSiblings.filter(c => c != item);
   }
   AddStudentPhones(){
    this.AddStudentPhone.PhoneTypeId= this.AddStudentPhone.PhoneType.Id;   
     this.GetStudentPhone.push(this.AddStudentPhone as StudentPhone ); 
     this.AddStudentPhone=new StudentPhone;
   }
   DeleteStudentPhones(item){
     this.GetStudentPhone = this.GetStudentPhone.filter(c => c != item);
   }
  PostStudent(form:NgForm){
   // this.AddStudent.StudentAttachment= this.GetStudentAttachment;
 this.AddStudent.EnrollmentDate=this.date;
   this.AddStudent.MoreInformation=this.AddMoreInformation;
   this.AddStudent.AddRegistrationDTO=this.AddRegsteration;
   this.AddStudent.StudentDegree=this.AddStudentDegree;
    this.AddStudent.StudentPhone= this.GetStudentPhone;
    this.AddStudent.ClosesetPersons= this.GetClosesetPersons;
    this.AddStudent.Partners= this.GetPartners;
    this.AddStudent.Siblings= this.GetSiblings;

    this.showSpinner();
console.log(JSON.stringify(this.AddStudent))
    this.StudentService.postStudent(this.AddStudent).subscribe(res=>{
     
     this.PostStudentAttachment(res.SSN)
      this.MainComponent.StudentService.StudentNeedProcessingCount();
      this.hideSpinner();
    this.HelperService.add();
    if(this.AddandShowStudent==true){
this.AddStudent=new Student
this.date= new Date();
this.AddMoreInformation=new MoreInformation
this.AddRegsteration=new Registration
this.AddStudentDegree=new StudentDegree
this.GetClosesetPersons=[]
this.GetPartners=[]
this.GetSiblings=[]
this.GetStudentPhone=[]
this.GetPersonPhone=[]
this.GetStudentAttachment=[]
//this.resetForm(form);
form.resetForm();
this.showMessage=false
this.date = new Date();
//location.reload();
    }if(this.AddandShowStudent==false){
      this.router.navigate(['/home/studentinfo/',res.SSN]);
     // localStorage.setItem("studentSSN",res.SSN );
    } 
   
    
    },err=>{
      this.hideSpinner();
     
    
    });
  
  
  }
  @ViewChild('fileInput') fileInput;
public stageFile(): void {
  this.AddStudentAttachment.Attachemnt = this.fileInput.nativeElement.files[0];
}
 PostStudentAttachment(ssn){

  this.GetStudentAttachment.forEach(item=>{
    item.Ssn=ssn
this.StudentService.AddStudentAttachment(item).subscribe(res=>{
  console.log(res)
},
err => {
  this.hideSpinner();
});
  })
 }
 
  constructor(public NationaltyService:NationalityService,
    public ConstraintService:ConstraintsService,
    public cityService:CountriesService,
    public LanguageService:LanguagesService,
    public CollegeService:CollegesService,
    public SpecializationService:SpecializationService,
    public TypeOfRegesterService:TypeOfRegisterService,
    public DegreeService:DegreeService,
    public AttachmentService:AttachmentService,
    public PhoneTypeService:PhoneTypeService,
    public YearService:YearService,
    public SocialStateServic:SocialStatesService,
    public CountryServic:MaincountryService,
    public StudentService:StudentService,
    public StudentAttachment:StudentAttachmentService,
    public spinner: NgxSpinnerService,
    public StudyPlanService:StudyPlanService,
    public HelperService:HelpService,
    public router:Router,
    public MainComponent:MainComponent
    ) {
    super( spinner,HelperService);
  }
    
  showMessage:boolean=false;
  ngOnInit() {

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
this.YearService.getYearAll()

this.YearService.GetNonBlockedYear();
this.NationaltyService.getEnabled();
this.StudentService.GetStudentState();
     this.AddStudent=new Student;
     this.AddMoreInformation=new MoreInformation;
     this.AddRegsteration=new Registration;
     this.AddStudentDegree=new StudentDegree;
     this.AddStudentAttachment=new StudentAttachment;
     this.AddStudentPhone=new StudentPhone;
     this.AddClosesetPersons=new ClosesetPersons;
     this.AddPartners=new Partners;
     this.AddSiblings=new Siblings;
     this.AddPersonPhone=new PersonPhone;
     
  }
  date = new Date();

DateAndYear(){
 // this.AddRegsteration.Year=this.YearService.yearall.find(c=>c.Id==this.AddRegsteration.YearId)
 // this.date=new Date(this.AddRegsteration.Year.FirstYear+"/"+this.date.getMonth()+1+"/"+this.date.getDay())
 
//  this. AddRegsteration.YearId==null
//   this.YearService.yearall.forEach(item=>{
//     if(this.date.value.getMonth()+1==1&&item.SecondYear==this.date.value.getFullYear())
//   return  this.AddRegsteration.YearId=item.Id;
//     if(item.FirstYear==this.date.value.getFullYear())
//     this.AddRegsteration.YearId=item.Id;
   
//   })
//   return  this.AddRegsteration.YearId
}

//#region  validation
errormessage:string;
//student
ChackAddStudent():boolean{
  if( this.AddStudent.LanguageId==null||this.AddStudent.FatherName==null
 ||this.AddStudent.FirstName==null||this.AddStudent.LastName==null
 ||this.AddStudent.NationalityId==null||this.AddStudent.Sex==null
 ||this.AddStudent.TypeOfRegistarId==null||this.AddStudent.SpecializationId==null
 ||this.AddStudent.BirthBlace==null||this.AddStudent.BirthDate==null)
 {
   if(this.checkAddStudentDegree()==true||this.checkAddRegestrations()==true)
  return true;
 }
else return false;

}
//Partner
checkWritePartnerName():boolean{
  if(this.AddPartners.Name==null||this.AddPartners.Name=="")
return true;
else
return false;
}
checkAddPartner():boolean{

      if(this.AddPartners.Name==null||this.AddPartners.Name==""||
      this.AddPartners.Nationaliry==null||
      this.AddPartners.PartnerWork==null||this.AddPartners.PartnerWork=="")
    { 
       this.errormessage="ادخل جميع القيم"
      return true;
    }
    else return false;
}
//Siblings
checkWriteSiblingsName():boolean{
  if(this.AddSiblings.Name==null||this.AddSiblings.Name=="")
return true;
else
return false;
}
checkAddSiblings():boolean{

      if(this.AddSiblings.Name==null||this.AddSiblings.Name==""||
      this.AddSiblings.Address==""||
      this.AddSiblings.getSocialState==null||this.AddSiblings.Work=="")
    { 
       this.errormessage="ادخل جميع القيم"
      return true;
    }
    else return false;
}
//closesPerson
checkAddClosesPresone():boolean{
if(this.AddClosesetPersons.Name==null||this.AddClosesetPersons.Address==null
 || this.AddClosesetPersons.PersonPhone==[]){
return true
}
else return false
}
//closesPersonphone
checkclosesPersonphone():boolean{
  if(this.AddPersonPhone.Phone==null||this.AddPersonPhone.PhoneType==null)
return true
else return false
}
//student phone
checkAddPhone():boolean{
if(this.AddStudentPhone.PhoneType==null||this.AddStudentPhone.Phone==null)
return true
else return false
}
//Regestration

checkAddRegestrations():boolean{

      if(this.AddRegsteration.CardDate==null||this.AddRegsteration.CardNumber==null||
      this.AddRegsteration.StudentState==null||this.AddRegsteration.StudyYear==null
      ||this.AddRegsteration.YearId==null)
    { 
       this.errormessage="ادخل جميع القيم"
      return true;
    }
    else return false;
}
//Moreinformation

checkAddMoreinformation():boolean{

  if(this.AddMoreInformation.MotherFirstName==null||this.AddMoreInformation.MotherLastName==null)
{ 
   this.errormessage="ادخل جميع القيم"
  return true;
}
else return false;
}
//student degree
checkAddStudentDegree():boolean{
if(this.AddStudentDegree.DegreeId==null||this.AddStudentDegree.Source==null||this.AddStudentDegree.Date==null
  ||this.AddStudentDegree.Degree==null)
return true
else return false
}
//#endregion
fastRegester(){
this.AddandShowStudent=true
}
AddAndShowStudentProfile(){
this.AddandShowStudent=false
}
// _keyUp(event: any) {
//     const pattern = /[0-9\+\.\ ]/;
//     let inputChar = String.fromCharCode(event.key);

//     if (!pattern.test(inputChar)) {
//       // invalid character, prevent input
//       event.preventDefault();
//     }
// }
}
