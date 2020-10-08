import { IEnabelableEntity } from '../interfaces/IEntity';
import { IModel } from '../interfaces/IModel';
import { SubjectType } from './subject-type.model';
import { EquivalentSubject } from '../Component/main/study-plan/study-plan.component';
export class Subject implements  IModel {
    TempId:number;
    Id: any;
    Name: string;
    SubjectCode: string;
   // IsEnabled: boolean;
    EquivalentSubjects: EquivalentSubject[];
    DependencySubjects:Subject[];
    SubjectType: SubjectType;
    SubjectTypeId: number;
    StudySemesterId:number;
    DependencySubjectsId: number[];
    EquivalentSubjectsId: number[];
    PracticalTime:number;
    TheoreticalTime:number;
    MainSemesterNumber:number

//response
DependOnSubjects:Subject[];
SubjectsDependOnMe:Subject[];
EqvuvalentSubject:EqvuvalentSubject[];
Created: Date;
CreatedBy: string;
Modified: Date;
ModifiedBy: string;

//
StudySemesterNumber:number
StudyYearId:number

}
export class EqvuvalentSubject{
  
           Id:number   
           SubjectCode:string   
           Name:string   
           SubjectType:SubjectType 
           SemesterNumber:any   
           StudyYearName:string   
           SpecializationName :string  
           FirstYear :number  
           SecondYear:number 
   
}
  export class UpdateSubjectDto
{
      Id 
      SubjectCode 
      Name 
      PracticalTime 
      TheoreticalTime 
      SubjectTypeId 
      StudySemesterId 
}