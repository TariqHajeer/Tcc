import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { CountriesService } from 'src/app/Service/countries.service';
import { NationalityService } from 'src/app/Service/nationality.service';
import { ConstraintsService } from 'src/app/Service/constraints.service';
import { UpdateStudnetInformation } from 'src/app/Model/update-studnet-information.model';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css']
})
export class InformationComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent,
    public cityService: CountriesService,
    public NationaltyService: NationalityService,
    public ConstraintService: ConstraintsService) { }
    tempStudentInformation
    tempMoreInformation
    showEdit:boolean=false
  ngOnInit(): void {
  }
  Information:UpdateStudnetInformation=new UpdateStudnetInformation()
  ClickEdit(){
    this.showEdit=true
    this.tempStudentInformation=Object.assign({}, this.StudentInformation.Student)
    this.tempMoreInformation=Object.assign({}, this.StudentInformation.MoreInformation)
  }
  CansalUpdate(){
    this.StudentInformation.Student= Object.assign(this.StudentInformation.Student, this.tempStudentInformation)
    this.StudentInformation.MoreInformation= Object.assign(this.StudentInformation.MoreInformation, this.tempMoreInformation)
    this.showEdit=false
  }
UpdateInformation(){
  this.Information.Ssn=this.StudentInformation.SSN
  this.Information=this.StudentInformation.Student
  this.Information.MoreInformation=this.StudentInformation.MoreInformation
  this.StudentInformation.showSpinner()
this.StudentInformation.studentService.UpdateStudentInformation(this.Information).subscribe(res=>{
  console.log(res)
  this.StudentInformation.hideSpinner()
},err=>{
  this.StudentInformation.hideSpinner()
})
}
}
