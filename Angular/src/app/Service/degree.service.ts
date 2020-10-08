import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { Degree } from '../Model/degree.model';
import { FormBuilder, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class DegreeService {
degreeall:Degree[]=[];
degree:Degree;
controller:string= "Degree/";
  constructor(private http:HttpClient,private url:HelpService) { }
  
  
 
   
  getDegree(){
   return this.http.get(environment.BaseUrl+this.controller);
  }

  postDegree(){
 
    return this.http.post(environment.BaseUrl+this.controller,this.degree);
  }

  putDegree(){

    return this.http.put(environment.BaseUrl +this.controller+ this.degree.Id ,this.degree);

  }

  deleteDegree(id){
    return this.http.delete(environment.BaseUrl+this.controller+id);
  }
  getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.degreeall = res as Degree[];
      }
    ) ;
   }
 
}
 