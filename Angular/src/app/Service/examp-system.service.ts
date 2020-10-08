import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { ExampSystem } from '../Model/examp-system.model';

@Injectable({
  providedIn: 'root'
})
export class ExampSystemService {
   //كنا مستخدمينو بالسنة
  exampsystem:ExampSystem[]=[];

examSystems:ExampSystem[]=[];
examsystem:ExampSystem;
  controller:string="ExamSystem/";
  constructor(private http:HttpClient) { }
  //كنا مستخدمينو بالسنة
  getExampSystem(){
    return this.http.get(environment.BaseUrl+this.controller).toPromise().then(res=>{
      this.exampsystem=res as ExampSystem[]
      
    });
   }
   GetExamSystem(){
    return this.http.get(environment.BaseUrl+this.controller)
   }
   AddExamSystem(){
    return this.http.post(environment.BaseUrl+this.controller,this.examsystem)
   }
   UpdateExamSystem(){
    return this.http.put(environment.BaseUrl+this.controller+this.examsystem.Id,this.examsystem)

   }
   DeleteExamSystem(id){
    return this.http.delete(environment.BaseUrl+this.controller+id)

   }
   GetEnabled(){
    return this.http.get(environment.BaseUrl+this.controller).toPromise().then(res=>{
      this.examSystems=res as ExampSystem[]
      
    });
   }
}
