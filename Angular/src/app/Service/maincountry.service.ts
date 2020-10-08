import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { Maincountry } from '../Model/maincountry.model';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class MaincountryService { 
  maincountryall:Maincountry[];
  maincountry:Maincountry;
  controller:string="Countries/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getcountry(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postcountry(){
     return this.http.post(environment.BaseUrl+this.controller,this.maincountry);
   }
 
   putcountry(){
     
     return this.http.put(environment.BaseUrl +this.controller + this.maincountry.Id,this.maincountry);
   }
 
   deletecountry(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.maincountryall = res as Maincountry[];
      }
    ) ;
   }
GetAllCountryAndCities(){
  this.http.get(environment.BaseUrl+this.controller+"GetAll").toPromise().then(
    res=>{
      
      this.maincountryall = res as Maincountry[];
    }
  ) ;
}
}
 