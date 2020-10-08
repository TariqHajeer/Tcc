import { IModel } from '../interfaces/IModel';

export class Attachment implements IModel{
    constructor() {
        this.Name="";
        this.Id=0;
        this.IsEnabled=true
     }
    Id:number;
    Name:string;
    IsEnabled:boolean;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
}
