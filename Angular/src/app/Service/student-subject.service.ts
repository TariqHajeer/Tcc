import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { StudentSubjectDTO } from '../Model/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentSubjectService {

  controller:string="StudentSubject/";
  constructor(private http:HttpClient) { }
  getStudentSubject(subjectid,yearid,examsemesterid){
  return  this.http.get(environment.BaseUrl+this.controller+"GetStudentPracticalDegree/"+subjectid+"/"+yearid+"/"+examsemesterid);
  }
  SetStudentsDegreeBySubject(StudentSubject:StudentSubjectDTO[]){
   // let Ssn= new HttpParams().set('ssn',ssStudentSubjectn);

  return  this.http.post(environment.BaseUrl+this.controller+"SetDegrees",StudentSubject);
  }
  CanReset(ssn){
    let Ssn= new HttpParams().set('ssn',ssn);
return this.http.get(environment.BaseUrl+this.controller+"CanReset?"+Ssn)
  }
  Reset(Subject){
return this.http.post(environment.BaseUrl+this.controller+"Reset",Subject)
  }
  SubjectNeedHelpDegree(ssn){
    return this.http.get(environment.BaseUrl+this.controller+"SubjectNeedHelpDegree/"+ssn)

  }
  SetHelpDgree(ssn,subjects){
    return this.http.post(environment.BaseUrl+this.controller+"SetHelpDgree/"+ssn,subjects)
  }
  SetDegreeForTransformStudent(studentSubjectDTO:StudentSubjectDTO[]){
    return this.http.post(environment.BaseUrl+this.controller+"SetDegreeForTransformStudent",studentSubjectDTO);
  }
  GetSubjectsBySSN(ssn){
    let Ssn= new HttpParams().set('ssn',ssn);
    return this.http.get(environment.BaseUrl+this.controller+"GetSubjectsBySSN?"+Ssn)
  }
  UpdateStudentSubject(StudentSubject){
    return this.http.patch(environment.BaseUrl+this.controller,StudentSubject);
  }
}
