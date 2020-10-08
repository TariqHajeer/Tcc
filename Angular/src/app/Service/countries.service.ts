import { Injectable } from '@angular/core';
import { Countries } from '../Model/countries.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';
@Injectable({
  providedIn: 'root'
})
export class CountriesService {
countryall:Countries[];
country:Countries;
controller:string="Cites/"; 
  constructor(private http:HttpClient,private url:HelpService) { }
  getcountry(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postcountry(){
  
      return this.http.post(environment.BaseUrl+this.controller,this.country);
   }
 
   putcountry(){
   
     return this.http.put(environment.BaseUrl +this.controller + this.country.Id,this.country);
   }
 
   deletecountry(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"Enabled").toPromise().then(
      res=>{
        this.countryall = res as Countries[];
      }
    ) ;
   }

}
 