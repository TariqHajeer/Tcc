import { StudyPlanSubject } from './study-plan-subject.model';
import { IEnabelableEntity, IEntity } from '../interfaces/IEntity';
import { Specialization } from './specialization.model';
import { IId } from '../interfaces/IId';
import {Year} from'./year.model';
import { IModel } from '../interfaces/IModel';
import {Subject} from'./subject.model';
export class StudyPlan {

    constructor() {
        this.SpecializationId = '';
    }
    //add study plan
    Id: number;
    YearId: number;
    SpecializationId: string;
    IsEnabled: boolean =true;
    Subjects:StudyPlanSubject[]=[];
}

export class ResponseStudyPlan implements IEnabelableEntity, IId {
    Id: any;
    IsEnabled: boolean;
    Created: Date;
    CreatedBy: string;
    Modified: Date;
    ModifiedBy: string;
    Specialization:Specialization;
    Year:Year;
    StudySemester:ResponseStudySemester[]
    Updateable:boolean
}

export class ResponseStudySemester implements IId,IEntity {
    Id: any;
    Number:number;
    SemesterNumber:number
    Created: Date;
    CreatedBy: string;
    Modified: Date;
    ModifiedBy: string;
    StudyYear:StudyYear
    Subjects:Subject[];
}
export class StudyYear implements IModel{
    Id: any;
    Name: string;
}