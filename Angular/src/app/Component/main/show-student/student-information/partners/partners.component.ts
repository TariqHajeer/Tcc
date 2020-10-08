import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';
import { Partners } from 'src/app/Model/partners.model';

@Component({
  selector: 'app-partners',
  templateUrl: './partners.component.html',
  styleUrls: ['./partners.component.css']
})
export class PartnersComponent implements OnInit {

  constructor(public StudentInformation:StudentInformationComponent) { }

  ngOnInit(): void {
    this.AddPartners = new Partners;
    this.StudentInformation.CheckArrayIsNull(this.StudentInformation.GetPartners, "أزواج")

  }
  AddPartners: Partners;
  AddPartner() {
    this.StudentInformation.showSpinner();
    this.AddPartners.Ssn = this.StudentInformation.SSN
    this.AddPartners.NationaliryId = this.AddPartners.Nationaliry.Id
    this.StudentInformation.studentService.AddPartners(this.StudentInformation.SSN, this.AddPartners).subscribe(
      res => {
        
        this.AddPartners.NationaliryId = this.AddPartners.Nationaliry.Id;
        this.StudentInformation.GetPartners.push(this.AddPartners as Partners);
        this.AddPartners = new Partners;
        this.StudentInformation.hideSpinner();
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    )

  }
  DeletePartner(item) {
    this.StudentInformation.showSpinner();
    this.StudentInformation.studentService.RemovePartners(this.StudentInformation.SSN, item).subscribe(
      res => {
        this.StudentInformation.hideSpinner();
        this.StudentInformation.GetPartners = this.StudentInformation.GetPartners.filter(c => c != item);
      }, err => {
        this.StudentInformation.hideSpinner();
      }
    );

  }
  checkWritePartnerName(): boolean {
    if (this.AddPartners.Name == null || this.AddPartners.Name == "")
      return true;
    else
      return false;
  }
  checkAddPartner(): boolean {

    if (this.AddPartners.Name == null || this.AddPartners.Name == "" ||
      this.AddPartners.Nationaliry == null ||
      this.AddPartners.PartnerWork == null || this.AddPartners.PartnerWork == "") {
      this.StudentInformation.errormessage = "ادخل جميع القيم"
      return true;
    }
    else return false;
  }
}
