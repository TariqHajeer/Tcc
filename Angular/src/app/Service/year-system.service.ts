import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { YearSystem } from '../Model/year-system.model';
import { Setting } from '../Model/setting.model';
import { SettingService } from './setting.service';
import { Settings } from 'http2';

@Injectable({
  providedIn: 'root'
})
export class YearSystemService {
  yearSystems: YearSystem[];
  yearsystem: YearSystem;

  controller: string = "YearSystem/";
  constructor(private http: HttpClient, private settingService: SettingService) {
    this.yearsystem = new YearSystem();
    this.FillSettinInYearSystem();
  }
  FillSettinInYearSystem(){
    this.settingService.Get().subscribe(res=>{
      this.yearsystem.Settings = res as Setting[];
      this.yearsystem.Settings.forEach(c=>{
        if(c.Id==1){
          c.Count=-1;
        }
        else {
          c.Count=0;
        }
    
      });
    });
  }
  getYearSystem() {
    return this.http.get(environment.BaseUrl + this.controller);
  }
  getYearSystemAll() {
    return this.http.get(environment.BaseUrl + this.controller).toPromise().then(res => {
      this.yearSystems = res as YearSystem[]
    });
  }
  postYearSystem(){
  return  this.http.post(environment.BaseUrl+this.controller,this.yearsystem);

  }
  UpdateYearSystem(id:number){
    return  this.http.put(environment.BaseUrl+this.controller+id,this.yearsystem);
  }
  DeleteYearSystem(id){
    return  this.http.delete(environment.BaseUrl+this.controller+id);
  }
}
