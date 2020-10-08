import { StudentSubjectDTO } from './student.model'

export class ResponseStudentHelpDegreeDto {
    StudentSubjects:StudentSubjectDTO[]=[]
    StillToSucess:number
    HelpDgreeCount:number
    HelpDegreeDivideOn:number
}
