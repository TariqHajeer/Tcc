import { Specialization } from './specialization.model';
import { Year } from './year.model';
import { StudySemester } from './study-semester.model';

export class StudyPlanResponce {
    Id:number;
    IsEnabled:boolean;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    Specialization:Specialization;
    Year:Year;
    StudySemester:StudySemester[];
}
 
