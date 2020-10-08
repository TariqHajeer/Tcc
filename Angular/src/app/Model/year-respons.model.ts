import { YearSystem } from './year-system.model';

export class YearRespons {
    Id:number;
    FirstYear:Number;
    SecondYear:Number;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    Yearstem:YearSystem;
     ExamSystem:{
         Id:number,
         Name:string
     };

}
