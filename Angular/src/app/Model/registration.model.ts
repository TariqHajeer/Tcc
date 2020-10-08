import { Year } from './year.model';
import { StudyYear } from './study-plan.model';
import { SocialStates } from './social-states.model';
import { Data } from '@angular/router';
import { TypeOfRegister } from './type-of-register.model';
import { StudentStateResponse } from './student.model';

export class Registration {
    //Add
    Id:number
    Ssn:string;
    YearId:number;
    StudyYearId:number;
    CardNumber:number;
    CardDate:Date;
    StudentStateId:number;
    SoldierDate:Date;
    Note:string;
    //get
    Year:Year;
    StudyYear:StudyYear;
    StudentState:StudentStateResponse;
    //respons
    FinalStateId:number
    FinalState:StudentStateResponse
    ActuallyStudyYear
    SystemNote:string
    TypeOfRegistarId:number
    TypeOfRegistar:object
    Created:Data
    CreatedBy:string
    Modified:Date
    ModifiedBy:string
    //validation
   


}
export class CreateRegistrationDTO{
    CardNumber:number;
    CardDate:Date;
    TypeOfRegistarId:number
    SoldierDate:Date;
    Note:string;
}