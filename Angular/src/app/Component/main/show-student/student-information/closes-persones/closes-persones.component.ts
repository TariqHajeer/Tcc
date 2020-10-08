import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { PersonPhone } from 'src/app/Model/person-phone.model';
import { ClosesetPersons } from 'src/app/Model/closeset-persons.model';

@Component({
  selector: 'app-closes-persones',
  templateUrl: './closes-persones.component.html',
  styleUrls: ['./closes-persones.component.css']
})
export class ClosesPersonesComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }
  AddPersonPhone: PersonPhone;
  GetPersonPhone: PersonPhone[] = [];
  AddClosesetPersons: ClosesetPersons;
  ngOnInit(): void {
    this.AddClosesetPersons = new ClosesetPersons;
    this.AddPersonPhone = new PersonPhone;
    this.StudentInformation.CheckArrayIsNull(this.StudentInformation.GetClosesetPersons, "أشخاص مقربون")

  }
  AddClosesetPerson() {
    this.StudentInformation.showSpinner(); 
    this.AddClosesetPersons.PersonPhone = this.GetPersonPhone;
    this.AddClosesetPersons.Ssn = this.StudentInformation.SSN
   
    this.StudentInformation.studentService.AddClosestPerson(this.StudentInformation.SSN, this.AddClosesetPersons).subscribe(
      res => {
        
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetClosesetPersons.push(this.AddClosesetPersons as ClosesetPersons);
        this.AddClosesetPersons = new ClosesetPersons;
        this.GetPersonPhone = [];
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
  DeleteClosesetPerson(item) {
    this.StudentInformation.showSpinner(); 
    this.StudentInformation.studentService.RemoveClosestPerson(this.StudentInformation.SSN, item).subscribe(
      res => {
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetClosesetPersons = this.StudentInformation.GetClosesetPersons.filter(c => c != item);
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
  AddPersonPhones() {

    this.StudentInformation.hideSpinner();
    this.AddPersonPhone.PhoneTypeId = this.AddPersonPhone.PhoneType.Id;
    this.GetPersonPhone.push(this.AddPersonPhone as PersonPhone);
    this.AddPersonPhone = new PersonPhone;


  }
  DeletePersonPhones(item) {
    this.StudentInformation.showSpinner(); 

    this.GetPersonPhone = this.GetPersonPhone.filter(c => c != item);
  }

}
