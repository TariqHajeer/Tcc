import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/Service/login.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Auth } from 'src/app/Model/auth.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppComponent } from 'src/app/app.component';
import { HelpService } from 'src/app/Help/help.service';



@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']

})
export class LoginComponent extends AppComponent implements OnInit {
  formModel = {
    UserName: '',
    Password: ''
  }
  constructor(public service: LoginService, private router: Router,public spinner: NgxSpinnerService,public HelperService:HelpService) {  super( spinner,HelperService);}

  ngOnInit(): void {
  }
  /// هاد منشان شو ؟
  invalidLogin: boolean;


  onSubmit(form: NgForm) {
     this.showSpinner();
    this.service.login(form.value).subscribe(
      res => {
        let user = (<any>res).Token;
        console.log(res)
        localStorage.setItem('login', JSON.stringify(res));
        localStorage.setItem('user', user);
        this.router.navigateByUrl('/home/welcome');
        this.hideSpinner();
        this.invalidLogin = false;
      },
      err => {
        console.log(err);
        this.hideSpinner();
        if (err.status == 400) {
       
          this.HelperService.toastr.error('اسم المستخدم او كلمة المرور غير صحيحة', 'لم يتم تسجيل الدخول');
          console.log(err);
          this.invalidLogin = true;
        }

        else {
       
          this.hideSpinner();
          console.log(err);
          this.invalidLogin = true;
        }
      }
    );
  }

}
