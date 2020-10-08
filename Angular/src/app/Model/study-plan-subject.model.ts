import { Subject } from './subject.model';
import { StudyYear } from './study-plan.model';

export class StudyPlanSubject {
    /**
     *
     */
    constructor() {
        this.AddSubjectDTO = new Subject();
        this.AddSubjectDTO.TempId =StudyPlanSubject.SubjectNumber++;
    }
    public static SubjectNumber: number = 1;
    AddSubjectDTO: Subject;
    StudyYearId: number;
    StudySemesterNumber: number;
}
