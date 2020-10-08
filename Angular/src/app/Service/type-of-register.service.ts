import { Injectable } from '@angular/core';
import { TypeOfRegister } from '../Model/type-of-register.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class TypeOfRegisterService {
typeofregister:TypeOfRegister;
typeofregisterall:TypeOfRegister[];
controller:string= "TypeOfRegister/";
  constructor(private http:HttpClient,private url:HelpService) { }
    
  getTypeOfRegister(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postTypeOfRegister(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.typeofregister);
   }
 
   putTypeOfRegister(){
 
     return this.http.put(environment.BaseUrl +this.controller+ this.typeofregister.Id ,this.typeofregister);
 
   }
 
   deleteTypeOfRegister(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.typeofregisterall = res as TypeOfRegister[];
      }
    ) ;
   }
}
