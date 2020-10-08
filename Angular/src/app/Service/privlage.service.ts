import { Injectable } from '@angular/core';
import { Privlage } from '../Model/privlage.model';
import { HttpClient } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class PrivlageService {
  Privilage:Privlage[];
  controller:string= "Privilage/";
  constructor(private http:HttpClient,private url:HelpService) { }
  getPrivlage(){
    return this.http.get(environment.BaseUrl+this.controller);
   }
}
 