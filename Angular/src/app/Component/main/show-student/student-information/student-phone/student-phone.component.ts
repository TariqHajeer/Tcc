import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { StudentPhone } from 'src/app/Model/student-phone.model';

@Component({
  selector: 'app-student-phone',
  templateUrl: './student-phone.component.html',
  styleUrls: ['./student-phone.component.css']
})
export class StudentPhoneComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }
  AddStudentPhone: StudentPhone;
  ngOnInit(): void {
    this.AddStudentPhone = new StudentPhone;
    this.StudentInformation.CheckArrayIsNull(this.StudentInformation.GetStudentPhone, "ارقام للطالب")

  }
  AddStudentPhones() {
    this.StudentInformation.showSpinner();
    this.AddStudentPhone.PhoneTypeId = this.AddStudentPhone.PhoneType.Id;
    this.AddStudentPhone.Ssn = this.StudentInformation.SSN
    this.StudentInformation.studentService.AddPhone(this.StudentInformation.SSN, this.AddStudentPhone).subscribe(
      res => {
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetStudentPhone.push(this.AddStudentPhone as StudentPhone);
        this.AddStudentPhone = new StudentPhone;
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
  DeleteStudentPhones(item) {
    this.StudentInformation.showSpinner();
    this.StudentInformation.studentService.RemovePhone(this.StudentInformation.SSN, item).subscribe(
      res => {
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetStudentPhone = this.StudentInformation.GetStudentPhone.filter(c => c != item);
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
}
