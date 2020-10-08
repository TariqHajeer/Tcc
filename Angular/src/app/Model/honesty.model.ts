import { IModel } from '../interfaces/IModel';

export class Honesty implements IModel{
    constructor() {
        this.Name="";
        this.Id=0;
     }
    Id:number;
    Name:string;
    IsEnabled:boolean;
    CountryId:number;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    Honesty:{
        Id:number;
        Name:string;
    }
    
}
