import { IModel } from '../interfaces/IModel';

export class Privlage implements IModel {
    constructor() {
        this.Name="";
        this.Id=0;
     }
    Id:number;
    Name:string;
    Description:string;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
   
}
 