import { IModel } from '../interfaces/IModel';

export class Specialization implements IModel {
    constructor() {
        this.Name="";
        this.Id="";
     }
    Id:string;
    Name:string;
    IsEnabled:boolean;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
   
}
