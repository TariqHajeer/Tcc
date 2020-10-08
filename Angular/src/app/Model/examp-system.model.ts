import { IModel } from '../interfaces/IModel';

export class ExampSystem implements IModel  {
    constructor() {
        this.Name="";
        this.Id=0;
     }
    //AddExamSystemDTO and UpdateExamSystemDTO
    Id:number;
    Name:string;
    HaveTheredSemester:boolean;
    IsDoubleExam:boolean;
    IsEnabled:boolean;
    GraduateStudentsSemester:number
    //ResponseExamSystemDTO
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    Updateable:boolean
}
