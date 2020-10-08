import { Component, OnInit } from '@angular/core';
import { StudentInformationComponent } from '../student-information.component';

@Component({
  selector: 'app-reperation-and-sanctions',
  templateUrl: './reperation-and-sanctions.component.html',
  styleUrls: ['./reperation-and-sanctions.component.css']
})
export class ReperationAndSanctionsComponent implements OnInit {

  constructor(public StudentInformation: StudentInformationComponent) { }
  AddReperations:Reperation=new Reperation
  AddSanctions:Sanctions= new Sanctions
  ngOnInit(): void {
    
  }
  CheckShowReperations():boolean{
if(this.StudentInformation.GetReperations==[]||this.StudentInformation.GetReperations==null||this.StudentInformation.GetReperations==undefined)
return false
else
return true
  }
  CheckShowSanctions():boolean{
    if(this.StudentInformation.GetSanctions==[]||this.StudentInformation.GetSanctions==null||this.StudentInformation.GetSanctions==undefined)
    return false
    else
    return true
      }
  CheckAdd(){
    if((this.AddReperations.Reparation!=null||this.AddReperations.Reparation!=undefined)||
    (this.AddSanctions.Sanction!=null||this.AddSanctions.Sanction!=undefined)){
this.DisabledAdd=false
    }else
    this.DisabledAdd=true
  }
  Add(){
    if((this.AddReperations.Reparation!=null||this.AddReperations.Reparation!=undefined)&&
    (this.AddSanctions.Sanction!=null||this.AddSanctions.Sanction!=undefined)){
this.SetReperations()
this.SetSanctions()
    }
    else if(this.AddReperations.Reparation!=null||this.AddReperations.Reparation!=undefined){
      this.SetReperations()
    }
    else if(this.AddSanctions.Sanction!=null||this.AddSanctions.Sanction!=undefined){
      this.SetSanctions()
    }
  }
  DisabledAdd=true
  SetReperations() {
    this.AddReperations.Ssn=this.StudentInformation.SSN
    this.StudentInformation.studentService.AddReperations(this.StudentInformation.SSN, this.AddReperations).subscribe(res => {
      this.StudentInformation.GetReperations.push(this.AddReperations);
      this.AddReperations=new Reperation
    })
  }
  SetSanctions() {
    this.AddSanctions.Ssn=this.StudentInformation.SSN
    this.StudentInformation.studentService.AddSanctions(this.StudentInformation.SSN, this.AddSanctions).subscribe(res => {
      this.StudentInformation.GetSanctions.push(this.AddSanctions)
      this.AddSanctions=new Sanctions
    })
  }
}
export class Sanctions{
  Sanction:string
  Ssn:string
}
export class Reperation{
  Reparation:string
  Ssn:string
}