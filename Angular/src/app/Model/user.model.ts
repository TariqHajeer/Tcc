import { Group } from './group.model';
import { IModel } from '../interfaces/IModel';

export class User implements IModel {
    constructor() {
        this.Name="";
        this.Id=0;
     }
    Id:number;
    Username:string;
    Name:string;
    Password:string;
    PasswordVerification:string;
    IsEnabled:boolean;
    groupid:number;
    GroupIds:number[];
    Created:Date;
    CreatedBy:string;
    Modified:Date;
    ModifiedBy:string;
    GroupCount:number;
    Group:Group[];
}
