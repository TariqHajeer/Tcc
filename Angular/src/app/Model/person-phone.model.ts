import { PhoneType } from './phone-type.model';

export class PersonPhone {
    //add
    PersonId:number;
    PhoneTypeId:number;
    Phone:string;
    //get
    PhoneType:PhoneType;

}
