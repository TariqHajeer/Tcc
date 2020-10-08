import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { Siblings } from 'src/app/Model/siblings.model';

@Component({
  selector: 'app-brothers',
  templateUrl: './brothers.component.html',
  styleUrls: ['./brothers.component.css']
})
export class BrothersComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }
  AddSiblings: Siblings;
  ngOnInit(): void {
    this.AddSiblings = new Siblings;
    this.StudentInformation.CheckArrayIsNull(this.StudentInformation.GetSiblings, "أشقاء")
  }
  AddSibling() {
    this.AddSiblings.SocialState = this.AddSiblings.getSocialState.Id;
    this.AddSiblings.Ssn = this.StudentInformation.SSN;
    this.StudentInformation.showSpinner();
    this.StudentInformation.studentService.AddSbiling(this.StudentInformation.SSN, this.AddSiblings).subscribe(
      res => {
        
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetSiblings.push(this.AddSiblings as Siblings);
        this.AddSiblings = new Siblings;
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
  DeleteSibling(item) {
    this.StudentInformation.showSpinner();
    this.StudentInformation.studentService.RemoveSibilng(this.StudentInformation.SSN, item).subscribe(
      res => {
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetSiblings = this.StudentInformation.GetSiblings.filter(c => c != item);
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
    checkWriteSiblingsName(): boolean {
    if (this.AddSiblings.Name == null || this.AddSiblings.Name == "")
      return true;
    else
      return false;
  }
  checkAddSiblings(): boolean {

    if (this.AddSiblings.Name == null || this.AddSiblings.Name == "" ||
      this.AddSiblings.Address == "" ||
      this.AddSiblings.getSocialState == null || this.AddSiblings.Work == "") {
      this.StudentInformation.errormessage = "ادخل جميع القيم"
      return true;
    }
    else return false;
  }
}
