import { Injectable } from '@angular/core';
import { environment } from '../../../src/environments/environment.prod';
import { Year } from '../Model/year.model';
import { HttpClient } from '@angular/common/http';
import { YearRespons } from '../Model/year-respons.model';
import { Observable } from 'rxjs';
import { idLocale } from 'ngx-bootstrap';

@Injectable({
  providedIn: 'root'
})
export class YearService {
  yearall: Year[] = [];
  years: Year[] = [];
  yearResponce: YearRespons[];
  year: Year;
  controller: string = "Year/";
  constructor(private http: HttpClient) { }
  getYear() {
    return this.http.get(environment.BaseUrl + this.controller);
  }
  getYearAll() {
    return this.http.get(environment.BaseUrl + this.controller).toPromise().then(res => {
      this.years = res as Year[]
      this.years.sort((n1, n2) => {
        if (n1.FirstYear > n2.FirstYear)
          return -1
        if (n1.FirstYear < n2.FirstYear)
          return 1
        return 0

      });
    });
  }
  // GetNonBlockedYear() {
  //   return this.http.get(environment.BaseUrl + this.controller + "GetYearCanAddStudyPaln").toPromise().then(res => {
  //     this.yearall = res as Year[]
  //     this.yearall.sort((n1, n2) => {
  //       if (n1.FirstYear > n2.FirstYear)
  //         return -1
  //       if (n1.FirstYear < n2.FirstYear)
  //         return 1
  //       return 0

  //     });
  //   });
  // }
  GetNonBlockedYear() {
  
      return this.http.get(environment.BaseUrl + this.controller + "GetNonBlockedYear").toPromise().then(res => {
        this.yearall = res as Year[]
    console.log(res)
      },err=>{
        console.log(err)
      });
   
  }
  GetYearBySpecialization(specializationId) {
    return this.http.get(environment.BaseUrl + this.controller + "GetYearBySpecialization?SpecializationId=" + specializationId)
  }
  postYear() {

    return this.http.post(environment.BaseUrl + this.controller, this.year);
  }
  GetYearWithoutStudyPlan(specializationId) {
    return this.http.get(environment.BaseUrl + this.controller + "yearWithoutStudyPlan?specializationId=" + specializationId).toPromise().then(
      res => {
        this.yearResponce = res as YearRespons[];
      }
    );

  }
  StudentDontRegister(yearId):Observable<any> {
   return this.http.get(environment.BaseUrl + this.controller +"StudentDontRegister/"+ yearId)
  }
  StudentDontTakeExam(yearId):Observable<any> {
   return this.http.get(environment.BaseUrl + this.controller+"StudentDontTakeExam/"+ yearId)
  }
  BlockYear(yearId) {
  return  this.http.put(environment.BaseUrl + this.controller+yearId, yearId)
  }
  UpdateYear(yearid,updateyear){
   return this.http.patch(environment.BaseUrl+this.controller+yearid,updateyear)
  }
  DeleteYear(id){
    return this.http.delete(environment.BaseUrl+this.controller+id)
  }
}
