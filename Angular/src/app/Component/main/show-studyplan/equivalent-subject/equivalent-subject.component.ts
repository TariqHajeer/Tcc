import { Component, OnInit } from '@angular/core';
import { ShowStudyplanComponent } from '../show-studyplan.component';
import { Subject } from 'src/app/Model/subject.model';
import { EquivalentSubject } from '../../study-plan/study-plan.component';
import { ResponseStudySemester, ResponseStudyPlan } from 'src/app/Model/study-plan.model';
import { Year } from 'src/app/Model/year.model';
import { Specialization } from 'src/app/Model/specialization.model';

@Component({
  selector: 'app-equivalent-subject',
  templateUrl: './equivalent-subject.component.html',
  styleUrls: ['./equivalent-subject.component.css']
})
export class EquivalentSubjectComponent implements OnInit {

  constructor(public ShowStudyPlan:ShowStudyplanComponent) { }

  ngOnInit(): void {
    this.ShowStudyPlan. GetStudyPlansForSpechaliztion() 
  }
  selectedoldStudySemeser: ResponseStudySemester;
  selectedOldYear: Year=new Year;
  equalSubjectsId: number[];
  oldSelectedspechaliztion: Specialization=new Specialization;
  getStudyPlan:ResponseStudyPlan=new ResponseStudyPlan
  GetStudyPlan(SpecializationId: string, YearId: number) {
  
    this.ShowStudyPlan.StudyPlaneService.GetStudyPlan(SpecializationId, YearId).subscribe(res => {
      this.getStudyPlan = res as ResponseStudyPlan;
    }, err => {
     
     
    });

  }
  GetSemestet() {
    this.GetStudyPlan(this.oldSelectedspechaliztion.Id,this.selectedOldYear.Id)
    
    return this.getStudyPlan.StudySemester;
  }
  GetSubjects(): Subject[] {
    if (this.selectedoldStudySemeser == null || this.selectedoldStudySemeser == undefined) {
      return [];
    }
    var subjects = this.selectedoldStudySemeser.Subjects;

    if (this.ShowStudyPlan.equivalentSubjects != undefined)
      subjects = this.ShowStudyPlan.helperSerive.DifferenceTowArrayById(subjects, this.ShowStudyPlan.equivalentSubjects.map(c => c.Subject));
    return subjects;
  }
  AddEquivalentSubject() {
    this.ShowStudyPlan.showSpinner()
    this.ShowStudyPlan.SubjectService.AddEquivlantSubject(this.ShowStudyPlan.getsubject.Id,this.equalSubjectsId).subscribe(res=>{
      
      this.ShowStudyPlan.hideSpinner()
this.ShowStudyPlan.helperService.add()
    var subjects = this.GetSubjects().filter(c => this.equalSubjectsId.indexOf(c.Id) > -1);
    subjects.forEach(element => {
      var equivalentSubject = new EquivalentSubject();
      equivalentSubject.Subject = element;
      equivalentSubject.SpecialzationName = this.oldSelectedspechaliztion.Name;
      equivalentSubject.StudyPlanYear = this.selectedOldYear.FirstYear + "/" + this.selectedOldYear.SecondYear;
      this.ShowStudyPlan.equivalentSubjects.push(equivalentSubject);
    });

},err=>{
  this.ShowStudyPlan.hideSpinner()
})
  }
  DeleteEquivalSubject(equivalentSubject: EquivalentSubject) {
    this.ShowStudyPlan.showSpinner()
    this.ShowStudyPlan.SubjectService.RemoveEquivlantSubject(this.ShowStudyPlan.getsubject.Id,this.equalSubjectsId).subscribe(res=>{
      this.ShowStudyPlan.equivalentSubjects = this.ShowStudyPlan.equivalentSubjects.filter(c => c != equivalentSubject);
      this.ShowStudyPlan.hideSpinner()
      this.ShowStudyPlan.helperService.delete()
    },err=>{
      this.ShowStudyPlan.hideSpinner()
    })
  }
 
}
