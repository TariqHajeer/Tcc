import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class StudentAttachmentService {
controller:string="Default";
  constructor(private http:HttpClient) { }
  AddStudentAttachment(StudentAttachment){
   let dataStudentAttachment=new FormData();
   dataStudentAttachment.append("StudentAttachment",StudentAttachment);
return this.http.post(environment.BaseUrl+this.controller,dataStudentAttachment);
  }
}
