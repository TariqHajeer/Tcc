import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HelpService } from '../Help/help.service';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http:HttpClient,private url:HelpService) { }
  login(formData) {
    return this.http.post(environment.BaseUrl+'auth', formData ,{ headers: new HttpHeaders({
      "Content-Type": "application/json"
    })});
  }
}
