import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { PhoneType } from '../Model/phone-type.model';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class PhoneTypeService {
  phonetypeall:PhoneType[]=[];
  phonetype:PhoneType;
  controller:string= "PhoneType/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getPhoneType(){
    return this.http.get(environment.BaseUrl+this.controller);  
   }
 
   postPhoneType(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.phonetype);
   }
 
   putPhoneType(){
     
     return this.http.put(environment.BaseUrl +this.controller + this.phonetype.Id,this.phonetype);
   }
 
   deletePhoneType(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.phonetypeall = res as PhoneType[];
      }
    ) ;
   }
}
 