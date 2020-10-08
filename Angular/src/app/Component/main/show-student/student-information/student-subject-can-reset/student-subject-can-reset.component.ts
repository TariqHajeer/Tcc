import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { StudentSubjectDTO } from 'src/app/Model/student.model';

@Component({
  selector: 'app-student-subject-can-reset',
  templateUrl: './student-subject-can-reset.component.html',
  styleUrls: ['./student-subject-can-reset.component.css']
})
export class StudentSubjectCanResetComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }

  ngOnInit(): void {
    this.GetSubjectCanReset()
  }
  getSubjectCanReset: StudentSubjectDTO[] = []
  //#region can reset subject
  GetSubjectCanReset() {
    
    this.StudentInformation.studentSubjectService.CanReset(this.StudentInformation.SSN).subscribe(res => {
      this.getSubjectCanReset = res as StudentSubjectDTO[]
      this.StudentInformation.CheckArrayIsNull(this.getSubjectCanReset,"مواد")
      
    })
  }
  Reset:Reset=new Reset()
  resetSubject(item: StudentSubjectDTO) {
    this.Reset.Id=item.Id
    this.Reset.Note=item.Note
    this.StudentInformation.showSpinner()
    this.StudentInformation.studentSubjectService.Reset(this.Reset).subscribe(res => {   
      this.getSubjectCanReset=  this.getSubjectCanReset.filter(s=>s!=item)
      this.StudentInformation.hideSpinner()
      this.StudentInformation.ReloadPage();
    },err=>{
      this.StudentInformation.hideSpinner()
    })
  }
  //#endregion


}
export class Reset{
  Id
  Note
}