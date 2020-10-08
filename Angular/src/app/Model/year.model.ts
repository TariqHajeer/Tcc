import {YearSystem} from './year-system.model';
import {IId} from '../interfaces/IId';
import { ExampSystem } from './examp-system.model';
export class Year implements IId {
    Id: any;
    FirstYear:any;
    SecondYear:any;
    YearSystem:number;
    //ExamSystem:number;
    //response
    Yearystem:YearSystem;
     ExamSystem:number
    Blocked:boolean;
    Updatepla:boolean
}
export class UpdateYearDTO{
    YearSystem:number
    ExamSystem:number
}
export class ResponseYear implements IId {
    Id: any;
    FirstYear:any;
    SecondYear:any;
    Yearystem:YearSystem;
     ExamSystem:ExampSystem
    Blocked:boolean;
    Updatepla:boolean
}