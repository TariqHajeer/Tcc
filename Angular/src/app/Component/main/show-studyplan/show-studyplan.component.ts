import { Component, OnInit } from '@angular/core';
import { StudyPlanComponent, EquivalentSubject } from '../study-plan/study-plan.component';
import { StudyPlanService } from 'src/app/Service/study-plan.service';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { SubjectTypeService } from 'src/app/Service/subject-type.service';
import { StudySemesterService } from 'src/app/Service/study-semester.service';
import { HelpService } from 'src/app/Help/help.service';
import { FormBuilder } from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { NgxSpinnerService } from 'ngx-spinner';
import { ResponseStudyPlan, ResponseStudySemester } from 'src/app/Model/study-plan.model';
import { Year } from 'src/app/Model/year.model';
import { StudySemester } from 'src/app/Model/study-semester.model';
import { Subject, UpdateSubjectDto } from 'src/app/Model/subject.model';
import { YearService } from 'src/app/Service/year.service';
import { SubjectService } from 'src/app/Service/subject.service';
import { StudyPlanSubject } from 'src/app/Model/study-plan-subject.model';
import { Specialization } from 'src/app/Model/specialization.model';


@Component({
  selector: 'app-show-studyplan',
  templateUrl: './show-studyplan.component.html',
  styleUrls: ['./show-studyplan.component.css']

})
export class ShowStudyplanComponent extends StudyPlanComponent implements OnInit {
  constructor(public StudyPlaneService: StudyPlanService,
    public SpecialzationService: SpecializationService,
    public YearService: YearService,
    public SubjectService: SubjectService,
    public SupjectTypeSerice: SubjectTypeService,
    public studySemesterService: StudySemesterService,
    public helperSerive: HelpService,
    public _formBuilder: FormBuilder,
    public _bottomSheet: MatBottomSheet
    , public spinner: NgxSpinnerService) {
    super(StudyPlaneService, SpecialzationService, YearService, SubjectService, SupjectTypeSerice,
      studySemesterService, helperSerive, _formBuilder, _bottomSheet, spinner);
  }
  getSpecializtion: Specialization;
  getStudyPlanYear: ResponseStudyPlan[]
  getStudyPlan: ResponseStudyPlan;
  getYear: Year[];
  studyPlan: ResponseStudyPlan[]
  subject: Subject[];
  checkDependOnSubject: boolean = false;
  ShowStudyPlan: boolean = false;



  ngOnInit(): void {
    this.SpecialzationService.getEnabled();
    this.SupjectTypeSerice.getEnabled();
    this.getStudyPlan = new ResponseStudyPlan();
    this.addSubject = new Subject();
    this.SubjectService.StudyPlaneSubject = new StudyPlanSubject
  }
  //#region Get Studyplan
  GetStudyPlansForSpechaliztion() {
    if (this.getSpecializtion.Id != undefined) {
      this.showSpinner();
      this.YearService.GetYearBySpecialization(this.getSpecializtion.Id).subscribe(res => {
        this.hideSpinner();
        this.getYear = res as Year[];
        //this.selectedOldYear = new Year

      });
    }

  }
  selectedYear: Year = new Year
  GetStudyPlan(SpecializationId: string, YearId: number) {
    this.spinner.show();
    this.StudyPlaneService.GetStudyPlan(SpecializationId, YearId).subscribe(res => {
      console.log(res)
      this.getStudyPlan = res as ResponseStudyPlan;
      // this.selectedOldYear=this.YearService.years.find(y=>y.Id==YearId)
      // console.log(this.getStudyPlan)
      this.ShowStudyPlan = true;
      this.spinner.hide();
    }, err => {
      this.ShowStudyPlan = false;
      this.spinner.hide();
    });

  }

  //#endregion

  //#region Get Semester and Subject
  subjectforStudySemester: Subject[];
  GetSubjectForStudyPlan(studySemester: StudySemester): Subject[] {
    if (studySemester.Number == 1 && studySemester.StudyYear.Id == 1)
      this.checkDependOnSubject = true;
    else
      this.checkDependOnSubject = false;
    return this.subjectforStudySemester = studySemester.Subjects;
  }
  //#endregion

  //#region subject in semster

  addSubject: Subject = new Subject
  oldSelectedspechaliztion: Specialization = new Specialization;
  selectedOldYear: Year = new Year;
  selectedoldStudySemeser: ResponseStudySemester = new ResponseStudySemester;
  dependacySubjectsId: number[] = [];
  ClickModel() {
    this.addSubject = new Subject;
    this.equivalentSubjects = []
  }
  AddSubjectInStudySemester(stydySemester: StudySemester) {
    this.addSubject.SubjectTypeId = this.addSubject.SubjectType.Id
    this.addSubject.StudySemesterId = stydySemester.Id
    this.showSpinner()
    this.SubjectService.AddSubject(this.addSubject).subscribe(res => {
      this.helperService.add()
      this.hideSpinner()
    }, err => {
      this.hideSpinner()
    })
    this.GetSubjectForStudyPlan(stydySemester).push(this.addSubject);
    this.GetSubjectForStudyPlan(stydySemester);
    this.addSubject = new Subject;
  }
  DeleteSubjectInStudySemester(studySemester: StudySemester, subject: Subject) {
    this.showSpinner()
    this.SubjectService.Remove(subject.Id).subscribe(res => {
      this.helperService.delete()
      var semester = this.getStudyPlan.StudySemester.find(s => s.Id == studySemester.Id)
      semester.Subjects.filter(s => s != subject);
      this.GetStudyPlan(this.getSpecializtion.Id, this.selectedOldYear.Id)

      this.hideSpinner()
    }, err => {
      this.hideSpinner()
    })
  }

