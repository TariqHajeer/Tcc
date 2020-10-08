import { Component, OnInit } from '@angular/core';
import { Subject } from 'src/app/Model/subject.model';
import { ResponseStudySemester } from 'src/app/Model/study-plan.model';
import { ShowStudyplanComponent } from '../show-studyplan.component';

@Component({
  selector: 'app-depend-on-subject',
  templateUrl: './depend-on-subject.component.html',
  styleUrls: ['./depend-on-subject.component.css']
})
export class DependOnSubjectComponent implements OnInit {

  constructor(public ShowStudyPlan: ShowStudyplanComponent) { }

  ngOnInit(): void {
  }
  //#region  DependOnSubject
  DependOnSubjectselect: Subject = new Subject
  DependOnSubjects: Subject[] = []


  showSubject: Subject[] = []
  DependOnSubjectSelectSemester: ResponseStudySemester = new ResponseStudySemester
  GetSubject(): Subject[] {
    var subjects = this.DependOnSubjectSelectSemester.Subjects
    if (subjects != null || subjects != undefined)
      subjects = this.ShowStudyPlan.helperSerive.DifferenceTowArrayById(subjects, this.ShowStudyPlan.GetDependOnSubject)
    return subjects
  }
  AddDependOnSubject() {
    this.ShowStudyPlan.showSpinner()
    if (this.DependOnSubjectselect == null || this.DependOnSubjectselect == undefined)  return; 
this.ShowStudyPlan.SubjectService.AddDependencySubject(this.ShowStudyPlan.getsubject.Id,this.DependOnSubjectselect.Id).subscribe(res=>{
  this.ShowStudyPlan.hideSpinner()
 this.ShowStudyPlan.helperService.add()
  this.ShowStudyPlan.GetDependOnSubject.push(this.DependOnSubjectselect);
  var filtersubject = this.ShowStudyPlan.StudySemester.map(s => s.Subjects)
  filtersubject.forEach(item => {
    item.forEach(sub => {
      if (sub.Id == this.DependOnSubjectselect.Id)
        sub.SubjectsDependOnMe.push(this.ShowStudyPlan.getsubject)

    })
  })
  this.DependOnSubjectselect = new Subject;
},err=>{
  this.ShowStudyPlan.hideSpinner()
})
    
  }
  DeleteDependOnSubject(item) {
    this.ShowStudyPlan.showSpinner()
    this.ShowStudyPlan.SubjectService.RemoveDependencySubject(this.ShowStudyPlan.getsubject.Id,this.DependOnSubjectselect.Id).subscribe(res=>{
      this.ShowStudyPlan.hideSpinner()
      this.ShowStudyPlan.helperService.delete()
      this.ShowStudyPlan.GetDependOnSubject = this.ShowStudyPlan.GetDependOnSubject.filter(c => c != item);
      this.GetSubject()
     this.ShowStudyPlan.GetStudyPlan(this.ShowStudyPlan.getSpecializtion.Id,this.ShowStudyPlan.selectedOldYear.Id)
    },err=>{
      this.ShowStudyPlan.hideSpinner()
    })
  }
  

  //#endregion

}
