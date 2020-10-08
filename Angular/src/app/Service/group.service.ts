import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { Group } from '../Model/group.model';
import { PrivlageService } from './privlage.service';
import { environment } from 'src/environments/environment.prod';


@Injectable({
  providedIn: 'root'
})
export class GroupService {
groupall:Group[];
group:Group;
controller:string="Group/";
  constructor(private http:HttpClient,private url:HelpService,public prservic:PrivlageService) { }
  getGroup(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postGroup(){  
  
     return this.http.post(environment.BaseUrl+this.controller,this.group);
   }
 
   updateGroup(id){
 
     var  formData=new FormData();
     formData.append("Name",this.group.Name);
    return this.http.patch(environment.BaseUrl +this.controller + id,formData);
  }
   AddPriveleges(id,idpr){


     var  formData=new FormData();
     formData.append("privelegeId",idpr);
     return this.http.patch(environment.BaseUrl +this.controller+"AddPriveleges/"+id,formData);
  }
  deletePriveleges(id,idpr){

    var  formData=new FormData();
    formData.append("privelegeId",idpr);
    return this.http.patch(environment.BaseUrl +this.controller+"RemovePrivilage/" + id,formData);
    
  }
 
   deleteGroup(id){

 var  formData=new FormData();
     formData.append("id",id);
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
 
}
 