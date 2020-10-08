import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { StudentStateResponse,  StudentResponseDTO, StudentSubjectDTO, Student } from '../Model/student.model';
import { Observable } from 'rxjs';
import { StudentAttachment } from 'src/app/Model/student-attachment.model';
import { CreateRegistrationDTO, Registration } from '../Model/registration.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
controler:string="Student/";
GetTransferStudent:StudentResponseDTO[]=[];
StudentState:StudentStateResponse[];
  constructor(private http:HttpClient) { }
  postStudent(student):Observable<any>{
   return this.http.post(environment.BaseUrl+this.controler,student);
  }
  GetStudentBySSN(ssn){
    let Ssn= new HttpParams().set('ssn',ssn);
    return this.http.get(environment.BaseUrl+this.controler+"BySSN?"+Ssn)
  }
  GetStudentState(){
    return this.http.get(environment.BaseUrl+this.controler+"GetStudentState").toPromise().then(
      res=>{
this.StudentState=res as StudentStateResponse[]
      }
    );
  }
StudentCount
  StudentNeedProcessingCount(){
    return this.http.get(environment.BaseUrl+this.controler+"StudentNeedProcessingCount").toPromise().then(
      res=>{
this.StudentCount=res as number
      }
    );
  }
  studentsNeedHelpDegreeCount
  StudentsNeedHelpDegreeCounts(){
    return this.http.get(environment.BaseUrl+this.controler+"StudentsNeedHelpDegreeCount").toPromise().then(
      res=>{
this.studentsNeedHelpDegreeCount=res as number

      }
    );
  }
  StudentNeedProcessing(){
    return  this.http.get(environment.BaseUrl+this.controler+"StudentsNeedProcessing");
 
  }

  StudentPreviousYearSetting(ssn ){
    let parameter= new HttpParams().set('SSN',ssn);
   return this.http.get(environment.BaseUrl+this.controler+"StudentPreviousYearSetting",{params:parameter});
  }
  // StudentNeedProcessingbySsn(ssn){
  //   let parameter= new HttpParams().set('ssn',ssn);
  //   return this.http.get('...')
  //   return this.http.get(environment.BaseUrl+this.controler+"StudentNeedProcessing",{params:parameter});
  // }
  StudentNeedProcessingbySsn(ssn){
    let parameter= new HttpParams().set('ssn',ssn);
    return this.http.get<StudentResponseDTO>(environment.BaseUrl+this.controler+"StudentNeedProcessing",{params:parameter});
  }
  // StudentNeedProcessingbySsn(ssn){
  //   let parameter= new HttpParams().set('ssn',ssn);
  //   return this.http.get(environment.BaseUrl+this.controler+"StudentNeedProcessing",{params:parameter});
  // }
  getStudent(ssn, specializationId,  firstName,  fatherName,  lastName,  enrollmentDate, pagingDTO){
    let SSN= new HttpParams().set('SSN',ssn);
     let SpecializationId= new HttpParams().set('specializationId',specializationId);
     let FirstName= new HttpParams().set('firstName',firstName);
     let FatherName= new HttpParams().set('fatherName',fatherName);
     let LastName= new HttpParams().set('lastName',lastName);
     let EnrollmentDate= new HttpParams().set('enrollmentDate',enrollmentDate);
     let RowCount= new HttpParams().set('RowCount',pagingDTO.RowCount);
     let Page= new HttpParams().set('Page',pagingDTO.Page);

  return  this.http.get<any>(environment.BaseUrl+"Student?"+SSN+"&"+SpecializationId+"&"+FirstName+"&"+FatherName+
 "&"+LastName+"&"+EnrollmentDate+"&"+RowCount+"&"+Page,{observe: 'response'});
   
}
StudentsNeedHelpDegree(ssn, specializationId,  firstName,  fatherName,  lastName,  enrollmentDate, pagingDTO){
  let SSN= new HttpParams().set('SSN',ssn);
   let SpecializationId= new HttpParams().set('specializationId',specializationId);
   let FirstName= new HttpParams().set('firstName',firstName);
   let FatherName= new HttpParams().set('fatherName',fatherName);
   let LastName= new HttpParams().set('lastName',lastName);
   let EnrollmentDate= new HttpParams().set('enrollmentDate',enrollmentDate);
   let RowCount= new HttpParams().set('RowCount',pagingDTO.RowCount);
   let Page= new HttpParams().set('Page',pagingDTO.Page);
   return  this.http.get<any>(environment.BaseUrl+this.controler+"StudentsNeedHelpDegree?"+SSN+"&"+SpecializationId+"&"+FirstName+"&"+FatherName+
"&"+LastName+"&"+EnrollmentDate+"&"+RowCount+"&"+Page,{observe: 'response'});
 
}

