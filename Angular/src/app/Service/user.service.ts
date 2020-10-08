import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { User } from '../Model/user.model';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  userall:User[];
  user:User;
  controller:string="User/";
  
  constructor(private http:HttpClient,private url:HelpService) { }
  getUser(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postUser(){
     return this.http.post(environment.BaseUrl+this.controller,this.user);
   }
 
   Addgroup(id,idgroup){
    var  formData=new FormData();
    formData.append("groupId",idgroup);
 
    return this.http.patch(environment.BaseUrl+this.controller+"AddGroup/" + id,formData);
  }
  deletegroup(id,idgroup){
    var  formData=new FormData();
    formData.append("groupId",idgroup);
    return this.http.patch(environment.BaseUrl+this.controller+"RemoveGroup/" + id,formData);
  }
 
   PutUser(){
    return this.http.patch(environment.BaseUrl +this.controller +"UpdateUser/"+ this.user.Id,this.user);
    
   }
   DeleteUser(id){
    return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"GetEnabled").toPromise().then(
      res=>{
        this.userall = res as User[];
      }
    ) ;
   }

}
