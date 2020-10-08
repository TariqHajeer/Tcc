import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { Specialization } from '../Model/specialization.model';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class SpecializationService {
  specialall:Specialization[];
  special:Specialization;
  controller:string= "Specialization/";
  constructor(private http:HttpClient) { }
  getSpecialization(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postSpecialization(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.special);
   }
 
   putSpecialization(){
     
     return this.http.put(environment.BaseUrl +this.controller + this.special.Id,this.special);
   }
 
   deleteSpecialization(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.specialall = res as Specialization[];
      }
    ) ;
   }
}
 