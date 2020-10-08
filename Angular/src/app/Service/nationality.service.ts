import { Injectable } from '@angular/core';
import { Nationality } from '../Model/nationality.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class NationalityService {
nationalty:Nationality;
nationaltyall:Nationality[];
controller:string= "Nationality/";
  constructor(private http:HttpClient,private url:HelpService) { }
     
  getNationality(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postNationality(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.nationalty);
   }
 
   putNationality(){
 
     return this.http.put(environment.BaseUrl +this.controller+ this.nationalty.Id ,this.nationalty);
 
   }
 
   deleteNationality(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller).toPromise().then(
      res=>{
        this.nationaltyall = res as Nationality[];
      }
    ) ;
   }
}
