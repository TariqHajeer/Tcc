import { Injectable } from '@angular/core';
import { StudentSubjectDTO } from 'src/app/Model/student.model';
@Injectable({
  providedIn: 'root'
})
export class StudentSubjectDTOService {
  IsSucess(studentSubjectDTO: StudentSubjectDTO): boolean {
    return (Number(studentSubjectDTO.PracticalDegree) + Number(studentSubjectDTO.TheoreticlaDegree) >= studentSubjectDTO.Subject.SubjectType.SuccessDegree) || studentSubjectDTO.HelpDegree;
  }
  HelpDgree(studentSubjectDTO :StudentSubjectDTO):number{
    if(!studentSubjectDTO.HelpDegree)
    return 0;
    return this.SucessMark(studentSubjectDTO)-this.TotalOrigenalDegree(studentSubjectDTO);
  }  
  TotalOrigenalDegree(studentSubjectDTO: StudentSubjectDTO): number {
    return Number(studentSubjectDTO.PracticalDegree) + Number(studentSubjectDTO.TheoreticlaDegree);
  }
  HaveHelpDegree(studentSubjectDTO: StudentSubjectDTO) {
    return studentSubjectDTO.HelperDegree != 0 || studentSubjectDTO.HelperDegree != null;
  }
  SucessMark(studentSubjectDTO: StudentSubjectDTO): number {
    return studentSubjectDTO.Subject.SubjectType.SuccessDegree;
  }
  IsNominate(studentSubjectDTO: StudentSubjectDTO): boolean {
    var NominateDegree = studentSubjectDTO.Subject.SubjectType.NominateDegree;
    return studentSubjectDTO.PracticalDegree >= NominateDegree
  }
  IsSucessWithHelpDegree(studentSubjectDTO: StudentSubjectDTO): boolean {
    return (this.TotalOrigenalDegree(studentSubjectDTO) + Number(studentSubjectDTO.HelperDegree)) >= studentSubjectDTO.Subject.SubjectType.SuccessDegree;
  }
  SubjectIsNull(item:StudentSubjectDTO):boolean{
   return item.PracticalDegree==null||item.TheoreticlaDegree==null
  }
}
