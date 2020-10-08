import { Injectable } from '@angular/core';
import { Honesty } from '../Model/honesty.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class HonestyService {
  honestyall:Honesty[];
  honesty:Honesty;
  controller:string="Honesty/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getHonesty(){
    return this.http.get(environment.BaseUrl+this.controller); 
   }
 
   postHonesty(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.honesty);
   }
 
   putHonesty(){
     
     return this.http.put(environment.BaseUrl +this.controller + this.honesty.Id,this.honesty);
   }
 
   deleteHonesty(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.honestyall = res as Honesty[];
      }
    ) ;
   }
}
 