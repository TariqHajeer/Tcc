import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm, FormGroup } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
import { SpecializationService } from 'src/app/Service/specialization.service';
import { YearService } from 'src/app/Service/year.service';
import { SubjectService } from 'src/app/Service/subject.service';
import { StudySemesterService } from 'src/app/Service/study-semester.service';
import { SubjectTypeService } from 'src/app/Service/subject-type.service';
import { StudyPlanService } from 'src/app/Service/study-plan.service';
import { StudyPlan, ResponseStudyPlan, StudyYear, ResponseStudySemester } from 'src/app/Model/study-plan.model';
import { StudyPlanSubject } from 'src/app/Model/study-plan-subject.model';
import { SubjectType } from 'src/app/Model/subject-type.model';
import { Subject, EqvuvalentSubject } from 'src/app/Model/subject.model';
import { Year } from 'src/app/Model/year.model';
import { HelpService } from 'src/app/Help/help.service';
import { StudySemester } from 'src/app/Model/study-semester.model';
import { Specialization } from 'src/app/Model/specialization.model';
import { element } from 'protractor';
import { MatBottomSheet, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { StudyPlanResponce } from 'src/app/Model/study-plan-responce.model';
import { AppComponent } from 'src/app/app.component';
import { NgxSpinnerService } from 'ngx-spinner';
// import * as jquery from 'jquery';
@Component({
  selector: 'app-study-plan',
  templateUrl: './study-plan.component.html',
  styleUrls: ['./study-plan.component.css']
})
export class StudyPlanComponent extends AppComponent implements OnInit {
  step
  panelOpenState(x: number) {
    this.step = x;
  }
  xpandStatus = false;
  oldSelectedspechaliztion: Specialization;
  oldYears: Year[];
  erroemessage: string;
  equivalentSubjects: EquivalentSubject[] = [];
  //#region EditSubject
  temp: StudyPlanSubject = new StudyPlanSubject;
  editSubject: StudyPlanSubject = new StudyPlanSubject;
  Edit() {
    //   var subject = this.editSubject.AddSubjectDTO;
    //   subject.EquivalentSubjects = this.equivalentSubjects;
    //   subject.SubjectTypeId = subject.SubjectType.Id;
    //   var dependancySubjects = this.GetDependancySubject().filter(c => this.dependacySubjectsId.indexOf(c.Id) > -1);
    //   subject.DependencySubjectsId = this.dependacySubjectsId;
    //   subject.DependencySubjects = dependancySubjects;
    //   subject.EquivalentSubjectsId = this.equivalentSubjects.map(c => c.Subject.Id);
    //   this.StudyPlaneService.StudyPlan.Subjects.map(c=>{c=this.editSubject})
    //   //  this.StudyPlaneService.StudyPlan.Subjects.forEach(c=>{
    //   //    if(c.AddSubjectDTO.TempId==subject.TempId)
    //   //   { c=this.editSubject
    //   //    console.log(c)
    //   //    this.getSubject();
    //   //   }
    //   //  })

    //  // this.SubjectService.StudyPlaneSubject=this.editSubject
  
    //   this.closeModal();
    //  // this.GetStudyPlanForSpechaliztion();

this.ResertAddSubject();
  }
  
  ResertAddSubject(){
    this.SubjectService.StudyPlaneSubject = new StudyPlanSubject();
    this.equivalentSubjects = [];
    this.equalSubjectsId = [];
    this.dependacySubjectsId = [];
    this.selectedOldYear=new Year
    this.selectedoldStudySemeser=new ResponseStudySemester
    this.oldSelectedspechaliztion=new Specialization
  }
  closeModal() {
    this.editSubject = Object.assign(this.editSubject, this.temp);
    this.editSubject.AddSubjectDTO = Object.assign(this.editSubject.AddSubjectDTO, this.temp.AddSubjectDTO);
    this.equivalentSubjects = [];
  }
  fillData(item: StudyPlanSubject) {
    this.editSubject = item;
    this.temp = Object.assign({}, item);
    this.temp.AddSubjectDTO = Object.assign({}, item.AddSubjectDTO);
    this.equivalentSubjects=item.AddSubjectDTO.EquivalentSubjects
  }
  ChangeStudySemester(subject: StudyPlanSubject) {
    if (subject.StudySemesterNumber != this.temp.StudySemesterNumber) {
      this.helperService.toastr.warning('عند  التعديل على الفصل الدراسي يؤدي الى فقدان المواد التي تعتمد على المادة ')
      subject.AddSubjectDTO. SubjectsDependOnMe = []
      subject.AddSubjectDTO.DependOnSubjects = []
    }
  }
  // closeModal() {

  //   this.editSubject = new StudyPlanSubject;
  //   this.equivalentSubjects = [];
  // }
  // fillData(item:StudyPlanSubject) {
  //    this.temp = Object.assign({}, item);
  //    console.log(this.temp);
  //    this.editSubject.AddSubjectDTO.Name = item.AddSubjectDTO.Name;
  //    this.editSubject.AddSubjectDTO.PracticalTime = item.AddSubjectDTO.PracticalTime;
  //    this.editSubject.AddSubjectDTO.TheoreticalTime = item.AddSubjectDTO.TheoreticalTime;
  //    this.editSubject.AddSubjectDTO.StudySemesterId = item.AddSubjectDTO.StudySemesterId;
  //    this.editSubject.AddSubjectDTO.TempId = item.AddSubjectDTO.TempId;
  //    this.editSubject.StudySemesterNumber = item.StudySemesterNumber;
  //    this.editSubject.StudyYearId = item.StudyYearId;
  //    this.editSubject.AddSubjectDTO.SubjectType = item.AddSubjectDTO.SubjectType;
  //    this.editSubject.AddSubjectDTO.SubjectCode = item.AddSubjectDTO.SubjectCode;
  //    this.editSubject.AddSubjectDTO.DependencySubjectsId= this.dependacySubjectsId
  //    this.editSubject.AddSubjectDTO.EquivalentSubjects=item.AddSubjectDTO.EquivalentSubjects
  //    this.equivalentSubjects = this.editSubject.AddSubjectDTO.EquivalentSubjects;


  // }
  //#endregion

  //#region validation


  checkStudyPlaneSubject(): boolean {
    // if(this.StudyPlaneService.StudyPlan.Subjects.length >= 4){
    //   var countOne=this.StudyPlaneService.StudyPlan.Subjects.filter(s=>s.StudySemesterNumber==1&&s.StudyYearId==1).length
    //   var counttow=this.StudyPlaneService.StudyPlan.Subjects.filter(s=>s.StudySemesterNumber==2&&s.StudyYearId==1).length
    //   var countthree=this.StudyPlaneService.StudyPlan.Subjects.filter(s=>s.StudySemesterNumber==1&&s.StudyYearId==2).length
    //   var countfore=this.StudyPlaneService.StudyPlan.Subjects.filter(s=>s.StudySemesterNumber==2&&s.StudyYearId==2).length
    //   if(countOne!=null&&counttow!=null&&countthree!=null&&countfore!=null)
    //   return false
    //   else
    //   return true
    // }
    if (this.StudyPlaneService.StudyPlan.Subjects.length < 4
      || this.StudyPlaneService.StudyPlan.SpecializationId == null
      || this.StudyPlaneService.StudyPlan.YearId == null
      || this.StudyPlaneService.StudyPlan.SpecializationId == undefined
      || this.StudyPlaneService.StudyPlan.YearId == undefined) {
      return true;
    } else return false;

  }
  checkSubject(): boolean {
    if (this.SubjectService.StudyPlaneSubject.AddSubjectDTO.Name == null
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.SubjectCode == null
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.SubjectType == null
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.Name == ''
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.SubjectCode == ''
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.Name == undefined
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.SubjectCode == undefined
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.StudySemesterId == null
      || this.SubjectService.StudyPlaneSubject.StudyYearId == null
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.PracticalTime == null
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.TheoreticalTime == null
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.PracticalTime == undefined
      || this.SubjectService.StudyPlaneSubject.AddSubjectDTO.TheoreticalTime == undefined
      || this.hiddenMessage == false
      || this.StudyPlaneService.StudyPlan.SpecializationId == null
      || this.StudyPlaneService.StudyPlan.YearId == null
      || this.StudyPlaneService.StudyPlan.SpecializationId == undefined
      || this.StudyPlaneService.StudyPlan.YearId == undefined


    ) {

      return true;
    } else return false;

  }
  CheckdependacySubjects(): boolean {
    if (this.SubjectService.StudyPlaneSubject.StudySemesterNumber == 1 &&
      this.SubjectService.StudyPlaneSubject.StudyYearId == 1) return true
    return false
  }
  checkEqualSubjects(EqualSubjectsid): boolean {
    if (EqualSubjectsid == null) {
      return true;
    } else return false;
  }

  checkYearAndSpecialization(): boolean {
    if (this.StudyPlaneService.StudyPlan.SpecializationId == '' && this.StudyPlaneService.StudyPlan.YearId == 0) {
      return true;
    } else return false;
  }
  CheckSubjectBeforDetele(subject: Subject) {
    var canDelete: boolean = true;
    var id = subject.TempId;
    var subjects = this.StudyPlaneService.StudyPlan.Subjects.map(c => c.AddSubjectDTO).filter(s => s.TempId != id);
    subjects.forEach(element => {
      if (element.DependencySubjectsId.indexOf(id) > -1) {
        canDelete = false;
        if (confirm("هناك مواد تعتمد على هذه المادة هل انت متأكد من الحذف")) {
          this.DeleteSubject(subject);
        }
      }
    });
    if (!canDelete) {
      return;
    }
    this.DeleteSubject(subject);
  }
  errorCodeMessage = ""
  hiddenMessage = true

  ChekcSubjectCode(subcode): boolean {
    var code = this.StudyPlaneService.StudyPlan.Subjects.find(c => c.AddSubjectDTO.SubjectCode == subcode)
    this.SubjectService.GetSubjectByCode(subcode).subscribe(
      res => {
        var subjectCode = res
        if (code != null || code != undefined || subjectCode != null || subjectCode != undefined) {
          this.errorCodeMessage = "الكود موجود مسبقا"
          this.hiddenMessage = false
          return true
        }
      }, err => {
        console.log(err)
        if (code != null || code != undefined) {
          this.errorCodeMessage = "الكود موجود مسبقا"
          this.hiddenMessage = false
          return true
        }
      }
    )

    this.hiddenMessage = true
    this.errorCodeMessage = ""
    return false
  }
  CheckSubjectCodeOfEdit(item) {
    var arrrOfSubject = this.StudyPlaneService.StudyPlan.Subjects.filter(c => c.AddSubjectDTO.TempId != this.temp.AddSubjectDTO.TempId);
    var findCode = arrrOfSubject.find(c => c.AddSubjectDTO.SubjectCode == item.AddSubjectDTO.SubjectCode)
    this.SubjectService.GetSubjectByCode(item.AddSubjectDTO.SubjectCode).subscribe(
      res => {
        var subjectCode = res
        if (findCode != null || findCode != undefined || subjectCode != null || subjectCode != undefined) {
          this.errorCodeMessage = "الكود موجود مسبقا"
          this.hiddenMessage = false

          return true
        }

      }, err => {
        if (findCode != null || findCode != undefined) {
          this.errorCodeMessage = "الكود موجود مسبقا"
          this.hiddenMessage = false

          return true
        }
      }
    )

    this.hiddenMessage = true
    this.errorCodeMessage = ""
    return false

  }
  

  //#endregion
  constructor(
    public StudyPlaneService: StudyPlanService,
    public SpecialzationService: SpecializationService,
    public YearService: YearService,
    public SubjectService: SubjectService,
    public SupjectTypeSerice: SubjectTypeService,
    public studySemesterService: StudySemesterService,
    public helperSerive: HelpService,
    public _formBuilder: FormBuilder,
    public _bottomSheet: MatBottomSheet
    , public spinner: NgxSpinnerService) {
    super(spinner, helperSerive);
  }

  ngOnInit(): void {
    this.SpecialzationService.getEnabled();
    this.StudyPlaneService.StudyPlan = new StudyPlan;
    this.SupjectTypeSerice.getEnabled();
    this.SubjectService.Subject = new Subject;
    this.SubjectService.StudyPlaneSubject = new StudyPlanSubject;
  }
  checkSpecialazitionId = true
  GetYearForStudyPlan() {
    this.showSpinner();
    this.YearService.GetYearWithoutStudyPlan(this.StudyPlaneService.StudyPlan.SpecializationId);
    this.checkSpecialazitionId = false
    this.hideSpinner();

  }
  selectedOldYear: Year;
  oldSelectedStudyYear: StudyYear;
  selectedoldStudySemeser: ResponseStudySemester;
  GetStudyPlanForSpechaliztion() {
    if (this.oldSelectedspechaliztion != undefined) {
      this.StudyPlaneService.GetBySpecialization(this.oldSelectedspechaliztion.Id).subscribe(res => {
        this.StudyPlaneService.StudyPlans = res as ResponseStudyPlan[];
        this.oldYears = this.StudyPlaneService.StudyPlans.map(sp => sp.Year)
        this.GetSemestet();
        this.GetSubjects();
      },
        err => {
          this.hideSpinner();
        });
    }
    // this.GetSemestet();
    //     this.GetSubjects();
  }
  GetSemestet() {
    if (this.StudyPlaneService.StudyPlans == null || this.StudyPlaneService.StudyPlans == undefined) {
      this.selectedoldStudySemeser = null;
      return [];
    }
    var studyPlan = this.StudyPlaneService.StudyPlans.filter(sp => sp.Year == this.selectedOldYear)[0];
    if (studyPlan == undefined || studyPlan == null) {
      this.selectedoldStudySemeser = null;
      return [];
    }
    return studyPlan.StudySemester;
  }
  GetSubjects(): Subject[] {
    if (this.selectedoldStudySemeser == null || this.selectedoldStudySemeser == undefined) {
      return [];
    }
    var subjects = this.selectedoldStudySemeser.Subjects;

    if (this.equivalentSubjects != undefined)
      subjects = this.helperSerive.DifferenceTowArrayById(subjects, this.equivalentSubjects.map(c => c.Subject));
    return subjects;
  }

  equalSubjectsId: number[];
  AddEquivalentSubject() {
    var subjects = this.GetSubjects().filter(c => this.equalSubjectsId.indexOf(c.Id) > -1);
    subjects.forEach(element => {
      var equivalentSubject = new EquivalentSubject();
      equivalentSubject.Subject = element;
      equivalentSubject.SpecialzationName = this.oldSelectedspechaliztion.Name;
      equivalentSubject.StudyPlanYear = this.selectedOldYear.FirstYear + "/" + this.selectedOldYear.SecondYear;
      this.equivalentSubjects.push(equivalentSubject);
    });

  }

  DeleteEquivalSubject(equivalentSubject: EquivalentSubject) {
    this.equivalentSubjects = this.equivalentSubjects.filter(c => c != equivalentSubject);
  }
  DisabledYearAndSpecialization=false;
  dependacySubjectsId: number[] = [];
  AddSubject(): void {
    this.DisabledYearAndSpecialization=true
    var subject = this.SubjectService.StudyPlaneSubject.AddSubjectDTO;
    subject.EquivalentSubjects = this.equivalentSubjects;
    subject.SubjectTypeId = subject.SubjectType.Id;
    var dependancySubjects = this.GetDependancySubject().filter(c => this.dependacySubjectsId.indexOf(c.Id) > -1);
    subject.DependencySubjectsId = this.dependacySubjectsId;
    subject.DependencySubjects = dependancySubjects;
    subject.EquivalentSubjectsId = this.equivalentSubjects.map(c => c.Subject.Id);
    this.StudyPlaneService.StudyPlan.Subjects.push(this.SubjectService.StudyPlaneSubject);
    this.helperService.add()
   this.ResertAddSubject();
  //  this.GetStudyPlanForSpechaliztion();

  }

  GetDependancySubject() {
    if (this.SubjectService.StudyPlaneSubject.StudyYearId == undefined || this.SubjectService.StudyPlaneSubject.StudyYearId == null) {
      return [];
    }
    if (this.SubjectService.StudyPlaneSubject.StudySemesterNumber == undefined || this.SubjectService.StudyPlaneSubject.StudySemesterNumber == null) {
      return [];
    }
    var subject = this.SubjectService.StudyPlaneSubject;
    var dependancySubject = this.StudyPlaneService.StudyPlan.Subjects.filter(c =>
      (c.StudyYearId < subject.StudyYearId) || (c.StudyYearId == subject.StudyYearId && c.StudySemesterNumber < subject.StudySemesterNumber));
    return dependancySubject.map(c => c.AddSubjectDTO);
  }
  Save() {

    /*    this.StudyPlaneService.StudyPlan.Subjects.forEach(element => {
          element.AddSubjectDTO.IsEnabled = this.StudyPlaneService.StudyPlan.IsEnabled
         
        });*/
        this.DisabledYearAndSpecialization=false
    this.showSpinner();
    this.StudyPlaneService.AddStudyPlan().subscribe(response => {
      this.hideSpinner();
      this.helperSerive.add();
      this.StudyPlaneService.StudyPlan = new StudyPlan
      this.SubjectService.StudyPlaneSubject = new StudyPlanSubject();
      this.equivalentSubjects = [];
      this.equalSubjectsId = [];
      this.dependacySubjectsId = [];
      console.log(response);
    },
      err => {
        this.hideSpinner();
      });
    // here shoud send api request 

  }

  DeleteSubject(subject: Subject) {
    
    this.StudyPlaneService.StudyPlan.Subjects =
      this.StudyPlaneService.StudyPlan.Subjects.filter(c => c.AddSubjectDTO.TempId != subject.TempId);
      if( this.StudyPlaneService.StudyPlan.Subjects.length==0)
    this.DisabledYearAndSpecialization=false
  }
  classCss():string{
    if(this.hiddenMessage==false)
    return "errorcode"
    
  }

}


export class EquivalentSubject {
  Subject: Subject;
  SpecialzationName: string;
  StudyPlanYear: string;
  /**
   *
   */
  constructor() {
    this.Subject == null;
    this.SpecialzationName = "";
    this.StudyPlanYear = "";
  }
  /**
   *
   */

}





