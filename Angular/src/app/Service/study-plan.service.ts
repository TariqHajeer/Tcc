import { Injectable } from '@angular/core';
import { StudyPlan, ResponseStudyPlan } from '../Model/study-plan.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class StudyPlanService {
StudyPlan:StudyPlan;
StudyPlans:ResponseStudyPlan[];
getSutdyPlan:ResponseStudyPlan=new ResponseStudyPlan;
controller:string="StudyPlan/";
  constructor(private http:HttpClient) { }

  AddStudyPlan(){
    console.log(JSON.stringify(this.StudyPlan))
    return this.http.post(environment.BaseUrl+this.controller,this.StudyPlan);
  }
  GetBySpecialization(specialization:string){
    return this.http.get(environment.BaseUrl+this.controller+'getbySpecialization?specializationId='+specialization);
  }
GetStudyPlan(specializationId:string,YearId:number){
return this.http.get(environment.BaseUrl+this.controller+"getStudyPlan?specializationId="+specializationId+"&yearId="+YearId)
}

}
