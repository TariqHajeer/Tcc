import { Injectable } from '@angular/core';
import { StudySemester } from '../Model/study-semester.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class StudySemesterService {
  //get
  StudySemesters:StudySemester[];
  controller:string="StudySemester/";
  constructor(private http:HttpClient) { }
  GetStudySemester(){
    this.http.get(environment.BaseUrl+this.controller).toPromise().then(res=>{
      this.StudySemesters=res as StudySemester[]
    });
  }
}
