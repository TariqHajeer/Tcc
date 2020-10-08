import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Subject } from '../Model/subject.model';
import { environment } from 'src/environments/environment.prod';
import { StudyPlanSubject } from '../Model/study-plan-subject.model';
import { Identifiers } from '@angular/compiler';
import { AppComponent } from '../app.component';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  Subject: Subject;
  Subjects: Subject[];
  StudyPlaneSubject: StudyPlanSubject;
  controller: string = "Subject/";
  constructor(private http: HttpClient,public Appcomponent:AppComponent) { }
  GetSubject() {
    this.http.get(environment.BaseUrl + this.controller);
  }
  GetSubjectByCode(code){
   this.Appcomponent.DisabledToster.push(true);
    let Code= new HttpParams().set('code',code);
   return this.http.get(environment.BaseUrl+this.controller+"bycode?"+Code)
  }
  
  AddSubject(Subject) {
    return this.http.post(environment.BaseUrl + this.controller, Subject);
  }
  Remove(id) {
    let Id = new HttpParams();
    Id.append("id", id)
   return this.http.delete(environment.BaseUrl + this.controller , {params:Id})
  }
  UpdateSubject(subject){
    return this.http.patch(environment.BaseUrl+this.controller,subject)
  }
  AddDependencySubject(SubjectId, DependencySubjectId) {
    let id=new FormData(); 
    id.append("dependencyId",DependencySubjectId)
  return  this.http.patch(environment.BaseUrl + this.controller + "AddDependencySubject/" + SubjectId, id);
  }
  RemoveDependencySubject(SubjectId, DependencySubjectId) {
    let id=new FormData(); 
    id.append("DependencyId",DependencySubjectId)
   return this.http.patch(environment.BaseUrl + this.controller + "RemoveDependencySubject/" + SubjectId, id);
  }
  AddEquivlantSubject(SubjectId, EquivlantSubjectId) {
    console.log(SubjectId, EquivlantSubjectId)
    let id=new FormData(); 
    id.append("FirstSubject",SubjectId)
    id.append("SecondSubject",EquivlantSubjectId)
   return this.http.patch(environment.BaseUrl + this.controller + "AddEquivlantSubject" , id);
  }
  RemoveEquivlantSubject(SubjectId, EquivlantSubjectId) {
    let id=new FormData();
    id.append("FirstSubject",SubjectId)
    id.append("SecondSubject",EquivlantSubjectId)
   return this.http.patch(environment.BaseUrl + this.controller + "RemoveEquivlantSubject" , id);
  }
}
