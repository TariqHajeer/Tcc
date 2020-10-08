import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constraints } from '../Model/constraints.model';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ConstraintsService {
constraintsall:Constraints[];
constraint:Constraints;
controller:string="Constraints/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getConstraints(){
    return  this.http.get(environment.BaseUrl+this.controller);
  }
 
  postConstraints(){
    return this.http.post(environment.BaseUrl+this.controller,this.constraint);
  }

  putConstraints(){
    return this.http.put(environment.BaseUrl +this.controller + this.constraint.Id,this.constraint);
  }

  deleteConstraints(id){
    return this.http.delete(environment.BaseUrl+this.controller+id);
  }
  getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.constraintsall = res as Constraints[];
      }
    ) ;
   }
}
