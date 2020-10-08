import { Degree } from './degree.model';
import { Maincountry } from './maincountry.model';
import { Year } from './year.model';

export class StudentDegree {
    //add
    Ssn:string;
    DegreeId:number;
    Degree:number;
    Source:number;
    Date:any;
    //response
    degree:Degree
    Country:Maincountry;
    Year:Year;
    //
   
 
}
