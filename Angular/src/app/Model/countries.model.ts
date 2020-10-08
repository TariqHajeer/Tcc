import { IModel } from '../interfaces/IModel';

export class Countries implements IModel {
    constructor() {
        this.Name="";
        this.Id=0;
     }
    Id:number;
    Name:string;
    MainCountry:number;
    IsEnabled:boolean;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    Country:{
        Id:number;
    Name:string;
    }
    
}
