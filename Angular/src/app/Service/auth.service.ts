import { Injectable } from '@angular/core';
import { Auth } from 'src/app/Model/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public auth: Auth=new Auth;
  constructor() {
    this.auth = JSON.parse(localStorage.getItem('login')) as Auth;
    
  }

}
