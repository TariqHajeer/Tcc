import { StudyPlan } from './study-plan.model';
import { Subject } from './subject.model';

export class StudySemester {
    //add study semester
    Id:number;
    Number:number;
    StudyPlanId:number;
    StudyYearId:number;
    YearId:number;
   
    StudyPlan:StudyPlan;

     //get study semester
     StudyYear:{
Id:number;
Name:string;
     }
  Subjects:Subject[];
}
