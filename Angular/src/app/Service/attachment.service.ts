import { Injectable } from '@angular/core';
import { Attachment } from '../Model/attachment.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class AttachmentService {
attachment:Attachment;
attachmentall:Attachment[]=[];
controller:string= "Attachment/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getAttachment(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
 
   postAttachment(){
  
     return this.http.post(environment.BaseUrl+this.controller,this.attachment);
   }
 
   putAttachment(){
 
     return this.http.put(environment.BaseUrl +this.controller+ this.attachment.Id ,this.attachment);
 
   }
 
   deleteAttachment(id){
     return this.http.delete(environment.BaseUrl+this.controller+id);
   }
   getEnabled(){
    return this.http.get(environment.BaseUrl+this.controller+"Enabled").toPromise().then(
      res=>{
        this.attachmentall = res as Attachment[];
      }
    ) ;
   }
}
