import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { StudentDegree } from 'src/app/Model/student-degree.model';

@Component({
  selector: 'app-student-degree',
  templateUrl: './student-degree.component.html',
  styleUrls: ['./student-degree.component.css']
})
export class StudentDegreeComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }
  tempStudentDegree:StudentDegree
  showEdit:boolean=false
  ngOnInit(): void {
  
  }
  ClickEdit(item:StudentDegree){
    this.showEdit=true
    this.tempStudentDegree=Object.assign({}, item)
  }
  EditStudentDegree(item:StudentDegree){
    this.StudentInformation.showSpinner()
    this.StudentInformation.studentService.updateStudentDgree(item).subscribe(res=>{
      console.log(res)
      this.StudentInformation.hideSpinner()

    },err=>{
      this.StudentInformation.hideSpinner()
    })
    this.showEdit=false
  }
  CancelEditStudentDegree(item:StudentDegree){
    item= Object.assign(item, this.tempStudentDegree)
    this.showEdit=false
  }
}
