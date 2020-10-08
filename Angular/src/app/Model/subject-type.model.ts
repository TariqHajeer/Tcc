import { IModel } from '../interfaces/IModel';
import { IEntity } from '../interfaces/IEntity';

export class SubjectType implements IModel,IEntity {
    constructor() {
        this.Name="";
        this.Id=0;
     }
    //add SubjectType
    Id:number;
    Name:string;
    IsEnabled:boolean;
    PracticalDegree:number;
    NominateDegree:number;
    TheoreticalDegree:number;
    SuccessDegree:number;
  //  YearId:number;
    //response
    SpecializationId:string; 
    Created:Date;  
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
}
 