import { Injectable } from '@angular/core';
import { Languages } from '../Model/languages.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class LanguagesService {
language:Languages;
languageall:Languages[];
controller:string= "Languages/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getLanguages(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postLanguages(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.language);
   }
 
   putLanguages(){
 
     return this.http.put(environment.BaseUrl +this.controller+ this.language.Id ,this.language);
 
   }
 
   deleteLanguages(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.languageall = res as Languages[];
      }
    ) ;
   }
}
