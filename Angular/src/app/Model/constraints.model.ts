import { IModel } from '../interfaces/IModel';

export class Constraints  implements IModel{
    constructor() {
        this.Name="";
        this.Id=0;
     }
    Id:number;
    Name:string;
    HonestyId:number;
    IsEnabled:boolean;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    Honesty:{
        Id:number;
    Name:string;
    }
}
