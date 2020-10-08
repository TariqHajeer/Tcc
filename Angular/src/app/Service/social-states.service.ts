import { Injectable } from '@angular/core';
import { SocialStates } from '../Model/social-states.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class SocialStatesService {
socialstate:SocialStates;
socialstateall:SocialStates[];
controller:string= "SocialStates/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getSocialStates(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postSocialStates(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.socialstate);
   }
 
   putSocialStates(){
     return this.http.put(environment.BaseUrl +this.controller+ this.socialstate.Id ,this.socialstate);
 
   }
 
   deleteSocialStates(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.socialstateall = res as SocialStates[];
      }
    ) ;
   }
}