  EditSubject: UpdateSubjectDto = new UpdateSubjectDto()
  fillDataEdit(semester: StudySemester, subject: Subject) {
    this.EditSubject.SubjectCode = subject.SubjectCode
    this.EditSubject.Id = subject.Id
    this.EditSubject.Name = subject.Name
    this.EditSubject.PracticalTime = subject.PracticalTime
    this.EditSubject.TheoreticalTime = subject.TheoreticalTime
    this.EditSubject.SubjectTypeId = subject.SubjectType.Id
    this.EditSubject.StudySemesterId = semester.Id
  }
  updateSubject() {
    this.showSpinner()
    console.log(this.EditSubject)
    this.SubjectService.UpdateSubject(this.EditSubject).subscribe(res => {
      this.closeModal()
      this.helperService.edit()
      this.hideSpinner()
    }, err => {
      this.hideSpinner()
    })
  }
  closeModal() {
    this.EditSubject = new UpdateSubjectDto
    this.GetStudyPlan(this.getSpecializtion.Id, this.selectedOldYear.Id)
  }
  ChangeStuySemester(subject: Subject) {
    if (subject.MainSemesterNumber != this.EditSubject.StudySemesterId) {
      this.helperService.toastr.warning('عند  التعديل على الفصل الدراسي يؤدي الى فقدان المواد التي تعتمد على المادة ')
      subject.SubjectsDependOnMe = []
      subject.DependOnSubjects = []
    }
  }
  //#endregion


  //#region  SubjectsDependOnMe

  GetSubjectsDependOnMe: Subject[];
  getSubjectsDependOnMe(subject: Subject) {
    this.getsubject = subject;
    this.GetSubjectsDependOnMe = subject.SubjectsDependOnMe;
  }


  //#endregion

  //#region Equivalent subjects
  equivalentSubjects: EquivalentSubject[] = [];
  equivalentSubject: EquivalentSubject = new EquivalentSubject
  GetEquivalentSubjects(item: Subject) {
    this.getsubject = item;
    this.equivalentSubject = new EquivalentSubject
    this.equivalentSubject.Subject = new Subject
    this.equivalentSubjects = []
    if (item.EqvuvalentSubject != [] || item.EqvuvalentSubject != null || item.EqvuvalentSubject != undefined) {
      item.EqvuvalentSubject.forEach(eq => {
        this.equivalentSubject.Subject.SubjectCode = eq.SubjectCode
        this.equivalentSubject.Subject.SubjectType = eq.SubjectType
        this.equivalentSubject.Subject.StudySemesterNumber = eq.SemesterNumber
        this.equivalentSubject.Subject.Name = eq.Name
        this.equivalentSubject.SpecialzationName = this.getSpecializtion.Name
        this.equivalentSubject.StudyPlanYear = this.selectedOldYear.FirstYear + "/" + this.selectedOldYear.SecondYear
        this.equivalentSubjects.push(this.equivalentSubject)
        this.equivalentSubject = new EquivalentSubject
        this.equivalentSubject.Subject = new Subject
      })
    }

    console.log(this.equivalentSubjects)

  }
  //#endregion

  //#region DependOnSubjects
  getsubject: Subject = new Subject
  GetDependOnSubject: Subject[] = [];
  StudySemester: ResponseStudySemester[] = []
  GetDependOnSubjects(subject: Subject, StudySemesters: ResponseStudySemester[], StudySemester: ResponseStudySemester) {
    this.getsubject = subject;
    this.GetDependOnSubject = subject.DependOnSubjects;
    StudySemesters.forEach(item => {
      if ((item.StudyYear.Id < StudySemester.StudyYear.Id) ||
        (item.StudyYear.Id == StudySemester.StudyYear.Id && item.Number < StudySemester.Number))
        this.StudySemester.push(item)
    })
  }
  //#endregion


  //#region validation
  ChekcSubjectCode(subcode): boolean {

    this.SubjectService.GetSubjectByCode(subcode).subscribe(
      res => {
        var subjectCode = res
        if (subjectCode != null || subjectCode != undefined) {
          this.errorCodeMessage = "الكود موجود مسبقا"
          this.hiddenMessage = false
          return true
        }
      }, err => {

      }
    )

    this.hiddenMessage = true
    this.errorCodeMessage = ""
    return false
  }


  //#endregion
}
