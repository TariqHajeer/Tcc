import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { YearService } from 'src/app/Service/year.service';
import { Year } from 'src/app/Model/year.model';
import { Subject } from 'src/app/Model/subject.model';
import { ResponseStudySemester, ResponseStudyPlan } from 'src/app/Model/study-plan.model';
import { StudyPlanService } from 'src/app/Service/study-plan.service';
import { HelpService } from 'src/app/Help/help.service';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { StudentSubjectDTO } from 'src/app/Model/student.model';
import { ExamSemesterService } from 'src/app/Service/exam-semester.service';
import { StudySemester } from 'src/app/Model/study-semester.model';


@Component({
  selector: 'app-add-subject-marks',
  templateUrl: './add-subject-marks.component.html',
  styleUrls: ['./add-subject-marks.component.css']
})
export class AddSubjectMarksComponent extends AppComponent implements OnInit {

  constructor( private router: Router
    ,public spinner: NgxSpinnerService
    ,    public YearService: YearService
    , public StudyPlaneService: StudyPlanService,
    public helperSerive: HelpService,
    public SpecialzationService: SpecializationService,
    public examsemesterservice:ExamSemesterService
    ) { super( spinner,helperSerive); }
  getSpecializtionId:string;
  getYear:Year[]=[];
  selectedoldStudySemeser: ResponseStudySemester=new ResponseStudySemester;
  subjects: Subject[] = [];
  subjectsSecondSemster: Subject[] = [];
  subjectinfo:StudentSubjectDTO;
  selectyear
  ngOnInit(): void {
   this.SpecialzationService.getEnabled();
   this.subjectinfo=new StudentSubjectDTO
   this.YearService.GetNonBlockedYear();
   
  }


  addparticalOrTheorticaldegree(){
   this.subjectinfo.ExamSemesterId=this.Studysemesters.Id
    localStorage.setItem("subjectinfo",JSON.stringify(this.subjectinfo)); 
    var sub=JSON.parse(localStorage.getItem('subjectinfo')) as StudentSubjectDTO;
     this.router.navigate(['/home/addPracticalorTheoreticalDegree/', sub.Subject.Id,sub.year.Id,sub.ExamSemesterId]);
   
  }
  

 

  // GetSubjects(): Subject[] {
  //   if (this.selectedoldStudySemeser == null || this.selectedoldStudySemeser == undefined) {
  //     return [];
  //   }
  //   var subjects = this.selectedoldStudySemeser.Subjects;

  //   if (this.subjects != undefined)
  //     subjects = this.helperSerive.DifferenceTowArrayById(subjects, this.subjects);
  //   return subjects;
  // }
  Studysemesters:ResponseStudySemester=new ResponseStudySemester 
  MainSemesterNumber
getStudySemester(){
  
  this.subjects=[]
this.examsemesterservice.GetAvilableExamSemester(this.getSpecializtionId,this.subjectinfo.year.Id,this.selectyear).subscribe(
  res=>{
    this.Studysemesters=res as ResponseStudySemester
   if(this.Studysemesters.Subjects!=undefined)
    this.subjects=this.Studysemesters.Subjects.filter(c=>c.MainSemesterNumber==this.MainSemesterNumber)
      
  },
  err => {
    this.hideSpinner();
  }
)
}
print(){
  window.print() 
}
//validation
ChangeYear(){
  this.selectyear=null
  this.Studysemesters=new ResponseStudySemester
  this.subjects=[]
  this.MainSemesterNumber=0
}
ChangeStudyYear(){
  this.Studysemesters=new ResponseStudySemester
  this.subjects=[]
  this.MainSemesterNumber=0
  this.examsemesterservice.GetAvilableExamSemester(this.getSpecializtionId,this.subjectinfo.year.Id,this.selectyear).subscribe(
    res=>{
      this.Studysemesters=res as ResponseStudySemester
    })
}
ChangeSpecialization(){
this.subjectinfo.year=new Year
this.ChangeYear()
}

}
