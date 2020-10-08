import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppComponent } from 'src/app/app.component';
import { HelpService } from 'src/app/Help/help.service';
import { RoleService } from 'src/app/Help/role.service';
import { Year } from 'src/app/Model/year.model';
import { YearService } from 'src/app/Service/year.service';

@Component({
  selector: 'app-blocked-year',
  templateUrl: './blocked-year.component.html',
  styleUrls: ['./blocked-year.component.css']
})
export class BlockedYearComponent extends AppComponent implements OnInit {

  constructor(public YearService:YearService,
    public getroute:ActivatedRoute
    ,public route:Router
    ,public spinner: NgxSpinnerService
    ,public helperService:HelpService) {  super( spinner,helperService); }
  YearId
  Year:Year
  StudentDontRegister={
    count:[]
  }
  CountStudentDontRegister
  StudentDontTakeExam={
    count:[]
  }
  CountStudentDontTakeExam
  ngOnInit(): void {
    this.YearService.getYearAll();
    this.getroute.params.subscribe(parm=>{
      this.YearId=parm['Id']
      this.Year=this.YearService.years.find(y=>y.Id==this.YearId)
      this.YearService.StudentDontRegister(this.YearId).subscribe(res=>{
        if(res.count==0)
          this.CountStudentDontRegister=0
        else
      {  this.StudentDontRegister=res as any
        this.CountStudentDontRegister=this.StudentDontRegister.count.length
        console.log(this.StudentDontRegister)
        console.log(this.CountStudentDontRegister)
      }
      })
      this.YearService.StudentDontTakeExam(this.YearId).subscribe(res=>{
        if(res.count==0)
        this.CountStudentDontTakeExam=0
      else
      {this.StudentDontTakeExam=res as any
         console.log(this.StudentDontTakeExam)
      this.CountStudentDontTakeExam= this.StudentDontTakeExam.count.length
      console.log(this.CountStudentDontTakeExam)
    }
      })
    })
  }
BlockYear(Id){
  this.showSpinner()
  this.YearService.BlockYear(Id).subscribe(res=>{
    this.hideSpinner()
    this.route.navigate(['/home/system/year'])  
  }
  ,err=>{
    this.hideSpinner()
  })
}
}