AddSbiling(ssn,AddSbiling)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.post(environment.BaseUrl+this.controler+"AddSbiling?"+SSN,AddSbiling)
}
AddClosestPerson(ssn,AddClosestPerson)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.post(environment.BaseUrl+this.controler+"AddClosestPerson?"+SSN,AddClosestPerson)
}
AddPartners(ssn,AddPartners)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.post(environment.BaseUrl+this.controler+"AddPartners?"+SSN,AddPartners)
}
AddReperations(ssn,AddReperations)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.post(environment.BaseUrl+this.controler+"AddReperations?"+SSN,AddReperations)
}
AddSanctions(ssn,AddSanctions)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.post(environment.BaseUrl+this.controler+"AddSanctions?"+SSN,AddSanctions)
}
AddPhone(ssn,AddPhone)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.post(environment.BaseUrl+this.controler+"AddPhone?"+SSN,AddPhone)
}
RemoveSibilng(ssn,RemoveSibilng)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+"RemoveSibilng?"+SSN,RemoveSibilng)
}
RemoveClosestPerson(ssn,RemoveClosestPerson)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+"RemoveClosestPerson?"+SSN,RemoveClosestPerson)
}
RemoveReperations(ssn,RemoveReperations)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+"RemoveReperations?"+SSN,RemoveReperations)
}
RemoveSanctions(ssn,RemoveSanctions)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+"RemoveSanctions?"+SSN,RemoveSanctions)
}
RemovePartners(ssn,RemovePartners)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+"RemovePartners?"+SSN,RemovePartners)
}
RemovePhone(ssn,RemovePhone)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+"RemovePhone?"+SSN,RemovePhone)
}
RemoveStudent(ssn)
{
  let SSN= new HttpParams().set('ssn',ssn);
  return this.http.delete(environment.BaseUrl+this.controler+SSN)
}

AddStudentAttachment(StudentAttachment:StudentAttachment): Observable<object> {
 
  let  formData:FormData=new FormData();
  formData.append("Attachemnt",StudentAttachment.Attachemnt,StudentAttachment.Attachemnt.name);
  formData.append("AttachmentId",StudentAttachment.AttachmentId.toString());
  formData.append("Ssn",StudentAttachment.Ssn);
  formData.append("Note",StudentAttachment.Note);
  const headers = new HttpHeaders().append('Content-Disposition', 'multipart/form-data');
 return this.http.post(environment.BaseUrl+this.controler+"UploadImage",formData,{headers: headers});
}
Register(Register:CreateRegistrationDTO,SSN) {
 return this.http.post(environment.BaseUrl+this.controler+"Register/"+SSN,Register);
}
UpdateStudentInformation(UpdateStudnetInformation){
  return this.http.patch(environment.BaseUrl+this.controler+"UpdateStudentInformation",UpdateStudnetInformation)
}
updateStudentDgree(updateStudentDgree){
  return this.http.patch(environment.BaseUrl+this.controler+"updateStudentDgree",updateStudentDgree)
}
}

