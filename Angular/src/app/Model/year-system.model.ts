import { IModel } from '../interfaces/IModel';
import { Setting } from './setting.model';
import {SettingService} from '../Service/setting.service';
export class YearSystem implements IModel{
    constructor( ) {
        this.Name="";
        this.Id=0;
     }
     Id:number;
     Name:string;
     Note:string;
     IsMain:boolean;
     Created:Date;
     CreatedBy:string;
     Modified:Date;
     ModifiedBy:string;
     Settings:Setting[];
     Updateable:boolean

}
