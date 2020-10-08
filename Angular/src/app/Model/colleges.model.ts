import { IModel } from '../interfaces/IModel';

export class Colleges implements IModel {
    constructor() {
        this.Name="";
        this.Id=0;
     }
     //add
    Id:number;
    Name:string;
    ProvinceId:number;
    IsEnabled:boolean;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    //response
    City:{
         Id:number;
        Name:string;
    }
        Country:{
            Id:number;
            Name:string;
        } 
}
