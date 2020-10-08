import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { SubjectType } from '../Model/subject-type.model';

@Injectable({
  providedIn: 'root'
})
export class SubjectTypeService {
  SubjectTypes:SubjectType[];
  SubjectType:SubjectType;
controller:string="SubjectType/";
  constructor(private http:HttpClient) { }
GetSubjectType(){
 return this.http.get(environment.BaseUrl+this.controller);
}
postSubjectType(){
  
  return this.http.post(environment.BaseUrl+this.controller,this.SubjectType);
}

putSubjectType(){
  
  return this.http.put(environment.BaseUrl +this.controller + this.SubjectType.Id,this.SubjectType);
}

deleteSubjectType(id){
  return this.http.delete(environment.BaseUrl+this.controller+id);
}
getEnabled(){
/* return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
   res=>{
    this.SubjectTypes=res as SubjectType[]
  });*/
  return this.http.get(environment.BaseUrl+this.controller).toPromise().then(
    res=>{
     this.SubjectTypes=res as SubjectType[]
   });
}
}
