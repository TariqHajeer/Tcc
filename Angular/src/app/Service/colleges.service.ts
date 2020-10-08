import { Injectable } from '@angular/core';
import { Colleges } from '../Model/colleges.model';
import { HelpService } from '../Help/help.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class CollegesService {
collage:Colleges;
collageall:Colleges[]=[];
controller:string= "Colleges/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getColleges(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postColleges(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.collage);
   }
 
   putColleges(){
     return this.http.put(environment.BaseUrl +this.controller+ this.collage.Id ,this.collage);
 
   }
 
   deleteColleges(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"Enabled").toPromise().then(
      res=>{
        this.collageall = res as Colleges[];
      }
    ) ;
   }
}
