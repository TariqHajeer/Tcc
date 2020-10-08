import { Component, OnInit } from '@angular/core';
import { CreateRegistrationDTO, Registration } from 'src/app/Model/registration.model';
import { StudentStateResponse } from 'src/app/Model/student.model';
import { AuthService } from 'src/app/Service/auth.service';
import { StudentInformationComponent } from '../student-information.component';

@Component({
  selector: 'app-regrsteration',
  templateUrl: './regrsteration.component.html',
  styleUrls: ['./regrsteration.component.css']
})
export class RegrsterationComponent implements OnInit {

  constructor(public StudentInformation: StudentInformationComponent
    , public AuthServic: AuthService) { }

  ngOnInit(): void {
    this.StudentInformation.CheckArrayIsNull(this.StudentInformation.Regsteration, "تسجيلات")
    this.Get()
  }
  step
  panelOpenState(x: number) {
    this.step = x;
  }
  typeOfRegesterId
  Get() {
  //  this.StudentInformation.getStudent()
  this.StudentInformation.YearService.getYearAll()
    this.StudentInformation.Regsteration.forEach(item => {
      this.FindObjectOfId(item)
    //  this.TypeOfRegester(item)
      this.typeOfRegesterId = item.TypeOfRegistarId
    })

  }
  FindObjectOfId(item: Registration) {
    if (item.FinalStateId == undefined || item.FinalStateId == null)
      item.FinalState = new StudentStateResponse()
    else
      item.FinalState = this.StudentInformation.studentService.StudentState.find(s => s.Id == item.FinalStateId)
    item.StudentState = this.StudentInformation.studentService.StudentState.find(s => s.Id == item.StudentStateId)
    item.Year = this.StudentInformation.YearService.years.find(s => s.Id == item.YearId)
    item.TypeOfRegistar = this.StudentInformation.TypeOfRegesterService.typeofregisterall.find(s => s.Id == item.TypeOfRegistarId)

  }
  TypeOfRegester(item: Registration) {
    
    //طالب منقول 
    //اذا كان مستجد
    if (item.StudentStateId == 1)
      item.TypeOfRegistar = this.StudentInformation.TypeOfRegesterService.typeofregisterall.find(s => s.Id == item.TypeOfRegistarId)
    //اولى ناجح او منقول
    else if ((item.StudentStateId == 3 || item.StudentStateId == 2) && item.StudyYearId == 1) {
      item.TypeOfRegistar = {
        Year: "أولى"
        , StudentState: item.StudentState.Name
      }
      item.StudyYearId = 2
    }
    // اولى راسب او مستنفذ
    else if ((item.StudentStateId == 4 || item.StudentStateId == 6) && item.StudyYearId == 1) {
      item.TypeOfRegistar = {
        Year: "أولى"
        , StudentState: item.StudentState.Name
      }
      item.StudyYearId = 1
    }
    //ثانية  مستنفذ او راسب او خارج المعهد او مرسوم او متخرج
    else if ((item.StudentStateId == 4 || item.StudentStateId == 5 || item.StudentStateId == 6
      || item.StudentStateId == 7 || item.StudentStateId == 9) && item.StudyYearId == 2) {
      return item.TypeOfRegistar = {
        Year: "ثانية"
        , StudentState: item.StudentState.Name
      }
    }
  }
  TempRegestration: Registration = new Registration
  ClickEdit(regester: Registration) {
    this.TempRegestration = Object.assign({}, regester)
  }
  EditRegesteration(regester: Registration) {
    this.Get()
  }
  CansleEditRegesteration(regester: Registration) {
    regester = Object.assign(regester, this.TempRegestration)

  }
  Registration: Registration = new Registration()
  AddRegiseration: CreateRegistrationDTO = new CreateRegistrationDTO
  ClickAdd(){
    this.Registration.TypeOfRegistarId=this.typeOfRegesterId
  }
  AddRegistration() {
    // this.Registration.StudentStateId=this.StudentInformation.Regsteration[this.StudentInformation.Regsteration.length-1].FinalStateId
    // this.Registration.StudyYearId=this.StudentInformation.Regsteration[this.StudentInformation.Regsteration.length-1].StudyYearId
    // this.Registration.FinalStateId=8
     //this.Registration.TypeOfRegistarId=this.typeOfRegesterId
    // this.Registration.Created=new Date()
    // this.Registration.CreatedBy=this.AuthServic.auth.Username
    // this.FindObjectOfId(this.Registration)
    // this.TypeOfRegester(this.Registration)
    this.StudentInformation.showSpinner()
    this.AddRegiseration = this.Registration
    console.log(JSON.stringify(this.AddRegiseration))
    this.StudentInformation.studentService.Register(this.AddRegiseration, this.StudentInformation.SSN).subscribe(res => {
      //  this.StudentInformation.Regsteration.push(this.Registration)
      this.Get()
      this.AddRegiseration = new CreateRegistrationDTO()
      this.StudentInformation.hideSpinner()
      console.log(res)
    }, err => {
      this.StudentInformation.hideSpinner()

    })

  }
  CloseModel() {
    this.Registration = new Registration()
  }
}
