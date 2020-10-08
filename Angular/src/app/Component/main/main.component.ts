import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/Service/auth.service';
import { Router } from '@angular/router';
import { RoleService } from 'src/app/Help/role.service';
import { HelpService } from 'src/app/Help/help.service';
import { StudentService } from 'src/app/Service/student.service';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { InputRangeDirective } from 'src/app/Custom/Directives/input-range.directive';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent extends AppComponent implements OnInit {

  constructor(public roleservic:RoleService,public router:Router 
    ,public authService:AuthService,
    public helpService:HelpService,
    public StudentService:StudentService,
    public spinner: NgxSpinnerService,) {
      super(spinner,helpService)
  }
  
  ngOnInit(): void {
  this.StudentService.StudentNeedProcessingCount();
  this.StudentService.StudentsNeedHelpDegreeCounts();

  }
 
  logout()
  {
   
    this.router.navigateByUrl('/');
    localStorage.removeItem('login');
   // localStorage.clear();
  }
}
