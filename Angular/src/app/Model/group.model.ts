import { Privlage } from './privlage.model';
import { User } from './user.model';
import { IModel } from '../interfaces/IModel';

export class Group implements IModel {
    constructor() {
        this.Name="";
        this.Id=0;
     }
    Id:number;
    Name:string;
    Priveleges:number[];
    privelegeId:number;
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    PrivilagesCount:number;
    UserCount:number;
    Privilages:Privlage[];
    User:User[];
}
